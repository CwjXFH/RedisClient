using RedisClient.Commons.Exceptions;
using System.Collections.Concurrent;

namespace RedisClient.Commons.Lua
{
    public static class LuaScriptLoader
    {
        private static readonly ConcurrentDictionary<string, WeakReference<string>> ScriptCache = new();

        public static async Task<string> LoadAsync(string scriptFileName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(scriptFileName))
            {
                throw new ArgumentNullException(nameof(scriptFileName));
            }

            if (ScriptCache.TryGetValue(scriptFileName, out var script))
            {
                if (script?.TryGetTarget(out var content) ?? false && string.IsNullOrWhiteSpace(content) == false)
                {
                    return content;
                }
            }

            var scriptContent = "";

            try
            {
                scriptContent = await File.ReadAllTextAsync(scriptFileName, cancellationToken);

            }
            catch (FileNotFoundException)
            {
                throw new LuaScriptException("Lua script not exists.");
            }

            if (string.IsNullOrWhiteSpace(scriptContent))
            {
                throw new LuaScriptException("Lua script content is empty.");
            }
            ScriptCache.TryAdd(scriptFileName, new WeakReference<string>(scriptContent));
            return scriptContent;
        }
    }

    public static class LuaScriptName
    {
        private const string LuaScriptPathPrefix = "Lua";
        private const string LuaScriptPathSuffix = ".lua";

        public static class KeyOperatorScript
        {
            private const string ScriptPathPrefix = "Key";

            public static string PEXPIRE => GetScriptFullPath(nameof(PEXPIRE));
            public static string EXPIRE => GetScriptFullPath(nameof(EXPIRE));
            public static string EXPIREAT => GetScriptFullPath(nameof(EXPIREAT));
            public static string PEXPIREAT => GetScriptFullPath(nameof(PEXPIREAT));
            public static string EXPIRETIME => GetScriptFullPath(nameof(EXPIRETIME));
            public static string PEXPIRETIME => GetScriptFullPath(nameof(PEXPIRETIME));
            public static string TTL => GetScriptFullPath(nameof(TTL));
            public static string PTTL => GetScriptFullPath(nameof(PTTL));
            public static string UNLINK => GetScriptFullPath(nameof(UNLINK));

            private static string GetScriptFullPath(string scriptName) => $"{LuaScriptPathPrefix}/{ScriptPathPrefix}/{scriptName}{LuaScriptPathSuffix}";
        }

        public static class StringOperatorScript
        {
            private const string ScriptPathPrefix = "String";

            public static string GETEX => GetScriptFullPath(nameof(GETEX));
            public static string LCS => GetScriptFullPath(nameof(LCS));
            public static string SET => GetScriptFullPath(nameof(SET));

            private static string GetScriptFullPath(string scriptName) => $"{LuaScriptPathPrefix}/{ScriptPathPrefix}/{scriptName}{LuaScriptPathSuffix}";
        }

    }
}
