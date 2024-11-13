using DpConnect.Interface;

namespace DpConnect
{
    internal class DpMethod : IDpMethod
    {
        public SourceDelegate SourceDelegate { get; set; }        



    }

    internal class DpFunc<TResult, T> : DpMethod,  IDpFunc<TResult, T>
    {
        public TResult Call(T arg1)
        {

            

            return (TResult)SourceDelegate(arg1);
        }
    }

}
