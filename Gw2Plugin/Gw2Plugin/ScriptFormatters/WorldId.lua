---------------
-- WorldId.lua
---------------
--
-- Exposes the world id from the Mumble API so it can be used as text.
--

id = "worldid"
name = "World id"
category = { "Server info" }
hooks = { "WorldId" }

function update()
	return getvar("WorldId")
end
