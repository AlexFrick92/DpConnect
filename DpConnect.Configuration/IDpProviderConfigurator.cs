using DpConnect.Interface;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DpConnect.Configuration
{
    public interface IDpProviderConfigurator
    {
        IList<IDpProvider> ConfigureProviders(XDocument xmlConfig);

        List<IDpProvider> ConfiguredProviders { get; set; }

        void StartProviders();
        void StopProviders();
    }
}