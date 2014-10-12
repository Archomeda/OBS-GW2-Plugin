using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;

namespace ObsGw2Plugin.Scripting
{
    public interface IScript
    {
        string LuaScriptFilename { get; }

        Script LuaScript { get; }

        void InitScript(string scriptFilename);
    }
}
