using System;

using System.Collections.Generic;

using Promatis.Opc.UA.Client;
using Promatis.Core.Logging;

using DpConnect.Connection;

using DpConnect.Configuration.Xml;

namespace DpConnect.OpcUa
{
    public class OpcUaConnection : IOpcUaConnection
    {
        Client client;
        ILogger logger;
        OpcUaConnectionConfiguration connectionConfiguration;

        IList<object> nodes = new List<object>();

        IList<IDpStatus> dpValuesStatus = new List<IDpStatus>();

        public string Id { get; private set; }

        public OpcUaConnection(ILogger logger)
        {
            this.logger = logger;
        }

        public void Configure(IDpConnectionConfiguration configuration)
        {
            Id = configuration.ConnectionId;

            if (configuration is OpcUaConnectionConfiguration)
                connectionConfiguration = (OpcUaConnectionConfiguration)configuration;

            else if (configuration is DpConnectionXmlConfiguration)
                connectionConfiguration = new OpcUaConnectionXmlConfiguration((DpConnectionXmlConfiguration)configuration);

            else
                throw new ArgumentException($"{nameof(OpcUaConnection)}: Неверный тип конфигурации соединения");

            logger.Info($"{nameof(OpcUaConnection)}: Соединение {connectionConfiguration.ConnectionId} законфигурировано: {connectionConfiguration.Endpoint}");
        }
        
        public void Open()
        {
            logger.Info($"{Id}: Запуск...");

            if (client != null && client.IsConnected)
            {
                logger.Info("Клиент уже запущен!");
                return;
            }

            try
            {
                logger.Info("Подключение к " + connectionConfiguration.Endpoint);
                if (client == null)
                    client = new Client(connectionConfiguration.Endpoint, logger);

                client.Start();

                logger.Info($"{Id}: Запустился!, конфигурируем точки...");

                foreach (var node in nodes)
                {
                    client.Subscription(node as dynamic);                    
                }
                foreach (var status in dpValuesStatus)
                {
                    status.IsConnected = true;
                }
                logger.Info($"{Id}: Точки законфигурированы.");

            }
            catch (Exception ex)
            {
                logger.Info($"{Id}: Не удалось запустить! {ex.Message}");
                throw;
            }

        }
        public void Close()
        {
            client?.Stop();
            logger.Info($"{Id}: Остановился");

            foreach(var status in dpValuesStatus)
            {
                status.IsConnected = false;
            }
        }

        public void ConnectDpValue<T>(IDpValueSource<T> dpValue, IDpSourceConfiguration sourceConfiguration) where T : new()
        {

            OpcUaDpValueSourceConfiguration opcuaSourceConfig;

            if (sourceConfiguration is OpcUaDpValueSourceConfiguration)            
                opcuaSourceConfig = (OpcUaDpValueSourceConfiguration)sourceConfiguration;
            
            else if (sourceConfiguration is DpSourceXmlConfiguration)            
                opcuaSourceConfig = new OpcUaDpValueSourceXmlConfiguration( (DpSourceXmlConfiguration)sourceConfiguration);   
            
            else
                throw new ArgumentException("Неправильный тип source-конфигурации");


            if (typeof(T).IsClass)
                nodes.Add(ConfigureNodeComplexValue(dpValue, opcuaSourceConfig));
            else
                nodes.Add(ConfigureNodeValue(dpValue, opcuaSourceConfig));

            logger.Info($"{Id}: Зарегистрирована точка {opcuaSourceConfig.NodeId}");            
        }


        NodeValue<T> ConfigureNodeValue<T>(IDpValueSource<T> dpValue, OpcUaDpValueSourceConfiguration config) where T : new()
        {
            NodeValue<T> node = new NodeValue<T>(config.NodeId, (e, v) => dpValue.UpdateValueFromSource(v));

            dpValue.ValueWritten += (e, v) =>
            {
                node.Value = v;
                if (client != null)
                {
                    client.ModifyNodeValue(node);
                    Console.WriteLine($"{Id}: В ноду {node.NodeId} записано значение {v}");
                }
                else
                {
                    throw new Exception($"{Id}: Подключение сервером не установлено!");
                }
            };

            dpValuesStatus.Add(dpValue);
            return node;
        }

        NodeValue<ComplexType<T>> ConfigureNodeComplexValue<T>(IDpValueSource<T> dpValue, OpcUaDpValueSourceConfiguration config) where T : new()
        {
            NodeValue<ComplexType<T>> node = new NodeValue<ComplexType<T>>(config.NodeId, (e, v) => dpValue.UpdateValueFromSource(v.ExtractedValue));

            dpValue.ValueWritten += (e, v) =>
            {
                Console.WriteLine("Запись сложной ноды");
                node.Value.ExtractedValue = v;
                client.ModifyNodeComplexValue(node);                
            };
            dpValuesStatus.Add(dpValue);
            return node;
        }

        public void ConnectDpMethod(IDpActionSource dpMethod, IDpSourceConfiguration sourceConfiguration)
        {
            OpcUaDpValueSourceConfiguration opcuaSourceConfig;

            if (sourceConfiguration is OpcUaDpValueSourceConfiguration)
                opcuaSourceConfig = (OpcUaDpValueSourceConfiguration)sourceConfiguration;

            else if (sourceConfiguration is DpSourceXmlConfiguration)
                opcuaSourceConfig = new OpcUaDpValueSourceXmlConfiguration((DpSourceXmlConfiguration)sourceConfiguration);

            else
                throw new ArgumentException("Неправильный тип source-конфигурации");


            dpMethod.SourceDelegate += (e) =>
            {
                if(client != null)
                {
                    
                    return client.CallMethod(opcuaSourceConfig.NodeId, e);
                }
                else
                {
                    throw new Exception($"{Id}: Подключение сервером не установлено!");
                }
            };
            dpValuesStatus.Add(dpMethod);
        }        

    }
}
