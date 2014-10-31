---
layout: docpage
title: Official Guild Wars 2 API
permalink: /documentation/gw2api/
group: docnavigation
docnavid: gw2api
navid: documentation
navtitle: Official GW2 API
navweight: 4
---
{% include urls.md %}

In order to get more useful information from the [built-in variables][api-builtinvariables], you can request data from the [official Guild Wars 2 API][gw2api]{:target="_blank"}. Only useful API endpoints are supported. This means that getting the current price of an item is not supported, because it's not very usable within a livestream.

The functions can be called from the global `gw2api` which is only available to [variable scripts][api-variablescripts]. Almost every return value is a table that is formatted the same way as the API. There are a few exceptions which you can find at the end of this page.


## Location
{% include doc_definition/global.html name="gw2api.continents(callback)" type="function" %}
Gets the [available continents][gw2api-continents]{:target="_blank"}.<br>
**callback:** A callback function that will be called when the request has been completed.<br>
**Returns** a table with all available continents [^1].

{% include doc_definition/global.html name="gw2api.map(map_id, callback)" type="function" %}
Gets a [map][gw2api-maps]{:target="_blank"} by its id.<br>
**map_id:** The map id.<br>
**callback:** A callback function that will be called when the request has been completed.<br>
**Returns** a table with the map details [^2].

{% include doc_definition/global.html name="gw2api.map_floor(continent_id, floor_id, callback)" type="function" %}
Gets a [map floor][gw2api-mapfloor]{:target="_blank"} by its id.<br>
**continent_id:** The continent id.<br>
**floor_id:** The floor id.<br>
**callback:** A callback function that will be called when the request has been completed.<br>
**Returns** a table with the map floor details [^1] [^2] [^3].


## Miscellaneous
{% include doc_definition/global.html name="gw2api.build(callback)" type="function" %}
Gets the current [server build][gw2api-build]{:target="_blank"}.<br>
**callback:** A callback function that will be called when the request has been completed.<br>
**Returns** a table with the server build details.


## Remarks
[^1]: Dimension properties differ from the official API. Instead of an indexed array, it's a table with the keys `width` and `height`.
[^2]: Rectangle properties differ from the official API. Instead of two indexed arrays, it's a table with the keys `x`, `y`, `width` and `height`.
[^3]: Coordinate properties differ from the official API. Instead of an indexed array, it's a table with the keys `x` and `y`.
