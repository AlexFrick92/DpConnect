using System;
using System.Reflection;

using DpConnect.Interface;

namespace DpConnect.Configuration
{
    public class DpXmlBuilder : IDpBuilder
    {
        IDpConnectionManager connectionManager;

        IDpWorkerManager workerManager;

        IDpBinder dpBinder;


        public DpXmlBuilder(IDpConnectionManager dpConnectionManager, IDpWorkerManager dpWorkerManager)
        {
            connectionManager = dpConnectionManager;
            workerManager = dpWorkerManager;        
            
            dpBinder = new DpBinder(connectionManager);
        }

        public IDpConnectionManager ConnectionManager { get { return connectionManager; } }


        public void Build()
        {

           
            //Создаём соединение

            IDpConnectionConfiguration connetionConfiguration = new DpConnectionXmlConfiguration() { ConnectionId = "Stend AKF" };

            string connectionTypeName = "DpConnect.OpcUa.IOpcUaConnection, DpConnect.OpcUa"; //From xml config            
            Type connectionType = Type.GetType(connectionTypeName);

            Console.WriteLine(connectionType.FullName);

            MethodInfo methodInfo = typeof(IDpConnectionManager).GetMethod(nameof(IDpConnectionManager.CreateConnection));
            MethodInfo genericMethod = methodInfo.MakeGenericMethod(connectionType);

            genericMethod.Invoke(connectionManager, new[] { connetionConfiguration });

            //Создаем конфигурацию точки

            IDpValueSourceConfiguration boolValueSourceConfig = new DpValueSourceXmlConfiguration();

            DpValueConfiguration boolValueConfig = new DpValueConfiguration() { PropertyName = "BoolValueProp", ConnectionId = "Stend AKF", SourceConfiguration = boolValueSourceConfig };


            //Создаём воркера
            string workerTypeName = "DpConnect.Test.BoolWorker, DpConnect.Test";
            Type workerType = Type.GetType(workerTypeName);

            MethodInfo methodInfo1 = typeof(IDpWorkerManager).GetMethod(nameof(IDpWorkerManager.CreateWorker));
            MethodInfo genericMethod1 = methodInfo1.MakeGenericMethod(workerType);

            IDpWorker worker =  (IDpWorker)genericMethod1.Invoke(workerManager, null);

            dpBinder.Bind(worker, new DpValueConfiguration[] { boolValueConfig });
        }
    }
}
