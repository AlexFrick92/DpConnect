

namespace DpConnect
{
    public interface IDpBuilder
    {
        void Build();

        IDpConnectionManager ConnectionManager { get; }
        IDpWorkerManager WorkerManager { get; }
    }
}
