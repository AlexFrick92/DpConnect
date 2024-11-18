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

namespace DpConnect.Example.Methods
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


            container.Register<IMethodCall, MethodCall>();
            container.Register<IDpWorkerManager, ContainerizedWorkerManager>();

            container.Register<IDpBuilder, DpXmlBuilder>();





            IDpBuilder builder = container.Resolve<IDpBuilder>();
            builder.Build();

            Task.Run(async () =>
            {
                await Task.Delay(2000);
                builder.ConnectionManager.OpenConnections();
            });


            IEnumerable<IMethodCall> workers = builder.WorkerManager.ResolveWorker<IMethodCall>();

            Console.WriteLine(workers.Count());

            var methodworker = workers.First();

            while(true)
            {
                string line = Console.ReadLine();
                switch (line.Split(' ')[0])
                {
                    case "getDate":
                        methodworker.GetDateMethod();
                        break;

                    case "mul":
                        methodworker.MultiplyReal(float.Parse(line.Split(' ')[1]), float.Parse(line.Split(' ')[2]));
                        break;

                    case "sum":
                        methodworker.SumInt(short.Parse(line.Split(' ')[1]), short.Parse(line.Split(' ')[2]));
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
