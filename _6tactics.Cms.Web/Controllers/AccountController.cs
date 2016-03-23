using _6tactics.AspIdentity.Repositories;
using _6tactics.AspIdentity.ViewModels.Account;
using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Services.Admin;
using _6tactics.SimpleCaptcha;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ApplicationUser = _6tactics.AspIdentity.Models.ApplicationUser;
using ExternalLoginConfirmationViewModel = _6tactics.AspIdentity.ViewModels.Account.ExternalLoginConfirmationViewModel;
using LoginViewModel = _6tactics.AspIdentity.ViewModels.Account.LoginViewModel;
using RegisterViewModel = _6tactics.AspIdentity.ViewModels.Account.RegisterViewModel;

namespace _6tactics.Cms.Web.Controllers
{
    [Authorize]
    public class AccountController : IdentityBaseController
    {
        private readonly IUserActivityTrackingService _activityTracking;
        private readonly IPostAbusingSettingsService _postAbusingSettings;

        #region Constructors

        public AccountController(IIdentityRepository identityRepository, IUserActivityTrackingService activityTracking, IPostAbusingSettingsService postAbusingSettings)
            : base(identityRepository)
        {
            _activityTracking = activityTracking;
            _postAbusingSettings = postAbusingSettings;
        }

        #endregion

        #region Actions

        [AllowAnonymous]
        public ActionResult Login(string returnUrl, AccountMessageId? message)
        {
            ViewBag.StatusMessage =
                message == AccountMessageId.ConfirmationEmailSended ? "Your password reset confirmation e-mail has been sended!"
                : message == AccountMessageId.ConfirmationSuccessed ? "Thank you for confirming your e-mail!"
                : message == AccountMessageId.ResetPasswordSuccess ? "Your password has been reseted!"
                : message == AccountMessageId.RegistrationCompleted ? "Your account has been created!"
                : message == AccountMessageId.UserOrEmailConfirmationError ? "Your e-mail wasn't confirmed or user under that e-mail doesen't exist!"
                : message == AccountMessageId.Error ? "An error occurred while processing your request!"
                : "";

            ViewBag.ReturnUrl = returnUrl;

            return View(new LoginViewModel { Captcha = new SimpleCaptchaInitializer().GenerateCaptchaString("Captcha") });
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl = "/admin")
        {
            if (!ModelState.IsValid)
            {
                // Remove CaptchaCode error
                ModelState.Remove("CaptchaCode");
                // Set new value to CaptchaCode
                ModelState.SetModelValue("CaptchaCode", new ValueProviderResult("", "", CultureInfo.InvariantCulture));
                // Set new Captcha task to Captcha
                ModelState.SetModelValue("Captcha", new ValueProviderResult(new SimpleCaptchaInitializer().GenerateCaptchaString("Captcha"), "", CultureInfo.InvariantCulture));
                return View(model);
            }

            ApplicationUser user = await IdentityRepository.UserManager.FindAsync(model.Username, model.Password);

            if (user == null && _activityTracking.IsRequestComingFromPossibleBot(Request.UserHostAddress, Request.UserHostName, _postAbusingSettings.Get(PostAbusingSettingsFor.LoginForm)))
                return RedirectToAction("Index", "Web");

            // Tracking of failed login attempts
            //var textWriter = new FileWriter("logins.txt", "D:\\");
            //textWriter.Append($"Addres: {Request.UserHostAddress}, Hostname: {Request.UserHostName}, Username: {model.Username}, Password: {model.Password}");

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            SignInStatus result = await IdentityRepository.SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: true);

            return ValidateAndReturnProperView(result, model, returnUrl);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
            IdentityResult result = await IdentityRepository.UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await IdentityRepository.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await IdentityRepository.UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                string callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                await IdentityRepository.UserManager.SendEmailAsync(user.Id, "Confirm your account", callbackUrl);

                return RedirectToAction("Index", "Web");
            }

