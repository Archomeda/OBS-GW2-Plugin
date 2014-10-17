using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using ObsGw2Plugin.Scripting.Exceptions;

namespace ObsGw2Plugin.Scripting
{
    public abstract class ScriptBase : IScriptWithCachedValue, IScriptWithCachedLocals, IHookableScript
    {
        public ScriptBase()
        {
            this.LuaScript = new Script();
            this.Locals = new Dictionary<string, DynValue>();
        }


        public string Id { get; protected set; }

        public DynValue CachedVariable { get; protected set; }

        public IDictionary<string, DynValue> Locals { get; protected set; }

        public ISet<string> Hooks { get; protected set; }

        public string LuaScriptFilename { get; protected set; }

        public Script LuaScript { get; protected set; }

        public IScriptsManager ScriptsManager { get; set; }


        public virtual void InitScript(string scriptFilename)
        {
            this.InitGlobals();
            this.LuaScriptFilename = scriptFilename;
            this.LuaScript.DoFile(this.LuaScriptFilename);
            this.InitObjectProperties();
        }

        protected virtual void InitGlobals()
        {
            this.LuaScript.Globals["localvar"] = new Func<string, DynValue, DynValue>((key, value) =>
            {
                if (value.IsNil() && this.Locals.ContainsKey(key))
                    return this.Locals[key];
                else if (!value.IsNil())
                    this.Locals[key] = value;
                return null;
            });
            this.LuaScript.Globals["getcurrent"] = new Func<DynValue>(() => this.CachedVariable);
            this.LuaScript.Globals["getvar"] = new Func<string, object>(id => this.ScriptsManager.GetCachedResult(id));
            this.LuaScript.Globals["timestamp"] = new Func<double>(() => (DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds);
        }

        protected virtual void InitObjectProperties()
        {
            DynValue value = this.LuaScript.Globals.Get("id");
            if (value.Type != DataType.String)
                throw new MissingGlobalException(this.LuaScriptFilename, "id");
            this.Id = value.CastToString();

            value = this.LuaScript.Globals.Get("hooks");
            if (value.Type == DataType.Table)
                this.Hooks = new HashSet<string>(value.Table.Values.Select(v => v.CastToString()));
        }


        public virtual DynValue GetLiveVariable()
        {
            DynValue function = this.LuaScript.Globals.Get("update");
            if (function.Type != DataType.Function)
                throw new MissingGlobalException(this.LuaScriptFilename, "update");

            return this.LuaScript.Call(function);
        }

        public virtual bool UpdateCachedVariable()
        {
            DynValue oldCachedValue = this.CachedVariable;
            this.CachedVariable = this.GetLiveVariable();
            return object.Equals(this.CachedVariable, oldCachedValue);
        }

    }
}
