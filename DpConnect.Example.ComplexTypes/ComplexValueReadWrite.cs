using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.ComplexTypes
{
    public class ComplexValueReadWrite : IComplexValueReadWrite
    {

        public IDpValue<ComplexValueClass> ComplexValue { get; set; }
        public class ComplexValueClass
        {
            public bool BoolTypeVar { get; set; }
            public float RealTypeVar { get; set; }
        }

        public void DpBound()
        {
            ComplexValue.ValueUpdated += (s, v) => Console.WriteLine("Новое значение: " + v.BoolTypeVar + " " + v.RealTypeVar);
        }

        public void Write(bool boolval, float floatval)
        {
            ComplexValue.Value = new ComplexValueClass { BoolTypeVar = boolval, RealTypeVar = floatval };
        }
    }



}
