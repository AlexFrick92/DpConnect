using Promatis.Core.Logging;
using System;
using System.Threading.Tasks;

namespace DpConnect.Example.PrimitiveValues
{
    internal class ConsoleReader : IConsoleReader
    {
        ILogger logger;

        public IDpValue<bool> BoolDp { get; set; }

        public IDpValue<short> ShortDp { get; set; }

        public IDpValue<float> FloatDp { get; set; }

        public IDpValue<string> StringDp { get; set; }

        public IDpValue<string> WStringDp { get; set; } //В юникоде

        public IDpValue<DateTime> DateTimeDp { get; set; }

        public void DpBound()
        {

            BoolDp.ValueUpdated += (s, v) => logger.Info("Значение булевой точки: {0}", v);
            ShortDp.ValueUpdated += (s, v) => logger.Info("Значение булевой точки: {0}", v);
            StringDp.ValueUpdated += (s, v) => logger.Info("Значение строковой точки: {0}", v);
            FloatDp.ValueUpdated += (s, v) => logger.Info("Значение вещественной точки: {0}", v);
            WStringDp.ValueUpdated += (s, v) => logger.Info("Значение строковой юникод точки: {0}", v);
            DateTimeDp.ValueUpdated += (s, v) => logger.Info("Значение точки времени: {0}", v);

            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        if (StringDp.IsConnected)
                        {
                            await Task.Delay(5000);

                            
                            string str = StringDp.Value;

                            if (str.Length > 10)
                            {
                                StringDp.Value = "s";
                            }
                            else
                                StringDp.Value = str + "s";
                            
                            await Task.Delay(2000);

                            
                            BoolDp.Value = !BoolDp.Value;
                            await Task.Delay(2000);

                            ShortDp.Value = (short)(ShortDp.Value + 2);
                            await Task.Delay(2000);

                            FloatDp.Value = FloatDp.Value + 0.6f;
                            await Task.Delay(2000);

                            string str2 = WStringDp.Value;

                            if (str2.Length > 10)
                            {
                                WStringDp.Value = "Ы";
                            }
                            else
                                WStringDp.Value = str2 + "Ы";

                            await Task.Delay(2000);

                            DateTimeDp.Value = DateTime.UtcNow;
                            await Task.Delay(2000);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Исключение при попытке записи:" + ex.Message);
                    }                    
                }
            });

        }        


        public ConsoleReader(ILogger logger)
        {
            this.logger = logger;
        }
    }
}
