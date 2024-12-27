using DpConnect.Building;
using DpConnect.ExampleWorker.Console;
using DpConnect.OpcUa;
using System;
using System.Collections.Generic;
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
                SourceConfigurator.Bind(binder, workerManager);

                WorkerCreated?.Invoke(this, null);
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

        public List<string> AvaibleTechParamConfigurators { get; set; } = new List<string>() { "FloatTechParam" };

        string selectedConfigurator;
        public string SelectedTechParamConfigurator
        {
            get => selectedConfigurator;
            set
            {
                selectedConfigurator = value;
                OnPropertyChanged(nameof(SelectedTechParamConfigurator));
            }
        }
        public IEnumerable<IConnectionViewModel> AvaibleConnections { get; private set; }

        IConnectionViewModel selectedConnection;
        public IConnectionViewModel SelectedConnection 
        {
            get => selectedConnection;            
            set
            {
                selectedConnection = value;

                switch(selectedConfigurator)
                {
                    case "FloatTechParam":
                        SourceConfigurator = new OpcUaSourceConfiguratorViewModel<TechParamReader>(value as OpcUaConnection);
                        OnPropertyChanged(nameof(SourceConfigurator));
                        Console.WriteLine("Выбрали");
                        break;
                }
            }
        }    
        
        public ISourceConfiguratorViewModel SourceConfigurator { get; set; }    
        

    }
}
