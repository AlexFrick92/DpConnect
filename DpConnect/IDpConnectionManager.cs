
using DpConnect.Connection;
using System;

namespace DpConnect
{
    public interface IDpConnectionManager
    {
        IDpConnection CreateConnection<T>(IDpConnectionConfiguration configuration) where T : IDpConnection;

        IDpConnection GetConnection(string Id);

        event EventHandler<IDpConnection> NewConnectionCreated;        

        void OpenConnections();

        void CloseConnections();
    }
}
