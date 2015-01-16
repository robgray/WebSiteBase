using System.Text;
using System.Web.Mvc;
using Castle.Core.Logging;
using Domain.Interfaces;
using Infrastructure.Common;
using WebBase.Mvc.ViewModels;

namespace WebBase.Mvc.Controllers
{    
	public abstract class ControllerBase : Controller
	{
        public ILogger Logger { get; set; }

		public IUserRepository UserRepository { get; set; }

        public ITransactioner Transactioner { get; set; }

        public IConfigurationManager ConfigurationManager { get; set; }

		protected ControllerBase() { }
      
		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{         			
			base.OnActionExecuted(filterContext);
		}        	

		public ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl)) {
				return Redirect(returnUrl);
			}
			else {
				return RedirectToAction("Index", "Home");
			}
		}
       
        public JsonResult AjaxResponse(ResponseType responseType, object value)
        {
            var response = new { ResponseType = responseType.ToString().ToUpper(), Data = value };
            return Json(response, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);            
        }

        public enum ResponseType
        {
            Success,
            Warning,
            Error,
            ValidationError,
            Redirect,
            RedirectAndMessage
        }
    
        public void SetSiteMessage(SiteMessageType messageType, string message, int? duration = 5000)
        {
            TempData["SiteMessage"] = new SiteMessageViewModel(messageType, message, duration.Value);
        }
	}
}