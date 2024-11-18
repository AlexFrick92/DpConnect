using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.ComplexTypes
{
    internal interface IComplexValueReadWrite : IDpWorker
    {
        void Write(bool boolval, float floatval);        
    }
}
