
using System;
using System.Xml.Linq;

namespace DpConnect.Configuration.Xml
{
    public class DpConnectionXmlConfiguration : IDpConnectionConfiguration
    {
        public string ConnectionId { get; set; }

        public XDocument Configuration { get; set; }

        public bool Active { get; set; }

        public Type ConnectionType { get; set; }
    }    

}
