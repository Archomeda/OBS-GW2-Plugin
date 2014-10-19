--------------------
-- GameModeName.lua
--------------------
--
-- Uses the map type from the Mumble API and translates it into a game mode.
--
-- NOTE: EXPERIMENTAL! NEEDS MORE RESEARCH
--

id = "GameModeName"
hooks = { "MapType" }

local translationTable = {
	[2] = "PvP",
	[4] = "PvE",  -- Probably every instance
	[5] = "PvE",  -- Probably every map
	[9] = "WvW",  -- Eternal Battlegrounds
	[10] = "WvW", -- Blue Borderlands
	[11] = "WvW", -- Green Borderlands
	[12] = "WvW", -- Red Borderlands
	[14] = "WvW", -- Obsidian Sanctum
	[15] = "WvW"  -- Edge of the Mists
}

function update()
	return translationTable[getvar("MapType")]
end
