using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Castle.Transactions;
using Domain.QueryFilters;
using WebBase.Mvc.Helpers;
using WebBase.Mvc.Services;
using WebBase.Mvc.ViewModels;

namespace WebBase.Mvc.Controllers
{
    [Authorize]    
	public class AccountController : MembershipControllerBase
    {
        private readonly IEmailService _emailService;
        
        public AccountController(IEmailService emailService)
        {
            _emailService = emailService;            
        }

        [AllowAnonymous]
        public JsonResult VerifyUserExists(string userName)
        {
            if (UserRepository.Exists(userName))
                return Json(ErrorCodeToString(MembershipCreateStatus.DuplicateUserName), JsonRequestBehavior.AllowGet);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //
		// POST: /Account/LogOff
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{			
            AuthenticationProvider.SignOut();

			return RedirectToAction("Index", "Home");
		}
        
        [AllowAnonymous]
        public ActionResult LostPassword()
        {
            return View();
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
        [AllowAnonymous]
        [Transaction]
        public virtual ActionResult LostPassword(LostPasswordViewModel viewModel)
        {            
            var filter = new ExistingUserEmailAddressFilter() {EmailAddress = viewModel.EmailAddress};
            var user = UserRepository.GetByFilter(filter).FirstOrDefault();
            if (user == null)
            {
                return AjaxResponse(ResponseType.ValidationError, "Invalid Email: " + viewModel.EmailAddress);
            }

            if (user.IsConfirmed)
            {
                user.SetResetRequest(CryptographyProvider);

                var resetUrl = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority) +
                               "/account/resetpassword/" + user.PasswordVerificationToken;
                _emailService.SendPasswordReset(user, resetUrl);

                var redirectUrl = Url.ActionFullPath("Index", "Home");
                return AjaxResponse(ResponseType.RedirectAndMessage, 
                    new { RedirectUrl = redirectUrl, Message = "A password reset email has been sent to your account." });
            }                         

            return AjaxResponse(ResponseType.Warning, "Your account exists but you have not yet completed registration.  " +
                                                      "Please click on the link in the email you received from your Prescriber.  " +
                                                      "If you cannot find your email please contact your prescriber.");            
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string resetToken)
        {
            var filter = new UserByPasswordResetTokenFilter() {ResetToken = resetToken};
            var user = UserRepository.GetByFilter(filter).SingleOrDefault();

            if (user == null)
            {
                SetSiteMessage(SiteMessageType.Error, "Could not validate active Password Reset Token. Please reset again");
                
            }

            var viewModel = new ResetPasswordViewModel() {Username = user.Username};


            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [Transaction]
        public virtual ActionResult ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = UserRepository.GetByUsername(viewModel.Username);
                if (user == null)
                {
                    throw new Exception("Username '" + viewModel.Username + "' not found");
                }
                
                user.SetPassword(CryptographyProvider, viewModel.NewPassword);

                AuthenticationProvider.DoAuth(viewModel.Username, false);

                SetSiteMessage(SiteMessageType.Success, "Password changed");
                
                return RedirectToAction("Index", "Home");                
            }
                        
            return View(viewModel);
        }
     

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(HomeIndexViewModel viewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = UserRepository.GetByLogin(viewModel.UserName, viewModel.Password);
                if (user == null)
                {
                    return AjaxResponse(ResponseType.ValidationError, "Invalid password account: " + viewModel.UserName);
                }

                if (user.IsConfirmed)
                {
                    AuthenticationProvider.DoAuth(user.Username, viewModel.RememberMe);
                    
                    return AjaxResponse(ResponseType.Redirect, returnUrl ?? Url.Action("Index", "Home"));
                }
            }
            // If we got this far, something failed, redisplay form            
            return AjaxResponse(ResponseType.ValidationError, "The username or password provided is incorrect.");
        }
      
	}
}
