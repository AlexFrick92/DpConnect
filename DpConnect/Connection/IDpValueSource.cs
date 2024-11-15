using System;


namespace DpConnect.Connection
{
    public interface IDpValueSource<T> : IDpStatus where T : new()
    {
        void UpdateValueFromSource(T value);

        event EventHandler<T> ValueWritten;
    }
}
