using DpConnect.OpcUa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class OpcUaConnectionConfigurationViewModel : IConnectionConfigurationViewModel
    {
        public OpcUaConnectionConfigurationViewModel()
        {
            
        }

        public OpcUaConnectionConfiguration Config { get; set; } = new OpcUaConnectionConfiguration();
        public string Endpoint { get => Config.Endpoint; set => Config.Endpoint = value; }
        public string ConId { get => Config.ConnectionId; set => Config.ConnectionId = value; }  
        
        public string PublishInterval { get; set; }

        public string ConnectTimeout { get; set; }

        public IEnumerable<NamedConfigParamViewModel> Parameters => new List<NamedConfigParamViewModel>() 
        { 
            new NamedConfigParamViewModel() { Name = "Par1"},
            new NamedConfigParamViewModel() { Name = "Par1"},
        };

        public string ConnectionName => "OpcUa";
    }
}
