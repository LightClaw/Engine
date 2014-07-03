﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightClaw.Engine.Core
{
    public class ControllableEventArgs : EventArgs
    {
        public object Parameter { get; private set; }

        public ControllableEventArgs() { }

        public ControllableEventArgs(object parameter)
        {
            this.Parameter = parameter;
        }
    }
}
