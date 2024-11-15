
using DpConnect.OpcUa;
using DpConnect.Configuration.Xml;


using Promatis.Core;
using Promatis.Core.Logging;
using Promatis.IoC.DryIoc;


using System;



namespace DpConnect.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {

            IIoCContainer container = new DryIocContainer();
            container.RegisterInstance(container);

            container.Register<ILogger, ConsoleLogger>();

            container.Register<IOpcUaConnection, OpcUaConnection>();
            container.Register<IDpConnectionManager, ContainerizedConnectionManager>();

            container.Register<IBoolWorker, BoolWorker>();
            container.Register<IDpWorkerManager, ContainerizedWorkerManager>();

            container.Register<IDpBuilder, DpXmlBuilder>();

            container.Register<IApp, Application>();


            container.Resolve<IApp>().Start();            



            

            Console.ReadLine();
        }



    }
}
