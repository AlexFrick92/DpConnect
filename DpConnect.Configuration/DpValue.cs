using System;
using DpConnect.Interface;

namespace DpConnect.Configuration
{
    public class DpValue<T> : IDpValue<T>, IDpValueSource<T>
    {
        public string Name {get; set;}

        T fvalue;
        public T Value 
        { 
            get { return fvalue; }

            set 
            {
                fvalue = value;
                ModifyValue?.Invoke(value);
            } 
        }

        public event EventHandler<T> ValueUpdated;
        public event EventHandler<bool> AliveStatusChanged;
        public bool AliveStatus { get; set; }

        public Action<T> ModifyValue { get; set; }

        public void UpdateValue(T value)
        {           
            fvalue = value;
            ValueUpdated?.Invoke(this, value);
        }

        public void UpdateAliveStatus(bool Alive)
        {
            throw new NotImplementedException();
        }
    }
}
