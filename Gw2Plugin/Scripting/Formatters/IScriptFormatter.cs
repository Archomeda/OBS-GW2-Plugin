using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using ObsGw2Plugin.Scripting.Variables;

namespace ObsGw2Plugin.Scripting.Formatters
{
    public interface IScriptFormatter : IScriptWithCachedValue, IScriptWithCachedLocals, IHookableScript, ICategorizableScript
    {

    }
}
