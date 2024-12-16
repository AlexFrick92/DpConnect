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
                config = configuration;
            else
                throw new ArgumentNullException(nameof(configuration));
        }

        OpcUaConnectionConfiguration config;
        public string Endpoint { get => config.Endpoint; set => config.Endpoint = value; }
        public string ConId { get => config.ConnectionId; set => config.ConnectionId = value; }  
        
        public string PublishInterval { get; set; }

        public string ConnectTimeout { get; set; }
    }
}
