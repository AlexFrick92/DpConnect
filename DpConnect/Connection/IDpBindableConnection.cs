﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Connection
{
    public interface IDpBindableConnection<TSourceConfiguration>
        where TSourceConfiguration : IDpSourceConfiguration
    {
    }
}