using DpConnect.Example.TechParamApp.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class CreateConnectionViewModel : INotifyPropertyChanged
    {


        public CreateConnectionViewModel()
        {
            CreateConnectionCmd = new RelayCommand((arg) => 
            {
                ConnectionCreated?.Invoke(this, EditConnection);
            });
            CancelCmd = new RelayCommand((arg) => CreatingCanceled?.Invoke(this, EditConnection));
        }

        public  ConnectionViewModel EditConnection { get; set; } = new ConnectionViewModel();

        public event EventHandler<ConnectionViewModel> ConnectionCreated;
        public event EventHandler<ConnectionViewModel> CreatingCanceled;
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public UIElement SelectedConnectionSettingsView { get; set; }

        public List<string> ConnectionsTypes { get; set; } = new List<string>()
        {
            "Не выбрано" , "OpcUa", "Modbus"
        };

        string selectedConnectionType;
        public string SelectedConnectionType
        {
            get => selectedConnectionType;
            set
            {
                selectedConnectionType = value;
                switch
                    (selectedConnectionType)
                {
                    case "OpcUa":

                        SelectedConnectionSettingsView =
                            new OpcUaConnectionSettingsView(new OpcUaConnectionSettingsViewModel());
                        OnPropertyChanged(nameof(SelectedConnectionSettingsView));
                        break;
                    default:
                        SelectedConnectionSettingsView = null;
                        OnPropertyChanged(nameof(SelectedConnectionSettingsView));
                        break;
                }
            }
        }

        public ICommand CreateConnectionCmd { get; set; }
        public ICommand CancelCmd { get; set; }

    }
}
