using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace Mvc.Tests.Installers.Plumbing
{
    public static class MethodInfoExtensions
    {
        /// <summary>
        /// Will return the name of the action specified in the ActionNameAttribute for a method if it has an ActionNameAttribute.
        /// Will return the name of the method otherwise.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string ActionName(this MethodInfo method)
        {
            if (method.IsDecoratedWith<ActionNameAttribute>()) return method.GetAttribute<ActionNameAttribute>().Name;

            return method.Name;
        }
    }

    public static class ReflectionHelpers
    {
        public static string GetMethodName<T, U>(Expression<Func<T, U>> expression)
        {
            var method = expression.Body as MethodCallExpression;
            if (method == null)
                throw new ArgumentException("Expression is wrong");

            var actionNameAttribute = method.Method.GetCustomAttributes(typeof (System.Web.Http.ActionNameAttribute), true).FirstOrDefault() as System.Web.Http.ActionNameAttribute;
            
            return actionNameAttribute != null ? actionNameAttribute.Name : method.Method.Name;                        
        }
    }
}
