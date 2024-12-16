using DpConnect.Example.TechParamApp.View;
using DpConnect.OpcUa;
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
    public class CreateConnectionViewModel : BaseViewModel
    {
        public CreateConnectionViewModel()
        {
            CreateConnectionCmd = new RelayCommand((arg) => 
            {                                
                ConnectionCreated?.Invoke(this, EditConnection);
            });
            CancelCmd = new RelayCommand((arg) => CreatingCanceled?.Invoke(this, EditConnection));

            EditConnection = new OpcUaConnectionConfiguration();

            SelectedConnectionSettingsView =
                new DefaultConnectionSettingsView();

        }        

        public event EventHandler<IDpConnectionConfiguration> ConnectionCreated;
        public event EventHandler<IDpConnectionConfiguration> CreatingCanceled;        

        public IDpConnectionConfiguration EditConnection { get; set; }
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
                        EditConnection = new OpcUaConnectionConfiguration();

                        SelectedConnectionSettingsView =
                            new OpcUaConnectionConfigurationView(new OpcUaConnectionConfigurationViewModel(EditConnection as OpcUaConnectionConfiguration));
                        
                        OnPropertyChanged(nameof(SelectedConnectionSettingsView));
                        break;

                    default:
                        Console.WriteLine("gg");
                        SelectedConnectionSettingsView = new DefaultConnectionSettingsView();
                        OnPropertyChanged(nameof(SelectedConnectionSettingsView));
                        break;
                }
            }
        }

        public ICommand CreateConnectionCmd { get; set; }
        public ICommand CancelCmd { get; set; }

    }
}
