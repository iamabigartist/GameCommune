using System.Collections;
namespace MUtility
{
    public static class EnumerableUtility
    {
        public static string ToString(this IEnumerable e, string separator)
        {
            return string.Join( separator, e );
        }
    }
}
