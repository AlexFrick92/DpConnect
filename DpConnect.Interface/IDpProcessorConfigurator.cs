using System.Collections.Generic;
using System.Xml.Linq;

namespace DpConnect.Interface
{
    public interface IDpProcessorConfigurator
    {
        IEnumerable<IDpProcessor> ConfiguredProcessors { get; set; }

        IEnumerable<IDpProcessor> ConfigureProcessors(XDocument xmlProcessors);

        void RegisterProcessor(IDpProcessor processor);

        void DpInitialized();
    }
}