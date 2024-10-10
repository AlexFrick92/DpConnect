using System;
using System.IO;
using System.Reflection;

using Promatis.Core;
using Promatis.Core.Logging;
using Promatis.IoC.DryIoc;

using DpConnect.Configuration;
using DpConnect.Provider.OpcUa;
using DpConnect.Interface;


namespace DpConnect.SimpleSample
{
    internal class Program
    {
        static void Main(string[] args)
        {

            IIoCContainer container = new DryIocContainer();

            container.Register<ILogger, ConsoleLogger>();
            
            container.Register<IOpcUaProvider, OpcUaProvider>();

            container.Register<IReadComplexNode, ReadComplexNode>();
            container.Register<IReadNodeProcessor, ReadNodeProcessor>();
            container.Register<ICallMethodProcessor, CallMethodProcessor>();               

            string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            
            DpFluentBuilder dataPointConfigurator = new DpFluentBuilder(container)                               
                .AddConfiguration($"{currentDirectory}/DpConfig.xml")                                                
                .Build();

           dataPointConfigurator.StartProviders();


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
