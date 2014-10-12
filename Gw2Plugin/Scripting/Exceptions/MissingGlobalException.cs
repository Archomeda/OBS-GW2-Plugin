using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObsGw2Plugin.Scripting.Exceptions
{
    public class MissingGlobalException : Exception
    {
        public MissingGlobalException(string scriptFilename, string globalName) : this(scriptFilename, globalName, null) { }

        public MissingGlobalException(string scriptFilename, string globalName, Exception innerException)
            : base(string.Format("Global \"{0}\" missing in \"{1}\"", globalName, scriptFilename), innerException)
        {
            this.ScriptFilename = scriptFilename;
            this.GlobalName = globalName;
        }

        public string ScriptFilename { get; set; }

        public string GlobalName { get; set; }
    }
}
