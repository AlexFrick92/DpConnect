﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public interface ISourceConfiguratorViewModel
    {
        IEnumerable<NamedConfigSettingViewModel> Settings { get; }


    }
}