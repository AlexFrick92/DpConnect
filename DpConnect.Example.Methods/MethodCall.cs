using System;


namespace DpConnect.Example.Methods
{
    internal class MethodCall : IMethodCall
    {


        public IDpAction<Func<DateTime>> GetDate { get; set; }

        public IDpAction<Func<float, float, float>> Multiply { get; set; }

        public IDpAction<Func<short, short, short>> Sum { get; set; }

        public void DpBound()
        {
            //Console.WriteLine(GetDateMethod.Call);
        }

        public void GetDateMethod()
        {
            Console.WriteLine( GetDate.Call() );
        }

        public void MultiplyReal(float arg1, float arg2)
        {
            Console.WriteLine(Multiply.Call(arg1, arg2));
        }

        public void SumInt(short arg1, short arg2)
        {
            Console.WriteLine( Sum.Call(arg1, arg2) );
        }
    }
}
