using System;

namespace DpConnect
{
    public interface IDpAction<T> where T : Delegate
    {
        T Call { get; }        
    }
}
