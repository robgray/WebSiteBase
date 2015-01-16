using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.QueryFilters;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace Infrastructure.Repositories.NHibernate
{
    public abstract class AbstractRepository<T> : IRepository<T> where T : IHasId
    {
        private readonly ISessionManager _sessionManager;

        protected AbstractRepository(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        /// <summary>
        /// Loads an instance of type TypeOfListItem from the DB based on its ID.
        /// </summary>
        public T GetById(int id)
        {
            return Session.Get<T>(id);

            //	return Session.Load<T>(id);
        }

        public List<T> GetByIdList(IList<int> idList)
        {
            if (idList.Count == 0) return new List<T>();

            object[] ids = new object[idList.Count];
            for (int i = 0; i < idList.Count; i++)
                ids[i] = idList[i];

            ICriterion criterium = new InExpression("Id", ids);

            return GetByCriteria(criterium);

        }

        /// <summary>
        /// Loads every instance of the requested type with no filtering.
        /// </summary>
        public List<T> GetAll()
        {
            return GetByCriteria();
        }

        public List<T> GetByFilter(IQueryFilter<T> filter)
        {
            return Query.Where(filter.GetFilter()).ToList();
        }

        public List<T> GetByFilter(int maxResults, IQueryFilter<T> filter)
        {
            return Query.Where(filter.GetFilter()).Take(maxResults).ToList();
        }

        public DataPage<T> GetAll<TProp>(int pageNumber, int pageSize, Expression<Func<T, TProp>> order, SortDirection direction)
        {
            var page = new DataPage<T>
                {
                    Items = Query.Sort(order, direction).Skip(pageSize*(pageNumber - 1)).Take(pageSize).Distinct().ToList(),
                    Total = Query.Distinct().Count()
                };

            return page;
        }

        public DataPage<T> GetAll<TProp>(int pageNumber, int pageSize, Expression<Func<T, TProp>> order,
                                         SortDirection direction, IQueryFilter<T> filter)
        {
            var query = Query.Where(filter.GetFilter());

            var page = new DataPage<T>
            {
                Items = query.Sort(order, direction).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList(),
                Total = query.Count()
            };

            return page;
        }
       
		/// <summary>
		/// Loads every instance of the requested type using the supplied <see cref="ICriterion" />.
		/// If no <see cref="ICriterion" /> is supplied, this behaves like <see cref="GetAll()" />.
		/// </summary>
		public List<T> GetByCriteria(params ICriterion[] criterion)
		{
			return GetByCriteria((LockMode)null, criterion);
		}

		public List<T> GetByCriteria(Order order, params ICriterion[] criterion)
		{
			return GetByCriteria(null, criterion, order);
		}

		public List<T> GetByCriteria(LockMode lockMode, params ICriterion[] criterion)
		{
			return GetByCriteria(lockMode, criterion, null);
		}

		public List<T> GetByCriteria(LockMode lockMode, ICriterion[] criterion, Order order)
		{
			return GetByCriteria(lockMode, criterion, order, 0, Int32.MaxValue);
		}
        
		public List<T> GetByCriteria(LockMode lockMode, ICriterion[] criterion, Order order, int pageNumber, int pageSize)
		{
			ICriteria criteria = Session.CreateCriteria(typeof(T));

			if (lockMode != null)
				criteria.SetLockMode(lockMode);

			foreach (ICriterion criterium in criterion) {
				criteria.Add(criterium);
			}

			if (order != null)
				criteria.AddOrder(order);

			if (pageSize < Int32.MaxValue && pageSize > 0) {
				criteria.SetFirstResult(pageNumber * pageSize);
				criteria.SetMaxResults(pageSize);
			}

			return criteria.List<T>() as List<T>;
		}

		public T GetUniqueByCriteria(params ICriterion[] criterion)
		{
			return GetUniqueByCriteria(null, criterion);
		}

		public T GetUniqueByCriteria(LockMode lockMode, params ICriterion[] criterion)
		{
			ICriteria criteria = Session.CreateCriteria(typeof(T));

			if (lockMode != null)
				criteria.SetLockMode(lockMode);

			foreach (ICriterion criterium in criterion) {
				criteria.Add(criterium);
			}
			criteria.SetMaxResults(1);

			return criteria.UniqueResult<T>();
		}

		/// <summary>
		/// For entities with automatatically generated IDs, such as identity, SaveOrUpdate may
		/// be called when saving a new entity. 
		/// </summary>        
        public virtual T Save(T entity)
		{
		    Session.SaveOrUpdate(entity);            
	        return entity;
        }
        
		public void Delete(T entity)
		{
			Session.Delete(entity);
		}

		/// <summary>
		/// Returns an OrderInPhase instance describing the specified sort column and direction
		/// </summary>
		static Order GetOrder(string sortColumn, SortDirection direction)
		{
			Order order = null;
			if (sortColumn != null)
				order = (direction == SortDirection.Ascending ? Order.Asc(sortColumn) : Order.Desc(sortColumn));

			return order;
		}

	    /// <summary>
	    /// Exposes the ISession used within the DAO.
	    /// </summary>	    
        public virtual ISession Session
        {
	        get 
            {                 
                return _sessionManager.OpenSession(); 
            }
        }		

	    protected IQueryable<T> Query
	    {
            get { return Session.Query<T>(); }
	    }
	}
}
