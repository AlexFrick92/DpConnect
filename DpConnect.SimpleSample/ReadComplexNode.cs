using DpConnect.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.SimpleSample
{
    internal class ReadComplexNode : DpProcessor, IReadComplexNode
    {
        public ReadComplexNode()
        {
            DpInitialized += ReadComplexNode_DpInitialized;
        }

        private void ReadComplexNode_DpInitialized(object sender, EventArgs e)
        {
            ComplexDp.ValueUpdated += ComplexDp_ValueUpdated;
        }

        private void ComplexDp_ValueUpdated(object sender, MyComplexNode e)
        {
            Console.WriteLine(Name + " " + e.ToString());
        }

        public IDpValue<MyComplexNode> ComplexDp { get; set; }
    }

    public class MyComplexNode
    {
        public bool BoolTypeVar { get; set; }
        public short IntTypeVar { get; set; }
        public float RealTypeVar { get; set; }
        public string StringTypeVar { get; set; }
        public string WStringTypeVar { get; set; }
        public DateTime DateTimeTypeVar { get; set; }
        public override string ToString()
        {
            return $"Bool: {BoolTypeVar} Int: {IntTypeVar} Real: {RealTypeVar} String: {StringTypeVar} WString: {WStringTypeVar} DT: {DateTimeTypeVar}";
        }
    }
}
