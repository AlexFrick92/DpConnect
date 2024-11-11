

namespace DpConnect.Interface
{
    public interface IDpWorkerManager
    {
        IDpWorker CreateWorker<T>() where T : IDpWorker;
    }
}
