using System;

namespace DpConnect.Interface
{
    public interface IDpValue<T> : IDataPoint
    {
        T Value { get; set; }

        event EventHandler<T> ValueUpdated;
    }
}
