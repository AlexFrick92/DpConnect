

using System;

namespace DpConnect.Interface
{
    public interface IDpValue<T> where T : new()
    {
        T Value { get; set; }

        void UpdateValueFromSource(T value);

        event EventHandler<T> ValueWritten;

        event EventHandler<T> ValueUpdated;
    }
}
