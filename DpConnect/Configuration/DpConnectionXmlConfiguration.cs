
using DpConnect.Interface;
using System.Xml.Linq;

namespace DpConnect.Configuration
{
    public class DpConnectionXmlConfiguration : IDpConnectionConfiguration
    {
        public string ConnectionId { get; set; }

        public XDocument Configuration { get; set; }
    }    

}
