---
layout: docpage
title: Variable scripts
permalink: /documentation/variable-scripts/
group: docnavigation
docnavid: variable-scripts
navid: documentation
navweight: 1
---
{% include urls.md %}

As noted in the [introduction][api-introduction], variable scripts are used to provide data to other variable or formatterscripts. Variable scripts are only used within scripts and cannot be used in the livestream directly.

Variable scripts are Lua scripts that are located in the `ScriptVariables` directory, both built-in and custom created. Go to the [GitHub repository][githubrepo-apivariablescripts]{:target="_blank"} to view the built-in and example variable scripts.


## File structure
Every variable script has to have a certain list of globals defined, otherwise the script will not load.

<span class="infoblock infoblock-info">
<span class="label label-info">Note</span><br>
Every global is required, unless otherwise specified.
</span>

{% include doc_definition/global.html name="id" type="string" %}
The id that is used to uniquely identify a script. This id is also used to refer from other scripts in e.g. `hooks` and `getvar()`.

{% include doc_definition/global.html name="hooks" type="table of strings (optional)" %}
A script can hook onto (a.k.a. subscribe to) the event when another value changes. This can be the value of another variable script or a [built-in variable][api-builtinvariables]. When a change occurs, `update()` will be called. Note that you cannot hook onto a formatter script from a variable script. This table should contain ids of
other scripts or built-in variables.

{% include doc_definition/global.html name="update()" type="function" %}
This is the main entry point of the script. In this function the script needs to return its updated value.


## Exposed globals
A variable script can call certain globals that provide more functionality than Lua itself:

{% include doc_definition/global.html name="localvar(id, value)" type="function" %}
Gets or sets a local variable of the script. This can be used to preserve variables throughout the lifetime of the plugin, since every defined local or global in the script will vanish after each run.<br>
**id:** An unique id to identify the variable with.<br>
**value:** The value of the variable to set; use `nil` or omit if you want to get a variable instead.<br>
**Returns** the value of the variable if *value* is `nil`; `nil` otherwise.

{% include doc_definition/global.html name="getcurrent()" type="function" %}
Gets the current cached value of the executing script.

{% include doc_definition/global.html name="getvar(id)" type="function" %}
Gets the cached value of another variable script or a [built-in variable][api-builtinvariables]. Note that you cannot get the value of a formatter script from a variable script.<br>
**id:** The id of the variable script or built-in variable.<br>
**Returns** the cached value.

{% include doc_definition/global.html name="timestamp()" type="function" %}
Gets the current Unix timestamp. Since Lua has no built-in support for getting the current time, this has been wrapped in C#. The returned value might include both whole and fractional seconds.

Other exposed globals include support of the
[official Guild Wars 2 API][api-gw2api].
