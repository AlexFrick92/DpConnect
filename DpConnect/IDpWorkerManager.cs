

namespace DpConnect
{
    public interface IDpWorkerManager
    {
        IDpWorker CreateWorker<T>() where T : IDpWorker;

        IDpWorker GetWorker();
    }
}
