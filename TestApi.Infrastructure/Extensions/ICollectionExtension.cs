namespace TestApi.Infrastructure.Extensions
{
    public static class ICollectionExtension
    {
        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            if (collection == null)
                return true;

            return collection.Count == 0;
        }
    }
}
