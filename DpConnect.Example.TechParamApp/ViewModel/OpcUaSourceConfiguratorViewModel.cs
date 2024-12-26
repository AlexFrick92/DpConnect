using DpConnect.OpcUa;
using System.Collections.Generic;


namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class OpcUaSourceConfiguratorViewModel : BaseViewModel, ISourceConfigurationViewModel
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
    }
}
