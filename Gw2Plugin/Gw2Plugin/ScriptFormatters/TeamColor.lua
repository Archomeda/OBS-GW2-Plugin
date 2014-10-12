----------------------
-- TeamName.lua
----------------------
--
-- Exposes the team color so it can be used as text.
--

id = "team"
name = "Team color"
category = { "Character info" }
hooks = { "TeamColor" }

function update()
	return getvar("TeamName")
end
