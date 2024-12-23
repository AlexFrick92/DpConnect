using DpConnect.Building;
using DpConnect.Configuration.Xml;
using DpConnect.OpcUa;
using Promatis.Core;
using Promatis.Core.Logging;
using Promatis.IoC.DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.ComplexTypes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IIoCContainer container = new DryIocContainer();


            container.RegisterInstance(container);

            container.RegisterInstance(new ConsoleLogger() as ILogger);

            container.Register<IOpcUaConnection, OpcUaConnection>();
            container.Register<IDpConnectionManager, ContainerizedConnectionManager>();


            container.Register<IComplexMethodCall, ComplexMethodCall>();
            container.Register<IComplexValueReadWrite, ComplexValueReadWrite>();
            container.Register<IDpWorkerManager, ContainerizedWorkerManager>();


            container.RegisterSingleton<IDpBinder, DpBinder>();

            container.Register<IDpBuilder, DpXmlBuilder>();


            IDpBuilder builder = container.Resolve<IDpBuilder>();
            builder.Build();

            Task.Run(async () =>
            {
                await Task.Delay(2000);
                builder.ConnectionManager.OpenConnections();
            });


            IComplexMethodCall methodworker = builder.WorkerManager.ResolveWorker<IComplexMethodCall>().First();
            IComplexValueReadWrite valueworker = builder.WorkerManager.ResolveWorker<IComplexValueReadWrite>().First();

            while (true)
            {
                string line = Console.ReadLine();
                switch (line.Split(' ')[0])
                {
                    case "call":
                        methodworker.Call();
                        break;

                    case "write":
                        valueworker.Write(bool.Parse(line.Split(' ')[1]), float.Parse(line.Split(' ')[2]));
                        break;                    

                    case "exit":
                        builder.ConnectionManager.CloseConnections();
                        Task.Delay(TimeSpan.FromSeconds(2)).Wait();
                        return;
                }
            }
        }
    }
}
