using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using DpConnect.Configuration;
using DpConnect.Interface;

namespace DpConnect
{
    public class DpBinder : IDpBinder
    {

        IDpConnectionManager connectionManager;
        public DpBinder(IDpConnectionManager dpConnectionManager)
        {
            connectionManager = dpConnectionManager;
        }


        //Создать точку такого типа, который имеет свойство воркера
        public void Bind(IDpWorker worker, IEnumerable<DpValueConfiguration> configs)
        {
            foreach (DpValueConfiguration config in configs)
            {
                //Здесь мы создадим типизированную точку

                PropertyInfo prop = worker.GetType().GetProperties().First(p => p.Name == config.PropertyName);

                dynamic dp = CreateDpValue(prop);
                     
                var connection = connectionManager.GetConnection(config.ConnectionId);

                connection.ConnectDpValue(dp, config.SourceConfiguration);

                prop.SetValue(worker, dp);

            }

            worker.DpBound();
        }

        dynamic CreateDpValue(PropertyInfo property)
        {
            Type[] propGenericType = property.PropertyType.GetGenericArguments();
            Type genericType = typeof(DpValue<>).MakeGenericType(propGenericType);
            return Activator.CreateInstance(genericType);
        }
    }
}
