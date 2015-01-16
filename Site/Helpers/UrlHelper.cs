using System.Web.Mvc;

namespace WebBase.Mvc.Helpers
{
    public static class UrlHelperExtension
    {
        public static string ActionFullPath(this UrlHelper url, string action, string controller)
        {
            return url.Action(action, controller, null, url.RequestContext.HttpContext.Request.Url.Scheme);
        }

        public static string ActionFullPath(this UrlHelper url, string action, string controller, object routeValues)
        {
            return url.Action(action, controller, routeValues, url.RequestContext.HttpContext.Request.Url.Scheme);
        }
    }
}