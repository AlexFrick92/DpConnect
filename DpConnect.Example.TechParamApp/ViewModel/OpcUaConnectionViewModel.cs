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
            dpConnection = connection;
        }

        public string ConnectionName { get => dpConnection.Id; }

        public string ConnectionType { get => dpConnection.GetType().ToString(); }        

        IDpConnection dpConnection;

        public void CreateSourceConfigurators(ITechParameterConfiguratorViewModel techParamConfigurator)
        {
            foreach(var setting in techParamConfigurator.Settings)
            {
                setting.SourceConfigurator = new OpcUaSourceConfiguratorViewModel();
            }
        }
    }
}
