-----------
-- Fps2.lua
-----------
--
-- Monitors the tick from the Mumble API and calculates the FPS.
-- Fps2 is a more detailed version than Fps that updates every tick and has
-- more precision. Normally you want Fps2 only if you want to do some
-- calculations yourself that requires more precision, otherwise use Fps.
--

id = "Fps2"
hooks = { "UITick" }

local function round(num, idp)
	local mult = 10 ^ (idp or 0)
	return math.floor(num * mult + 0.5) / mult
end

function update()
	local prevTimestamp = localvar("prevTimestamp")
	local prevDiff = localvar("prevDiff")

	local currTimestamp = timestamp()
	local diff = nil
	local fps = 0

	if prevTimestamp ~= nil then
		diff = currTimestamp - prevTimestamp
		if prevDiff ~= nil then
			diff = prevDiff * 0.9 + diff * 0.1
		end
		fps = round(1 / diff, 1)
	end

	localvar("prevTimestamp", currTimestamp)
	localvar("prevDiff", diff)
	return fps
end
