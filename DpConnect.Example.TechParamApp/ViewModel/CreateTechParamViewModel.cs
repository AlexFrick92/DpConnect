﻿using DpConnect.Building;
using DpConnect.Configuration;
using DpConnect.Connection;
using DpConnect.Example.TechParamApp.View;
using DpConnect.ExampleWorker;
using DpConnect.ExampleWorker.Console;
using DpConnect.OpcUa;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class CreateTechParamViewModel : BaseViewModel
    {
        public CreateTechParamViewModel(IEnumerable<IConnectionViewModel> configuredConnections, IDpWorkerManager workerManager, IDpBinder binder)
        {
            AvaibleConnections = configuredConnections;

            CreateWorkerCmd = new RelayCommand((arg) =>
            {
                IDpWorker techParamWorker = workerManager.CreateWorker<TechParamReader>();

                ITechParamViewModel techParam = SelectedTechParamConfigurator.CreateTechParameter(binder, workerManager, selectedConnection.DpConnection);


                WorkerCreated?.Invoke(this, techParam);
            });
            CancelCmd = new RelayCommand((arg) => CreatingCanceled?.Invoke(this, null));

            this.workerManager = workerManager;
            this.binder = binder;

        }

        IDpWorkerManager workerManager;
        IDpBinder binder;

        public event EventHandler<ITechParamViewModel> WorkerCreated;
        public event EventHandler<IDpConnectionConfiguration> CreatingCanceled;

        public ICommand CreateWorkerCmd { get; set; }
        public ICommand CancelCmd { get; set; }

        public List<ITechParameterConfiguratorViewModel> AvaibleTechParamConfigurators { get; set; } = new List<ITechParameterConfiguratorViewModel>() { new TechParamReaderConfiguratorViewModel() };

        ITechParameterConfiguratorViewModel selectedConfigurator;
        public ITechParameterConfiguratorViewModel SelectedTechParamConfigurator
        {
            get => selectedConfigurator;
            set
            {
                selectedConfigurator = value;
                OnPropertyChanged(nameof(SelectedTechParamConfigurator));
            }
        }



        IConnectionViewModel selectedConnection;
        public IConnectionViewModel SelectedConnection 
        {
            get => selectedConnection;            
            set
            {
                selectedConnection = value;
                selectedConnection.CreateSourceConfigurators(SelectedTechParamConfigurator);
            }
        }        
        public IEnumerable<IConnectionViewModel> AvaibleConnections { get; private set; }
        

    }
}
