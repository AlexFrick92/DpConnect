﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

using Promatis.Core.Logging;

using DpConnect.Building;
using System.Linq;
using DpConnect.Connection;

namespace DpConnect.Configuration.Xml
{
    public class DpXmlBuilder : IDpBuilder
    {
        readonly IDpConnectionManager connectionManager;
        readonly IDpWorkerManager workerManager;
        readonly IDpBinder dpBinder;
        readonly ILogger logger;
        
        public IDpWorkerManager WorkerManager { get { return workerManager; } }
        public IDpConnectionManager ConnectionManager { get { return connectionManager; } }

        readonly XDocument ConnectionConfiguration;
        readonly XDocument WorkerConfiguration;

        const string Xml_FileName = "DpConnect/DpConfig.xml";                

        const string Xml_DpConfigRootTag = "DpConfiguration";

        const string Xml_ConnectionRootTag = "Connections";
        const string Xml_ConnectionDeclareTag = "Connection";
        const string Xml_ConnectionTypeNameAttribute = "TypeName";
        const string Xml_ConnectionIdAttribute = "ConnectionId";
        const string Xml_ConnectionActiveAttribute = "Active";

        const string Xml_WorkerRootTag = "Workers";
        const string Xml_WorkerDeclareTag = "Worker";
        const string Xml_WorkerTypeNameAttribute = "TypeName";

        const string Xml_DpValueDeclareTag = "DpValue";
        const string Xml_DpValuePropNameAttribute = "PropertyName";
        const string Xml_DpSourceConfigurationTag = "SourceConfiguration";

        const string Xml_DpActionDeclareTag = "DpAction";


        public DpXmlBuilder(IDpConnectionManager connectionManager, IDpWorkerManager workerManager, ILogger logger, IDpBinder binder)
        {
            this.logger = logger;
            this.connectionManager = connectionManager;
            this.workerManager = workerManager;

            dpBinder = binder;


            logger.Info($"Загружаем конфигурацию {Xml_FileName}...");


            XDocument config = XDocument.Load(Xml_FileName);            

            ConnectionConfiguration = new XDocument(config.Element(Xml_DpConfigRootTag).Element(Xml_ConnectionRootTag));
            WorkerConfiguration = new XDocument(config.Element(Xml_DpConfigRootTag).Element(Xml_WorkerRootTag));
            
            logger.Info("Конфигурация завершена.");
        }

        public void Build()
        {
            CreateConnections();
            CreateWorkers();
        }

        void CreateConnections()
        {

            logger.Info("Создаём соединения...");

            foreach(XElement configuredConnection in ConnectionConfiguration.Root.Elements(Xml_ConnectionDeclareTag))
            {                

                string typeName = configuredConnection.Attribute(Xml_ConnectionTypeNameAttribute).Value;                

                bool conActive = configuredConnection.Attribute(Xml_ConnectionActiveAttribute) != null ? bool.Parse(configuredConnection.Attribute(Xml_ConnectionActiveAttribute).Value) : true;

                //IDpConnectionConfiguration connetionConfiguration = new DpConnectionXmlConfiguration() 
                //{
                //    ConnectionId = conId, 
                //    Active = conActive,
                //    Configuration = new XDocument(configuredConnection),
                //    ConnectionType = Type.GetType(typeName)
                //};

                //connectionManager.CreateConnection(connetionConfiguration);


                // Получаем тип для OpcUaConnection
                Type connectionType = Type.GetType(typeName);

                // Получаем все интерфейсы, которые реализует OpcUaConnection
                Type iface = connectionType.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDpConfigurableConnection<>))
                    .FirstOrDefault();

