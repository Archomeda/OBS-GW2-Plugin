-------------------------
-- PercentSignEscape.lua
-------------------------
--
-- Escapes the percent sign that is otherwise used as an identifier for text
-- replacements.
--

id = ""
name = "Percent sign (%)"
category = { "Miscellaneous" }

function update()
	return "%"
end
