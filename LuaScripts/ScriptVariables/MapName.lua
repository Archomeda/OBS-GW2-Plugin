id = "MapName"
hooks = { "APIBuildId", "MapId" }

function update()
	-- The API data is static per build, so we cache every map we come across,
	-- as long as the build stays the same.
	local currentBuild = getvar("APIBuildId")
	local lastBuild = localvar("lastAPIBuildId")

	if currentBuild ~= lastBuild then
		-- New build: clear cache
		localvar("mapCache", {})
		localvar("lastAPIBuildId", currentBuild)
	end

	local cache = localvar("mapCache")
	local mapId = getvar("MapId")

	if cache ~= nil and cache[mapId] ~= nil then
		return cache[mapId].map_name
	elseif mapId > 0 then
		gw2api.map(mapId, mapCallback)
		return getcurrent()
	else
		return nil
	end
end

function mapCallback(map)
	local cache = localvar("mapCache")
	if cache == nil then
		cache = {}
	end
	cache[map.map_id] = map
	localvar("mapCache", cache)
	return map.map_name
end
