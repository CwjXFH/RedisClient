using System.Collections;

namespace RedisClient.Commons.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsEmpty(this ICollection collection)
        {
            if (collection == null)
            {
                return true;
            }
            return collection.Count <= 0;
        }

        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            if (collection == null)
            {
                return true;
            }
            return collection.Count <= 0;
        }

    }
}
