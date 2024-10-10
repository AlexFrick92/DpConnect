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
    public class DpProviderConfigurator
    {
        ILogger _logger;
        List<Type> _registeredProviders;
        public List<IDpProvider> ConfiguredProviders;
        IIoCContainer _container;

        public DpProviderConfigurator(IIoCContainer container)
        {
            _logger = container.Resolve<ILogger>();
            _registeredProviders = new List<Type>();
            ConfiguredProviders = new List<IDpProvider>();
            _container = container;

        }

        public IList<IDpProvider> ConfigureProviders(XDocument xmlConfig)
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
