using DpConnect;
using DpConnect.Connection;

using Promatis.Core;
using Promatis.Core.Extensions;
using Promatis.Core.Logging;

using System;
using System.Collections.Generic;
using System.Linq;

namespace DpConnect
{
    public class ContainerizedConnectionManager : IDpConnectionManager
    {
        ILogger logger;
        IIoCContainer container;
        List<IDpConnection> connections = new List<IDpConnection>();

        public IEnumerable<IDpConnection> ConfiguredConnections => connections;

        public event EventHandler<IDpConnection> NewConnectionCreated;

        public ContainerizedConnectionManager(ILogger logger, IIoCContainer container)
        {
            this.logger = logger;
            this.container = container;
        }



        public IDpConnection CreateConnection<T>(IDpConnectionConfiguration configuration) where T : IDpConnection
        {            
            IDpConnection con = container.Resolve<T>();            

            logger.Info($"Менеджер соединений: Создано новое подключение: {configuration.ConnectionId} с типом {con.GetType()}");
            con.Configure(configuration);

            connections.Add(con);

            NewConnectionCreated?.Invoke(this, con);

            return con;
         
        }

        public IDpConnection GetConnection(string Id)
        {
            return connections.First(c => c.Id == Id);
        }

        public void OpenConnections()
        {
            logger.Info("Открываем соединения...");
            connections.Where(c => c.Active).ForEach(c => c.Open());
            logger.Info("Соединения открыты.");
        }

        public void CloseConnections()
        {
            logger.Info("Закрываем соединения...");
            connections.Where(c => c.Active).ForEach(c => c.Close());
            logger.Info("Соединения закрыты.");
        }
    }
}
