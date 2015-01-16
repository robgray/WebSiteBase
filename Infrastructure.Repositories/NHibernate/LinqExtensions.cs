using System;
using System.Linq;
using System.Linq.Expressions;
using Domain.Interfaces;

namespace Infrastructure.Repositories.NHibernate
{
    public static class LinqExtensions
    {       
        public static IQueryable<T> Sort<T,TProp>(this IQueryable<T> query,
                                                    Expression<Func<T, TProp>> order,
                                                    SortDirection direction)
        {
            return direction == SortDirection.Ascending ? query.OrderBy(order) : query.OrderByDescending(order);
        }
    }
}
