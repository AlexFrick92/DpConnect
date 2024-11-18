using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.ComplexTypes
{
    public class ComplexMethodCall : IComplexMethodCall
    {


        public IDpAction<Multiply> MulMethod { get; set; }

        public delegate ReturnVal Multiply(InputArgs args);

        public class ReturnVal
        {
            public float Value { get; set; }
        }
        public class InputArgs
        {
            public float A { get; set; }
            public float B { get; set; }
        }




        public void Call()
        {
            var result = MulMethod.Call(new InputArgs() { A = 2, B = 3 });

            Console.WriteLine(result.Value.ToString());
        }

        public void DpBound()
        {
            
        }
    }
}
