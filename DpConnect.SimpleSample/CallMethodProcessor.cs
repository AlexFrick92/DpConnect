using DpConnect.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.SimpleSample
{
    internal class CallMethodProcessor : DpProcessor
    {
        public CallMethodProcessor()
        {
            DpInitialized += CallMethodProcessor_DpInitialized;
        }

        private async void CallMethodProcessor_DpInitialized(object sender, EventArgs e)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            Console.WriteLine(NodeMethod.Call(new InputArgsFloat() { Arg1 = 2, Arg2 = 2 }));
            
            Console.WriteLine(NodeMethod2.Call(new InputArgsInt() { Arg1 = 5, Arg2 = 5}).Result);
        }

        public IDpMethod<InputArgsFloat, float> NodeMethod { get; set; }
        public IDpMethod<InputArgsInt, OutputArgInt> NodeMethod2 { get; set; }
    }

    public class InputArgsFloat
    {
        public float Arg1 { get; set; }
        public float Arg2 { get; set; }

    }

    public class InputArgsInt
    {
        public short Arg1 { get; set; }
        public short Arg2 { get; set; }
    }

    public class OutputArgInt
    {
        public short Result { get; set; }
    }
}
