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
        public BoolNodeReaderFactory(ILogger logger)
        {
            _logger = logger;            

        }
        public BoolNodeReader Create(string name)
        {
            return new BoolNodeReader(_logger, name);
        }
    }
}
