-------------------------
-- Fps.lua
-------------------------
--
-- Exposes the FPS so it can be used as text.
--

id = "fps"
name = "FPS"
category = { "Miscellaneous" }
hooks = { "Fps" }

function update()
	return getvar("Fps")
end
