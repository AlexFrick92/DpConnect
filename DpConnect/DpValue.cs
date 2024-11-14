using DpConnect.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect
{
    internal class DpValue<T> : IDpValue<T> where T :new()
    {
        T value;
        public T Value
        {
            get
            {
                return value;
            }
            set
            {
                ValueWritten(this, value);
            }
        }
        

        public event EventHandler<T> ValueWritten;
        public event EventHandler<T> ValueUpdated;

        public void UpdateValueFromSource(T value)
        {
            this.value = value;

            ValueUpdated.Invoke(this, value);
        }
    }    
}
