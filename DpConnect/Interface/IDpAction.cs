using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Interface
{
    public interface IDpAction<T> where T : Delegate
    {
        T Call { get; }
    }
}
