using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using DpConnect.Configuration;

using Promatis.Core.Logging;

namespace DpConnect.Building
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
        public void Bind(IDpWorker worker, IEnumerable<DpConfiguration> configs)
        {
            logger.Info($"Связываем {worker.GetType()}...");
            foreach (DpConfiguration config in configs)
            {
                PropertyInfo prop = null;
                try
                {
                    prop = worker.GetType().GetProperties().First(p => p.Name == config.PropertyName);
                }
                catch (InvalidOperationException ex)
                {
                    throw new DpConfigurationException($"В типе {worker.GetType()} не найдено публичное свойство {config.PropertyName}");
                }                

                var connection = connectionManager.GetConnection(config.ConnectionId);

                if (prop.PropertyType.GetGenericTypeDefinition() == typeof(IDpValue<>))
                {
                    object dp = CreateDpValue(prop);
                    connection.ConnectDpValue(dp as dynamic, config.SourceConfiguration);
                    prop.SetValue(worker, dp);
                    logger.Info($"Свойство {config.PropertyName} типа {dp.GetType()} для {config.ConnectionId}");

                }
                else if (prop.PropertyType.GetGenericTypeDefinition() == typeof(IDpAction<>))
                {
                    object dp = CreatDpFunc(prop);
                    connection.ConnectDpMethod(dp as dynamic, config.SourceConfiguration);
                    prop.SetValue(worker, dp);
                    logger.Info($"Метод {config.PropertyName} типа {dp.GetType()} для {config.ConnectionId}");
                }
                else
                {
                    string messageError = $"Ошибка при связывании воркера. Свойство {prop.Name} : {prop.PropertyType} воркера должно быть одним из следующих типов: {typeof(IDpValue<>)}, {typeof(IDpAction<>)}";
                    logger.Error(messageError);
                    throw new DpConfigurationException(messageError);
                }
                                    
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
        object CreatDpFunc(PropertyInfo property)
        {
            Type[] propGenericType = property.PropertyType.GetGenericArguments();
            Type genericType = typeof(DpAction<>).MakeGenericType(propGenericType);            
            
            return Activator.CreateInstance(genericType);


        }
    }
}
