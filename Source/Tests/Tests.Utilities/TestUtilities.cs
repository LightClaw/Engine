﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Utilities
{
    public class TestUtilities
    {
        public static TimeSpan DoMeasuredAction(Action action)
        {
            Contract.Requires<ArgumentNullException>(action != null);

            Stopwatch st = Stopwatch.StartNew();
            action();
            return st.Elapsed;
        }

        public static async Task<TimeSpan> DoMeasuredActionAsync(Func<Task> func)
        {
            Contract.Requires<ArgumentNullException>(func != null);

            Stopwatch st = Stopwatch.StartNew();
            await func();
            return st.Elapsed;
        }
    }
}
