using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using ObsGw2Plugin.Scripting.Exceptions;
using ObsGw2Plugin.Scripting.Variables;

namespace ObsGw2Plugin.Scripting.Formatters
{
    public class ScriptFormatter : ScriptBase, IScriptFormatter
    {
        public ScriptFormatter() : base() { }


        public string Name { get; protected set; }

        public IEnumerable<string> Category { get; protected set; }


        protected override void InitGlobals()
        {
            base.InitGlobals();
            this.LuaScript.Globals["gettext"] = new Func<string, object>(id => this.ScriptsManager.GetCachedResult("%" + id + "%"));
        }

        protected override void InitObjectProperties()
        {
            base.InitObjectProperties();

            DynValue value = this.LuaScript.Globals.Get("name");
            if (value.Type != DataType.String)
                throw new MissingGlobalException(this.LuaScriptFilename, "name");
            this.Name = value.CastToString();

            value = this.LuaScript.Globals.Get("category");
            if (value.Type == DataType.Table)
                this.Category = new List<string>(value.Table.Values.Select(v => v.CastToString()));
        }

    }
}
