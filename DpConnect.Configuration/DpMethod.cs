using System.Collections.Generic;
using System;
using DpConnect.Interface;

namespace DpConnect.Configuration
{
    public class DpMethod<I,O> : IDpMethod<I,O>, IDpMethodSource, IDpMethodConfig where O : new()
    {

        //Используется для явного мэппинга свойстс и аргументов. В дальнейшем можно сделать
        private List<string> inputProperties = new List<string>();
        private List<string> outputProperties = new List<string>();

        public string Name { get; set; }
        public CommonDelegate CallLower { get; set; }
        public void AddInputProperties(string name)
        {
            inputProperties.Add(name);
        }

        public void AddOutputProperties(string name)
        {
            outputProperties.Add(name);
        }
        public O Call(I args)
        {
            return GetResult(CallLower(PrepareArgs(args)));
        }
        object[] PrepareArgs(I arg)
        {
            List<object> preparedArgs = new List<object>();

            if (arg != null)
            {
                if (inputProperties.Count > 0)
                {
                    foreach (var property in inputProperties)
                    {
                        Console.WriteLine($"ищем свойство: {property}");
                        var propertyInfo = typeof(I).GetProperty(property);
                        var value = propertyInfo.GetValue(arg);
                        preparedArgs.Add(value);
                    }
                }
                else if (typeof(I).IsValueType)
                {
                    preparedArgs.Add(arg);
                }
                else
                    throw new Exception("Ошибка конфигурации метода");


                

                //foreach (var property in typeof(I).GetProperties())
                //{
                //    preparedArgs.Add(property.GetValue(arg));
                //}


            }
            return preparedArgs.ToArray();
        }
        O GetResult(IList<object> result)
        {
            O orderedResult = new O();
                
            int i = 0;

            if (outputProperties.Count > 0)
            {
                foreach (var property in outputProperties)
                {
                    var propertyInfo = typeof(O).GetProperty(property);
                    propertyInfo.SetValue(orderedResult, result[i]);
                    i++;
                }
            }
            else if (typeof(O).IsValueType)
            {
                orderedResult = (O)result[0];
            }
            else
                throw new Exception("Ошибка конфигурации метода");


            //foreach(var property in typeof(O).GetProperties())
            //{
            //    property.UpdateValue(orderedResult, result[i]);
            //    i++;
            //}

            return orderedResult;
        }
    }
    
}
