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
        public IEnumerable<IDpProcessor> ConfiguredProcessors { get; set; }
        List<IDpProcessor> _configuredProcessors;
        IIoCContainer _container;


        public DpProcessorConfigurator(IIoCContainer container)
        {
            _container = container;
            _configuredProcessors = new List<IDpProcessor>();
            ConfiguredProcessors = _configuredProcessors;
        }
        public IEnumerable<IDpProcessor> ConfigureProcessors(XDocument xmlProcessors)
        {
            foreach (XElement xmlProc in xmlProcessors.Element(DpXmlConfiguration.Tag_ProcessorDefinition).Elements("Processor"))
            {

                string typeName = xmlProc.Attribute("TypeName").Value;

                Type procType = Type.GetType(typeName);

                MethodInfo methodInfo = typeof(IIoCContainer).GetMethod(nameof(IIoCContainer.Resolve), new[] { typeof(Type) });
                MethodInfo genericMethod = methodInfo.MakeGenericMethod(procType);

                IDpProcessor processor = (IDpProcessor)genericMethod.Invoke(_container, new[] { procType });

                processor.Name = xmlProc.Attribute("Name").Value;

                _configuredProcessors.Add(processor);

                Console.WriteLine($"Законфигурирован процессор: {processor.GetType()} : {processor.Name}");    
                
            }
            return ConfiguredProcessors;
        }
        public void RegisterProcessor(IDpProcessor processor)
        {
            if (_configuredProcessors.FirstOrDefault(p => p.Name == processor.Name) != null)
                throw new Exception("Процессор уже существует");
            else
                _configuredProcessors.Add(processor);
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
