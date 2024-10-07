using System;
using System.IO;
using System.Reflection;

using Promatis.Core.Logging;

using DpConnect.Configuration;
using DpConnect.Provider.OpcUa;



namespace DpConnect.SimpleSample
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            ConsoleLogger logger = new ConsoleLogger();

            DpFluentBuilder dataPointConfigurator = new DpFluentBuilder()
                .SetLogger(logger)
                .AddConfiguration($"{currentDirectory}/DpConfig.xml")                
                .SetProviders(new Type[] { typeof(OpcUaProvider) })
                .SetProcessors(new Type[] { typeof(ReadNodeProcessor)})                
                .Build();

           // dataPointConfigurator.StartProviders();


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
