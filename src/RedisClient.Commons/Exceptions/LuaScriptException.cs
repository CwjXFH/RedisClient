namespace RedisClient.Commons.Exceptions
{
    public class LuaScriptException : Exception
    {
        public LuaScriptException() : base() { }
        public LuaScriptException(string message) : base(message) { }
        public LuaScriptException(string message, Exception innerException) : base(message, innerException) { }
    }
}

