using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;

namespace ObsGw2Plugin.Scripting
{
    public interface IHookableScript : IScript
    {
        string Id { get; }

        ISet<string> Hooks { get; }


        IScriptsManager ScriptsManager { get; set; }
    }
}
