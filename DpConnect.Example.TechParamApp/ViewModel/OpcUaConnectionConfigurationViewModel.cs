using DpConnect.OpcUa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class OpcUaConnectionConfigurationViewModel : BaseViewModel, IConnectionConfigurationViewModel
    {        
        public OpcUaConnectionConfigurationViewModel()
        {
            List<NamedConfigParamViewModel> pars = new List<NamedConfigParamViewModel>();
            var paramId = new NamedConfigParamViewModel();
            paramId.Name = "Имя соединения";
            paramId.PropertyChanged += (s, v) =>
            {
                ConId = paramId.Value.ToString();
            };

            pars.Add(paramId);
            Parameters = pars;
        }        



        OpcUaConnectionConfiguration Config { get; set; } = new OpcUaConnectionConfiguration();
        public string Endpoint { get => Config.Endpoint; set => Config.Endpoint = value; }
        public string ConId { get => Config.ConnectionId; set => Config.ConnectionId = value; }  
        
        public string PublishInterval { get; set; }

        public string ConnectTimeout { get; set; }

        public IEnumerable<NamedConfigParamViewModel> Parameters { get; }   

        public string ConnectionName => "OpcUa";

        public void CreateConnection(IDpConnectionManager manager)
        {
            manager.CreateConnection<IOpcUaConnection, OpcUaConnectionConfiguration>(Config);
        }
    }
}
