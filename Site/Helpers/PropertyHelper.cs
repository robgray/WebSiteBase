using System;
using System.Linq.Expressions;

namespace WebBase.Mvc.Helpers
{
    public static class PropertyHelper
    {
        public static string PropertyName<TModel, TProp>(Expression<Func<TModel, TProp>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
                return null;

            return memberExpression.Member.Name;
        }        
    }
}