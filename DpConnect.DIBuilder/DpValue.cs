using DpConnect.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.DIBuilder
{
    internal class DpValue<T> : IDataPoint, IDpValue<T>, IDpValueSource<T>
    {
        public string Name { get; set; }
        public Action<T> ModifyValue { get; set; }
        public T Value { get; set; }

        public event EventHandler<T> ValueUpdated;

        public void UpdateValue(T value)
        {
            Value = value;           
            ValueUpdated?.Invoke(this, value);
        }
    }
}
