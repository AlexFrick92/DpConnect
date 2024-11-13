using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Configuration
{
    internal class DpConfigurationException : Exception
    {
        public DpConfigurationException(string message) : base(message) { }
        
    }
}
