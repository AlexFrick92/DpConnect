using System;
using System.Linq;
using DpConnect.Interface;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DpConnect.Configuration
{
    public class DpProcessorConfigurator
    {
        List<Type> _registeredProcessors;
        public List<IDpProcessor> ConfiguredProcessors;

        public DpProcessorConfigurator()
        {
            ConfiguredProcessors = new List<IDpProcessor>();
            _registeredProcessors = new List<Type>();
        }
        public IList<IDpProcessor> ConfigureProcessors(XDocument xmlProcessors)
        {
            foreach (XElement xmlProc in xmlProcessors.Element(DpXmlConfiguration.Tag_ProcessorDefinition).Elements())
            {
                if (ConfiguredProcessors.FirstOrDefault(p => p.Name == xmlProc.Attribute("Name").Value) == null)
                {
                    IDpProcessor processor = (IDpProcessor)Activator.CreateInstance(_registeredProcessors.First(p => p.Name == xmlProc.Name));
                    processor.Name = xmlProc.Attribute("Name").Value;

                    ConfiguredProcessors.Add(processor);

                    Console.WriteLine($"Законфигурирован процессор: {processor.GetType()} : {processor.Name}");
                }     
                
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
