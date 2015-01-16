using System;
using System.Linq.Expressions;
using Domain.Entities;

namespace Domain.QueryFilters
{
    public interface IQueryFilter<T> where T : IHasId
    {
        Expression<Func<T, bool>> GetFilter();
    }
}
