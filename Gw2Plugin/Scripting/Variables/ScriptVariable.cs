using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GW2DotNET;
using MoonSharp.Interpreter;
using ObsGw2Plugin.Scripting.Exceptions;
using ObsGw2Plugin.Extensions.GW2DotNET;

namespace ObsGw2Plugin.Scripting.Variables
{
    public class ScriptVariable : ScriptBase, IScriptVariable
    {
        public ScriptVariable() : base() { }

        private ServiceManager apiServiceManager = new ServiceManager();


        protected void RequestAPIAsync<TResult>(Func<TResult> apiFunc, Action<TResult> callback)
        {
            ThreadPool.QueueUserWorkItem(o => callback(apiFunc()));
        }

        protected void RequestAPIAsync<T, TResult>(Func<T, TResult> apiFunc, T parameter, Action<TResult> callback)
        {
            ThreadPool.QueueUserWorkItem(o => callback(apiFunc(parameter)));
        }

        protected void RequestAPIAsync<T1, T2, TResult>(Func<T1, T2, TResult> apiFunc, T1 parameter1, T2 parameter2, Action<TResult> callback)
        {
            ThreadPool.QueueUserWorkItem(o => callback(apiFunc(parameter1, parameter2)));
        }

        protected override void InitGlobals()
        {
            base.InitGlobals();

            var apiTable = new Dictionary<string, object>()
            {
                // Map information
                { "continents", new Action<Closure>(
                    callback => this.RequestAPIAsync(this.apiServiceManager.GetContinents,
                        continents => this.UpdateCachedVariable(callback, continents.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToDictionary())))) },
                { "map", new Action<int, Closure>(
                    (mapId, callback) => this.RequestAPIAsync(this.apiServiceManager.GetMap, mapId,
                        map => this.UpdateCachedVariable(callback, map.ToDictionary()))) },
                { "map_floor", new Action<int, int, Closure>(
                    (continentId, floorId, callback) => this.RequestAPIAsync(this.apiServiceManager.GetMapFloor, continentId, floorId,
                        mapFloor => this.UpdateCachedVariable(callback, mapFloor.ToDictionary()))) },
                
                // Miscellaneous
                { "build", new Action<Closure>(
                    callback => this.RequestAPIAsync(this.apiServiceManager.GetBuild,
                        build => this.UpdateCachedVariable(callback, build.ToDictionary()))) }
            };

            this.LuaScript.Globals["gw2api"] = apiTable;
        }

    }
}
