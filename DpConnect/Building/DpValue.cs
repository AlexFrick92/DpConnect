using System;

using DpConnect;
using DpConnect.Connection;


namespace DpConnect.Building
{
    internal class DpValue<T> : IDpValueSource<T>, IDpValue<T> where T : new()
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

        public bool IsConnected { get; set; }
    

        public event EventHandler<T> ValueWritten;
        public event EventHandler<T> ValueUpdated;
        public event EventHandler<EventArgs> StatusChanged;

        public void UpdateValueFromSource(T value)
        {
            this.value = value;

            ValueUpdated.Invoke(this, value);
        }
    }    
}
