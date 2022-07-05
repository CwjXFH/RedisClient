using RedisClient.Commons.Lua;
using StackExchange.Redis;

namespace RedisClient.StackExchange.Internal
{
    internal class RedisOperator
    {
        protected readonly IDatabase Database;

        public RedisOperator(IDatabase database)
        {
            this.Database = database;
        }

        protected async Task<LuaScript> GetLuaScriptAsync(string scriptName, CancellationToken cancellationToken = default)
        {
            var setLuaScript = await LuaScriptLoader.LoadAsync(scriptName, cancellationToken);
            var luaScript = LuaScript.Prepare(setLuaScript);

            return luaScript;
        }

    }
}
