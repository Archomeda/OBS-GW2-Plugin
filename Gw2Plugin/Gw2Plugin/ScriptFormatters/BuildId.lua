---------------
-- BuildId.lua
---------------
--
-- Exposes the build id from the Mumble API so it can be used as text.
--

id = "buildid"
name = "Build id"
category = { "Server info" }
hooks = { "BuildId" }

function update()
	return getvar("BuildId")
end