                if(iface != null)
                {
                    Type[] genericArguments = iface.GetGenericArguments();

                    Type connectionConfigurationType = genericArguments[0];

                    Console.WriteLine("TConnectionConfiguration: " + genericArguments[0].Name); // Первый тип: TConnectionConfiguration                    

                    MethodInfo methodInfo = typeof(IDpConnectionManager).GetMethod(nameof(IDpConnectionManager.CreateConnection));
                    MethodInfo createConnectionMethod = methodInfo.MakeGenericMethod(connectionType, connectionConfigurationType);

                    IDpConnectionConfiguration connectionConfig = Activator.CreateInstance(connectionConfigurationType) as IDpConnectionConfiguration;

                    connectionConfig.FromXml(new XDocument(configuredConnection));

                    createConnectionMethod.Invoke(connectionManager, new object[] { connectionConfig });                    
                }
                else
                {
                    Console.WriteLine("GG");
                }

            }

            logger.Info("Соединения созданы.");
        }


        void CreateWorkers()
        {
            logger.Info("Создаём воркеров...");

            try
            {
                IEnumerable<XElement> workers = WorkerConfiguration.Root.Elements(Xml_WorkerDeclareTag);
                if (workers == null || workers.Count() == 0)
                    throw new ArgumentNullException();
            }
            catch 
            {
                logger.Info("Нет воркеров в конфигурации.");
                return;
            }

            foreach (XElement configuredWorker in WorkerConfiguration.Root.Elements(Xml_WorkerDeclareTag))
            {
                string typeName = configuredWorker.Attribute(Xml_WorkerTypeNameAttribute).Value;

                Type workerType = Type.GetType(typeName);
                if (!typeof(IDpWorker).IsAssignableFrom(workerType))
                    throw new DpConfigurationException($"Тип {workerType} должен реализовывать интерфейс {nameof(IDpWorker)}");


                MethodInfo methodInfo1 = typeof(IDpWorkerManager).GetMethod(nameof(IDpWorkerManager.CreateWorker));
                MethodInfo genericMethod1 = methodInfo1.MakeGenericMethod(workerType);

                IDpWorker worker = (IDpWorker)genericMethod1.Invoke(workerManager, null);

                //Создадим точки

                List<DpConfiguration> workerDpConfig = new List<DpConfiguration>();

                foreach(XElement configuredDp in configuredWorker.Elements(Xml_DpValueDeclareTag))
                {
                    IDpSourceConfiguration sourceConfig = new DpSourceXmlConfiguration() { Configuration = new XDocument(configuredDp.Element(Xml_DpSourceConfigurationTag))};

                    string propertyName = configuredDp.Attribute(Xml_DpValuePropNameAttribute).Value;
                    string connectionId = configuredDp.Element(Xml_DpSourceConfigurationTag).Attribute(Xml_ConnectionIdAttribute).Value;

                    DpConfiguration dpValueConfig = new DpConfiguration() { PropertyName = propertyName, ConnectionId = connectionId, SourceConfiguration = sourceConfig };

                    workerDpConfig.Add(dpValueConfig);
                }

                foreach(XElement configuredDpAction in configuredWorker.Elements(Xml_DpActionDeclareTag))
                {
                    IDpSourceConfiguration sourceConfig = new DpSourceXmlConfiguration() { Configuration = new XDocument(configuredDpAction.Element(Xml_DpSourceConfigurationTag)) };

                    string propertyName = configuredDpAction.Attribute(Xml_DpValuePropNameAttribute).Value;
                    string connectionId = configuredDpAction.Element(Xml_DpSourceConfigurationTag).Attribute(Xml_ConnectionIdAttribute).Value;

                    DpConfiguration dpValueConfig = new DpConfiguration() { PropertyName = propertyName, ConnectionId = connectionId, SourceConfiguration = sourceConfig };

                    workerDpConfig.Add(dpValueConfig);

                }

                dpBinder.Bind(worker, workerDpConfig);
            }
            logger.Info("Воркеры созданы.");
        }

        public IDpWorker BuildWorker<T>(IEnumerable<DpConfiguration> propertyConfiguration) where T : IDpWorker
        {
            IDpWorker worker = workerManager.CreateWorker<T>();
            dpBinder.Bind(worker, propertyConfiguration);

            return worker;
        }
    }
}
