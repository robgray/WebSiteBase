using System.ComponentModel.DataAnnotations;

namespace WebBase.Mvc.ViewModels
{
	public class HomeIndexViewModel	
	{		
		[Display(Name = "Username")]
		public string UserName { get; set; }
	
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
	}
}