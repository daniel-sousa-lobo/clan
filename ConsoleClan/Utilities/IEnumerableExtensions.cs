using System.Diagnostics.CodeAnalysis;

namespace ConsoleClan.Utilities
{
    public static class IEnumerableExtensions
    {

        [return: NotNull]
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T>? source)
        {
            return source ?? Enumerable.Empty<T>();
        }
    }
}
