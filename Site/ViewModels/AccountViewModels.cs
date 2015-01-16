using System.ComponentModel.DataAnnotations;

namespace WebBase.Mvc.ViewModels
{
	public class RegisterExternalLoginViewModel
	{		
		[Display(Name = "Username")]
		public string UserName { get; set; }

		public string ExternalLoginData { get; set; }
	}

    public class LostPasswordViewModel
    {
        public string EmailAddress { get; set; }
    }

	public class ResetPasswordViewModel 
	{
        [Display(Name = "Username")]
        public string Username { get; set; }
		
		[Display(Name = "New password")]
		public string NewPassword { get; set; }

		[Display(Name = "Confirm password")]
		public string ConfirmPassword { get; set; }
	}

	public class LoginViewViewModel 
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

	public class ExternalLoginViewModel
	{
		public string Provider { get; set; }
		public string ProviderDisplayName { get; set; }
		public string ProviderUserId { get; set; }
	}
}
