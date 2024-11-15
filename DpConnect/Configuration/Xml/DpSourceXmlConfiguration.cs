

using System.Xml.Linq;

namespace DpConnect.Configuration.Xml
{
    public class DpSourceXmlConfiguration : IDpSourceConfiguration
    {
        public XDocument Configuration { get; set; }
    }
}
