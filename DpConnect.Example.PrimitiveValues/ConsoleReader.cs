using Promatis.Core.Logging;
using System;
using System.Threading.Tasks;

namespace DpConnect.Example.PrimitiveValues
{
    internal class ConsoleReader : IConsoleReader
    {
        ILogger logger;

        public IDpValue<bool> BoolDp { get; set; }

        public IDpValue<float> FloatDp { get; set; }

        public void DpBound()
        {
            BoolDp.ValueUpdated += (s, v) => logger.Info("Значение булевой точки: {0}", v);
            FloatDp.ValueUpdated += (s, v) => logger.Info("Значение вещественной точки: {0}", v);

            Task.Run(async ()=>                
                {
                    while(true)
                    {
                        if (BoolDp.IsConnected)
                        {
                            BoolDp.Value = !BoolDp.Value;
                        }
                        else
                        {
                            Console.WriteLine("Нет соединения");
                        }
                        await Task.Delay(1000);
                    }
                });

            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        FloatDp.Value = FloatDp.Value + 0.6f;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Исключение при попытке записи:" + ex.Message);
                    }
                    await Task.Delay(500);
                }
            });

        }

        public ConsoleReader(ILogger logger)
        {
            this.logger = logger;
        }
    }
}
