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
        public App(ILogger logger, IBoolNodeReader boolNodeReader)
        {
            logger.Info("Hello from App!");

            boolNodeReader.BoolValuesUpdated += (sender, val) =>
            {
                logger.Info("From App, new value accepted: " + val);
            };

            

        }
        
        
    }
}
