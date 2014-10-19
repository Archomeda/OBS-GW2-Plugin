---------------------
-- ServerAddress.lua
---------------------
--
-- Exposes the server IP address from the Mumble API so it can be used as text.
--

id = "ip"
name = "Server IP address"
category = { "Server info" }
hooks = { "ServerAddress" }

function update()
	local ip = getvar("ServerAddress")
	if ip ~= nil and ip[1] > 0 and ip[2] > 0 and ip[3] > 0 and ip[4] > 0 then
		return tostring(ip[1]) .. "." .. tostring(ip[2]) .. "." .. tostring(ip[3]) .. "." .. tostring(ip[4])
	else
		return nil
	end
end
