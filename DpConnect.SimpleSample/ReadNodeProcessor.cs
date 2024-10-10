using System;
using DpConnect.Interface;

namespace DpConnect.SimpleSample
{
    internal class ReadNodeProcessor : DpProcessor, IReadNodeProcessor
    {        
        public ReadNodeProcessor()
        {            
            DpInitialized += ReadNodeProcessor_DpInitialized;
        }

        private void ReadNodeProcessor_DpInitialized(object sender, System.EventArgs e)
        {
            BoolNode.ValueUpdated += BoolNode_ValueUpdated;
        }

        private void BoolNode_ValueUpdated(object sender, bool e)
        {
            Console.WriteLine("Считано новое значение по подписке: " + e);
        }

        public IDpValue<bool> BoolNode { get; set; }
    }
}
