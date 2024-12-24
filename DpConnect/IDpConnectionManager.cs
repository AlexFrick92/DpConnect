
using DpConnect.Connection;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DpConnect
{
    public interface IDpConnectionManager
    {
        IDpConnection CreateConnection<T>(IDpConnectionConfiguration configuration) where T : IDpConnection;        

        IDpConnection GetConnection(string Id);

        IEnumerable<IDpConnection> ConfiguredConnections { get; }

        event EventHandler<IDpConnection> NewConnectionCreated;        

        void OpenConnections();

        void CloseConnections();
    }
}
