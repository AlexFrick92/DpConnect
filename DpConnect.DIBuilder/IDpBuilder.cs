using DpConnect.Interface;

namespace DpConnect.DIBuilder
{
    public interface IDpBuilder
    {
        void Bind(IDpProcessor processor);

        T CreateProcessor<T>(string name) where T : IDpProcessor;
    }
}
