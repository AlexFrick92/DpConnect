using System;
using Promatis.Core.Logging;
using Promatis.Opc.UA.Client;


namespace TestComplexValue
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client("opc.tcp://10.10.10.95:4840", new ConsoleLogger());

            client.Start();

            NodeValue<PrimitiveTypes> strandNode = new NodeValue<PrimitiveTypes>("ns=3;s=\"PrimitiveTypes\".\"Complex\"", OnNodeChange);
            

            client.Subscription(strandNode);
            //client.Subscription(strandNodeFloat);

            Console.WriteLine("Клиент запущен");

            Console.ReadLine();

        }

        static void OnNodeChange(object sender, PrimitiveTypes val)
        {            
            Console.WriteLine(val.floatVar);
        }


        class PrimitiveTypes : ComplexType
        {
            public float floatVar => GetValue<float>("RealTypeVar");            
        }
        
    }
}
