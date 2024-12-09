using DpConnect.OpcUa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class OpcUaConnectionConfigurationViewModel : ConnectionConfigurationViewModel
    {



        OpcUaConnectionConfiguration config;
        public string Endpoint { get => config.Endpoint; set => config.Endpoint = value; }
        public string ConId { get => config.ConnectionId; set => config.ConnectionId = value; }        
    }
}
