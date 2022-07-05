local command = 'EXPIREAT'
if @expireBehavior == '' then
    return redis.pcall(command, @key, @timestamp)
else
    return redis.pcall(command, @key, @timestamp, @expireBehavior)
end
