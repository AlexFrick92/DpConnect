using System;


namespace DpConnect.Connection
{
    public interface IDpValueSource<T>
    {
        void UpdateValueFromSource(T value);

        event EventHandler<T> ValueWritten;
    }
}
