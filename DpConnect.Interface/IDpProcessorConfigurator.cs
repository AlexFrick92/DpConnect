using System.Collections.Generic;
using System.Xml.Linq;

namespace DpConnect.Interface
{
    public interface IDpProcessorConfigurator
    {
        List<IDpProcessor> ConfiguredProcessors { get; set; }

        IList<IDpProcessor> ConfigureProcessors(XDocument xmlProcessors);

        void RegisterProcessor(IDpProcessor processor);

        void DpInitialized();
    }
}