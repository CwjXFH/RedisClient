local key = KEYS[1]
return redis.pcall('PEXPIRETIME', key)
