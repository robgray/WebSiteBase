using System.Web.Mvc;
using WebBase.Mvc.Filters;

namespace WebBase.Mvc.App_Start
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
            filters.Add(new LoggingErrorHandler());
		}
	}
}