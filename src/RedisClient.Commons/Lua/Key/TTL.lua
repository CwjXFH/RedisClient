local key = KEYS[1]
return redis.pcall('TTL', key)
