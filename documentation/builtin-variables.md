---
layout: docpage
title: Built-in variables
permalink: /documentation/builtin-variables/
group: docnavigation
docnavid: builtin-variables
navid: documentation
navweight: 3
---
{% include urls.md %}

The plugin is able to read the [Mumble Link API][mumblelinkapi]{:target="_blank"} that Guild Wars 2 [officially supports][gw2mumble]{:target="_blank"}. This allows you to read live data from the stuff you are currently doing. Without this ability, this plugin would not have existed.

Below is a list of the variables that you can access in both [variable][api-variablescripts] and [formatter scripts][api-formatterscripts] by calling `getvar(id)`. Please note that when the Mumble Link is not available, some variables can be `nil` or arbitrary.


## Character info
{% include doc_definition/global.html name="CharacterName" type="string" %}
The name of the character that is currently active in Guild Wars 2.

{% include doc_definition/global.html name="ProfessionId" type="number" %}
The profession id of the character.

{% include doc_definition/global.html name="MapId" type="number" %}
The id of the map where the character is currently in.

{% include doc_definition/global.html name="WorldId" type="number" %}
The shard id of the map (note that, since the introduction of the Megaservers, this does not equal the world id anymore (sadly)).

{% include doc_definition/global.html name="TeamColorId" type="number" %}
The team color id when playing PvP or WvW.

{% include doc_definition/global.html name="IsCommander" type="boolean" %}
True when the character is currently a commander; false otherwise.

{% include doc_definition/global.html name="ServerAddress" type="1-indexed table with 4 elements (numbers)" %}
The IP address of the current server.

{% include doc_definition/global.html name="MapType" type="number" %}
The type of the map.

{% include doc_definition/global.html name="ShardId" type="number" %}
The shard id of the map.

{% include doc_definition/global.html name="BuildId" type="number" %}
The current build id of the client.


## Positioning
{% include doc_definition/global.html name="AvatarPosition" type="table with elements x, y, z (numbers)" %}
Represents the current position of the character. This is measured in meters and left-handed coordinates.

{% include doc_definition/global.html name="AvatarFront" type="table with elements x, y, z (numbers)" %}
Represents the current orientation of the character's eyes. This is measured in meters and left-handed coordinates.

{% include doc_definition/global.html name="CameraPosition" type="table with elements x, y, z (numbers)" %}
Same as `AvatarPosition`, but for the camera.

{% include doc_definition/global.html name="CameraFront" type="table with elements x, y, z (numbers)" %}
Same as `AvatarFront`, but for the camera.


## Miscellaneous
{% include doc_definition/global.html name="UITick" type="number" %}
Represents the tick of the Mumble Link. Every time the game renders a frame and updates the Mumble Link, this value is increased with 1.

{% include doc_definition/global.html name="UIVersion" type="number" %}
Represents the version of the Mumble Link struct that has been used. Currently, Guild Wars 2 should always set this to 2.

{% include doc_definition/global.html name="Name" type="string" %}
Represents the name of the game that is currently writing to the Mumble Link. When Guild Wars 2 is running, this is always Guild Wars 2.


## Not used or reserved for future use
{% include doc_definition/global.html name="AvatarTop" type="table with elements x, y, z (numbers)" %}
Represents the current orientation if the character were to look straight up. This is measured in meters and left-handed coordinates. Currently not used by Guild Wars 2.

{% include doc_definition/global.html name="CameraTop" type="table with elements x, y, z (numbers)" %}
Same as `AvatarTop`, but for the camera. Currently not used by Guild Wars 2.

{% include doc_definition/global.html name="Description" type="string" %}
Reserved. Currently not used by Guild Wars 2.

{% include doc_definition/global.html name="Instance" type="number" %}
Reserved. Currently not used by Guild Wars 2.
