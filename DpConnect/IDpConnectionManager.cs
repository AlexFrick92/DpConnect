
using DpConnect.Connection;

namespace DpConnect
{
    public interface IDpConnectionManager
    {
        IDpConnection CreateConnection<T>(IDpConnectionConfiguration configuration) where T : IDpConnection;

        IDpConnection GetConnection(string Id);

        void OpenConnections();
    }
}
