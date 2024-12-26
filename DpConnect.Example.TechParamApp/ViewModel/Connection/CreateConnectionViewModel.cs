﻿using DpConnect.Connection;

using System;
using System.Collections.Generic;

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

            ConnectionsTypes = new List<IConnectionConfiguratorViewModel>() { new OpcUaConnectionConfiguratorViewModel()};

            CreateConnectionCmd = new RelayCommand((arg) => 
            {

                SelectedConnectionType.CreateConnection(connectionManager);

                ConnectionCreated?.Invoke(this, null);
            });
            CancelCmd = new RelayCommand((arg) => CreatingCanceled?.Invoke(this, null));            
        }        

        public event EventHandler<IDpConnection> ConnectionCreated;
        public event EventHandler<IDpConnectionConfiguration> CreatingCanceled;                       
        
        public IEnumerable<IConnectionConfiguratorViewModel> ConnectionsTypes { get; private set; }

        IConnectionConfiguratorViewModel selectedConnectionType;
        public IConnectionConfiguratorViewModel SelectedConnectionType
        {
            get => selectedConnectionType;
            set
            {
                selectedConnectionType = value;                      
                OnPropertyChanged(nameof(SelectedConnectionType));
            }
        }

        public ICommand CreateConnectionCmd { get; set; }
        public ICommand CancelCmd { get; set; }

    }
}