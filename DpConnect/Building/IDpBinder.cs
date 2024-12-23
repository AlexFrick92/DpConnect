
using System.Collections.Generic;

using DpConnect;
using DpConnect.Configuration;

namespace DpConnect.Building
{
    public interface IDpBinder
    {
        void Bind(IDpWorker worker, IEnumerable<DpConfiguration> configs);
    }
}
