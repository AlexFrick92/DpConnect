using DpConnect.Interface;
using System.Xml.Linq;

namespace DpConnect.Configuration
{
    public class DpValueSourceXmlConfiguration : IDpValueSourceConfiguration
    {
        public XDocument Configuration { get; set; }
    }
}
