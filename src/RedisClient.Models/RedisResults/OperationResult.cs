namespace RedisClient.Models.RedisResults
{
    public readonly record struct OperationResult(bool Successed)
    {
        public static implicit operator bool(OperationResult result) => result.Successed;

        public static implicit operator OperationResult(bool successed) => new OperationResult(successed);
    }


    public class OperationResult<T> where T : class
    {
        public OperationResult(bool successed, T data)
        {
            Successed = successed;
            Data = data;
        }


        public bool Successed { get; init; }
        public T Data { get; init; }

        // implicit convert will lose data
        public static explicit operator bool(OperationResult<T> result) => result.Successed;

        public static implicit operator OperationResult<T>(bool successed) => new OperationResult<T>(successed, default!);
    }

}
