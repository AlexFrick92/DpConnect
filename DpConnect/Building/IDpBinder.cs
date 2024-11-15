
using System.Collections.Generic;

using DpConnect;
using DpConnect.Configuration;

namespace DpConnect.Building
{
    internal interface IDpBinder
    {
        void Bind(IDpWorker worker, IEnumerable<DpConfiguration> configs);
    }
}
