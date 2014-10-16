# Guild Wars 2 Plugin for Open Broadcaster Software
[![Build status](https://ci.appveyor.com/api/projects/status/r46833nfb6cf76jk?svg=true)](https://ci.appveyor.com/project/Archomeda/obs-gw2-plugin)

In short, this plugin adds some Guild Wars 2 related stuff in Open Broadcaster
Software that you can use as text source. It features the basic configuration
you're already familiar with, but better.

If you're used to the default text source, you should be able to understand this
plugin quickly. This plugin adds another way of putting text on your video, but
it also adds some Guild Wars 2 related things like your character name. This is
done live, so for example, when you're switching over to another character, the
text updates automatically. But not only that, there are more triggers for when
a text can be updated. If you feel there's something missing that you would like
to have included, like displaying your race name automatically, you can add it
yourself! You can script your own formatters and variables in Lua!

- [Features](#features)
- [Requirements](#requirements)
- [Downloads](#downloads)
- [Installation](#installation)
- [Contribute](#contribute)
- [Planned features](#planned-features)
- [Legal stuff](#legal-stuff)


## Features
- Customize your own text based on live Guild Wars 2 variables; a selection of
the current supported variables are: character name, profession name, game mode,
team color, fps, build id and server IP (more are available in scripts)
- Change the text style to something you like best
- Extend the functionality by scripting your own variables and formatters if the
built-in ones are not enough (be sure to check the
`plugins/Gw2Plugin/README.txt` file in the installation folder of OBS when this
plugin is installed for more information)
- Checks for new releases on GitHub automatically in a non-intrusive way
- More planned...


## Requirements
- Windows Vista SP2+, 7 SP1+, or 8 (32-bit or 64-bit) with [Open Broadcaster
Software](https://obsproject.com/)
- [.NET Framework 4.5](http://www.microsoft.com/download/details.aspx?id=30653)
- [CLR Host Plugin](https://obsproject.com/forum/resources/clr-host-plugin.21/)
- Guild Wars 2 (well... obviously)


## Downloads
*As this plugin is in active development and not yet ready for a final release,
updates may happen frequently. Because of this, downloads are not yet available
on GitHub and you need to check for development releases manually.*

You can find the [latest development releases on AppVeyor](https://ci.appveyor.com/project/Archomeda/obs-gw2-plugin/history).
Just click on the latest version that has been successfully built (be sure that
you select the latest build from the master branch, unless you want otherwise).
From there, go to *Configuration: Release*, *Artifacts* and download the zip
archive that contains the plugin.

If, for some reason, the latest build does not work as intended (they are
development builds for a reason!), you can always try an older version.


## Installation
*Be sure that you have downloaded and installed the
[CLR Host Plugin](https://obsproject.com/forum/resources/clr-host-plugin.21/)
before installing this plugin.*

After downloading the plugin, go to your Open Broadcaster Software installation
directory. Go into the directory `plugins/CLRHostPlugin` and unzip the
contents of the zip file. It's possible that you will be asked to overwrite
`CLRHost.Interop.dll`, please do so. This plugin uses a slightly newer version
than the version that is included in the CLR Host Plugin. If you happen to use
the [CLR Browser Source Plugin](https://obsproject.com/forum/resources/clr-browser-source-plugin.22/)
as well, don't worry, that plugin uses the same version of the file.


## Contribute
If you want to contribute to the project, make a fork, change stuff and do a
pull request. I'll review whether or not it's awesome what you've accomplished.
After that I'll decide if it's viable to include it.

Things that are always awesome:
- Awesome features
- More useful scripts (be it example or default scripts)
- More useful unit tests
- Bugfixes if you encounter a bug
- Spelling and/or grammar corrections (English is not my native language and I'm
bound to make mistakes somewhere)


## Planned features
*Please note that, although these features are planned, I cannot guarantee when
it will be implemented, if at all. I also cannot guarantee that it will be
implemented as advertised; everything is subject to change.*

### Guild Wars 2 API
Supporting the official Guild Wars 2 API is an important feature. For example,
it will allow you to get the name of a map you're currently exploring instead of
the raw id (which is not really useful in streams). Things that are not really
valuable to you as a streamer/recorder, like colors, commerce, items, recipes,
quaggans (yes, even those!), etc., will not be added. Things I have in mind are
for example maps, WvW, events (need to wait to see what v2 will provide).

### Multiple lines of text
Instead of having one big line of text, the idea is to split it into multiple
different lines. This will allow you to:
- Select when a line of text should be visible based on a specific variable (for
example, show line A only when in PvE and show line B only when in WvW)
- Edit how long a line should be visible, based on text length and/or time
- Hide certain lines when the Mumble Link is not available
- Hide lines that have no text at the moment
- Provide some nice, basic transitions between the lines

In order to do this, a new user interface for the configuration window is
needed.

### More example scripts
Yes, there are not enough example scripts at the moment. Example scripts are
basically there to give you a general idea of how scripting works and to
(hopefully) stimulate your brain to give you some new, nice ideas for your own
script. And who knows? Maybe if your script is very useful, throw me an issue or
a pull request to have me include your script! Just be sure you add some
reasoning on why it should definitely be added ;)


## Legal stuff
In order to get the up-to-date information from Guild Wars 2, this plugin uses the Mumble Link API (which Guild Wars 2 officially supports). This method does **not** interact with the game in any way and therefore it should be safe to use. Check the [thread on the Guild Wars 2 forums](https://forum-en.guildwars2.com/forum/community/api/Map-API-Mumble-Mashup/first#post2256444) for more information.
