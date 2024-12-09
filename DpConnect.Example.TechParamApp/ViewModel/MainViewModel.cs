using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using DpConnect.Example.TechParamApp.View;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        IDpConnectionManager connectionManager;

        public MainViewModel(IDpConnectionManager conManager)
        {
            connectionManager = conManager;

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
                    //ConfiguredConnections.Add(v);
                    createConnectionView.Close();
                };
                createConnectionViewModel.CreatingCanceled += (s, v) => createConnectionView.Close();                
                createConnectionView.ShowDialog();
                
            });

            AddTechParamCmd = new RelayCommand((arg) =>
            {
                Console.WriteLine("Добавить тех. параметр");
            });
        }
        public ICommand AddConnectionCmd { get; private set; }
        public ICommand AddTechParamCmd  { get; private set; }

        public ObservableCollection<ConnectionViewModel> ConfiguredConnections { get; private set; } =
            new ObservableCollection<ConnectionViewModel>();

    }
}
