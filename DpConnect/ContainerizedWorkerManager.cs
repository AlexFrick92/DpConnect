using DpConnect.Interface;
using Promatis.Core;
using Promatis.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
