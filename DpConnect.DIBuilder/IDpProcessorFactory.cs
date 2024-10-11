using DpConnect.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.DIBuilder
{
    internal interface IDpProcessorFactory<T> where T: IDpProcessor
    {

        T Create(string name);        
    }
}
