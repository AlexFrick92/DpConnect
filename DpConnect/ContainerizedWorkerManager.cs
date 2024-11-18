using System;
using System.Collections.Generic;
using System.Linq;
using Promatis.Core;
using Promatis.Core.Logging;


namespace DpConnect
{
    public class ContainerizedWorkerManager : IDpWorkerManager
    {
        private readonly ILogger logger;
        private readonly IIoCContainer container;

        private readonly IList<IDpWorker> workers = new List<IDpWorker>();

        public ContainerizedWorkerManager(ILogger logger, IIoCContainer container)
        {
            this.logger = logger;
            this.container = container;
        }

        public IDpWorker CreateWorker<T>() where T : IDpWorker
        {
            IDpWorker worker = container.Resolve<T>();
            workers.Add(worker);

            return worker;
        }

        public IEnumerable<T> ResolveWorker<T>()
        {
            IEnumerable<T> resolved = workers.Where(w => typeof(T).IsAssignableFrom( w.GetType() )).Select(p => (T)p);            

            if(resolved.Count() == 0)            
                throw new InvalidOperationException($"В коллекции воркеров не найден воркер с интерфейсом {typeof(T)}");            
            else
                return resolved;
        }
    }
}
