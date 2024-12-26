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
    public class OpcUaConnectionViewModel : BaseViewModel, IConnectionViewModel
    {
        IDpBinder dpBinder;

        OpcUaConnection OpcUaConnection;
        public OpcUaConnectionViewModel(OpcUaConnection connection, IDpBinder binder)
        {
            DpConnection = connection;
            OpcUaConnection = connection;
            this.dpBinder = binder;
        }

        public string ConnectionName { get => DpConnection.Id; }

        public string ConnectionType { get => DpConnection.GetType().ToString(); }        

        public IDpConnection DpConnection { get; private set; }

        public ISourceConfiguratorViewModel SourceConfigurator => new OpcUaSourceConfiguratorViewModel();

        public void BindProperties(IDpWorker worker, IEnumerable<IDpConfiguration> configs)
        {
            dpBinder.Bind(worker, OpcUaConnection, configs.OfType<DpConfiguration<OpcUaDpValueSourceConfiguration>>());
        }
    }
}
