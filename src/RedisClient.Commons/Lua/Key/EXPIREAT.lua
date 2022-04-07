local command = 'EXPIREAT'
local key = KEYS[1]
local timestamp = ARGV[1]
local expireBehavior = ARGV[2]
if expireBehavior == '' then
    return redis.pcall(command, key, timestamp)
else
    return redis.pcall(command, key, timestamp, expireBehavior)
end
