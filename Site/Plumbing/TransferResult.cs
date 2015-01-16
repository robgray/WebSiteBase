using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebBase.Mvc.Plumbing
{
    public class TransferResult : ActionResult
    {
        public string Url { get; private set; }

        public TransferResult(string url)
        {
            this.Url = url;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var httpContext = HttpContext.Current;

            if (HttpRuntime.UsingIntegratedPipeline)
            {
                httpContext.Server.TransferRequest(Url, true);
            }
            else
            {
                httpContext.RewritePath(Url, false);

                IHttpHandler httpHandler = new MvcHttpHandler();
                httpHandler.ProcessRequest(httpContext);
            }
        }
    }

    public class TransferToRouteResult : ActionResult
    {
        public string RouteName { get; set; }
        public RouteValueDictionary RouteValues { get; set; }

        public TransferToRouteResult(string routeName, RouteValueDictionary routeValues)
        {
            RouteName = routeName ?? string.Empty;
            RouteValues = routeValues ?? new RouteValueDictionary();
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var urlHelper = new UrlHelper(context.RequestContext);
            var url = urlHelper.RouteUrl(this.RouteName, this.RouteValues);

            var actualResult = new TransferResult(url);
            actualResult.ExecuteResult(context);
        }
    }
}
