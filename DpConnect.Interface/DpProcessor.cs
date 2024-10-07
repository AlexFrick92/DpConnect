using DpConnect.Interface;
using System;

namespace DpConnect.Interface
{
    public abstract class DpProcessor : IDpProcessor
    {
        public string Name { get; set; } = "DpProcessor";

        public event EventHandler<EventArgs> DpInitialized;

        public virtual void OnDpInitialized()
        {
            DpInitialized?.Invoke(this, EventArgs.Empty);
        }
    }
}
