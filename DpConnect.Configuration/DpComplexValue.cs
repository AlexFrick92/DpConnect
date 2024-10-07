using System;
using System.Collections.Generic;
using System.Linq;
using DpConnect.Interface;
using Toolkit.Reactive;

namespace DpConnect.Configuration
{
    public class DpComplexValue<T> : IDpValue<T>, IDpComplexValueConfig where T : new()
    {
        public string Name { get; set; }
        private EventDebounce<T> _debounceEvent;
        public DpComplexValue()
        {
            fvalue = new T();

            _debounceEvent = new EventDebounce<T>(10); // Задержка миллисекунд
            _debounceEvent.DebouncedEvent += (s, v) => ValueUpdated?.Invoke(s, v);            
        }

        ICollection<IDataPoint> dpProperties = new List<IDataPoint>();

        public IDataPoint AddProperty(Type type, string name)
        {
            Type gtype = typeof(DpValue<>).MakeGenericType(type);

            IDataPoint dp =(IDataPoint) Activator.CreateInstance(gtype);

            dp.Name = name;

            dpProperties.Add(dp);

            SubscribeToChanges(dp as dynamic);

            return dp;
        }

        void SubscribeToChanges<P>(DpValue<P> dp)
        {
            dp.ValueUpdated += (s, v) =>
            {                
                typeof(T).GetProperties().First(p => p.Name == dp.Name).SetValue(fvalue, v);
                
                _debounceEvent.RiseEvent(this, fvalue);

                //ValueUpdated?.Invoke(this, fvalue);
            };
                        
        }        

        T fvalue;
        public T Value 
        { 
            get { return fvalue; }

            set 
            {
                foreach(var dp in dpProperties)
                {
                    WriteValues(dp as dynamic, value);
                }                
            } 
        }


        void WriteValues<P>(DpValue<P> dp, T newValue)
        {
            dp.Value = (P)typeof(T).GetProperties().First(p => p.Name == dp.Name).GetValue(newValue);
        }

        public event EventHandler<T> ValueUpdated;
    }
}
