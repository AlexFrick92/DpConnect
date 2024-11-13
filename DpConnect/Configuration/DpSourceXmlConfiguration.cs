using DpConnect.Interface;
using System.Xml.Linq;

namespace DpConnect.Configuration
{
    public class DpSourceXmlConfiguration : IDpSourceConfiguration
    {
        public XDocument Configuration { get; set; }
    }
}
