using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.ExampleWorker
{
    public interface ITechParamWorker
    {
        event EventHandler<float> ValueUpdated;
    }
}
