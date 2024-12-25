using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public interface IConnectionConfigurationViewModel
    {
        IEnumerable<NamedConfigParamViewModel> Parameters { get; }

        string ConnectionName { get; }        

        void CreateConnection(IDpConnectionManager connectionManager);
    }
}
