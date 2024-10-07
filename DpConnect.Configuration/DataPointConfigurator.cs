using Promatis.Core.Logging;
using DpConnect.Interface;
using System.Xml.Linq;
using System.Collections.Generic;
using System;
using System.Linq;

namespace DpConnect.Configuration
{
    public class DataPointConfigurator
    {
        readonly ILogger _logger;
        readonly ICollection<IDpProvider> _providers;
        readonly ICollection<IDpProcessor> _processors;

        public DataPointConfigurator(ILogger logger, List<IDpProvider> configuredProviders, List<IDpProcessor> configuredProcessors)
        {
            _providers = configuredProviders;
            _processors = configuredProcessors;
            _logger = logger;
            if (_providers == null || _processors == null)
            {
                throw new ArgumentNullException();                
            }            
        }           
        public void ConfigureDatapoints(XDocument xmlConfig)
        {            
            Console.WriteLine($"DataPoint Configurator: Регистрация точек данных");            

            foreach (var dataPoint in xmlConfig.Element(DpXmlConfiguration.Tag_DataPointDefinition).Elements())
            {
                IDataPoint dp = null;

                // Регистрируем точкки для процессоров
                foreach (var dpProcessorConfig in dataPoint.Elements(DpXmlConfiguration.Tag_Processor))
                {

                    var processor = _processors.First(x => (x.Name == dpProcessorConfig.Attribute(DpXmlConfiguration.Tag_Description).Value));
                    var processorProps = processor.GetType().GetProperties();

                    foreach (var procProp in processorProps)
                    {
                        if (procProp.Name == dpProcessorConfig.Attribute(DpXmlConfiguration.Tag_TargetProperty).Value)
                        {
                            if (dp == null) //Если точка еще не создавалась. Т.к. разные процессоры могут использовать одни и те же точки.
                            {
                                //Получим обобщенный тип от первого процессора который использует эту точку.
                                //Если есть еще процессоры, которые используют эту точку, то они обязаны иметь такой же тип точки
                                Type[] genericTypeArgument = procProp.PropertyType.GetGenericArguments();
                                dynamic dpInstance = null;
                                
                                if (dataPoint.Name == DpXmlConfiguration.Tag_DpValue)
                                {
                                    Type dpType = genericTypeArgument[0];
                                    
                                    if (dpType.IsValueType || dpType == typeof(string))
                                    {
                                        //Создаём простую точку взяв тип от свойства процессора                                        
                                        string dpName = dataPoint.Attribute("Name") != null ? dataPoint.Attribute("Name").Value : "Имя не задано";                                       
                                        Console.WriteLine($"DataPoint Configurator: Обнаружена простая точка  {dpName} : {dpType}" );
                                        Type genericType = typeof(DpValue<>).MakeGenericType(dpType);
                                        dpInstance = Activator.CreateInstance(genericType);                                                                            
                                        (dpInstance as IDataPoint).Name = dpName;


                                        XElement dpProviderConfig = dataPoint.Element(DpXmlConfiguration.Tag_Provider);
                                        IDpProvider provider = _providers.First(x => (x.Name == dpProviderConfig.Attribute(DpXmlConfiguration.Tag_Description).Value));


                                        provider.RegisterDp(dpInstance, new XDocument(dpProviderConfig));
                                    }
                                    else
                                    {

                                        string dpName = dataPoint.Attribute("Name") != null ? dataPoint.Attribute("Name").Value : "Имя не задано";

                                        Console.WriteLine($"DataPoint Configurator: Обнаружена сложная точка {dpName} : {dpType}");

                                        Type genericType = typeof(DpComplexValue<>).MakeGenericType(genericTypeArgument[0]);
                                        dpInstance = Activator.CreateInstance(genericType);
                                        (dpInstance as IDataPoint).Name = dpName;

                                        Console.WriteLine("DataPoint Configurator: Регистрируем свойства сложной точки:");
                                        foreach (var dpProp in  dataPoint.Elements(DpXmlConfiguration.Tag_DpProperty))
                                        {                                           
                                            Type propertyType = genericTypeArgument[0].GetProperties().First(p => p.Name == dpProp.Attribute(DpXmlConfiguration.Tag_Description).Value).PropertyType;
                                            dynamic dpProperty = ((IDpComplexValueConfig)dpInstance).AddProperty(propertyType, dpProp.Attribute(DpXmlConfiguration.Tag_Description).Value);

                                            XElement dpProviderConfig = dpProp.Element(DpXmlConfiguration.Tag_Provider);
                                            IDpProvider provider = _providers.First(x => (x.Name == dpProviderConfig.Attribute(DpXmlConfiguration.Tag_Description).Value));

                                            provider.RegisterDp(dpProperty, new XDocument(dpProviderConfig));
                                        }
                                    }

                                }
                                else if (dataPoint.Name == DpXmlConfiguration.Tag_DpMethod)
                                {
                                    Type genericMethodType = typeof(DpMethod<,>).MakeGenericType(genericTypeArgument[0], genericTypeArgument[1]);
                                    dpInstance = Activator.CreateInstance(genericMethodType);


                                    dataPoint.Element("InputArguments")?.Elements("Argument")
                                        .Where(arg => arg.Attribute("Name") != null)
                                        .ToList()
                                        .ForEach(arg => ((IDpMethodConfig)dpInstance).AddInputProperties(arg.Attribute("Name").Value));

                                    dataPoint.Element("OutputArguments")?.Elements("Argument")
                                        .Where(arg => arg.Attribute("Name") != null)
                                        .ToList()
                                        .ForEach(arg => ((IDpMethodConfig)dpInstance).AddOutputProperties(arg.Attribute("Name").Value));
                                    

                                    XElement dpProviderConfig = dataPoint.Element(DpXmlConfiguration.Tag_Provider);
                                    IDpProvider provider = _providers.First(x => (x.Name == dpProviderConfig.Attribute(DpXmlConfiguration.Tag_Description).Value));
                                    provider.RegisterMethod(dpInstance, new XDocument(dpProviderConfig));                                    
                                }

                                dp = (IDataPoint)dpInstance;
                            }
                            procProp.SetValue(processor, dp);
                        }
                    }
                }
            }

        }
    }
}
