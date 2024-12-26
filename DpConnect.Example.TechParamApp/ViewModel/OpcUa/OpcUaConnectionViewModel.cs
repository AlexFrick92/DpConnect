using DpConnect.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class OpcUaConnectionViewModel : BaseViewModel, IConnectionViewModel
    {
        
        public OpcUaConnectionViewModel(IDpConnection connection)
        {
            DpConnection = connection;
        }

        public string ConnectionName { get => DpConnection.Id; }

        public string ConnectionType { get => DpConnection.GetType().ToString(); }        

        public IDpConnection DpConnection { get; private set; }

        public void CreateSourceConfigurators(ITechParamConfiguratorViewModel techParamConfigurator)
        {
            foreach(var setting in techParamConfigurator.Settings)
            {
                setting.SourceConfigurator = new OpcUaSourceConfiguratorViewModel();
            }
        }
    }
}
