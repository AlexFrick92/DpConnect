using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public interface IConnectionConfiguratorViewModel
    {
        IEnumerable<NamedConfigSettingViewModel> Settings { get; }

        string ConnectionTypeName { get; }        

        void CreateConnection(IDpConnectionManager connectionManager);
    }
}
