using DpConnect.Building;
using DpConnect.Configuration;
using DpConnect.Connection;
using DpConnect.OpcUa;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class OpcUaSourceConfiguratorViewModel : BaseViewModel, ISourceConfiguratorViewModel
    {


        public OpcUaSourceConfiguratorViewModel()
        {
            var pars = new List<NamedConfigSettingViewModel>();
            var nodeId = new NamedConfigSettingViewModel();
            nodeId.Name = "NodeId";
            nodeId.PropertyChanged += (s, v) =>
            {
                NodeId = nodeId.Value.ToString();
            };

            pars.Add(nodeId);


            Settings = pars;



        }

        public OpcUaDpValueSourceConfiguration sourceConfiguration = new OpcUaDpValueSourceConfiguration();

        public string NodeId
        {
            get => sourceConfiguration.NodeId;
            set => sourceConfiguration.NodeId = value;
        }

        public IEnumerable<NamedConfigSettingViewModel> Settings { get; private set; }

        public IDpConfiguration CreateConfiguration(string dpName)
        {
            DpConfiguration<OpcUaDpValueSourceConfiguration> config = new DpConfiguration<OpcUaDpValueSourceConfiguration>();
            config.PropertyName = dpName;
            config.SourceConfiguration = sourceConfiguration;

            return config;
        }

        public void BindProperties(IDpWorker worker, IEnumerable<IDpConfiguration> configs, IDpBinder binder, IDpConnection connection)
        {
            Console.WriteLine("Тип :" + configs.First().GetType());

            binder.Bind(worker, 
                connection as IDpBindableConnection<OpcUaDpValueSourceConfiguration>, 
                configs.OfType<DpConfiguration<OpcUaDpValueSourceConfiguration>>());
        }
    }
}
