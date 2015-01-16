using System.Web.Http;
using WebBase.Mvc.App_Start;

namespace Mvc.Tests.Api
{
    public abstract class BaseRouteTests
    {
        private HttpConfiguration _config;

        protected BaseRouteTests()
        {
            _config = new HttpConfiguration();
            _config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            _config.Routes.Clear();

            WebApiConfig.Register(_config);    
        }

        public HttpConfiguration Config { get { return _config; } }
    }
}
