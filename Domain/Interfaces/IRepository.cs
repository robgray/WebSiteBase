using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.QueryFilters;
using NHibernate;

namespace Domain.Interfaces
{
	public interface IRepository<T> where T : IHasId
	{
		T GetById(int id);
		List<T> GetByIdList(IList<int> idList);
		List<T> GetAll();

        List<T> GetByFilter(IQueryFilter<T> filter);
        List<T> GetByFilter(int maxResults, IQueryFilter<T> filter);
	    DataPage<T> GetAll<TProp>(int pageNumber, int pageSize, Expression<Func<T, TProp>> order, SortDirection direction);
        DataPage<T> GetAll<TProp>(int pageNumber, int pageSize, Expression<Func<T, TProp>> order, SortDirection direction, IQueryFilter<T> filter);

		void Delete(T entity);
		T Save(T entity);
		
        ISession Session { get;}
	}
}
