﻿using DpConnect.Connection;
using DpConnect.OpcUa;
using System;
using System.Collections.Generic;

using System.Windows.Input;

namespace DpConnect.Example.TechParamApp.ViewModel
{

    public class CreateConnectionViewModel : BaseViewModel
    {
        IDpConnectionManager dpConnectionManager;
        public CreateConnectionViewModel(IDpConnectionManager connectionManager, IEnumerable<Type> configurators)
        {
            dpConnectionManager = connectionManager;

            ConnectionsTypes = configurators;   


            CreateConnectionCmd = new RelayCommand((arg) => 
            {
                IDpConnectionConfiguration conConfig = ConnectionConfigurator.CreateConfiguration();

                connectionManager.CreateConnection(conConfig as dynamic);

                ConnectionCreated?.Invoke(this, null);
            });
            CancelCmd = new RelayCommand((arg) => CreatingCanceled?.Invoke(this, null));            
        }        

        public event EventHandler<IDpConnection> ConnectionCreated;
        public event EventHandler<IDpConnectionConfiguration> CreatingCanceled;                       
        
        public IEnumerable<Type> ConnectionsTypes { get; private set; }

        Type selectedConnectionType;
        public Type SelectedConnectionType
        {
            get => selectedConnectionType;
            set
            {
                selectedConnectionType = value;
                ConnectionConfigurator = (IConnectionConfiguratorViewModel)Activator.CreateInstance(value);
                OnPropertyChanged(nameof(ConnectionConfigurator));
                OnPropertyChanged(nameof(SelectedConnectionType));
            }
        }

        public IConnectionConfiguratorViewModel ConnectionConfigurator { get; private set; }

        public ICommand CreateConnectionCmd { get; set; }
        public ICommand CancelCmd { get; set; }

    }
}
