using DpConnect.Configuration;
using DpConnect.Interface;

namespace DpConnect.OpcUa
{
    public class OpcUaDpValueSourceConfiguration : IDpValueSourceConfiguration
    {
        public OpcUaDpValueSourceConfiguration()
        {
            
        }
        public OpcUaDpValueSourceConfiguration(DpValueSourceXmlConfiguration xmlConfig)
        {
            NodeId = "ns=3;s=\"PrimitiveTypes\".\"BoolTypeVar\"";
        }

        public string NodeId { get; set; }
    }
}
