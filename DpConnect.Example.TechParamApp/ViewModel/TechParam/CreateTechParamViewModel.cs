using DpConnect.Building;

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
                ITechParamViewModel techParam = SelectedTechParamConfigurator.CreateTechParameter(workerManager);

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

        public List<ITechParamConfiguratorViewModel> AvaibleTechParamConfigurators { get; set; } = new List<ITechParamConfiguratorViewModel>() { new TechParamReaderConfiguratorViewModel() };

        ITechParamConfiguratorViewModel selectedConfigurator;
        public ITechParamConfiguratorViewModel SelectedTechParamConfigurator
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
                SelectedTechParamConfigurator.SetConnection(value);
            }
        }        
        public IEnumerable<IConnectionViewModel> AvaibleConnections { get; private set; }
        

    }
}
