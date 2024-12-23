using DpConnect.ExampleWorker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class TechParamWorkerViewModel : BaseViewModel
    {
        public TechParamWorkerViewModel(ITechParamWorker worker)
        {
            worker.ValueUpdated += Worker_ValueUpdated;
        }

        private void Worker_ValueUpdated(object sender, float e)
        {
            Value = e;
            OnPropertyChanged(nameof(Value));
        }

        public float Value { get; set; }
    }
}
