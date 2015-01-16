using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.Windsor;
using Infrastructure.Common;

namespace WebBase.Mvc.Plumbing
{
	public class WindsorActionInvoker : ControllerActionInvoker
	{
		private readonly IWindsorContainer _container;

		public WindsorActionInvoker(IWindsorContainer container)
		{
			_container = container;
		}

		protected override ActionExecutedContext InvokeActionMethodWithFilters(ControllerContext controllerContext, 
			IList<IActionFilter> filters, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters)
		{
			foreach (var actionFilter in filters)
			{
			    _container.Kernel.InjectProperties(actionFilter);
			}

			return base.InvokeActionMethodWithFilters(controllerContext, filters, actionDescriptor, parameters);
		}

        protected override ExceptionContext InvokeExceptionFilters(ControllerContext controllerContext, IList<IExceptionFilter> filters, System.Exception exception)
        {
            foreach (
                var actionFilter in
                    filters.Where(actionFilter => !(actionFilter.GetType() == controllerContext.Controller.GetType())))
            {
                _container.Kernel.InjectProperties(actionFilter);
            }

            return base.InvokeExceptionFilters(controllerContext, filters, exception);
        }

        protected override AuthorizationContext InvokeAuthorizationFilters(ControllerContext controllerContext, IList<IAuthorizationFilter> filters, ActionDescriptor actionDescriptor)
        {
            foreach (
                var authFilter in
                    filters.Where(filter => !(filter.GetType() == controllerContext.Controller.GetType())))
            {
                _container.Kernel.InjectProperties(authFilter);
            }

            return base.InvokeAuthorizationFilters(controllerContext, filters, actionDescriptor);
        }
	}
}