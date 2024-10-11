

using DpConnect.Interface;
using Promatis.Core.Logging;
using System;

namespace DpConnect.DIBuilder
{
    internal class BoolNodeReader : DpProcessorBase
    {

        private readonly ILogger _logger;        

        public event EventHandler<bool> BoolValuesUpdated;

        public BoolNodeReader(ILogger logger, IDpBinder dpBinder, string name) : base(dpBinder, name) 
        {
                    
            _logger = logger;
            _logger.Info("Создан " + Name);

        }

        public override void OnDpInitialized()
        {
            BoolNode.ValueUpdated += (sender, args) =>
            {
                BoolValuesUpdated?.Invoke(this, args);
            };
        }


        public string GetValues()
        {
            return BoolNode.Value.ToString();
        }

        public IDpValue<bool> BoolNode { get; set; }
    }
}
