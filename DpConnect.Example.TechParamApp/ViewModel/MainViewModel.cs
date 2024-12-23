using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DpConnect.Building;
using DpConnect.Example.TechParamApp.View;
using DpConnect.ExampleWorker;
using DpConnect.OpcUa;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        IDpConnectionManager connectionManager;
        IDpWorkerManager workerManager;
        IDpBinder binder;
        IDpBuilder dpBuilder;


        public MainViewModel(IDpConnectionManager conManager, IDpWorkerManager workerManager, IDpBuilder builder, IDpBinder binder)
        {
            connectionManager = conManager;
            this.workerManager = workerManager;
            this.dpBuilder = builder;
            this.binder = binder;


            workerManager.WorkerCreated += (s, w) => ConfiguredWorkers.Add(new WorkerViewModel(w));


            foreach(var con in connectionManager.ConfiguredConnections)
            {
                ConfiguredConnections.Add(new ConnectionViewModel(con));
            }

            AddConnectionCmd = new RelayCommand((arg) =>
            {
                Console.WriteLine("Добавить соединение");                

                CreateConnectionViewModel createConnectionViewModel = new CreateConnectionViewModel();
                CreateConnectionView createConnectionView = new CreateConnectionView(createConnectionViewModel);
                createConnectionViewModel.ConnectionCreated += (s, v) =>
                {
                    //Тут нужно создать соединение через билдер

                    var con = conManager.CreateConnection<IOpcUaConnection>(v);
                    ConfiguredConnections.Add(new ConnectionViewModel(con));
                    createConnectionView.Close();
                };
                createConnectionViewModel.CreatingCanceled += (s, v) => createConnectionView.Close();                
                createConnectionView.ShowDialog();
                
            });

            AddTechParamCmd = new RelayCommand((arg) =>
            {
                Console.WriteLine("Добавить тех. параметр");

                CreateTechParamViewModel createWorkerViewModel = new CreateTechParamViewModel(ConfiguredConnections, workerManager, binder);
                CreateTechParamView createWorkerView = new CreateTechParamView(createWorkerViewModel);

                createWorkerViewModel.CreatingCanceled += (s, v) => createWorkerView.Close();
                createWorkerViewModel.WorkerCreated += (s, v) =>
                {
                    TechParamWorkers.Add(new TechParamWorkerViewModel(v));
                    createWorkerView.Close();
                };

                createWorkerView.ShowDialog();
                

            });

            OpenConnectionsCmd = new RelayCommand((arg) =>
            {
                Console.WriteLine("Открываем соединения...");
                Task.Run(() => connectionManager.OpenConnections());
                
            });
        }
        public ICommand AddConnectionCmd { get; private set; }
        public ICommand AddTechParamCmd  { get; private set; }
        public ICommand OpenConnectionsCmd { get; private set; }
        

        public ObservableCollection<WorkerViewModel> ConfiguredWorkers { get; private set; } = new ObservableCollection<WorkerViewModel>();
        public ObservableCollection<TechParamWorkerViewModel> TechParamWorkers { get; private set; } = new ObservableCollection<TechParamWorkerViewModel>();
        public ObservableCollection<ConnectionViewModel> ConfiguredConnections { get; private set; } = new ObservableCollection<ConnectionViewModel>();

    }
}
