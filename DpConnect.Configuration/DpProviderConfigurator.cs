using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DpConnect.Interface;
using Promatis.Core.Logging;

namespace DpConnect.Configuration
{
    public class DpProviderConfigurator
    {
        ILogger _logger;
        List<Type> _registeredProviders;
        public List<IDpProvider> ConfiguredProviders;

        public DpProviderConfigurator(ILogger logger)
        {
            _logger = logger;
            _registeredProviders = new List<Type>();
            ConfiguredProviders = new List<IDpProvider>();
        }

        public void RegisterProvider(Type type) 
        {
            if (typeof(IDpProvider).IsAssignableFrom(type))
            {
                _registeredProviders.Add(type);
                System.Console.WriteLine($"Зарегистрирован провайдер: {type.Name}");
            }
            else
                throw new ArgumentException($"Тип {type.Name} не является наследником {typeof(IDpProvider)}");
        }

        public IList<IDpProvider> ConfigureProviders(XDocument xmlConfig)
        {
            foreach (XElement xmlProvider in xmlConfig.Element(DpXmlConfiguration.Tag_ProviderDefinition).Elements())
            {
                IDpProvider provider = (IDpProvider)Activator.CreateInstance(_registeredProviders.First(p => p.Name == xmlProvider.Name));
                provider.SetLogger(_logger);

                provider.Name = xmlProvider.Attribute("Name").Value;
                provider.ConfigureHost(new XDocument(xmlProvider));
                ConfiguredProviders.Add(provider);

                Console.WriteLine($"Законфигурирован провайдер: {provider.GetType()} : {provider.Name}");
            }
            return ConfiguredProviders;

        }           
        public void StartProviders()
        {
            Console.WriteLine("Запуск провайдеров...");
            foreach (var provider in ConfiguredProviders)
                provider.Start();

            Console.WriteLine("Провайдеры запустились!");
        }

        public void StopProviders()
        {
            foreach (var provider in ConfiguredProviders)
                provider.Stop();
        }


    }
}
