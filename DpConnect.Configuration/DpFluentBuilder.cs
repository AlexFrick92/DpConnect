using DpConnect.Interface;
using Promatis.Core;
using Promatis.Core.Logging;
using System;
using System.Data.SqlTypes;
using System.Xml.Linq;

namespace DpConnect.Configuration
{
    public class DpFluentBuilder
    {

        DpProviderConfigurator _providerConfigurator;
        DpProcessorConfigurator _processorConfigurator;
        DataPointConfigurator _dpConfigurator;
        IIoCContainer _container;

        DpXmlConfiguration _configuration;
        ILogger _logger;        
        Type[] _processorTypes;
        IDpProcessor[] _dpProcessors;

        public DpFluentBuilder SetContainer(IIoCContainer container)
        {
            _container = container;
            return this;
        }
        public DpFluentBuilder AddConfiguration(params string[] configPath)
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
        public DpFluentBuilder SetProcessors(Type[] processorTypes)
        {
            _processorTypes = processorTypes;
            return this;    
        }
        public DpFluentBuilder SetProcessors(IDpProcessor[] processorInstances) 
        { 
            _dpProcessors = processorInstances;
            return this;
        }
        public DpFluentBuilder Build()
        {
            if (_logger == null)
                _logger = new ConsoleLogger();
            if (_container == null)
                throw new ArgumentNullException("Не задан IoC");

            _providerConfigurator = new DpProviderConfigurator(_logger, _container);
            _processorConfigurator = new DpProcessorConfigurator();

            if(_dpProcessors != null)
                foreach (var processor in _dpProcessors)
                    _processorConfigurator.RegisterProcessor(processor);            

            if(_processorTypes != null)
                foreach (var processor in _processorTypes)
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
