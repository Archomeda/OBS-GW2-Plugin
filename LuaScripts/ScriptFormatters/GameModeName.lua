--------------------
-- GameModeName.lua
--------------------
--
-- Exposes the game mode so it can be used as text.
--

id = "gamemode"
name = "Game mode"
category = { "Character info" }
hooks = { "GameModeName" }

function update()
	return getvar("GameModeName")
end
