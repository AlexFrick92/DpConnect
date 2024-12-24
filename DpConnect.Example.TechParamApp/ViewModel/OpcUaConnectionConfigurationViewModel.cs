using DpConnect.OpcUa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class OpcUaConnectionConfigurationViewModel
    {

        public OpcUaConnectionConfigurationViewModel(OpcUaConnectionConfiguration configuration)
        {
            if(configuration != null)
                Config = configuration;
            else
                throw new ArgumentNullException(nameof(configuration));
        }

        public OpcUaConnectionConfiguration Config { get; set; }
        public string Endpoint { get => Config.Endpoint; set => Config.Endpoint = value; }
        public string ConId { get => Config.ConnectionId; set => Config.ConnectionId = value; }  
        
        public string PublishInterval { get; set; }

        public string ConnectTimeout { get; set; }
    }
}
