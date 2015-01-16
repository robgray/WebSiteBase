using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Common;

namespace Domain.QueryFilters
{
    public class ExistingUserEmailAddressFilter : IQueryFilter<User>
    {
        public string EmailAddress { get; set; }

        public int UserId { get; set; }

        public Expression<Func<User, bool>> GetFilter()
        {
            var predicate = PredicateBuilder.True<User>();

            predicate = predicate.And(u => u.EmailAddress == EmailAddress && u.Id != UserId);

            return predicate;
        }        
    }
}
