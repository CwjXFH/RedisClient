using System.Collections;

namespace RedisClient.Commons.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsEmpty(this IEnumerable? input)
        {
            if (input == null)
            {
                return true;
            }

            var count = 0;
            foreach (var _ in input)
            {
                count += 1;
                break;
            }

            return count <= 0;
        }
    }
}