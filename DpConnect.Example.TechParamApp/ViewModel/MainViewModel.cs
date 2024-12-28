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
using DpConnect.Connection;
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

        List<Type> ConnectionTypes = new List<Type> { typeof(IDpConfigurableConnection<OpcUaConnectionConfiguration>) }; //Допустим, это мы загрузили сборки сюда
        //Теперь с каждым соединением нужно ассоциировать конфигуратор
        //Для этого, при создании конфигуратора, мы укажем через обобщение, что он реализует интерфейс IConfigurator<>
        List<Type> ConnectionConfiguratorsType = new List<Type>() { typeof(OpcUaConnectionConfiguratorViewModel) };
        //Если мы хотим динамически менять конфигуратор и соединения, то создаваться они тоже будут динамически. 
        //Либо через активатор, либо через контейнер
        //если бы мы загружали их вручную, то можно было бы явно указать и создать нужные конфигураторы

        public MainViewModel(IDpConnectionManager conManager, IDpWorkerManager workerManager, IDpBuilder builder, IDpBinder binder)
        {
            connectionManager = conManager;
            this.workerManager = workerManager;
            this.dpBuilder = builder;
            this.binder = binder;


            workerManager.WorkerCreated += (s, w) =>
            {                
                ConfiguredWorkers.Add(new WorkerViewModel(w));
            };
            connectionManager.NewConnectionCreated += (s, c) => ConfiguredConnections.Add(new ConnectionViewModel(c as OpcUaConnection));


            foreach(var con in connectionManager.ConfiguredConnections)
            {
                ConfiguredConnections.Add(new ConnectionViewModel(con as OpcUaConnection));
            }

            AddConnectionCmd = new RelayCommand((arg) =>
            {
                Console.WriteLine("Добавить соединение");

                CreateConnectionViewModel createConnectionViewModel = new CreateConnectionViewModel(conManager, ConnectionConfiguratorsType);
                


                CreateConnectionView createConnectionView = new CreateConnectionView(createConnectionViewModel);
                createConnectionViewModel.ConnectionCreated += (s, v) =>
                {
                    createConnectionView.Close();
                };
                createConnectionViewModel.CreatingCanceled += (s, v) => createConnectionView.Close();                
                createConnectionView.ShowDialog();
                
            });

            AddTechParamCmd = new RelayCommand((arg) =>
            {
                Console.WriteLine("Добавить тех. параметр");

                CreateWorkerViewModel createWorkerViewModel = new CreateWorkerViewModel(ConfiguredConnections, workerManager, binder);
                CreateTechParamView createWorkerView = new CreateTechParamView(createWorkerViewModel);

                createWorkerViewModel.CreatingCanceled += (s, v) => createWorkerView.Close();
                createWorkerViewModel.TechParamCreated += (s, v) =>
                {
                    TechParamWorkers.Add(v);
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
        public ObservableCollection<ITechParamViewModel> TechParamWorkers { get; private set; } = new ObservableCollection<ITechParamViewModel>();
        public ObservableCollection<IConnectionViewModel> ConfiguredConnections { get; private set; } = new ObservableCollection<IConnectionViewModel>();

    }
}
