using System;
using System.Collections.Generic;

namespace Session13
{
    // Custom comparer used by GroupBy: two words are "equal" (same group)
    // when they are made of exactly the same characters, regardless of order.
    public class AnagramComparer : IEqualityComparer<string>
    {
        public bool Equals(string? x, string? y)
        {
            if (x == null || y == null)
                return x == y;

            return Normalize(x) == Normalize(y);
        }

        public int GetHashCode(string obj)
        {
            return Normalize(obj).GetHashCode();
        }

        // "from" and "form" both become "fmor"
        private static string Normalize(string word)
        {
            char[] chars = word.ToCharArray();
            Array.Sort(chars);
            return new string(chars);
        }
    }
}