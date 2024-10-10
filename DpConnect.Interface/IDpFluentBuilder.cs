using DpConnect.Interface;
using System.Collections.Generic;

namespace DpConnect.Interface
{
    public interface IDpFluentBuilder
    {
        IDpFluentBuilder AddConfiguration(params string[] configPath);

        IDpFluentBuilder Build ();
        IDpFluentBuilder SetProcessors(IEnumerable<IDpProcessor> processorInstances);        
        void StartProviders();
    }
}