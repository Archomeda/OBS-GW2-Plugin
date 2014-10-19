----------------
-- TeamName.lua
----------------
--
-- Uses the team color id from the Mumble API and translates it into a team
-- name.
--
-- NOTE: EXPERIMENTAL! NEEDS MORE RESEARCH
--

id = "TeamColor"
hooks = { "TeamColorId" }

local translationTable = {
	[9]   = "Blue",  -- PvP/WvW
	[55]  = "Green", -- WvW
	[376] = "Red",   -- PvP/WvW
}

function update()
	return translationTable[getvar("TeamColorId")]
end
