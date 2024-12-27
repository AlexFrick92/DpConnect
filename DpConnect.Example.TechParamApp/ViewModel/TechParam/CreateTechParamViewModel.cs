using DpConnect.Building;
using DpConnect.Example.TechParamApp.ViewModel.TechParam;
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
            });
            CancelCmd = new RelayCommand((arg) => CreatingCanceled?.Invoke(this, null));

            this.workerManager = workerManager;
            this.binder = binder;

        }

        IDpWorkerManager workerManager;
        IDpBinder binder;

        public event EventHandler<ITechParamViewModel> TechParamCreated;
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
                        SourceConfigurator = new OpcUaSourceConfiguratorViewModel<TechParamReader>(value.DpConnection as OpcUaConnection);

                        //Здесь нужно создать ViewModel для ридера
                        //так как воркер не может реализовать интерфейс ITechParamViewModel, мы должен создать для него свой Vm.

                        (SourceConfigurator as ISourceConfiguratorBinder<TechParamReader>).WorkerBinded += (s, w) =>
                        {
                            TechParamCreated?.Invoke(this, new TechParamReaderViewModel(w));
                        };
                        

                        //Здесь мы создаем OpcUaSourceConfig с параметром TechParamReader. Здесь же мы и должны создать к нему TechParamReaderViewModel
                        //

                        OnPropertyChanged(nameof(SourceConfigurator));
                        Console.WriteLine("Выбрали");
                        break;
                }
            }
        }    
        
        public ISourceConfiguratorViewModel SourceConfigurator { get; set; }            
    }
}
