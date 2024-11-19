using System;

using DpConnect;
using DpConnect.Connection;


namespace DpConnect.Building
{
    internal class DpValue<T> : IDpValueSource<T>, IDpValue<T>
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

        bool isConnected;
        public bool IsConnected 
        { 
            get => isConnected;
            set
            {
                isConnected = value;
                StatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    
        public event EventHandler<T> ValueWritten;
        public event EventHandler<T> ValueUpdated;
        public event EventHandler<EventArgs> StatusChanged;

        public void UpdateValueFromSource(T value)
        {
            this.value = value;

            ValueUpdated?.Invoke(this, value);
        }
    }    
}
