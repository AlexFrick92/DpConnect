using DpConnect.Building;
using DpConnect.Configuration;
using DpConnect.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public interface ISourceConfiguratorViewModel
    {
        IEnumerable<NamedDpConfigSettingViewModel> DpPropSettings { get; }
        IDpWorker Bind(IDpBinder binder, IDpWorkerManager workerManager);
    }
}
