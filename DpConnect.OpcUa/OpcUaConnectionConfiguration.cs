using DpConnect.Configuration;
using DpConnect.Interface;


namespace DpConnect.OpcUa
{
    public class OpcUaConnectionConfiguration : IDpConnectionConfiguration
    {
        public string ConnectionId { get; set; }

        public string Endpoint { get; set; }

        public OpcUaConnectionConfiguration()
        {
            
        }
        public OpcUaConnectionConfiguration(DpConnectionXmlConfiguration xmlConfiguration)
        {
            ConnectionId = xmlConfiguration.ConnectionId;
            Endpoint = "opc.tcp://10.10.10.95:4840";
        }

    }
}
