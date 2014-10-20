----------------
-- RaceName.lua
----------------
--
-- When properly written, it uses the character name from the Mumble API to
-- distinguish different characters and sets the race name.
--
-- This is an example variable script. In order to use it, please copy it to
-- "%APPDATA%/OBS/pluginData/Gw2Plugin/CustomScriptVariables" and change the
-- script and filename.
--
-- If you want to make it visible as a formatter (and therefore use it in your
-- customized text), don't forget to add a custom formatter too! Just look at
-- the README and the included script formatters to see how this can be done.
--

id = "RaceName"
hooks = { "CharacterName" }

-- Change the following table to reflect your own character names
local translationTable = {
	["Zojja"] = "Asura",
	["Caithe"] = "Sylvari",
	["Logan Thackeray"] = "Human",
	["Eir Stegalkin"] = "Norn",
	["Rytlock Brimstone"] = "Charr"
}

function update()
	return translationTable[getvar("CharacterName")]
end
