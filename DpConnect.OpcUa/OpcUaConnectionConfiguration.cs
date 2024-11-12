using System;
using DpConnect.Configuration;
using DpConnect.Interface;


namespace DpConnect.OpcUa
{
    public class OpcUaConnectionConfiguration : IDpConnectionConfiguration
    {
        public string ConnectionId { get; set; }

        public string Endpoint { get; set; }
    }
}
