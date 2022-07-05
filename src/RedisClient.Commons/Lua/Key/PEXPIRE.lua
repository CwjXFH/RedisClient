local command = 'PEXPIRE'
if @expireBehavior == '' then
    return redis.pcall(command, @key, @seconds)
else
    return redis.pcall(command, @key, @seconds, @expireBehavior)
end
