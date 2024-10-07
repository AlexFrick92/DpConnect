using System.Xml.Linq;

namespace DpConnect.Provider.OpcUa
{
    internal class OpcUaHostConfiguration
    {
        public string Endpoint;
        public OpcUaHostConfiguration(XDocument xmlConfig) 
        {
            Endpoint = xmlConfig.Element(typeof(OpcUaProvider).Name).Attribute("Endpoint").Value;
        }
    }
}
