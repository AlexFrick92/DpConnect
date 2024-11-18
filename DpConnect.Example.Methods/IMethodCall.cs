
namespace DpConnect.Example.Methods
{
    internal interface IMethodCall : IDpWorker
    {
        void GetDateMethod();
        void MultiplyReal(float arg1, float arg2);
        void SumInt(short arg1, short arg2);
    }
}
