using Promatis.Core.Logging;
using DpConnect.Interface;
using System.Xml.Linq;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

namespace DpConnect.Configuration
{
    public class DataPointConfigurator
    {
        readonly ILogger _logger;
        readonly IEnumerable<IDpProvider> _providers;
        readonly IEnumerable<IDpProcessor> _processors;

        public DataPointConfigurator(ILogger logger, IEnumerable<IDpProvider> configuredProviders, IEnumerable<IDpProcessor> configuredProcessors)
        {
            _providers = configuredProviders;
            _processors = configuredProcessors;
            _logger = logger;
            if (_providers == null || _processors == null)
            {
                throw new ArgumentNullException();                
            }            
        }   
        
        //Процедура связи процессоров с провайдерами
        public void ConfigureDatapoints(XDocument xmlConfig)
        {                        
            Console.WriteLine($"DataPoint Configurator: Регистрация точек данных");            

            //В конфигурационном файле лежат определение точек <DataPointDefinition/>, в нем лежит либо <DpValue/> либо <DpMethod/>
            foreach (var dataPoint in xmlConfig.Element(DpXmlConfiguration.Tag_DataPointDefinition).Elements())
            {
                //Здесь мы сначала создаем пустой объект IDataPoint - общий и для метода и для точки, и для составной точки
                IDataPoint dp = null; 

                foreach(var dpProcessorConfig in dataPoint.Elements(DpXmlConfiguration.Tag_Processor))
                {
                    var processor = _processors.First(x => (x.Name == dpProcessorConfig.Attribute(DpXmlConfiguration.Tag_Description).Value));
                    var processorProps = processor.GetType().GetProperties();

                    var procProp = processorProps.First(x => x.Name == dpProcessorConfig.Attribute(DpXmlConfiguration.Tag_TargetProperty).Value);

                    if (dp == null) dp = GetConfiguredDataPoint(dataPoint, procProp);

                    procProp.SetValue(processor, dp);
                }
               
            }

        }

        IDataPoint GetConfiguredDataPoint(XElement dataPoint, PropertyInfo procProp)
        {
            //Получим обобщенный тип DpValue<T> или DpMethod<I,O> от первого процессора который использует эту точку.
            //Если есть еще процессоры, которые используют эту точку, то они обязаны иметь такой же тип точки
            //Мы получаем массив либо из 1 либо из 2 переменных. Почему мы это делаем до того, Как узнали DpValue или DpMethod? Чтобы написать эту строчку 1 раз.
            Type[] genericTypeArgument = procProp.PropertyType.GetGenericArguments();

            //Здесь нам нужен объект dynamic (почему не object? Почему не IDataPoint? из за IDpValueSource<T> и других обобщенных методах.),
            //который мы заполним либо DpValue, либо DpMethod. 
            dynamic dpInstance = null;

            //Если это DpValue<>
            if (dataPoint.Name == DpXmlConfiguration.Tag_DpValue)
            {
                Type dpType = genericTypeArgument[0]; //DpValue<T>

                //Простой тип - любой значимый (включая DateTime) либо строка.
                if (dpType.IsValueType || dpType == typeof(string))
                {
                   dpInstance = GetDpValue(dataPoint, dpType);
                }
                //Если T - ссылочный тип, значит имеем дело со сложной точкой данных, в которой свойства будут как отдельный DpValue
                else
                {

                    dpInstance = GetDpComplexValue(dataPoint, dpType);
                }

            }

            //Если это DpMethod
            else if (dataPoint.Name == DpXmlConfiguration.Tag_DpMethod)
            {
                Type inputType = genericTypeArgument[0];
                Type outputType = genericTypeArgument[1];
                dpInstance = GetDpMethod(dataPoint, inputType, outputType);
            }
            return dpInstance as IDataPoint;
        }


        dynamic GetDpValue(XElement dataPoint, Type dpType)
        {
            dynamic dpInstance = null;
            //Создаём простую точку взяв тип от свойства процессора
            //Берем имя из <DpValue Name="ИМЯ ТОЧКИ">
            string dpName = dataPoint.Attribute("Name") != null ? dataPoint.Attribute("Name").Value : "Имя не задано";

            Console.WriteLine($"DataPoint Configurator: Обнаружена простая точка  {dpName} : {dpType}");

            //Создаем типизированную DpValue
            Type genericType = typeof(DpValue<>).MakeGenericType(dpType);
            dpInstance = Activator.CreateInstance(genericType);
            (dpInstance as IDataPoint).Name = dpName;

            //Теперь ищем законфигурированный провайдер <Provider Name=""> по имени в коллекции
            XElement dpProviderConfig = dataPoint.Element(DpXmlConfiguration.Tag_Provider);
            IDpProvider provider = _providers.First(x => (x.Name == dpProviderConfig.Attribute(DpXmlConfiguration.Tag_Description).Value));

            //Передаёем точку в провайдер и конфиг, который находится внутри <Provider>.
            //Здесь нам нужно, чтобы точка реализовывала типизированный интерфейс IDpValueSource<T>, поэтому мы передаём dynamic?
            provider.RegisterDp(dpInstance, new XDocument(dpProviderConfig));

            return dpInstance;
        }

