﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Interface
{
    public interface IDpFunc<TResult, T>
    {
        TResult Call(T arg1);
    }
}
