using DpConnect.Interface;
using Promatis.Core;
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

        public ContainerizedConnectionManager(ILogger logger, IIoCContainer container)
        {
            this.logger = logger;
            this.container = container;
        }

        public IDpConnection CreateConnection<T>(IDpConnectionConfiguration configuration) where T : IDpConnection
        {            
            IDpConnection con = container.Resolve<T>();

            logger.Info($"Менеджер соединений: Создано новое подключение: {con.GetType()}");
            con.Configure(configuration);

            connections.Add(con);

            return con;
         
        }

        public IDpConnection GetConnection(string Id)
        {
            return connections.First(c => c.Id == Id);
        }

        public void OpenConnections()
        {
            connections.ForEach(c => c.Open());
        }
    }
}
