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

In order to get more useful information from the [built-in variables][api-builtinvariables], you can request data from the [official Guild Wars 2 API][gw2api]{:target="_blank"}. Only useful API endpoints are supported. This means that e.g. getting the current price of an item is not supported, because it's not very usable within a livestream. The functions can be called from the global `gw2api` which is only available to [variable scripts][api-variablescripts]. Almost every return value is a table that is formatted the same way as the API. There are a few exceptions which you can find at the end of this page.

<span class="infoblock infoblock-info">
<span class="label label-info">Note</span><br>
All functions are asynchronous, which means that you need to give a function as callback parameter that will be called when the API request has been completed. In order to update the cached value of the variable script, return the updated value in this function.
</span>

<span class="infoblock infoblock-warning">
<span class="label label-warning">Important</span><br>
Cache your data! Every time you request data, a new request is sent to the Guild Wars 2 API server. This means that if you expect that certain data does not change frequently, you need to cache this data yourself by using the `localvar(id, value)` function. If you do not do this, it's possible that you request the same data over and over again in a short amount of time. You can use the `timestamp()` function or the current build to verify the validity of your cached data.
</span>


## Location
{% include doc_definition/global.html name="gw2api.continents(callback)" type="function" %}
Gets the [available continents][gw2api-continents]{:target="_blank"} asynchronously.<br>
**callback:** A callback function that will be called when the request has been completed. This function needs to accept one parameter, this parameter is a table with all available continents [^1]. In this function, return the updated value of the variable script.<br>
**Returns** `nil` (see callback).

{% include doc_definition/global.html name="gw2api.map(map_id, callback)" type="function" %}
Gets a [map][gw2api-maps]{:target="_blank"} by its id asynchronously.<br>
**map_id:** The map id.<br>
**callback:** A callback function that will be called when the request has been completed. This function needs to accept one parameter, this parameter is a table with the map details [^2]. In this function, return the updated value of the variable script.<br>
**Returns** `nil` (see callback).

{% include doc_definition/global.html name="gw2api.map_floor(continent_id, floor_id, callback)" type="function" %}
Gets a [map floor][gw2api-mapfloor]{:target="_blank"} by its id asynchronously.<br>
**continent_id:** The continent id.<br>
**floor_id:** The floor id.<br>
**callback:** A callback function that will be called when the request has been completed. This function needs to accept one parameter, this parameter is a table with the map floor details [^1] [^2] [^3]. In this function, return the updated value of the variable script.<br>
**Returns** `nil` (see callback).


## Miscellaneous
{% include doc_definition/global.html name="gw2api.build(callback)" type="function" %}
Gets the current [server build][gw2api-build]{:target="_blank"} asynchronously.<br>
**callback:** A callback function that will be called when the request has been completed. This function needs to accept one parameter, this parameter is a table with the server build details. In this function, return the updated value of the variable script.<br>
**Returns** `nil` (see callback).


## Remarks
[^1]: Dimension properties differ from the official API. Instead of an array, it's a table with the keys `width` and `height`.
[^2]: Rectangle properties differ from the official API. Instead of two arrays within an array, it's a table with the keys `x`, `y`, `width` and `height`.
[^3]: Coordinate properties differ from the official API. Instead of an array, it's a table with the keys `x` and `y`.
