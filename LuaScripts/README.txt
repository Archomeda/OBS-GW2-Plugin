================================================================================
 Lua script API readme
================================================================================
                                                       Updated on 7 October 2014

Scripts can be used to personalize your text a bit more than what can be done
from within the configuration window. If you are into Lua scripting and want to
create more advanced text, this is definitely something you would want to look
into.

Examples can be found in the respective directories relative to this readme
file.


===================
 Table of contents
===================

1. Lua
2. Creating custom scripts
    2a. Dynamic overrides
    2b. Finding bugs
3. API documentation
    3a. Script structure
    3b. Exposed globals
    3c. Exposed Mumble Link variables
    3d. Referring to other scripts
4. Get started!


========
 1. Lua
========

The language that is used to support scripting, is a variation of Lua 5.2,
called Moon# (http://www.moonsharp.org/). Most standard Lua functions work, but
there are some slight differences that you will encounter only if you make very
advanced scripts.


============================
 2. Creating custom scripts
============================

You need to place your custom made scripts in "%APPDATA%/OBS/pluginData". Create
the following folders if they do not exist already:
"Gw2Plugin/CustomScriptFormatters" and "Gw2Plugin/CustomScriptVariables". The
plugin automatically loads every *.lua file that is in the root folder of both
folders.

If you are convinced that your custom script should be included with the plugin,
convince me as well! File an issue or make a pull request on GitHub
(https://github.com/Archomeda/OBS-GW2-plugin). Explain why your script is
awesome and why should be included by default. Please provide me some use cases
as well so I can understand how and when it can be used. If you don't do that, I
may have a hard time understanding it and a decline can be a result of that.


-----------------------
 2a. Dynamic overrides
-----------------------

The plugin supports dynamic script overrides. This means that if you create a
custom script with an id that is already being used by another script, it
automatically replaces the existing one with the custom one when loading the
plugin in OBS.


------------------
 2b. Finding bugs
------------------

Finding bugs in your script can be tedious. If your script does not load for
some reason, you can find the error in the logs of OBS if it has failed to load
or execute.


======================
 3. API documentation
======================

PLEASE NOTE THAT THIS API IS NOT FINAL AND CAN BE CHANGED AT ANY TIME IN THE
FUTURE.

There are two different types of scripts: VARIABLE scripts and FORMATTER
scripts.
Variable scripts are exactly as the name suggests: each variable script
represents one variable that can be accessed by every other variable or
formatter script. These scripts are not used directly within the plugin
interface and are only used in scripting. Variable scripts cannot access
the value that a formatter script provides, only the value of other variable
scripts.
Formatter scripts are used to replace a certain keyword (that is surrounded with
percent signs) in the given text format string, with the text that the script
will provide. Formatters can access other variables and formatters in order to
provide its own return value.


----------------------
 3a. Script structure
----------------------

Certain Lua globals are mandatory in a script. These globals provide the main
functionality or metadata of the script. For each global it is noted if the
global is mandatory or optional. If no details are given for which script type
it is applied on, it is applied on all script types.

id (string - mandatory):
    The id that is used to uniquely identity a script. Go to [3d] to see how to
    refer to those ids.

name (string - mandatory in formatter, not used in variable):
    The name is used in the configuration window of the plugin to provide a
    better understanding for humans what the script actually represents.

category (table of strings - optional in formatter, not used in variable):
    A script can be categorized into submenus. This is mainly useful to prevent
    cluttering of the formatter selection menu in the configuration window. The
    script is categorized first-to-last entry in the given table. If this global
    is not defined, the script will be placed in the root category.

hooks (table of strings - optional):
    A script can hook onto (or subscribe to) the values of other scripts (or
    variables that are provided from the plugin itself). When that value is
    changed, the update() function will be called so that the script can update
    its own value.
    In order to refer to the plugin variables, see [3c] for more information.
    In order to refer to values of other scripts, see [3d] for more information.
    

update() (function - mandatory):
    This is the main entry point for the script. In this function the script
    needs to return the updated value.
    Please note that it is not recommended to update the value every frame. This
    will cause the plugin to update the imaged version of the text at a very
    high rate. It may cause a major drop in your recording/streaming framerate.


---------------------
 3b. Exposed globals
---------------------

Since scripting without the ability of getting any information of the current
state of the plugin is pretty hard, there are some globals that you can use in
the script. For each global it is noted for which script type it can be used. If
no details are given, it can be used for all script types.

localvar(id, value) (function):
    This can be used to preserve local variables throughout the lifetime of the
    plugin, but it's only accessible by the script that has called this
    function. In order to set a variable, call the function with an id and a
    value. In order to get a variable, call the function with an id only, it
    returns the saved variable.

getcurrent() (function):
    Gets the current cached variable of the executing script.

gettext(id) (function, formatters only):
    Gets the cached value of a formatter, referred by its id. Note that only
    FORMATTERS can be accessed this way. Also note that the id is WITHOUT the
    percent signs.

getvar(id) (function):
    Gets a cached variable, referred by its id. Note that only VARIABLES can be
    accessed this way. Next to script variables, there are also plugin
    variables. See [3c] for more information about referring to those.

timestamp() (function):
    Gets the current Unix timestamp. Since Lua has no built-in support for
    getting the current time, this is wrapped in C#. The returned value might
    include both whole and fractional seconds.


-----------------------------------
 3c. Exposed Mumble Link variables
-----------------------------------

Scripts are nothing without variables. Especially this plugin which aims to
provide live information from the Mumble Link that Guild Wars 2 officially
supports. Some functions have the ability to get these variables, but in order
to get those, you obviously need the respective id.

The following variables are available:
 - UITick (number)
 - UIVersion (number)
 - Name (string)
 - AvatarPosition (table with elements x, y, z (numbers))
 - AvatarFront (table with elements x, y, z (numbers))
 - AvatarTop (table with elements x, y, z (numbers))
 - CameraPosition (table with elements x, y, z (numbers))
 - CameraFront (table with elements x, y, z (numbers))
 - CameraTop (table with elements x, y, z (numbers))
 - Description (string)
 - CharacterName (string)
 - ProfessionId (number)
 - MapId (number)
 - WorldId (number)
 - TeamColorId (number)
 - IsCommander (boolean)
 - ServerAddress (1-indexed table with 4 elements (numbers) that represent an
   IP)
 - MapType (number)
 - ShardId (number)
 - Instance (number)
 - BuildId (number)

When the Mumble Link is not available, it is possible that a variable is nil or
arbitrary.
 

--------------------------------
 3d. Referring to other scripts
--------------------------------

Certain globals need the id of another script or plugin variable. You can refer
to the value of a variable script (or plugin variable) directly by its id. In
order to refer to the value of a formatter script, you need to surround the id
with percent signs (e.g. %id%).

PLEASE NOTE that variable scripts can only refer to other variable scripts (or
plugin variables) and NOT to other formatter scripts. Formatter scripts,
however, can refer to both.


=================
 4. Get started!
=================

If you are still reading this, you probably have no idea what you can possibly
do with all this scripting (that, or you are just reading the readme completely,
which is good!). In order to get you started, there are some examples provided
in the ScriptFormatters and ScriptVariables folders which you can extend or
change. After that, you should have some ideas of your own to make your own
scripts that makes up the text for your stream. It can be as simple as just
copying over some variable to a formatter, or as advanced as different outputs
based on your current location in Guild Wars 2.

Have fun!
