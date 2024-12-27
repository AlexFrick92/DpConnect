using DpConnect.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel.Connection
{
    internal interface IConnectionConfigurator<TConnection, TConfig>
        where TConfig : IDpConnectionConfiguration
        where TConnection : IDpConfigurableConnection<TConfig>
    {

    }
}