        dynamic GetDpComplexValue(XElement dataPoint, Type dpType)
        {

            dynamic dpInstance = null;

            string dpName = dataPoint.Attribute("Name") != null ? dataPoint.Attribute("Name").Value : "Имя не задано";

            Console.WriteLine($"DataPoint Configurator: Обнаружена сложная точка {dpName} : {dpType}");

            //DpComplexValue<T> класс для составной точки
            Type genericType = typeof(DpComplexValue<>).MakeGenericType(dpType);

            dpInstance = Activator.CreateInstance(genericType);
            (dpInstance as IDataPoint).Name = dpName;

            Console.WriteLine("DataPoint Configurator: Регистрируем свойства сложной точки:");


            //В конфигурации перечислены свойства типа. Для каждого перечисленного свойства мы найдем свойство в типе IDpValue<T>
            foreach (var dpProp in dataPoint.Elements(DpXmlConfiguration.Tag_DpProperty))
            {

                //propertyType - IDpValue<T> аргумент T, в нём мы и ищем свойство, имя которого совпадает с заданным в конфиге и берем его тип.
                Type propertyType = dpType.GetProperties().First(p => p.Name == dpProp.Attribute(DpXmlConfiguration.Tag_Description).Value).PropertyType;

                //Теперь зарегестрируем его в объекте сложной точки. 
                dynamic dpProperty = ((IDpComplexValueConfig)dpInstance).AddProperty(propertyType, dpProp.Attribute(DpXmlConfiguration.Tag_Description).Value);

                //Зарегестрируем эту точку в провайдере. Разные свойсвтва сложной точки могут быть считаны разными провайдерами.
                XElement dpProviderConfig = dpProp.Element(DpXmlConfiguration.Tag_Provider);
                IDpProvider provider = _providers.First(x => (x.Name == dpProviderConfig.Attribute(DpXmlConfiguration.Tag_Description).Value));
                provider.RegisterDp(dpProperty, new XDocument(dpProviderConfig));
            }
            return dpInstance;
        }

        dynamic GetDpMethod(XElement dataPoint, Type InputType, Type OutputType)
        {
            dynamic dpInstance = null;
            //DpMethod создается с двумя аргументами: класс входных параметров и класс выходных.
            //В качество типа входных или выходных может быть простой (значимый тип), либо составной.
            //Если тип составной, нужно явно указать имя аргументов, которые используются в этом классе
            //Этот механизм нужно сильно пересмотреть...

            //Создаём тип на основе аргументов
            Type genericMethodType = typeof(DpMethod<,>).MakeGenericType(InputType, OutputType);
            dpInstance = Activator.CreateInstance(genericMethodType);

            //Если указан тег <InputArguments>, то внутри должен быть <InputArgument Name=""> в котором будет указано имя свойства. 

            //Так мы добавляем входные
            dataPoint.Element("InputArguments")?.Elements("Argument")
                .Where(arg => arg.Attribute("Name") != null)
                .ToList()
                .ForEach(arg => ((IDpMethodConfig)dpInstance).AddInputProperties(arg.Attribute("Name").Value));
            //Так мы добавляем выходные аргументы
            dataPoint.Element("OutputArguments")?.Elements("Argument")
                .Where(arg => arg.Attribute("Name") != null)
                .ToList()
                .ForEach(arg => ((IDpMethodConfig)dpInstance).AddOutputProperties(arg.Attribute("Name").Value));

            //То есть, если мы не добавим ни одного входного или выходного аргумента, то класс DpMethod будет считать, что он работает с простым типом

            //Регистрируем метод в провайдере
            XElement dpProviderConfig = dataPoint.Element(DpXmlConfiguration.Tag_Provider);
            IDpProvider provider = _providers.First(x => (x.Name == dpProviderConfig.Attribute(DpXmlConfiguration.Tag_Description).Value));
            provider.RegisterMethod(dpInstance, new XDocument(dpProviderConfig));

            return dpInstance;
        }
    }
}
