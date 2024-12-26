﻿using DpConnect.Connection;
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
        public CreateConnectionViewModel(IDpConnectionManager connectionManager)
        {
            dpConnectionManager = connectionManager;

            //ConnectionsTypes = avaibleConnectionsTypes;   

            ConnectionsTypes = new List<IConnectionConfigurationViewModel>() { new OpcUaConnectionConfigurationViewModel()};

            CreateConnectionCmd = new RelayCommand((arg) => 
            {

                SelectedConnectionType.CreateConnection(connectionManager);

                ConnectionCreated?.Invoke(this, null);
            });
            CancelCmd = new RelayCommand((arg) => CreatingCanceled?.Invoke(this, null));            
        }        

        public event EventHandler<IDpConnection> ConnectionCreated;
        public event EventHandler<IDpConnectionConfiguration> CreatingCanceled;                       

        public IEnumerable<NamedConfigSettingViewModel> ConfigSettings { get; set; }
        public IEnumerable<IConnectionConfigurationViewModel> ConnectionsTypes { get; private set; }

        IConnectionConfigurationViewModel selectedConnectionType;
        public IConnectionConfigurationViewModel SelectedConnectionType
        {
            get => selectedConnectionType;
            set
            {
                selectedConnectionType = value;      
                ConfigSettings = value.Settings;
                OnPropertyChanged(nameof(ConfigSettings));
                OnPropertyChanged(nameof(SelectedConnectionType));
            }
        }

        public ICommand CreateConnectionCmd { get; set; }
        public ICommand CancelCmd { get; set; }

    }
}
