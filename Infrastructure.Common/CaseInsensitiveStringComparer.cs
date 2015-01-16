using System;
using System.Collections;

namespace Infrastructure.Common
{
    public class CaseInsensitiveStringComparer : IEqualityComparer
    {
        public bool Equals(object x, object y)
        {
            if (x == null || y == null)
                return false;

            if (x.GetType().Name != y.GetType().Name)
                return false;

            var xString = x as string;
            var yString = y as string;

            if (xString == null || yString == null)
                return false;

            return String.Compare(xString, yString, StringComparison.OrdinalIgnoreCase) == 0;

        }

        public int GetHashCode(object obj)
        {
            throw new System.NotImplementedException();
        }
    }
}    
