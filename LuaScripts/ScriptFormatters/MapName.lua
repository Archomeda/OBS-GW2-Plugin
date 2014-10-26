id = "mapname"
name = "Map name"
category = { "Character info" }
hooks = { "MapName" }

function update()
	return getvar("MapName")
end