            AddErrors(result);

            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null) return RedirectToAction("Login", new { message = AccountMessageId.Error });

            IdentityResult result = await IdentityRepository.UserManager.ConfirmEmailAsync(userId, code);

            return RedirectToAction("Login", result.Succeeded
                ? new { message = AccountMessageId.ConfirmationSuccessed }
                : new { message = AccountMessageId.Error });
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            ApplicationUser user = await IdentityRepository.UserManager.FindByEmailAsync(model.Email);

            if (user == null || !(await IdentityRepository.UserManager.IsEmailConfirmedAsync(user.Id)))
                return RedirectToAction("Login", "Account", new { message = AccountMessageId.Error });

            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            // Send an email with this link
            string code = await IdentityRepository.UserManager.GeneratePasswordResetTokenAsync(user.Id);
            string callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

            await IdentityRepository.UserManager.SendEmailAsync(user.Id, "Reset Password", callbackUrl);

            return RedirectToAction("Login", "Account", new { message = AccountMessageId.ConfirmationEmailSended });
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            ApplicationUser user = await IdentityRepository.UserManager.FindByEmailAsync(model.Email);

            if (user == null)
                // Don't reveal that the user does not exist
                return RedirectToAction("Login", "Account", new { message = AccountMessageId.ResetPasswordSuccess });

            IdentityResult result = await IdentityRepository.UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);

            if (result.Succeeded)
                return RedirectToAction("Login", "Account", new { message = AccountMessageId.ResetPasswordSuccess });

            return RedirectToAction("Login", "Account", new { message = AccountMessageId.Error });
        }

        [AllowAnonymous]
        public ActionResult CompleteRegistration(string code)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> CompleteRegistration(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            ApplicationUser user = await IdentityRepository.UserManager.FindByEmailAsync(model.Email);

            if (user == null)
                return RedirectToAction("Login", "Account", new { message = AccountMessageId.RegistrationCompleted });

            IdentityResult result = await IdentityRepository.UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);

            if (result.Succeeded)
                return RedirectToAction("Login", "Account", new { message = AccountMessageId.RegistrationCompleted });

            return RedirectToAction("Login", "Account", new { message = AccountMessageId.Error });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Web");
        }

        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            string userId = await IdentityRepository.SignInManager.GetVerifiedUserIdAsync();

            if (userId == null) return View("Error");

            IList<string> userFactors = await IdentityRepository.UserManager.GetValidTwoFactorProvidersAsync(userId);
            List<SelectListItem> factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid) return View();

            // Generate the token and send it
            if (!await IdentityRepository.SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider)) return View("Error");

            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await IdentityRepository.SignInManager.HasBeenVerifiedAsync()) return View("Error");

            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            SignInStatus result = await IdentityRepository.SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);

            return ValidateAndReturnProperView(result, model);
        }

        //[HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        //public ActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    // Request a redirect to the external login provider
        //    return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        //}

        //[AllowAnonymous]
        //public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //{
        //    ExternalLoginInfo loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

        //    if (loginInfo == null) return RedirectToAction("Login");

        //    // Sign in the user with this external login provider if the user already has a login
        //    SignInStatus result = await IdentityRepository.SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);

        //    return ValidateAndReturnProperView(result, returnUrl, loginInfo);
        //}

        //[HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        //public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        //{
        //    if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Manage");

        //    if (ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        ExternalLoginInfo info = await AuthenticationManager.GetExternalLoginInfoAsync();

        //        if (info == null) return View("ExternalLoginFailure");

        //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

        //        IdentityResult result = await IdentityRepository.UserManager.CreateAsync(user);

        //        if (result.Succeeded)
        //        {
        //            result = await IdentityRepository.UserManager.AddLoginAsync(user.Id, info.Login);

        //            if (result.Succeeded)
        //            {
        //                await IdentityRepository.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //                return RedirectToLocal(returnUrl);
        //            }
        //        }
        //        AddErrors(result);
        //    }

        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}


        //[AllowAnonymous]
        //public ActionResult ExternalLoginFailure()
        //{
        //    return View();
        //}

        #endregion


        #region Helpers

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);

            return RedirectToAction("Index", "Web");
        }

        private ActionResult ValidateAndReturnProperView(SignInStatus result, LoginViewModel model, string returnUrl)
        {
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        private ActionResult ValidateAndReturnProperView(SignInStatus result, VerifyCodeViewModel model)
        {
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        private ActionResult ValidateAndReturnProperView(SignInStatus result, string returnUrl, ExternalLoginInfo loginInfo)
        {
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        #endregion

    }
}