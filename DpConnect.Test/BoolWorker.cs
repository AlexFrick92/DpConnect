using DpConnect.Interface;
using Promatis.Core.Logging;
using System;

namespace DpConnect.Test
{
    public class BoolWorker : IBoolWorker
    {
        ILogger logger;

        public BoolWorker(ILogger logger)
        {
            this.logger = logger;
        }

        public IDpValue<bool> BoolValueProp { get; set; }

        public IDpFunc<bool> dpBoolAction { get; set; }

        

        public void DpBound()
        {
            BoolValueProp.ValueUpdated += BoolValueProp_ValueUpdated;

            dpBoolAction.Call(true);
        }

        private void BoolValueProp_ValueUpdated(object sender, bool e)
        {
            logger.Info(e.ToString());            
        }
    }
}
