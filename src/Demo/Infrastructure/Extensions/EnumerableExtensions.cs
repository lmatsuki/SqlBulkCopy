using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Infrastructure.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<TSource>> Chunk<TSource>(this IEnumerable<TSource> source, int size)
        {
            TSource[] bucket = null;
            int count = 0;

            foreach (TSource item in source)
            {
                if (bucket == null)
                {
                    bucket = new TSource[size];
                }

                bucket[count++] = item;
                if (count != size)
                {
                    continue;
                }

                yield return bucket;

                bucket = null;
                count = 0;
            }

            if (bucket != null && count > 0)
            {
                yield return bucket.Take(count);
            }
        }

    }
}
