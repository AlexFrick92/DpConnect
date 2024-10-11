using DpConnect.Interface;
using Promatis.Core;
using Promatis.Core.Logging;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;

namespace DpConnect.DIBuilder
{
    internal class DumbProviderConfigurator : IDpProviderConfigurator
    {
        private readonly ILogger _logger;
        private readonly IIoCContainer _container;
        public DumbProviderConfigurator(ILogger logger, IIoCContainer container)
        {
            _logger = logger;
            _container = container;
            _logger.Info("Создан ProviderConfigurator");
        }

        public IEnumerable<IDpProvider> ConfiguredProviders { get; set; }

        public IEnumerable<IDpProvider> ConfigureProviders(XDocument xmlConfig)
        {
            List<IDpProvider> providers = new List<IDpProvider>();


            IOpcProvider opcProvider = _container.Resolve<IOpcProvider>();
            

            providers.Add(opcProvider);

            ConfiguredProviders = providers;

            return ConfiguredProviders;
        }

        public void StartProviders()
        {
            
        }

        public void StopProviders()
        {
            
        }
    }
}
