----------------------
-- ProfessionName.lua
----------------------
--
-- Exposes the profession name so it can be used as text.
--

id = "prof"
name = "Profession name"
category = { "Character info" }
hooks = { "ProfessionName" }

function update()
	return getvar("ProfessionName")
end
