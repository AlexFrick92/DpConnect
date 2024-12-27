using DpConnect.Configuration;
using DpConnect.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public interface IConnectionViewModel
    {
        string ConnectionName { get; }

        string ConnectionType { get; }        

        IDpConnection DpConnection { get; }
    }
}
