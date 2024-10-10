using System;
using System.Linq;
using DpConnect.Interface;
using System.Collections.Generic;
using System.Xml.Linq;
using Promatis.Core;
using System.Reflection;

namespace DpConnect.Configuration
{
    public class DpProcessorConfigurator : IDpProcessorConfigurator
    {
        List<Type> _registeredProcessors;
        public List<IDpProcessor> ConfiguredProcessors { get; set; }
        IIoCContainer _container;


        public DpProcessorConfigurator(IIoCContainer container)
        {
            _container = container;
            ConfiguredProcessors = new List<IDpProcessor>();
            _registeredProcessors = new List<Type>();
        }
        public IList<IDpProcessor> ConfigureProcessors(XDocument xmlProcessors)
        {
            foreach (XElement xmlProc in xmlProcessors.Element(DpXmlConfiguration.Tag_ProcessorDefinition).Elements("Processor"))
            {

                string typeName = xmlProc.Attribute("TypeName").Value;

                Type procType = Type.GetType(typeName);

                MethodInfo methodInfo = typeof(IIoCContainer).GetMethod(nameof(IIoCContainer.Resolve), new[] { typeof(Type) });
                MethodInfo genericMethod = methodInfo.MakeGenericMethod(procType);

                IDpProcessor processor = (IDpProcessor)genericMethod.Invoke(_container, new[] { procType });

                processor.Name = xmlProc.Attribute("Name").Value;

                ConfiguredProcessors.Add(processor);

                Console.WriteLine($"Законфигурирован процессор: {processor.GetType()} : {processor.Name}");    
                
            }
            return ConfiguredProcessors;
        }
        public void RegisterProcessor(Type type)
        {
            if (typeof(IDpProcessor).IsAssignableFrom(type))
            {
                _registeredProcessors.Add(type);
                System.Console.WriteLine($"Зарегистрирован процессор: {type.Name}");
            }
            else
                throw new ArgumentException($"Тип {type.Name} не является наследником {typeof(IDpProcessor)}");
        }
        public void RegisterProcessor(IDpProcessor processor)
        {
            if (ConfiguredProcessors.FirstOrDefault(p => p.Name == processor.Name) != null)
                throw new Exception("Процессор уже существует");
            else    
                ConfiguredProcessors.Add(processor);
        }
        public void DpInitialized()
        {
            foreach(var proc in ConfiguredProcessors)
            {
                proc.OnDpInitialized();
            }
        }
    }
}
