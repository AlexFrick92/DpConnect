using DpConnect.Interface;
using Promatis.Core;
using Promatis.Core.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Xml.Linq;

namespace DpConnect.Configuration
{
    public class DpFluentBuilder : IDpFluentBuilder
    {

        readonly IDpProviderConfigurator _providerConfigurator;
        readonly IDpProcessorConfigurator _processorConfigurator;
        DataPointConfigurator _dpConfigurator;        

        DpXmlConfiguration _configuration;
        ILogger _logger;        
        IEnumerable<IDpProcessor> _dpProcessors;

        public DpFluentBuilder(ILogger logger,
            IDpProviderConfigurator dpProviderConfigurator, 
            IDpProcessorConfigurator dpProcessorConfigurator)
        {
            _logger = logger;
            _providerConfigurator = dpProviderConfigurator;
            _processorConfigurator = dpProcessorConfigurator;
        }
        public IDpFluentBuilder AddConfiguration(params string[] configPath)
        {
            if (_configuration == null)
                _configuration = new DpXmlConfiguration(configPath);
            else
                _configuration.Add(new DpXmlConfiguration(configPath));

            return this;
        }
        public DpFluentBuilder AddConfiguration(params XDocument[] configs)
        {
            if (_configuration == null)
                _configuration = new DpXmlConfiguration(configs);
            else
                _configuration.Add(new DpXmlConfiguration(configs));

            return this;
        }
        public IDpFluentBuilder SetProcessors(IEnumerable<IDpProcessor> processorInstances) 
        { 
            _dpProcessors = processorInstances;
            return this;
        }
        public IDpFluentBuilder Build()
        {                       
            if(_dpProcessors != null)
                foreach (var processor in _dpProcessors)
                    _processorConfigurator.RegisterProcessor(processor);            
            
            _providerConfigurator.ConfigureProviders(_configuration.ProviderConfiguration);
            _processorConfigurator.ConfigureProcessors(_configuration.ProcessorConfiguration);

            _dpConfigurator = new DataPointConfigurator(_logger, _providerConfigurator.ConfiguredProviders, _processorConfigurator.ConfiguredProcessors);
            _dpConfigurator.ConfigureDatapoints(_configuration.DataPointConfiguration);

            _processorConfigurator.DpInitialized();

            return this;
        }
        public void StartProviders() => _providerConfigurator.StartProviders();
        public void StopProviders() => _providerConfigurator.StopProviders();
    }
}
