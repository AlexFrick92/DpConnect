using DpConnect.ExampleWorker.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    internal class TechParamReaderViewModel : BaseViewModel, ITechParamViewModel
    {
        public TechParamReaderViewModel(TechParamReader reader)
        {
            reader.ValueUpdated += Reader_ValueUpdated;
        }

        private void Reader_ValueUpdated(object sender, float e)
        {
            LastValue = e;
            OnPropertyChanged(nameof(LastValue));
            
        }

        public float LastValue { get; set; }
    }
}
