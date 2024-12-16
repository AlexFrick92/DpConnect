using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class CreateWorkerViewModel : BaseViewModel
    {
        public CreateWorkerViewModel(IEnumerable<ConnectionViewModel> configuredConnections)
        {
            CreateConnectionCmd = new RelayCommand((arg) =>
            {
                //ConnectionCreated?.Invoke(this, EditConnection);
            });
            CancelCmd = new RelayCommand((arg) => CreatingCanceled?.Invoke(this, null));

        }
        public event EventHandler<IDpConnectionConfiguration> ConnectionCreated;
        public event EventHandler<IDpConnectionConfiguration> CreatingCanceled;

        public ICommand CreateConnectionCmd { get; set; }
        public ICommand CancelCmd { get; set; }
    }
}
