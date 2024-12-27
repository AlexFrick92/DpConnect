using DpConnect.Building;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel.TechParam
{
    public interface ISourceConfiguratorBinder<TWorker> where TWorker : IDpWorker
    { 
        event EventHandler<TWorker> WorkerBinded;
    }
}
