-----------
-- Fps.lua
-----------
--
-- Monitors the tick from the Mumble API and calculates the FPS.
-- Fps is a more practical version of Fps2 that updates every second. This is
-- enough under normal circumstances.
--

id = "Fps"
hooks = { "Fps2" }

local function round(num, idp)
	local mult = 10 ^ (idp or 0)
	return math.floor(num * mult + 0.5) / mult
end

function update()
	local prevTimestamp = localvar("prevTimestamp")
	local currTimestamp = timestamp()

	local fps = getcurrent()
	if prevTimestamp == nil or currTimestamp - prevTimestamp >= 1 then
		fps = getvar("Fps2")
		if fps ~= nil then
			fps = round(fps, 0)
		end
		localvar("prevTimestamp", currTimestamp)
	end

	return fps
end
