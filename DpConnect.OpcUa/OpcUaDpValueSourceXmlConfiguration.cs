using DpConnect.Configuration;

namespace DpConnect.OpcUa
{
    public class OpcUaDpValueSourceXmlConfiguration : OpcUaDpValueSourceConfiguration
    {
        public OpcUaDpValueSourceXmlConfiguration(DpSourceXmlConfiguration config)
        {

            NodeId = config.Configuration.Root.Element("NodeId").Value;            
        }
    }
}
