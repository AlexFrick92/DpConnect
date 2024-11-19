using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

using Promatis.Core.Logging;

using DpConnect.Building;

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

        const string Xml_FileName = "DpConfig.xml";
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


        public DpXmlBuilder(IDpConnectionManager connectionManager, IDpWorkerManager workerManager, ILogger logger)
        {
            this.logger = logger;
            this.connectionManager = connectionManager;
            this.workerManager = workerManager;

            dpBinder = new DpBinder(this.connectionManager, this.logger);


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
                string conId = configuredConnection.Attribute(Xml_ConnectionIdAttribute).Value;      


                bool conActive = configuredConnection.Attribute(Xml_ConnectionActiveAttribute) != null ? bool.Parse(configuredConnection.Attribute(Xml_ConnectionActiveAttribute).Value) : true;

                IDpConnectionConfiguration connetionConfiguration = new DpConnectionXmlConfiguration() 
                {
                    ConnectionId = conId, 
                    Active = conActive,
                    Configuration = new XDocument(configuredConnection) 
                };

                Type connectionType = Type.GetType(typeName);

                MethodInfo methodInfo = typeof(IDpConnectionManager).GetMethod(nameof(IDpConnectionManager.CreateConnection));
                MethodInfo genericMethod = methodInfo.MakeGenericMethod(connectionType);

                genericMethod.Invoke(connectionManager, new[] { connetionConfiguration });

            }

            logger.Info("Соединения созданы.");
        }


        void CreateWorkers()
        {
            logger.Info("Создаём воркеров...");

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
    }
}
