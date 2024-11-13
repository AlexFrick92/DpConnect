using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Interface
{
    public interface IDpBuilder
    {
        void Build();

        IDpConnectionManager ConnectionManager { get; }
        IDpWorkerManager WorkerManager { get; }
    }
}
