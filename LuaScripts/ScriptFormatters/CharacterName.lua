----------------------
-- CharacterName.lua
----------------------
--
-- Exposes the character name from the Mumble API so it can be used as text.
--

id = "char"
name = "Character name"
category = { "Character info" }
hooks = { "CharacterName" }

function update()
	return getvar("CharacterName")
end
