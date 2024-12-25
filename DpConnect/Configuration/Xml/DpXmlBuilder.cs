using System;
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

        const string Xml_ConnectionIdTag = Xml_ConnectionIdAttribute;
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

                // Получаем тип 
                Type connectionType = Type.GetType(typeName);

                // Получаем все интерфейсы, которые реализует 
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


            //Пройдемся по всем воркерам
            foreach (XElement configuredWorker in WorkerConfiguration.Root.Elements(Xml_WorkerDeclareTag))
            {
                string typeName = configuredWorker.Attribute(Xml_WorkerTypeNameAttribute).Value;

                string connectionId = configuredWorker.Element(Xml_ConnectionIdTag).Value;

                Type workerType = Type.GetType(typeName);
                if (!typeof(IDpWorker).IsAssignableFrom(workerType))
                    throw new DpConfigurationException($"Тип {workerType} должен реализовывать интерфейс {nameof(IDpWorker)}");

                MethodInfo createWorkerMethodInfo = typeof(IDpWorkerManager).GetMethod(nameof(IDpWorkerManager.CreateWorker));
                MethodInfo createWorkerMethod = createWorkerMethodInfo.MakeGenericMethod(workerType);
                IDpWorker worker = (IDpWorker)createWorkerMethod.Invoke(workerManager, null);

                //Получим соединение, к которому привязан воркер
                IDpConnection connection = connectionManager.GetConnection(connectionId);
                //Теперь мы должны взять тип конфигурации точки и создать эту конфигурацию.
                //Для этого, соединение должна реализовывать интерфейс IDpBindableConnection<T>, где T - тип конфигурации.

                Type connectionType = connection.GetType();
                //if(!typeof(IDpConfigurableConnection<>).IsAssignableFrom(connectionType))
                //    throw new DpConfigurationException($"Тип соединения {connectionType} должен реализовывать интерфейс {nameof(IDpBindableConnection<IDpSourceConfiguration>)}");

                // Найдем интерфейс
                Type iface = connectionType.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDpBindableConnection<>))
                    .FirstOrDefault();
                if (iface == null) 
                {
                    throw new DpConfigurationException($"Тип соединения {connectionType} должен реализовывать интерфейс {nameof(IDpBindableConnection<IDpSourceConfiguration>)}");
                }

                Type[] genericArguments = iface.GetGenericArguments();

                Type sourceConfigurationType = genericArguments[0];
                Type genericDpConfigType = typeof(DpConfiguration<>).MakeGenericType(sourceConfigurationType);
                Console.WriteLine("TSourceConfiguration: " + genericArguments[0].Name); // Первый тип: TConnectionConfiguration
                    
                //Создадим список конфигураций
                //List<object> listOfConfigs = new List<object>();
                // Создаём список с типом 'List<>', который соответствует IEnumerable<T>
                var listType = typeof(List<>).MakeGenericType(genericDpConfigType);
                var listOfConfigs = Activator.CreateInstance(listType);

                // Добавляем элемент в коллекцию (например, создаём экземпляр объекта)
                var addMethod = listType.GetMethod("Add");


                foreach (XElement configuredDp in configuredWorker.Elements(Xml_DpValueDeclareTag))
                {
                    IDpSourceConfiguration sourceConfiguration = Activator.CreateInstance(sourceConfigurationType) as IDpSourceConfiguration;                    
                        
                    IDpConfiguration dpConfig = Activator.CreateInstance(genericDpConfigType) as IDpConfiguration;

                    dpConfig.PropertyName = configuredDp.Attribute(Xml_DpValuePropNameAttribute).Value;
                    sourceConfiguration.FromXml(new XDocument(configuredDp.Element(Xml_DpSourceConfigurationTag)));
                    dpConfig.SourceConfiguration = sourceConfiguration;

                    addMethod.Invoke(listOfConfigs, new[] { dpConfig });

                }


                MethodInfo bindMethodInfo = typeof(IDpBinder).GetMethod(nameof(IDpBinder.Bind));
                MethodInfo bindMethod = bindMethodInfo.MakeGenericMethod(sourceConfigurationType);
                    
                bindMethod.Invoke(dpBinder, new object[] { worker, connection, listOfConfigs });
                


                //foreach(XElement configuredDpAction in configuredWorker.Elements(Xml_DpActionDeclareTag))
                //{
                //    IDpSourceConfiguration sourceConfig = null;//new DpSourceXmlConfiguration() { Configuration = new XDocument(configuredDpAction.Element(Xml_DpSourceConfigurationTag)) };

                //    string propertyName = configuredDpAction.Attribute(Xml_DpValuePropNameAttribute).Value;                    

                //    DpConfiguration dpValueConfig = new DpConfiguration() { PropertyName = propertyName, ConnectionId = connectionId, SourceConfiguration = sourceConfig };

                //    workerDpConfig.Add(dpValueConfig);
                //}

            }
            logger.Info("Воркеры созданы.");
        }
    }
}
