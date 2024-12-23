using DpConnect.Example.TechParamApp.View;
using DpConnect.Example.TechParamApp.ViewModel;
using System;
using System.Collections.Generic;

using Promatis.Core;
using Promatis.Core.Logging;
using Promatis.IoC.DryIoc;
using DpConnect.Configuration.Xml;
using DpConnect.OpcUa;
using DpConnect.ExampleWorker.Console;

namespace DpConnect.Example.TechParamApp
{
    internal class App : System.Windows.Application
    {
        IIoCContainer container;
        public App()
        {
            container = new DryIocContainer();

            container.RegisterInstance(container);

            container.RegisterInstance(new ConsoleLogger() as ILogger);

            container.Register<IOpcUaConnection, OpcUaConnection>();
            container.RegisterSingleton(typeof(IDpConnectionManager), typeof(ContainerizedConnectionManager));


            container.Register<TechParamReader, TechParamReader>();
            container.RegisterSingleton(typeof(IDpWorkerManager), typeof(ContainerizedWorkerManager));

            container.Register<IDpBuilder, DpXmlBuilder>();

            container.Register(typeof(MainViewModel), typeof(MainViewModel));
            container.Register<MainView, MainView>();

            IDpBuilder builder = container.Resolve<IDpBuilder>();
            builder.Build();
        }
        public new void Run()
        {

            container.Resolve<MainView>().Show();

            base.Run();
        }
    }
}
