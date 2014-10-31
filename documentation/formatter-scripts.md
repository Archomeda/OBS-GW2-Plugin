---
layout: docpage
title: Formatter scripts
permalink: /documentation/formatter-scripts/
group: docnavigation
docnavid: formatter-scripts
navid: documentation
navweight: 2
---
{% include urls.md %}

As noted in the [introduction][api-introduction], formatter scripts are used to format data before outputting it to the livestream. A formatter script will replace a certain keyword (here `id`) that is surround with percent signs. Unless specified otherwise, you need to refer to formatter scripts by its id surrounded with percent signs (e.g. `%formatter%` instead of just `formatter`). This is done to provide consistency with the configuration window of the plugin.


Formatter scripts are Lua scripts that are located in the `ScriptFormatters` directory, both built-in and custom created. Go to the [GitHub repository][githubrepo-apiformatterscripts]{:target="_blank"} to view the built-in and example formatter scripts.


## File structure
Every formatter script has to have a certain list of globals defined, otherwise the script will not load. Every global is required, unless specified otherwise.

{% include doc_definition/global.html name="id" type="string" %}
The id that is used to uniquely identify a script. This id is also used to refer from other scripts in e.g. `hooks` and `getvar()`.

{% include doc_definition/global.html name="name" type="string" %}
The name is used in the configuration window of the plugin to provide a better understanding for users what the script actually represents.

{% include doc_definition/global.html name="category" type="table of strings (optional)" %}
Formatter scripts are categorized into submenus. This will prevent cluttering of the selection menu in the configuration window. The script is categorized first-to-last entry from the given table. If this global is not defined, the script will be placed in the root category.

{% include doc_definition/global.html name="hooks" type="table of strings (optional)" %}
A script can hook onto (a.k.a. subscribe to) the event when another value changes. This can be the value of another variable or formatter script or a [built-in variable][api-builtinvariables]. When a change occurs, `update()` will be called. This table should contain ids of other scripts or built-in variables.

{% include doc_definition/global.html name="update()" type="function" %}
This is the main entry point of the script. In this function the script needs to return its updated value. Please note that it is not recommended to update the value of a formatter script every frame. This will cause the plugin to update the imaged version of the text at a very high rate which may cause a major frame rate drop in OBS.


## Exposed globals
A formatter script can call certain globals that provide more functionality than Lua itself:

{% include doc_definition/global.html name="localvar(id, value)" type="function" %}
Gets or sets a local variable of the script. This can be used to preserve variables throughout the lifetime of the plugin, since every defined local or global in the script will vanish after each run.<br>
**id:** An unique id to identify the variable with.<br>
**value:** The value of the variable to set; use `nil` or omit if you want to get a variable instead.<br>
**Returns** the value of the variable if *value* is `nil`; `nil` otherwise.

{% include doc_definition/global.html name="getcurrent()" type="function" %}
Gets the current cached value of the executing script.

{% include doc_definition/global.html name="getvar(id)" type="function" %}
Gets the cached value of another variable script or a [built-in variable][api-builtinvariables]. Note that you cannot get the value of a formatter script through this function.
Use `gettext(id)` if you want to do this.<br>
**id:** The id of the variable script or built-in variable.<br>
**Returns** the cached value.

{% include doc_definition/global.html name="gettext(id)" type="function" %}
Gets the cached value of another formatter script.<br>
**id:** The id of the formatter script *without the surrounding percent signs*.<br>
**Returns** the cached value.

{% include doc_definition/global.html name="timestamp()" type="function" %}
Gets the current Unix timestamp. Since Lua has no built-in support for getting the current time, this has been wrapped in C#. The returned value might include both whole and fractional seconds.
