using Promatis.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.DIBuilder
{
    internal class BoolNodeReaderFactory : IDpProcessorFactory<BoolNodeReader>
    {
        private readonly ILogger _logger;
        private readonly IDpBinder _binder;
        public BoolNodeReaderFactory(ILogger logger, IDpBinder dpBinder)
        {
            _logger = logger;
            _binder = dpBinder;

        }
        public BoolNodeReader Create(string name)
        {
            return new BoolNodeReader(_logger, _binder, name);
        }
    }
}
