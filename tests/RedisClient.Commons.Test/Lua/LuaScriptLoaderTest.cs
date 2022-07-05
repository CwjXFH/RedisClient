using RedisClient.Commons.Exceptions;
using RedisClient.Commons.Lua;

namespace RedisClient.Commons.Test.Lua
{
    public class LuaScriptLoaderTest
    {
        [Fact]
        public async Task LoadAsync_KeyTTL_ReturnScript()
        {
            var ttlScript = await LuaScriptLoader.LoadAsync(LuaScriptName.KeyOperatorScript.TTL);

            Assert.NotEmpty(ttlScript);
        }

        [Fact]
        public async Task LoadAsync_NotExists_Throw()
        {
            await Assert.ThrowsAsync<LuaScriptException>(async () => await LuaScriptLoader.LoadAsync("script"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task LoadAsync_NullOrEmptyScriptName_Throw(string scriptName)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await LuaScriptLoader.LoadAsync(scriptName));
        }
    }
}
