using DpConnect.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class ConnectionViewModel : BaseViewModel
    {
        
        public ConnectionViewModel(IDpConnection connection)
        {
            dpConnection = connection;
        }

        public string ConnectionName { get => dpConnection.Id; }

        public string ConnectionType { get => dpConnection.GetType().ToString(); }

        public Type ConnectionTypeType { get => dpConnection.GetType(); }

        IDpConnection dpConnection;
    }
}
