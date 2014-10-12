using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CLROBS;
using ObsGw2Plugin.MumbleLink;
using ObsGw2Plugin.Scripting;
using ObsGw2Plugin.Scripting.Formatters;
using ObsGw2Plugin.Scripting.Variables;

namespace ObsGw2Plugin
{
    public class Gw2Plugin : AbstractPlugin
    {
        public static Gw2Plugin Instance { get; private set; }

        public Gw2Plugin()
        {
            Instance = this;

            this.Name = "Guild Wars 2 Plugin";
            this.Description = "This plugin integrates various Guild Wars 2 features into Open Broadcaster Software.";

            this.MumbleLinkManager = new MumbleLinkManager();
            this.MumbleLinkManager.UseMumbleLinkFile<Gw2MumbleLinkFile>();
            this.ScriptsManager = new ScriptsManager();
            this.ScriptsManager.UseMumbleLinkFile(this.MumbleLinkManager.MumbleLinkFile);
        }

        public MumbleLinkManager MumbleLinkManager { get; protected set; }

        public ScriptsManager ScriptsManager { get; protected set; }


        public override bool LoadPlugin()
        {
            Debug.AttachDebugger();

            API.Instance.AddImageSourceFactory(new Gw2InfoSourceFactory());

            RegisterScriptsInFolder<ScriptVariable>(Path.Combine(AssemblyDirectory, "Gw2Plugin", "ScriptVariables"), this.ScriptsManager.RegisterScriptVariable);
            RegisterScriptsInFolder<ScriptVariable>(Path.Combine(API.Instance.GetPluginDataPath(), "Gw2Plugin", "CustomScriptVariables"), this.ScriptsManager.RegisterScriptVariable);
            RegisterScriptsInFolder<ScriptFormatter>(Path.Combine(AssemblyDirectory, "Gw2Plugin", "ScriptFormatters"), this.ScriptsManager.RegisterScriptFormatter);
            RegisterScriptsInFolder<ScriptFormatter>(Path.Combine(API.Instance.GetPluginDataPath(), "Gw2Plugin", "CustomScriptFormatters"), this.ScriptsManager.RegisterScriptFormatter);

            this.MumbleLinkManager.StartListener();

            return true;
        }


        public override void UnloadPlugin()
        {
            this.MumbleLinkManager.StopListener();
        }


        private void RegisterScriptsInFolder<T>(string path, Action<T> registerAction) where T : IScript, new()
        {
            foreach (string filename in this.EnumerateFiles(path, "*.lua"))
            {
                try
                {
                    T script = new T();
                    script.InitScript(filename);
                    registerAction(script);
                }
                catch (Exception ex)
                {
                    API.Instance.Log("Gw2Plugin: Error while registering script '{0}': {1}", filename, ex.ToString());
                    Debug.BreakDebugger();
                }
            }
        }

        private IEnumerable<string> EnumerateFiles(string path, string searchPattern)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return Directory.EnumerateFiles(path, searchPattern);
        }



        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }


        static Gw2Plugin()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            var name = args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll";
            var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(s => s.EndsWith(name));

            if (!string.IsNullOrEmpty(resourceName))
            {
                API.Instance.Log("Gw2Plugin: Loading embedded assembly '{0}' as '{1}'", name, resourceName);

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    byte[] block = new byte[stream.Length];
                    stream.Read(block, 0, block.Length);
                    return Assembly.Load(block);
                }
            }
            else
                return null;
        }
    }
}
