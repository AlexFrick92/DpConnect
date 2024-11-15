

using System;

namespace DpConnect
{
    public interface IDpValue<T> where T : new()
    {
        T Value { get; set; }

        event EventHandler<T> ValueUpdated;        
    }
}
