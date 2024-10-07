using Promatis.Core.Logging;
using DpConnect.Interface;
using Promatis.Opc.UA.Client;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Reflection;



//Адаптер для клиента опс уа

namespace DpConnect.Provider.OpcUa
{
    public class OpcUaProvider : IDpProvider
    {
        Client _client;
        ILogger _logger;
        OpcUaHostConfiguration _hostConfiguration;
        IList<object> _dpNodes = new List<object>();
        public OpcUaProvider()
        {
            //_client = new Client("opc.tcp://10.10.10.92:4840", logger);
        }        
        public string Name { get; set; }

        public IDpProvider Clone()
        {
            return new OpcUaProvider();
        }

        public void ConfigureHost(XDocument xmlConfiguration)
        {
            _hostConfiguration = new OpcUaHostConfiguration(xmlConfiguration);           
        }

        public void RegisterDp<T>(IDpValueSource<T> dp, XDocument xmlConfig)
        {
            //string nodeIdT = "ns=3;s=\"CommonParameters\".\"LocalTime\"";

            var node = new NodeValue<T>(new OpcUaDpConfiguration(xmlConfig).NodeId, (e, t) => dp.UpdateValue(t));            
            dp.ModifyValue = (v) =>
            {
                node.Value = v;
                if(_client != null)
                {
                    _client.ModifyNodeValue(node);
                    Console.WriteLine($"{Name}: В ноду {node.NodeId} записано значение {v}");
                }
                else
                {
                    throw new Exception($"{Name}: Подключение сервером не установлено!");
                }

            };

            _dpNodes.Add(node);
            Console.WriteLine($"{Name}: Зарегистрирована точка {dp.Name}");
        }

        public void Start()
        {
            _logger?.Info($"{Name}: Запуск...");

            if (_client != null && _client.IsConnected)
            {
                _logger?.Info("Клиент уже запущен!");
                return;
            }

            try
            {     
                if(_client == null)
                    _client = new Client(_hostConfiguration.Endpoint, new ConsoleLogger());

                _client.Start();

                _logger?.Info($"{Name}: Запустился!, конфигурируем точки...");

                foreach (var node in _dpNodes)
                {
                    Type nodeType = node.GetType();
                    Type dataType = nodeType.GetGenericArguments()[0];

                    MethodInfo methodInfo = typeof(Client).GetMethod(nameof(Client.Subscription));
                    MethodInfo genericMethod = methodInfo.MakeGenericMethod(dataType);

                    genericMethod.Invoke(_client, new object[] { node });                    
                }
                _logger.Info($"{Name}: Точки законфигурированы.");

            }
            catch (Exception ex)
            {                
                _logger?.Info($"{Name}: Не удалось запустить! {ex.Message}");
            }

            
        }
        public void Stop()
        {            
            _client?.Stop();            
            _logger?.Info($"{Name}: Остановился");            
        }

        public void RegisterMethod(IDpMethodSource method, XDocument xmlConfig)
        {
            method.CallLower += (e) => CallMethod(new OpcUaDpConfiguration(xmlConfig).NodeId, e);
        }
        private IList<object> CallMethod(string NodeId, params object[] args)
        {            
            return _client.CallMethod(NodeId, args);
        }

        public void SetLogger(ILogger logger)
        {
            _logger = logger;
        }
    }
}
