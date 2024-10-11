using DpConnect.Interface;
using Promatis.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DpConnect.DIBuilder
{
    internal class OpcUaProvider : IOpcProvider
    {
        private readonly ILogger _logger;

        IDpValueSource<bool> dp1 = null;
        bool val = false;

        public OpcUaProvider(ILogger logger)
        {
            _logger = logger;
            _logger.Info("Создан OpcUaProvider");
        }

        public string Name { get; set; }

        public IDpProvider Clone()
        {
            throw new NotImplementedException();
        }

        public void ConfigureHost(XDocument xmlConfig)
        {
            _logger.Info("Провайдер законфигурирован");
        }

        public void RegisterDp<T>(IDpValueSource<T> dp, XDocument xmlConfig)
        {
            dp1 = (IDpValueSource<bool>)dp;
            _logger.Info("Привязали к OpcUa");

            Start();

        }

        public void RegisterMethod(IDpMethodSource method, XDocument xmlConfig)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            _logger.Info("Запускаем провайдер OpcUa");
            Task.Run(async () =>
            {
                
                while(true)
                {
                    val = !val;
                    dp1.UpdateValue(val);                    
                    await Task.Delay(1000);
                }
            });
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
