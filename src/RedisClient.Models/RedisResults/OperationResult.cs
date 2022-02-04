namespace RedisClient.Models.RedisResults
{
    public readonly record struct OperationResult(bool Succeeded)
    {
        public static implicit operator bool(OperationResult result) => result.Succeeded;

        public static implicit operator OperationResult(bool succeeded) => new OperationResult(succeeded);
    }


    public class OperationResult<T> where T : class
    {
        public OperationResult(bool succeeded, T data)
        {
            Succeeded = succeeded;
            Data = data;
        }


        public bool Succeeded { get; init; }
        public T Data { get; init; }

        // implicit convert will lose data
        public static explicit operator bool(OperationResult<T> result) => result.Succeeded;

        public static implicit operator OperationResult<T>(bool succeeded) => new OperationResult<T>(succeeded, default!);
    }

}
