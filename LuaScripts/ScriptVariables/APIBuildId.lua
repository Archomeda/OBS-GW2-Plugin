id = "APIBuildId"
hooks = { "UITick" }

function update()
	-- We need to cache the build for a while (here we do it for 5 minutes).
	-- We can accomplish this by comparing the timestamps.
	local currentTimestamp = timestamp()
	local lastTimestamp = localvar("lastTimestamp")

	if lastTimestamp == nil or lastTimestamp + 5 * 60 < currentTimestamp then
		-- In order to prevent this from being fired too frequently,
		-- we set the last timestamp to the current timestamp.
		-- In the callback function we set it again so it's set to
		-- the correct update time, since an API call can take a while.
		localvar("lastTimestamp", currentTimestamp)
		gw2api.build(buildCallback)
	end

	return getcurrent()
end

function buildCallback(build)
	localvar("lastTimestamp", timestamp())
	return build.build_id
end
