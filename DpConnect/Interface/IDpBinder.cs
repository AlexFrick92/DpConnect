
using System.Collections.Generic;

using DpConnect.Configuration;

namespace DpConnect.Interface
{
    public interface IDpBinder
    {
        void Bind(IDpWorker worker, IEnumerable<DpValueConfiguration> configs);
    }
}
