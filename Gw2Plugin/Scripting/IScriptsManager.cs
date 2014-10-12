using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using ObsGw2Plugin.MumbleLink;
using ObsGw2Plugin.Scripting.Formatters;
using ObsGw2Plugin.Scripting.Variables;

namespace ObsGw2Plugin.Scripting
{
    public interface IScriptsManager
    {
        IMumbleLinkFile MumbleLinkFile { get; }


        IScriptVariable GetScriptVariable(string id);

        IScriptFormatter GetScriptFormatter(string id);

        void RegisterScriptVariable(IScriptVariable scriptVariable);

        void RegisterScriptFormatter(IScriptFormatter scriptFormatter);

        void UnregisterScriptVariable(string id);

        void UnregisterScriptFormatter(string id);

        IList<IScriptVariable> GetListOfScriptVariables();

        IList<IScriptFormatter> GetListOfScriptFormatters();


        void UseMumbleLinkFile(IMumbleLinkFile mumbleLinkFile);


        object GetCachedResult(string id);


        void NotifySubscribersToUpdate(string id);

        void NotifySubscribersToUpdate(IScriptVariable sourceScript);

        void NotifySubscribersToUpdate(IScriptFormatter sourceScript);


        string FormatString(string input);

    }
}
