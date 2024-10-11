using Promatis.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.DIBuilder
{
    internal class App : IApp
    {

        BoolNodeReader _boolNodeReader;

        public App(ILogger logger, IDpBuilder builder)
        {
            logger.Info("Hello from App!");


            _boolNodeReader = builder.CreateProcessor<BoolNodeReader>("RedNode1");

            _boolNodeReader.BoolValuesUpdated += (sender, val) =>
            {
                logger.Info("From App, new value accepted: " + val);
            };            

        }
        
        
    }
}
