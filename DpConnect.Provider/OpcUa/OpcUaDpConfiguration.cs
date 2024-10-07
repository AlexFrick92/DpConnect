using System.Xml.Linq;
using DpConnect.Configuration;

namespace DpConnect.Provider.OpcUa
{
    internal class OpcUaDpConfiguration 
    {
        public string NodeId { get; set; }

        public OpcUaDpConfiguration(XDocument xmlConfig)
        {
            foreach(var prop in xmlConfig.Element(DpXmlConfiguration.Tag_Provider).Elements())
            {
                if(prop.Name == "NodeId")
                {
                    NodeId = prop.Value.Trim();
                }
            }            
        }            
    }
}
