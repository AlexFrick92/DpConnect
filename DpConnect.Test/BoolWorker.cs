using DpConnect.Interface;
using Promatis.Core.Logging;
using System;
using System.Threading.Tasks;

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
                
        public IDpAction<Action<float, float>> FloatMultiply { get; set; }


        public delegate float MultiplyDelegate (float a, float b);

        public delegate MultiplyResult MultiplyByClasses(MultiplyArgs args);
        public class MultiplyResult
        {
            public float result { get; set; }
        }
        public class MultiplyArgs
        {
            public float arg1 { get; set; }
            public float arg2 { get; set; }
        }

        public IDpAction<Func<DateTime>> DpGetDate { get; set; }        

        public IDpValue<CmlVal> ComplexValue { get; set; }

        public class CmlVal
        {
            public bool BoolTypeVar { get; set; }
            public float RealTypeVar { get; set; }
        }

        public void DpBound()
        {                                  
            //BoolValueProp.ValueUpdated += BoolValueProp_ValueUpdated;

            ComplexValue.ValueUpdated += ComplexValue_ValueUpdated;

            return;

            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                logger.Info("Вызываем метод");
                //FloatMultiply.Call(2, 2);
                try
                {
                    //logger.Error("Result: " + FloatMultiply.Call(new MultiplyArgs() { arg1 = 2, arg2 = 2 }).result.ToString());
                    //logger.Error(FloatMultiply.Call(2,4).ToString());

                    logger.Error(DpGetDate.Call().ToString());

                    FloatMultiply.Call(2, 3);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                }
            });
        }

        private void ComplexValue_ValueUpdated(object sender, CmlVal e)
        {
            logger.Info(e.RealTypeVar + " " + e.BoolTypeVar);

            Task.Delay(1000).Wait();

            ComplexValue.Value = new CmlVal() { RealTypeVar = e.RealTypeVar + 1.0f, BoolTypeVar = !e.BoolTypeVar };
        }

        private void BoolValueProp_ValueUpdated(object sender, bool e)
        {
            logger.Info(e.ToString());            
        }
    }
}
