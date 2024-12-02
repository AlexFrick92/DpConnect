using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class CreateConnectionViewModel
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

        public ICommand CreateConnectionCmd { get; set; }
        public ICommand CancelCmd { get; set; }

    }
}
