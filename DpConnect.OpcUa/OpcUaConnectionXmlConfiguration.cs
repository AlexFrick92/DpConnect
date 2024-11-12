using DpConnect.Configuration;

namespace DpConnect.OpcUa
{
    public class OpcUaConnectionXmlConfiguration : OpcUaConnectionConfiguration
    {
        const string Xml_EndpointTag = "Endpoint";

        public OpcUaConnectionXmlConfiguration(DpConnectionXmlConfiguration configuration)
        {
            ConnectionId = configuration.ConnectionId;
            Endpoint = configuration.Configuration.Root.Element(Xml_EndpointTag).Value;
        }
    }
}
