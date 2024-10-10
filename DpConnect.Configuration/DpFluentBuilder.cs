﻿using DpConnect.Interface;
using Promatis.Core;
using Promatis.Core.Logging;
using System;
using System.Data.SqlTypes;
using System.Xml.Linq;

namespace DpConnect.Configuration
{
    public class DpFluentBuilder : IDpFluentBuilder
    {

        readonly IDpProviderConfigurator _providerConfigurator;
        readonly IDpProcessorConfigurator _processorConfigurator;
        DataPointConfigurator _dpConfigurator;
        IIoCContainer _container;

        DpXmlConfiguration _configuration;
        ILogger _logger;        
        IDpProcessor[] _dpProcessors;

        public DpFluentBuilder(IIoCContainer container, 
            IDpProviderConfigurator dpProviderConfigurator, 
            IDpProcessorConfigurator dpProcessorConfigurator)
        {
            _container = container;
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
        public IDpFluentBuilder SetProcessors(IDpProcessor[] processorInstances) 
        { 
            _dpProcessors = processorInstances;
            return this;
        }
        public IDpFluentBuilder Build()
        {
            if (_logger == null)
                _logger = new ConsoleLogger();
            if (_container == null)
                throw new ArgumentNullException("Не задан IoC");                        

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
