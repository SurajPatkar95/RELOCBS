using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> initial, IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("collection");
            }
            foreach (var item in other)
            {
                initial.Add(item);
            }

        }

        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {

            if (source is ICollection<T>) return ((ICollection<T>)source).Count == 0;
            return !source.Any();
        }

    }
}