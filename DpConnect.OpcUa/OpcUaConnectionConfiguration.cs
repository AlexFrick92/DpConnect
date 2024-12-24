using System;
using DpConnect.Configuration;


namespace DpConnect.OpcUa
{
    public class OpcUaConnectionConfiguration : IDpConnectionConfiguration
    {
        public string ConnectionId { get; set; }

        public string Endpoint { get; set; }

        public bool Active { get; set; }

        public Type ConnectionType { get; private set; } = typeof(IOpcUaConnection);
    }
}
