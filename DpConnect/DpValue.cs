using DpConnect.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect
{
    public class DpValue<T> : IDpValue<T>
    {
        public T Value { get; set; }

        public event EventHandler<T> ValueWritten;
        public event EventHandler<T> ValueUpdated;

        public void UpdateValueFromSource(T value)
        {
            Value = value;

            ValueUpdated.Invoke(this, value);
        }
    }    
}
