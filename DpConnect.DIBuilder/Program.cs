using DpConnect.Interface;

using Promatis.Core;
using Promatis.Core.Logging;
using Promatis.IoC.DryIoc;
using System;

namespace DpConnect.DIBuilder
{
    internal class Program
    {
        static void Main(string[] args)
        {

            IIoCContainer container = new DryIocContainer();

            container.RegisterInstance(container);

            container.RegisterInstance<ILogger>(new ConsoleLogger());            

            container.Register<IOpcProvider, OpcUaProvider>();

            container.Register<IDpProcessorFactory<BoolNodeReader>, BoolNodeReaderFactory>();
           
            container.Register<IDpBinder, DpBinder>();

            container.Register<IDpProviderConfigurator, DumbProviderConfigurator>();

            container.Register<IApp, App>();








            container.Resolve<IApp>();

            Console.ReadLine();
        }
    }
}
