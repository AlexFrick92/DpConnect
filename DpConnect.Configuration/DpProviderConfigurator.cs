using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DpConnect.Interface;
using Promatis.Core.Logging;
using Promatis.Core;
using System.Reflection;

namespace DpConnect.Configuration
{
    public class DpProviderConfigurator : IDpProviderConfigurator
    {
        ILogger _logger;        
        public IEnumerable<IDpProvider> ConfiguredProviders { get; set; }
        List<IDpProvider> _configuredProviders;
        IIoCContainer _container;

        public DpProviderConfigurator(ILogger logger, IIoCContainer container)
        {
            _logger = logger;            
            _configuredProviders = new List<IDpProvider>();
            ConfiguredProviders = _configuredProviders;
            _container = container;

        }

        public IEnumerable<IDpProvider> ConfigureProviders(XDocument xmlConfig)
        {
            foreach (XElement xmlProvider in xmlConfig.Element(DpXmlConfiguration.Tag_ProviderDefinition).Elements("Provider"))
            {

                string typeName = xmlProvider.Attribute("TypeName").Value;                
                
                Type providerType = Type.GetType(typeName);

                MethodInfo methodInfo = typeof(IIoCContainer).GetMethod(nameof(IIoCContainer.Resolve), new[] {typeof(Type)}  );
                MethodInfo genericMethod = methodInfo.MakeGenericMethod(providerType);

                IDpProvider provider =  (IDpProvider)genericMethod.Invoke(_container, new[] { providerType });                    
                
                                
                provider.Name = xmlProvider.Attribute("Name").Value;
                provider.ConfigureHost(new XDocument(xmlProvider));
                _configuredProviders.Add(provider);

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
        public IDpProvider GetProviderByName(string name)
        {
            return ConfiguredProviders.First(x => (x.Name == name));
        }

        public void StopProviders()
        {
            foreach (var provider in ConfiguredProviders)
                provider.Stop();
        }


    }
}
