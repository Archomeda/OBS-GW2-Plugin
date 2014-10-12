----------------------
-- ProfessionName.lua
----------------------
--
-- Uses the profession id from the Mumble API and translates it into a string
-- variable.
--

id = "ProfessionName"
hooks = { "ProfessionId" }

local translationTable = {
	[1] = "Guardian",
	[2] = "Warrior",
	[3] = "Engineer",
	[4] = "Ranger",
	[5] = "Thief",
	[6] = "Elementalist",
	[7] = "Mesmer",
	[8] = "Necromancer"
}

function update()
	return translationTable[getvar("ProfessionId")]
end
