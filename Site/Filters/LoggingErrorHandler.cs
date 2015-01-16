using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Logging;

namespace WebBase.Mvc.Filters
{
    public class LoggingErrorHandler : HandleErrorAttribute
    {
        public ILogger Logger { get; set; }

        public override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            var shouldILog = true;
            
            if (ex is HttpException) // Let Http errors do their thing
            {
                var hex = ex as HttpException;
                if (hex.GetHttpCode() == (int)HttpStatusCode.NotFound)
                {
                    shouldILog = false;
                }

            }

            if (shouldILog)
            {                
                if (Logger != null)
                    Logger.Error("Exception caught in LoggingErrorHandler(Global)", ex);
            }

            base.OnException(filterContext);
        }
    }
}