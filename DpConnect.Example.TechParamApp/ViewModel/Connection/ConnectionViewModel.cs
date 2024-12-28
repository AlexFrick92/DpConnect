using DpConnect.Building;
using DpConnect.Configuration;
using DpConnect.Connection;
using DpConnect.OpcUa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class ConnectionViewModel : BaseViewModel, IConnectionViewModel
    {

        OpcUaConnection OpcUaConnection;
        public ConnectionViewModel(OpcUaConnection connection)
        {
            DpConnection = connection;
            OpcUaConnection = connection;
        }

        public string ConnectionName { get => DpConnection.Id; }

        public string ConnectionType { get => DpConnection.GetType().ToString(); }        

        public IDpConnection DpConnection { get; private set; }        

    }
}
