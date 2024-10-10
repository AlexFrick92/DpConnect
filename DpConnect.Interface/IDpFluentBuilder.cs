using DpConnect.Interface;

namespace DpConnect.Interface
{
    public interface IDpFluentBuilder
    {
        IDpFluentBuilder AddConfiguration(params string[] configPath);

        IDpFluentBuilder Build ();
        IDpFluentBuilder SetProcessors(IDpProcessor[] processorInstances);        
        void StartProviders();
    }
}