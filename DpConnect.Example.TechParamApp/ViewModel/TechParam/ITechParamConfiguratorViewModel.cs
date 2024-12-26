using DpConnect.Building;
using DpConnect.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public interface ITechParamConfiguratorViewModel
    {
        string ConfiguratorName { get; }

        IEnumerable<NamedDpConfigSettingViewModel> Settings { get; }        

        ITechParamViewModel CreateTechParameter(IDpWorkerManager workerManager);

        void SetConnection(IConnectionViewModel connection);
    }
}
