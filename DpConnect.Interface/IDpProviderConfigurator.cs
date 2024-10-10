
using System.Collections.Generic;
using System.Xml.Linq;

namespace DpConnect.Interface
{
    public interface IDpProviderConfigurator
    {
        IEnumerable<IDpProvider> ConfigureProviders(XDocument xmlConfig);

        IEnumerable<IDpProvider> ConfiguredProviders { get; set; }

        void StartProviders();
        void StopProviders();
    }
}