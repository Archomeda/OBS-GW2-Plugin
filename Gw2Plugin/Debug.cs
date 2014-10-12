using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObsGw2Plugin
{
    public static class Debug
    {
        [Conditional("DEBUG")]
        public static void AttachDebugger()
        {
            if (!System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Launch();
        }

        [Conditional("DEBUG")]
        public static void BreakDebugger()
        {
            System.Diagnostics.Debugger.Break();
        }
    }
}
