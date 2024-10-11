using DpConnect.Interface;

namespace DpConnect.DIBuilder
{
    public interface IDpBinder
    {
        void Bind(IDpProcessor processor);

        T CreateProcessor<T>(string name) where T : IDpProcessor;
    }
}
