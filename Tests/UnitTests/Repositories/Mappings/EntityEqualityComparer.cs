using System;
using System.Collections;
using Domain.Entities;

namespace UnitTests.Repositories.Mappings
{
    public class EntityEqualityComparer<T> : IEqualityComparer where T : IHasId
    {
        public bool Equals(object x, object y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            if (x is T && y is T)
            {
                return ((T) x).Id == ((T) y).Id;
            }
            return x.Equals(y);
        }

        public int GetHashCode(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
