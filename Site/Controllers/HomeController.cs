using System.Web.Mvc;

namespace WebBase.Mvc.Controllers
{
	public class HomeController : ControllerBase
	{		
		public HomeController() { }
        
		public ActionResult Index(string returnUrl = null)
		{                        		    
		    ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index");

			return View();
		}	    
	}
}
