
using DpConnect.Connection;
using System;

using System.Collections.Generic;

namespace DpConnect
{
    public interface IDpConnectionManager
    {
        /// <summary>
        /// Создать соединение на основе конфигурации
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>        
        IDpConnection CreateConnection<TConfigurableConnection, TConnectionConfig>(TConnectionConfig configuration)
            where TConfigurableConnection : IDpConfigurableConnection<TConnectionConfig>
            where TConnectionConfig : IDpConnectionConfiguration;

        /// <summary>
        /// Получить соединение из коллекции соединений
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        IDpConnection GetConnection(string Id);

        IEnumerable<IDpConnection> ConfiguredConnections { get; }

        /// <summary>
        /// Вызывается, когда создано новое соединение
        /// </summary>
        event EventHandler<IDpConnection> NewConnectionCreated;        


        /// <summary>
        /// Открыть все соединения в коллекции
        /// </summary>
        void OpenConnections();

        /// <summary>
        /// Закрыть все соединения в коллекции
        /// </summary>
        void CloseConnections();
    }
}
