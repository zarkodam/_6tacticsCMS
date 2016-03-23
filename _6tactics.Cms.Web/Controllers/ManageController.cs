using _6tactics.AspIdentity.Models;
using _6tactics.AspIdentity.Repositories;
using _6tactics.AspIdentity.ViewModels.Manage;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _6tactics.Cms.Web.Controllers
{
    public class ManageController : IdentityBaseController
    {
        #region Constructors

        public ManageController(IIdentityRepository identityRepository)
            : base(identityRepository)
        { }

        #endregion

        #region Actions

        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed!"
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set!"
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set!"
                : message == ManageMessageId.Error ? "An error has occurred!"
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added!"
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed!"
                : message == ManageMessageId.ChangeEmailSuccess ? "Your e-mail has been changed!"
                : "";

            string userId = User.Identity.GetUserId();

            var model = new IndexViewModel
            {
                HasPassword = await HasPassword(),
                PhoneNumber = await IdentityRepository.UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await IdentityRepository.UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await IdentityRepository.UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }


        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        //{
        //    ManageMessageId? message;

        //    IdentityResult result = await IdentityRepository.UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));

        //    if (result.Succeeded)
        //    {
        //        ApplicationUser user = await IdentityRepository.UserManager.FindByIdAsync(User.Identity.GetUserId());

        //        if (user != null)
        //            await IdentityRepository.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

        //        message = ManageMessageId.RemoveLoginSuccess;
        //    }
        //    else
        //        message = ManageMessageId.Error;

        //    return RedirectToAction("ManageLogins", new { Message = message });
        //}

        //public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        //{
        //    ViewBag.StatusMessage =
        //        message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
        //        : message == ManageMessageId.Error ? "An error has occurred."
        //        : "";

        //    ApplicationUser user = await IdentityRepository.UserManager.FindByIdAsync(User.Identity.GetUserId());

        //    if (user == null) return View("Error");

        //    IList<UserLoginInfo> userLogins = await IdentityRepository.UserManager.GetLoginsAsync(User.Identity.GetUserId());

        //    IList<AuthenticationDescription> otherLogins = AuthenticationManager.GetExternalAuthenticationTypes()
        //        .Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();

        //    ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;

        //    return View(new ManageLoginsViewModel
        //    {
        //        CurrentLogins = userLogins,
        //        OtherLogins = otherLogins
        //    });
        //}

        public async Task<ActionResult> ChangeEmail()
        {
            ApplicationUser user = await IdentityRepository.UserManager.FindByIdAsync(User.Identity.GetUserId());
            return View(new ChangeEmailViewModel { Email = user.Email });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeEmail(ChangeEmailViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            ApplicationUser user = await IdentityRepository.UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null) return View(model);

            user.Email = model.Email;

            IdentityResult result = await IdentityRepository.UserManager.UpdateAsync(user);

            if (result.Succeeded)
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangeEmailSuccess });

            AddErrors(result);
            return View(model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            IdentityResult result = await IdentityRepository.UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                ApplicationUser user = await IdentityRepository.UserManager.FindByIdAsync(User.Identity.GetUserId());

                if (user != null)
                    await IdentityRepository.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }

            AddErrors(result);
            return View(model);
        }

        //public ActionResult SetPassword()
        //{
        //    return View();
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid) return View(model);

        //    IdentityResult result = await IdentityRepository.UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

        //    if (result.Succeeded)
        //    {
        //        ApplicationUser user = await IdentityRepository.UserManager.FindByIdAsync(User.Identity.GetUserId());

        //        if (user != null)
        //            await IdentityRepository.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

        //        return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
        //    }

        //    AddErrors(result);

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}


        //public ActionResult AddPhoneNumber()
        //{
        //    return View();
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        //{
        //    if (!ModelState.IsValid) return View(model);

        //    // Generate the token and send it
        //    string code = await IdentityRepository.UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);

        //    if (IdentityRepository.UserManager.SmsService == null)
        //        return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });

        //    var message = new IdentityMessage
        //    {
        //        Destination = model.Number,
        //        Body = "Your security code is: " + code
        //    };

        //    await IdentityRepository.UserManager.SmsService.SendAsync(message);

        //    return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<ActionResult> EnableTwoFactorAuthentication()
        //{
        //    await IdentityRepository.UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);

        //    ApplicationUser user = await IdentityRepository.UserManager.FindByIdAsync(User.Identity.GetUserId());

        //    if (user != null)
        //        await IdentityRepository.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

        //    return RedirectToAction("Index", "Manage");
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<ActionResult> DisableTwoFactorAuthentication()
        //{
        //    await IdentityRepository.UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);

        //    ApplicationUser user = await IdentityRepository.UserManager.FindByIdAsync(User.Identity.GetUserId());

        //    if (user != null)
        //        await IdentityRepository.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

        //    return RedirectToAction("Index", "Manage");
        //}

        //public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        //{
        //    string code = await IdentityRepository.UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);

        //    // Send an SMS through the SMS provider to verify the phone number
        //    return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        //{
        //    if (!ModelState.IsValid) return View(model);

        //    IdentityResult result = await IdentityRepository.UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);

        //    if (result.Succeeded)
        //    {
        //        ApplicationUser user = await IdentityRepository.UserManager.FindByIdAsync(User.Identity.GetUserId());

        //        if (user != null)
        //            await IdentityRepository.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

        //        return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
        //    }

        //    // If we got this far, something failed, redisplay form
        //    ModelState.AddModelError("", "Failed to verify phone");
        //    return View(model);
        //}

        //public async Task<ActionResult> RemovePhoneNumber()
        //{
        //    IdentityResult result = await IdentityRepository.UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);

        //    if (!result.Succeeded)
        //        return RedirectToAction("Index", new { Message = ManageMessageId.Error });

        //    ApplicationUser user = await IdentityRepository.UserManager.FindByIdAsync(User.Identity.GetUserId());

        //    if (user != null)
        //        await IdentityRepository.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

        //    return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult LinkLogin(string provider)
        //{
        //    // Request a redirect to the external login provider to link a login for the current user
        //    return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        //}

        //public async Task<ActionResult> LinkLoginCallback()
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());

        //    if (loginInfo == null)
        //    {
        //        return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        //    }
        //    var result = await IdentityRepository.UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
        //    return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        //}

        #endregion

    }
}