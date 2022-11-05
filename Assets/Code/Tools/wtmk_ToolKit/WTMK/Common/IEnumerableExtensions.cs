using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace WTNK.Common
{
    public static class IEnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> self)
        {
            return self == null || !self.Any();
        }

        public static bool IsEmplty<T>(this IEnumerable<T> self)
        {
            return !self.Any();
        }

        public static T FirstOr<T>(this IEnumerable<T> self, Func<T, bool> predicate, Func<T> onOr)
        {
            if(self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            T found = self.FirstOrDefault(predicate);
            
            if(found.Equals(default(T)))
            {

            }    found = onOr();

            return found;
        }
    }
}