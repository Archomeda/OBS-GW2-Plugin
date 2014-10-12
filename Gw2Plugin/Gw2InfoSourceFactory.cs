using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLROBS;

namespace ObsGw2Plugin
{
    class Gw2InfoSourceFactory : AbstractImageSourceFactory
    {
        public Gw2InfoSourceFactory()
        {
            this.ClassName = "Gw2InfoSource";
            this.DisplayName = "Guild Wars 2 Info";
        }

        public override ImageSource Create(XElement data)
        {
            return new Gw2InfoSource(data);
        }

        public override bool ShowConfiguration(XElement data)
        {
            Gw2PluginConfigurationDialog dialog = new Gw2PluginConfigurationDialog(data);
            return dialog.ShowDialog().GetValueOrDefault(false);
        }
    }
}
