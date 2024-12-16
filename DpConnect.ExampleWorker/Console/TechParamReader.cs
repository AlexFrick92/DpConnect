using Promatis.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.ExampleWorker.Console
{
    public class TechParamReader : IDpWorker
    {
        ILogger logger;
        public TechParamReader(ILogger logger)
        {
            this.logger = logger;
        }
        public IDpValue<float> TechParam { get; set; }

        public void DpBound()
        {
            TechParam.ValueUpdated += TechParam_ValueUpdated;
        }

        private void TechParam_ValueUpdated(object sender, float e)
        {
            logger.Info("Считано значение: " + e.ToString());
        }
    }
}
