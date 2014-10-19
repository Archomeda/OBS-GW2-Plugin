---------------
-- ShardId.lua
---------------
--
-- Exposes the shard id from the Mumble API so it can be used as text.
--

id = "shardid"
name = "Shard id"
category = { "Server info" }
hooks = { "ShardId" }

function update()
	return getvar("ShardId")
end
