local command = 'EXPIRE'
local key = KEYS[1]
local seconds = ARGV[1]
local expireBehavior = ARGV[2]
if expireBehavior == '' then
    return redis.pcall(command, key, seconds)
else
    return redis.pcall(command, key, seconds, expireBehavior)
end
