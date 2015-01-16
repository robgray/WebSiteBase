using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Infrastructure.Common;
using WebBase.Mvc.ViewModels;

namespace WebBase.Mvc.Helpers
{
	public static class HtmlHelpers
	{
		public static string Image(this HtmlHelper helper, string src, string alt, object parameters = null, string defaultImage = "/Images/defaultprofile64.png")
		{
            var dict = GeneralExtensions.AnonymousObjectToDictionary(parameters);               

			if (string.IsNullOrEmpty(src))
				src = defaultImage;

			var tb = new TagBuilder("img");
			tb.Attributes.Add("src", helper.Encode(src));
			tb.Attributes.Add("alt", helper.Encode(alt));
            foreach (var entry in dict)            
                tb.Attributes.Add(helper.Encode(entry.Key), helper.Encode(entry.Value));            
			return tb.ToString(TagRenderMode.SelfClosing);            
		}

        public static MvcHtmlString DatePicker(this HtmlHelper helper, string name, DateTime? value)
        {
            var builder = new TagBuilder("div");
            builder.Attributes.Add("class", "input-append date");
            if (value.HasValue) 
                builder.Attributes.Add("data-date", value.Value.ToString("dd-MM-yyyy"));
            builder.Attributes.Add("data-date-format", "dd-mm-yyyy");
            
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString DatePickerFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression,
                                              object attributes)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            return DatePicker(helper, name, metadata.Model as DateTime?);
        }

        public static MvcHtmlString Alert(this HtmlHelper helper, string type, string message, string title)
        {
            if (string.IsNullOrEmpty(message))
                return MvcHtmlString.Create("");

            if (string.IsNullOrEmpty(type))
            {
                type = "success";
            }
            
            var stuff = "<div class='alert alert-" + type + "'>";
            if (!string.IsNullOrEmpty(title))
            {
                stuff += "<div class='media'>" +
                         "<span class='label label-" + type + " pull-left'>" + title + "</span>" +
                         "<div class='media-body'>" + message + "</div></div>";
            }
            else
            {
                stuff += message;
            }

            stuff += "</div>";
            
            return MvcHtmlString.Create(stuff);
        }

        public static MvcHtmlString SuccessAlert(this HtmlHelper helper, string message, string title)
        {
            return helper.Alert("success", message, title);
        }

        public static MvcHtmlString ErrorAlert(this HtmlHelper helper, string message, string title)
        {
            return helper.Alert("error", message, title);
        }    
    
        public static MvcHtmlString RenderSiteMessage(this HtmlHelper helper)
        {
            var siteMessage = helper.ViewContext.TempData["SiteMessage"] as SiteMessageViewModel;
            if (siteMessage != null)
            {

                string toastrMethod;
                switch (siteMessage.MessageType)
                {
                    case SiteMessageType.Success:
                        toastrMethod = "success";
                        break;
                    case SiteMessageType.Warning:
                        toastrMethod = "warning";
                        break;
                    case SiteMessageType.Info:
                        toastrMethod = "info";
                        break;
                    default:
                        toastrMethod = "error";
                        break;
                }

                var outputString = "<script type='text/javascript'>toastr.clear();" +                        
                                   "toastr." + toastrMethod + "('" + siteMessage.Message + "', null, { timeOut: " +
                                   siteMessage.Duration + "});" +
                                   "</script>";

                return MvcHtmlString.Create(outputString);
            }
            return MvcHtmlString.Empty;
        }

		public static IEnumerable<SelectListItem> MonthSelectListItems
		{
			get
			{
				return DateTimeFormatInfo.InvariantInfo.MonthNames
				                         .Take(12)
				                         .Select((mn, ii) => new SelectListItem() {Value = (ii + 1).ToString(), Text = mn});
			}
		}

		public static IEnumerable<SelectListItem> YearSelectListItems
		{
			get
			{
				return Enumerable.Range(DateTimeHelper.Now.Year, 10)
								 .Select(yy => yy.ToString())
								 .Select(yy => new SelectListItem() { Value = yy, Text = yy });
			}
		}
	}
}