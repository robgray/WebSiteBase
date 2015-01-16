using System.Text;
using System.Web.Mvc;

namespace WebBase.Mvc.Helpers
{
    public static class MyHtmlHelpers
    {
        public static MvcHtmlString BootstrapValidationSummary(this HtmlHelper helper, string validationMessage = "")
        {
            if (helper.ViewData.ModelState.IsValid)
                return new MvcHtmlString("");

            var builder = new StringBuilder();

            builder.Append("<div class='errors-summary'><span>");
            if (!string.IsNullOrEmpty(validationMessage))
                builder.Append(helper.Encode(validationMessage));
            builder.Append("</span>");
            builder.Append("<ul>");
            foreach (var key in helper.ViewData.ModelState.Keys)
            {
                foreach (var err in helper.ViewData.ModelState[key].Errors)
                {
                    builder.Append("<li>" + helper.Encode(err.ErrorMessage) + "</li>");
                }                
            }
            builder.Append("</ul>");
            builder.Append("</div>");

            return new MvcHtmlString(builder.ToString());
        }
    }
}