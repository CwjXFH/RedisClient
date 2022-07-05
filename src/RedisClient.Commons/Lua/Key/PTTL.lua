local key = KEYS[1]
return redis.pcall('PTTL', @key)
