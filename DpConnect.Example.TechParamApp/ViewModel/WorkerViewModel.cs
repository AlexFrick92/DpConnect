﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class WorkerViewModel : BaseViewModel
    {

        IDpWorker worker;

        public string WorkerType { get; set; }

        public WorkerViewModel(IDpWorker worker)
        {
            this.worker = worker;

            WorkerType = worker.GetType().ToString();
            OnPropertyChanged(nameof(WorkerType));
            
        }


    }
}
