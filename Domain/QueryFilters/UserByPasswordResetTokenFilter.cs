using System;
using System.Linq.Expressions;
using Domain.Entities;
using Infrastructure.Common;

namespace Domain.QueryFilters
{
    public class UserByPasswordResetTokenFilter : IQueryFilter<User>
    {
        public string ResetToken { get; set; }

        public Expression<Func<User, bool>> GetFilter()
        {
            return PredicateBuilder.True<User>()
                                   .And(u => u.PasswordVerificationToken == ResetToken)
                                   .And(u => u.PasswordVerificationTokenExpirationDate > DateTimeHelper.Now);
        }
    }
}
