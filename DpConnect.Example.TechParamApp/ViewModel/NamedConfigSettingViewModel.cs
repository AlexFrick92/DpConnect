using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class NamedConfigSettingViewModel : BaseViewModel
    {
        public string Name { get; set; }

        object val;
        public object Value
        {
            get => val;
            set 
            {                
                val = value;
                ValueChanged?.Invoke(this, val);    
            }
        }
        public event EventHandler<object> ValueChanged;



    }
}
