
using System.Collections.Generic;

using DpConnect.Configuration;

namespace DpConnect.Interface
{
    internal interface IDpBinder
    {
        void Bind(IDpWorker worker, IEnumerable<DpConfiguration> configs);
    }
}
