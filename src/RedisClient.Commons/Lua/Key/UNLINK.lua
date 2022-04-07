local keyCount = ARGV[1]
local result = 0
local key = ''
for i = 2, keyCount, 1 do
    key = KEYS[i]
    result = redis.pcall('UNLINK', key) + result
end
return result
