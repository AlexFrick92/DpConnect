using DpConnect.Configuration;
using DpConnect.Connection;
using DpConnect.ExampleWorker.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class CreateTechParamViewModel : BaseViewModel
    {
        public CreateTechParamViewModel(IEnumerable<ConnectionViewModel> configuredConnections, IDpWorkerManager workerManager)
        {
            AvaibleConnections = configuredConnections;

            CreateConnectionCmd = new RelayCommand((arg) =>
            {
                //ConnectionCreated?.Invoke(this, EditConnection);
            });
            CancelCmd = new RelayCommand((arg) => CreatingCanceled?.Invoke(this, null));

            this.workerManager = workerManager;

        }

        IDpWorkerManager workerManager { get; set; }

        public event EventHandler<IDpConnectionConfiguration> ConnectionCreated;
        public event EventHandler<IDpConnectionConfiguration> CreatingCanceled;

        public ICommand CreateConnectionCmd { get; set; }
        public ICommand CancelCmd { get; set; }

        public List<string> AvaibleWorkers { get; set; } = new List<string> { "Простой тех. параметр", "Непростой тех. параметр"};

        string selectedWorkerType;
        public string SelectedWorkerType
        {
            get => selectedWorkerType;
            set
            {
                selectedWorkerType = value;
                switch
                    (selectedWorkerType)
                {
                    case "Простой тех. параметр":

                        workerManager.CreateWorker<TechParamReader>();


                        break;

                    default:
                        break;
                }
            }
        }

        //Конфигурация
        DpConfiguration dpConfig;


        public IEnumerable<ConnectionViewModel> AvaibleConnections { get; private set; }
        public ConnectionViewModel SelectedConnection { get; set; }

    }
}
