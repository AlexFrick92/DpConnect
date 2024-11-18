

using System.Collections.Generic;

namespace DpConnect
{
    public interface IDpWorkerManager
    {
        IDpWorker CreateWorker<T>() where T : IDpWorker;

        IDpWorker GetWorker();

        IEnumerable<T> ResolveWorker<T>();
    }
}
