

using System;

namespace DpConnect.Interface
{
    public interface IDpValue<T>
    {
        T Value { get; set; }

        void UpdateValueFromSource(T value);

        event EventHandler<T> ValueWritten;

        event EventHandler<T> ValueUpdated;
    }
}
