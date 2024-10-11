

using DpConnect.Interface;
using Promatis.Core.Logging;
using System;

namespace DpConnect.DIBuilder
{
    public class BoolNodeReader : IDpProcessor, IBoolNodeReader
    {

        private readonly ILogger _logger;
        private readonly IDpBinder _builder;

        public event EventHandler<bool> BoolValuesUpdated;

        public BoolNodeReader(ILogger logger, IDpBinder dpBinder)
        {

            Name = "BoolReader";
            _builder = dpBinder;
            _logger = logger;
            _logger.Info("Создан BoolNodeReader");

            dpBinder.Bind(this);

            BoolNode.ValueUpdated += (sender, args) =>
            {
                BoolValuesUpdated?.Invoke(this, args);
            };
        }

        public string Name { get; set; }

        public void OnDpInitialized()
        {
            
        }

        public string GetValues()
        {
            return BoolNode.Value.ToString();
        }

        public IDpValue<bool> BoolNode { get; set; }
    }
}
