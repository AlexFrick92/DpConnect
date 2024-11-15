using DpConnect.Configuration.Xml;
using DpConnect.OpcUa;

using Promatis.Core;
using Promatis.Core.Logging;
using Promatis.IoC.DryIoc;
using System;
using System.Threading.Tasks;




namespace DpConnect.Example.PrimitiveValues
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


            container.Register<IConsoleReader, ConsoleReader>();
            container.Register<IDpWorkerManager, ContainerizedWorkerManager>();

            container.Register<IDpBuilder, DpXmlBuilder>();





            IDpBuilder builder = container.Resolve<IDpBuilder>();
            builder.Build();

            Task.Run(async () =>
            {
                await Task.Delay(2000);
                builder.ConnectionManager.OpenConnections();
            });


            Console.ReadLine();
        }
    }
}