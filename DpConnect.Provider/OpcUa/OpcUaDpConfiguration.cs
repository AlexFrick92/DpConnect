using System.Xml.Linq;

using DpConnect.Interface;
using DpConnect.Configuration;
using System;

namespace DpConnect.Provider.OpcUa
{
    internal class OpcUaDpConfiguration : IDpSourceConfiguration
    {
        public string NodeId { get; set; }
        public Guid Guid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
