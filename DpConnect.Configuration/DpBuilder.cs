using Promatis.Core.Logging;
using DpConnect.Interface;
using System;
using System.Xml.Linq;

namespace DpConnect.Configuration
{
    public class DpBuilder
    {
        readonly DpProviderConfigurator _providerConfigurator;
        readonly DpProcessorConfigurator _processorConfigurator;
        readonly DpXmlConfiguration _configuration;
        readonly ILogger _logger;
        readonly DataPointConfigurator _dpConfigurator;
        public DpBuilder(ILogger logger, string[] configPath, XDocument[] configs, Type[] providerTypes, Type[] processorTypes, IDpProcessor[] processors)
        {
            _logger = logger;

            if (configPath != null)
                _configuration = new DpXmlConfiguration(configPath);
            else if (configs != null)
                _configuration = new DpXmlConfiguration(configs);


            _providerConfigurator = new DpProviderConfigurator(logger);
            _processorConfigurator = new DpProcessorConfigurator();

            foreach (var provider in providerTypes)
                _providerConfigurator.RegisterProvider(provider);

            if (processors != null)
            {
                foreach (var processor in processors)
                    _processorConfigurator.RegisterProcessor(processor);
            }

            if (processorTypes != null)
            {
                foreach (var processor in processorTypes)
                    _processorConfigurator.RegisterProcessor(processor);
            }

            _providerConfigurator.ConfigureProviders(_configuration.ProviderConfiguration);
            _processorConfigurator.ConfigureProcessors(_configuration.ProcessorConfiguration);

            _dpConfigurator = new DataPointConfigurator(logger, _providerConfigurator.ConfiguredProviders, _processorConfigurator.ConfiguredProcessors);
            _dpConfigurator.ConfigureDatapoints(_configuration.DataPointConfiguration);

            _processorConfigurator.DpInitialized();
        }
        public void StartProviders() => _providerConfigurator.StartProviders();
        public void StopProviders() => _providerConfigurator.StopProviders();
    }
}
