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
        IEnumerable<NamedConfigSettingViewModel> Settings { get; }

        IDpConfiguration CreateConfiguration(string dpName);

        void BindProperties(IDpWorker worker, IEnumerable<IDpConfiguration> configs, IDpBinder binder, IDpConnection connection);

    }
}
