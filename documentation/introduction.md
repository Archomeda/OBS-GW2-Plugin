---
layout: docpage
title: Introduction
permalink: /documentation/introduction/
group: docnavigation
docnavid: introduction
navid: documentation
navweight: 0
---
{% include urls.md %}

<span class="label label-danger">Beware!</span> **The API is still in active development. It may change a couple of times before the final version of the plugin has been released.**

*Please note that this documentation assumes that you have basic knowledge of the [Lua programming language][lua]{:target="_blank"}.*

Lua is the programming language that used in API part of the plugin. Because the used interpreter is a third party library called Moon#, the supported functions may differ somewhat. Please go to the [website of Moon#][moonsharp]{:target="_blank"} for more information about this.

Built-in scripts are located in the `Gw2Plugin` directory within the plugin installation directory. You can use those scripts as a reference to make your own. Custom created scripts need to be placed inside `%APPDATA%\OBS\pluginData\Gw2Plugin`.

There are two different script types: [variables][api-variablescripts] and [formatters][api-formatterscripts]. Variable scripts are used to provide data to other variable or formatter scripts, while formatter scripts are used to format this data before outputting it to the livestream. This is also the reason why the data from formatter scripts cannot be accessed by variable scripts.

In case you want to overwrite or extend the functionality of an existing script, you can just create a new script with the same id as the existing one. This will cause the plugin to load your custom script instead of the existing script.

If you've made your own script and you want to share it with the community because you think it's really awesome, [fork the project on GitHub][githubrepo]{:target="_blank"}, add your script, and submit a pull request with detailed information how to use it. I'll review it to see if it's viable to include it.
