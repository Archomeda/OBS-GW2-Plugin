using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;

namespace ObsGw2Plugin.Scripting
{
    public interface ICategorizableScript : IScript
    {
        string Name { get; }

        IEnumerable<string> Category { get; }
    }
}
