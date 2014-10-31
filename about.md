---
layout: page
title: About
permalink: /about/
group: navigation
navid: about
navweight: 1
---
{% include urls.md %}

This is a [Guild Wars 2][gw2]{:target="_blank"} plugin for [Open Broadcaster Software][obs]{:target="_blank"} written in C#. It allows you to add Guild Wars 2 data to your livestream; for example your character name, profession, game mode, current location, team color, fps and more. The reason why this plugin is unique, is that it will update this data automatically while you are busy doing stuff in Tyria or the Mists. How? It monitors the Mumble Link API which Guild Wars 2 [officially supports][gw2mumble]{:target="_blank"} and when something has changed, the plugin updates it on your livestream.

You can fully customize the text that will be shown on the livestream. And best of all, if you think there's something missing, you can extend the functionality by scripting your own variables! You can even use relevant information directly from the [Guild Wars 2 API][gw2api]{:target="_blank"}! You can make it as advanced as you'd like. If are into this, it's highly recommended to read the plugin [API documentation][api-doc].

## Current features
- Customize your own text based on live Guild Wars 2 variables; a selection of the current supported variables are: character name, profession name, game mode, team color, fps, build id and server IP (more are available in scripts)
- Supports the [official Guild Wars 2 API][gw2api]{:target="_blank"}
- Change the text style to something you like the most
- Extend the functionality by scripting your own variables and formatters if the built-in ones are not enough (be sure to check out the [API documentation][apidoc])
- Checks for new releases on GitHub automatically in a non-intrusive way

If you want to contribute to this project, go to [GitHub][githubrepo]{:target="_blank"}. You can request new features, submit bug reports or fork the project in order to do things yourself. You can even fork this site by branching into gh-pages, if you believe there's an error somewhere here.

See you in the Mists!
