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
using DpConnect.Building;
using DpConnect.Connection;

namespace DpConnect.Example.TechParamApp
{
    /// <summary>
    /// 
    /// </summary>
    internal class App : System.Windows.Application
    {
        IIoCContainer container;
        public App()
        {

            //Регистрируем сервисы в контейнере.
            //

            container = new DryIocContainer();

            //Регистрируем сам контейнер. Его используют ConnectionManager и WorkerManager
            container.RegisterInstance(container);

            container.RegisterInstance(new ConsoleLogger() as ILogger);

            container.Register<IDpConfigurableConnection<OpcUaConnectionConfiguration>, OpcUaConnection>();
            container.RegisterSingleton(typeof(IDpConnectionManager), typeof(ContainerizedConnectionManager));


            container.Register<TechParamReader, TechParamReader>();
            container.RegisterSingleton(typeof(IDpWorkerManager), typeof(ContainerizedWorkerManager));


            container.RegisterSingleton<IDpBinder, DpBinder>();

            container.Register<IDpBuilder, DpXmlBuilder>();

            container.Register(typeof(MainViewModel), typeof(MainViewModel));
            container.Register<MainView, MainView>();

            IDpBuilder builder = container.Resolve<IDpBuilder>();
            builder.Build();
        }
        public new void Run()
        {
            //Достаем окно приветствия и запускаем UI.

            container.Resolve<MainView>().Show();

            base.Run();
        }
    }
}
