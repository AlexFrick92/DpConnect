using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using DpConnect.Configuration;
using DpConnect.Interface;
using Promatis.Core.Logging;

namespace DpConnect
{
    internal class DpBinder : IDpBinder
    {

        readonly IDpConnectionManager connectionManager;
        readonly ILogger logger;


        public DpBinder(IDpConnectionManager connectionManager, ILogger logger)
        {
            this.connectionManager = connectionManager;
            this.logger = logger;
        }


        //Создать точку такого типа, который имеет свойство воркера
        public void Bind(IDpWorker worker, IEnumerable<DpValueConfiguration> configs)
        {
            logger.Info($"Связываем {worker.GetType()}...");
            foreach (DpValueConfiguration config in configs)
            {

                PropertyInfo prop = worker.GetType().GetProperties().First(p => p.Name == config.PropertyName);

                object dp = CreateDpValue(prop);
                     
                var connection = connectionManager.GetConnection(config.ConnectionId);

                connection.ConnectDpValue(dp as dynamic, config.SourceConfiguration);

                prop.SetValue(worker, dp);

                logger.Info($"Свойство {config.PropertyName} типа {dp.GetType()} для {config.ConnectionId}");
            }

            worker.DpBound();
            logger.Info($"{worker.GetType()} связан.");
        }

        object CreateDpValue(PropertyInfo property)
        {
            Type[] propGenericType = property.PropertyType.GetGenericArguments();
            Type genericType = typeof(DpValue<>).MakeGenericType(propGenericType);
            return Activator.CreateInstance(genericType);
        }
    }
}
