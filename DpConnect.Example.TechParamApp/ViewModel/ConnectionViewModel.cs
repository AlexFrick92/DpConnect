using DpConnect.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class ConnectionViewModel
    {
        public ConnectionViewModel()
        {
            
        }

        public ConnectionViewModel(IDpConnection connection)
        {
            ConnectionName = connection.Id;
            ConnectionType = connection.GetType().ToString();
        }

        public string ConnectionName { get; set; }

        public string ConnectionType { get; set; }

        public IDpConnectionConfiguration ConnectionConfiguration { get; set; }
    }
}
