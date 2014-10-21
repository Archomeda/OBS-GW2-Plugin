using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;

namespace ObsGw2Plugin.Scripting.Events
{
    public class CachedVariableChangedEventArgs : EventArgs
    {
        public CachedVariableChangedEventArgs(object oldVariable, object newVariable)
        {
            this.OldVariable = oldVariable;
            this.NewVariable = newVariable;
        }

        public object OldVariable { get; set; }

        public object NewVariable { get; set; }
    }
}
