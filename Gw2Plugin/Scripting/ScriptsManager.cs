using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using ObsGw2Plugin.Extensions;
using ObsGw2Plugin.MumbleLink;
using ObsGw2Plugin.Scripting.Formatters;
using ObsGw2Plugin.Scripting.Variables;

namespace ObsGw2Plugin.Scripting
{
    public class ScriptsManager : IScriptsManager
    {
        private IDictionary<string, IScriptVariable> scriptVariables = new Dictionary<string, IScriptVariable>();
        private IDictionary<string, IScriptFormatter> scriptFormatters = new Dictionary<string, IScriptFormatter>();

        public IMumbleLinkFile MumbleLinkFile { get; protected set; }


        public virtual IScriptVariable GetScriptVariable(string id)
        {
            return this.scriptVariables[id];
        }

        public virtual IScriptFormatter GetScriptFormatter(string id)
        {
            return this.scriptFormatters[id];
        }

        public virtual void RegisterScriptVariable(IScriptVariable scriptVariable)
        {
            this.scriptVariables[scriptVariable.Id] = scriptVariable;
            scriptVariable.ScriptsManager = this;
        }

        public virtual void RegisterScriptFormatter(IScriptFormatter scriptFormatter)
        {
            this.scriptFormatters[scriptFormatter.Id] = scriptFormatter;
            scriptFormatter.ScriptsManager = this;
        }

        public virtual void UnregisterScriptVariable(string id)
        {
            this.scriptVariables.Remove(id);
        }

        public virtual void UnregisterScriptFormatter(string id)
        {
            this.scriptFormatters.Remove(id);
        }


        public IList<IScriptVariable> GetListOfScriptVariables()
        {
            return this.scriptVariables.Values.ToList();
        }

        public IList<IScriptFormatter> GetListOfScriptFormatters()
        {
            return this.scriptFormatters.Values.ToList();
        }


        public void UseMumbleLinkFile(IMumbleLinkFile mumbleLinkFile)
        {
            this.MumbleLinkFile = mumbleLinkFile;
            this.MumbleLinkFile.PropertyChanged += MumbleLinkFile_PropertyChanged;
        }

        private void MumbleLinkFile_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifySubscribersToUpdate(e.PropertyName);
        }


        public virtual object GetCachedResult(string id)
        {
            if (!id.StartsWith("%") || !id.EndsWith("%"))
            {
                switch (id)
                {
                    case "UIVersion": return this.MumbleLinkFile.UIVersion;
                    case "UITick": return this.MumbleLinkFile.UITick;
                    case "Name": return this.MumbleLinkFile.Name;
                    case "AvatarPosition": return this.MumbleLinkFile.AvatarPosition.ToDictionary();
                    case "AvatarFront": return this.MumbleLinkFile.AvatarFront.ToDictionary();
                    case "AvatarTop": return this.MumbleLinkFile.AvatarTop.ToDictionary();
                    case "CameraPosition": return this.MumbleLinkFile.CameraPosition.ToDictionary();
                    case "CameraFront": return this.MumbleLinkFile.CameraFront.ToDictionary();
                    case "CameraTop": return this.MumbleLinkFile.CameraTop.ToDictionary();
                    case "Description": return this.MumbleLinkFile.Description;
                }

                if (this.MumbleLinkFile is Gw2MumbleLinkFile)
                {
                    Gw2MumbleLinkFile gw2MumbleLinkFile = (Gw2MumbleLinkFile)this.MumbleLinkFile;
                    switch (id)
                    {
                        case "CharacterName": return gw2MumbleLinkFile.CharacterName;
                        case "ProfessionId": return gw2MumbleLinkFile.ProfessionId;
                        case "MapId": return gw2MumbleLinkFile.MapId;
                        case "WorldId": return gw2MumbleLinkFile.WorldId;
                        case "TeamColorId": return gw2MumbleLinkFile.TeamColorId;
                        case "IsCommander": return gw2MumbleLinkFile.IsCommander;
                        case "ServerAddress": return gw2MumbleLinkFile.ServerAddress;
                        case "MapType": return gw2MumbleLinkFile.MapType;
                        case "ShardId": return gw2MumbleLinkFile.ShardId;
                        case "Instance": return gw2MumbleLinkFile.Instance;
                        case "BuildId": return gw2MumbleLinkFile.BuildId;
                    }
                }

                if (this.scriptVariables.ContainsKey(id))
                {
                    IScriptVariable script = this.scriptVariables[id];
                    if (!script.HasCachedVariable)
                    {
                        if (script.UpdateCachedVariable())
                            this.NotifySubscribersToUpdate(script);
                    }
                    return script.CachedVariable;
                }
            }
            else
            {
                id = id.Substring(1, id.Length - 2);
                if (this.scriptFormatters.ContainsKey(id))
                {
                    IScriptFormatter script = this.scriptFormatters[id];
                    if (!script.HasCachedVariable)
                    {
                        if (script.UpdateCachedVariable())
                            this.NotifySubscribersToUpdate(script);
                    }
                    return script.CachedVariable;
                }
            }

            return null;
        }


        private void NotifyVariableSubscribersToUpdate(string id)
        {
            IEnumerable<string> variableIds = this.scriptVariables
                .Where(kvp => kvp.Value.Hooks != null && kvp.Value.Hooks.Contains(id))
                .Select(kvp => kvp.Key);
            foreach (string variableId in variableIds)
            {
                if (this.scriptVariables[variableId].UpdateCachedVariable())
                    this.NotifySubscribersToUpdate(this.scriptVariables[variableId]);
            }
        }

        private void NotifyFormatterSubscribersToUpdate(string id)
        {
            IEnumerable<string> formatterIds = (this.scriptFormatters
                .Where(kvp => kvp.Value.Hooks != null && kvp.Value.Hooks.Contains(id))
                .Select(kvp => kvp.Key));
            foreach (string formatterId in formatterIds)
            {
                if (this.scriptFormatters[formatterId].UpdateCachedVariable())
                    this.NotifySubscribersToUpdate(this.scriptFormatters[formatterId]);
            }

        }

        public virtual void NotifySubscribersToUpdate(string id)
        {
            this.NotifyVariableSubscribersToUpdate(id);
            this.NotifyFormatterSubscribersToUpdate(id);
        }

        public virtual void NotifySubscribersToUpdate(IScriptVariable sourceScript)
        {
            this.NotifyVariableSubscribersToUpdate(sourceScript.Id);
            this.NotifyFormatterSubscribersToUpdate(sourceScript.Id);
        }

        public virtual void NotifySubscribersToUpdate(IScriptFormatter sourceScript)
        {
            this.NotifyFormatterSubscribersToUpdate("%" + sourceScript.Id + "%");
        }


        public virtual string FormatString(string input)
        {
            return Regex.Replace(input, "(%[^%]*%)", match =>
            {
                string id = match.Groups[1].Value;
                object result = this.GetCachedResult(id);
                if (result is DynValue)
                    return ((DynValue)result).CastToString();
                else if (result != null)
                    return result.ToString();
                else
                    return id;
            });
        }

    }
}
