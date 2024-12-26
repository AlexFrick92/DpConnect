using DpConnect.OpcUa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class OpcUaConnectionConfigurationViewModel : BaseViewModel, IConnectionConfiguratorViewModel
    {        
        public OpcUaConnectionConfigurationViewModel()
        {
            List<NamedConfigSettingViewModel> pars = new List<NamedConfigSettingViewModel>();
            var paramId = new NamedConfigSettingViewModel();
            paramId.Name = "Имя соединения";
            paramId.PropertyChanged += (s, v) =>
            {
                ConId = paramId.Value.ToString();
            };

            pars.Add(paramId);

            var paramEndpoint = new NamedConfigSettingViewModel();
            paramEndpoint.Name = "Адрес";
            paramEndpoint.PropertyChanged += (s, v) =>
            {
                Endpoint = paramEndpoint.Value.ToString();
            };
            pars.Add(paramEndpoint);

            Settings = pars;
        }        



        OpcUaConnectionConfiguration Config { get; set; } = new OpcUaConnectionConfiguration();
        public string Endpoint { get => Config.Endpoint; set => Config.Endpoint = value; }
        public string ConId { get => Config.ConnectionId; set => Config.ConnectionId = value; }  
        
        public string PublishInterval { get; set; }

        public string ConnectTimeout { get; set; }

        public IEnumerable<NamedConfigSettingViewModel> Settings { get; }   

        public string ConnectionTypeName => "OpcUa";

        public void CreateConnection(IDpConnectionManager manager)
        {
            manager.CreateConnection<IOpcUaConnection, OpcUaConnectionConfiguration>(Config);
        }
    }
}
