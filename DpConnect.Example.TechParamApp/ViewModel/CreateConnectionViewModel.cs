using DpConnect.Connection;
using DpConnect.Example.TechParamApp.View;
using DpConnect.OpcUa;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DpConnect.Example.TechParamApp.ViewModel
{

    public class CreateConnectionViewModel : BaseViewModel
    {
        IDpConnectionManager dpConnectionManager;
        public CreateConnectionViewModel(IDpConnectionManager connectionManager, IEnumerable<IConnectionConfigurationView> avaibleConnectionsTypes)
        {
            dpConnectionManager = connectionManager;

            ConnectionsTypes = avaibleConnectionsTypes;   

            CreateConnectionCmd = new RelayCommand((arg) => 
            {

                IDpConnection con = dpConnectionManager.CreateConnection(selectedConnectionType.Configuration);                

                ConnectionCreated?.Invoke(this, con);
            });
            CancelCmd = new RelayCommand((arg) => CreatingCanceled?.Invoke(this, selectedConnectionType.Configuration));

            SelectedConnectionSettingsView = new DefaultConnectionSettingsView();
        }        

        public event EventHandler<IDpConnection> ConnectionCreated;
        public event EventHandler<IDpConnectionConfiguration> CreatingCanceled;        
        
        public UIElement SelectedConnectionSettingsView { get; set; }

        public IEnumerable<IConnectionConfigurationView> ConnectionsTypes { get; private set; }

        IConnectionConfigurationView selectedConnectionType;
        public IConnectionConfigurationView SelectedConnectionType
        {
            get => selectedConnectionType;
            set
            {
                selectedConnectionType = value;
                SelectedConnectionSettingsView = value.View;
                OnPropertyChanged(nameof(SelectedConnectionSettingsView));
                OnPropertyChanged(nameof(SelectedConnectionType));
            }
        }

        public ICommand CreateConnectionCmd { get; set; }
        public ICommand CancelCmd { get; set; }

    }
}
