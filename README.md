# Guild Wars 2 Plugin for Open Broadcaster Software
[![Build status](https://ci.appveyor.com/api/projects/status/r46833nfb6cf76jk?svg=true)](https://ci.appveyor.com/project/Archomeda/obs-gw2-plugin)

In short, this plugin adds some Guild Wars 2 related stuff in Open Broadcaster
Software that you can use as text source. For more information about this
plugin, visit the site at http://archomeda.github.io/OBS-GW2-Plugin/. This
readme contains developer related information only.


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
