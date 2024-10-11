using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.DIBuilder
{
    public interface IBoolNodeReader
    {
        event EventHandler<bool> BoolValuesUpdated;
    }
}
