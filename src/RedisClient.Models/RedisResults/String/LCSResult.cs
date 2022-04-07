namespace RedisClient.Models.RedisResults.String
{
    public class LCSResult<TResult>
    {
        private readonly TResult _result;

        public LCSResult(TResult result)
        {
            this._result = result;
        }

        public static implicit operator string(LCSResult<TResult> lcsResult)
        {
            if (lcsResult._result is string str)
            {
                return str;
            }
            return "";
        }

        public static implicit operator long(LCSResult<TResult> lcsResult)
        {
            if (lcsResult._result is long num)
            {
                return num;
            }
            return 0;
        }
    }
}
