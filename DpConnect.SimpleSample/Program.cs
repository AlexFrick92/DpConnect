using System;
using System.IO;
using System.Reflection;

using Promatis.Core;
using Promatis.Core.Logging;
using Promatis.IoC.DryIoc;

using DpConnect.Configuration;
using DpConnect.Provider.OpcUa;
using DpConnect.Interface;
using Promatis.Opc.UA.Client;


namespace DpConnect.SimpleSample
{
    internal class Program
    {
        static void Main(string[] args)
        {

            IIoCContainer container = new DryIocContainer();

            container.RegisterInstance(container);

            container.RegisterInstance<ILogger>(new ConsoleLogger());
            
            container.Register<IOpcUaProvider, OpcUaProvider>();

            container.Register<IReadComplexNode, ReadComplexNode>();
            container.Register<IReadNodeProcessor, ReadNodeProcessor>();
            container.Register<ICallMethodProcessor, CallMethodProcessor>();
            
            container.Register<IDpProviderConfigurator, DpProviderConfigurator>();
            container.Register<IDpProcessorConfigurator, DpProcessorConfigurator>();
            container.Register<IDpFluentBuilder, DpFluentBuilder>();

            string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var dpBuilder = container.Resolve<IDpFluentBuilder>()
                .AddConfiguration($"{currentDirectory}/DpConfig.xml")
                .SetProcessors(new IDpProcessor[] { new ReadComplexNode() { Name = "RcNode3Pro" } })
                .Build();


            dpBuilder.StartProviders();


            while (true)
            {
                var inputString = Console.ReadLine();

                switch (inputString)
                {

                    case "exit": return;                        
                }
            }
        }
    }
}
