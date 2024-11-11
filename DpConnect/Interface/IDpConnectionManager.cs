using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Interface
{
    public interface IDpConnectionManager
    {
        IDpConnection CreateConnection<T>(IDpConnectionConfiguration configuration) where T : IDpConnection;

        IDpConnection GetConnection(string Id);

        void OpenConnections();
    }
}
