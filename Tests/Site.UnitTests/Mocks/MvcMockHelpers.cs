using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NSubstitute;
using System.Linq;

namespace Mvc.Tests.Mocks
{
	/// <summary>
	/// Adapted from http://www.hanselman.com/blog/ASPNETMVCSessionAtMix08TDDAndMvcMockHelpers.aspx
	/// </summary>	
	public static class MvcMockHelpers
	{
		public static HttpContextBase FakeHttpContext()
		{
			var context = Substitute.For<HttpContextBase>();
			var request = Substitute.For<HttpRequestBase>();
			var response = Substitute.For<HttpResponseBase>();
			var session = Substitute.For<HttpSessionStateBase>();
			var server = Substitute.For<HttpServerUtilityBase>();
            var user = new MockPrincipal();
		
			context.Request.Returns(request);
			context.Response.Returns(response);
			context.Session.Returns(session);
			context.Server.Returns(server);
			context.User.Returns(user);

            // From http://stackoverflow.com/questions/674458/asp-net-mvc-unit-testing-controllers-that-use-urlhelper "Steven Pena"
            request.ApplicationPath.Returns("/");
		    request.Url.Returns(new Uri("http://localhost/a", UriKind.Absolute));
            request.ServerVariables.Returns(new NameValueCollection());
         
            response.ApplyAppPathModifier("").ReturnsForAnyArgs(info => info.Args().First());
            // End From 
            
			return context;
		}

		public static HttpContextBase FakeHttpContext(string url)
		{
			HttpContextBase context = FakeHttpContext();
			context.Request.SetupRequestUrl(url);
			return context;
		}

		public static void SetFakeControllerContext(this Controller controller)
		{
			var httpContext = FakeHttpContext();
			ControllerContext context = new ControllerContext(new RequestContext(httpContext, new RouteData()), controller);
			controller.ControllerContext = context;
		}

		static string GetUrlFileName(string url)
		{
			if (url.Contains("?"))
				return url.Substring(0, url.IndexOf("?"));
			else
				return url;
		}

		static NameValueCollection GetQueryStringParameters(string url)
		{
			if (url.Contains("?")) {
				NameValueCollection parameters = new NameValueCollection();

				string[] parts = url.Split("?".ToCharArray());
				string[] keys = parts[1].Split("&".ToCharArray());

				foreach (string key in keys) {
					string[] part = key.Split("=".ToCharArray());
					parameters.Add(part[0], part[1]);
				}

				return parameters;
			}
			else {
				return null;
			}
		}

		public static void SetHttpMethodResult(this HttpRequestBase request, string httpMethod)
		{
			request.HttpMethod.Returns(httpMethod);			
		}

		public static void SetupRequestUrl(this HttpRequestBase request, string url)
		{
			if (url == null)
				throw new ArgumentNullException("url");

			if (!url.StartsWith("~/"))
				throw new ArgumentException("Sorry, we expect a virtual url starting with \"~/\".");

			request.QueryString.Returns(GetQueryStringParameters(url));
			request.AppRelativeCurrentExecutionFilePath.Returns(GetUrlFileName(url));
			request.PathInfo.Returns(string.Empty);			
		}
	}
}
