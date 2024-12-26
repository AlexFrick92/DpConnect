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
        public CreateTechParamViewModel(IEnumerable<ConnectionViewModel> configuredConnections, IDpWorkerManager workerManager, IDpBinder binder)
        {
            AvaibleConnections = configuredConnections;

            CreateWorkerCmd = new RelayCommand((arg) =>
            {
                IDpWorker techParamWorker = workerManager.CreateWorker<TechParamReader>();

                //dpConfig = new DpConfiguration()
                //{
                //    ConnectionId = SelectedConnection.ConnectionTypeName,
                //    PropertyName = "TechParam",
                //    SourceConfiguration = dpSourceConfiguration
                //};

                //binder.Bind(techParamWorker, new DpConfiguration[] { dpConfig });


                WorkerCreated?.Invoke(this, techParamWorker as ITechParamWorker);
            });
            CancelCmd = new RelayCommand((arg) => CreatingCanceled?.Invoke(this, null));

            this.workerManager = workerManager;
            this.binder = binder;

        }

        IDpWorkerManager workerManager;
        IDpBinder binder;

        public event EventHandler<ITechParamWorker> WorkerCreated;
        public event EventHandler<IDpConnectionConfiguration> CreatingCanceled;

        public ICommand CreateWorkerCmd { get; set; }
        public ICommand CancelCmd { get; set; }

        public List<ITechParameterConfiguratorViewModel> AvaibleTechParamConfigurators { get; set; } = new List<ITechParameterConfiguratorViewModel>() { new TechParamReaderConfigViewModel() };

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



        ConnectionViewModel selectedConnection;
        public ConnectionViewModel SelectedConnection 
        {
            get => selectedConnection;            
            set
            {
                selectedConnection = value;

                var opcUaConfigSourceVm = new OpcUaConfigSourceViewModel();
                //dpSourceConfiguration = opcUaConfigSourceVm.sourceConfiguration;

                SelectedConnectionSourceConfig = new OpcUaConfigSourceView(opcUaConfigSourceVm);
                OnPropertyChanged(nameof(SelectedConnectionSourceConfig));
            }
        }

        public UIElement SelectedConnectionSourceConfig { get; set; }
        public IEnumerable<ConnectionViewModel> AvaibleConnections { get; private set; }
        

    }
}
