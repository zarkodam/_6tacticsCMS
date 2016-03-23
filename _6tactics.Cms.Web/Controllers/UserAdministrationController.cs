using _6tactics.AspIdentity.Models;
using _6tactics.AspIdentity.Repositories;
using _6tactics.AspIdentity.ViewModels.Account;
using _6tactics.AspIdentity.ViewModels.UsersHandler;
using _6tactics.Cms.Core.Attributes;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _6tactics.Cms.Web.Controllers
{
    [Authorizer(Roles = "Administrators,AdminReadOnly")]
    public class UserAdministrationController : IdentityBaseController
    {
        #region Constructors

        public UserAdministrationController(IIdentityRepository identityRepository)
            : base(identityRepository)
        { }

        #endregion

        #region Actions

        public async Task<ActionResult> Index()
        {
            var model = new List<UsersHandlerIndexViewModel>();

            IList<ApplicationUser> usersList = await IdentityRepository.UserManager.Users.ToListAsync();
            IList<ApplicationUser> usersListWithoutAdmins = usersList.Where(i => !i.UserName.Equals("administrator", StringComparison.InvariantCultureIgnoreCase) &&
            !i.UserName.Equals("admin", StringComparison.InvariantCultureIgnoreCase)).ToList();

            foreach (ApplicationUser user in usersListWithoutAdmins)
            {
                var viewModelItem = new UsersHandlerIndexViewModel { ApplicationUser = user };

                IEnumerable<string> roles = await IdentityRepository.UserManager.GetRolesAsync(user.Id);

                if (roles.Any())
                    foreach (string role in roles)
                        viewModelItem.RolesList.Add(role);

                model.Add(viewModelItem);
            }

            return View(model.OrderBy(i => i.ApplicationUser.UserName));
        }

        public async Task<ActionResult> Create()
        {
            return View(new UsersHandlerCreateViewModel { SelectedRoles = await IdentityRepository.RoleManager.Roles.ToListAsync() });
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel registerViewModel, params string[] selectedRoles)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index");

            var model = new UsersHandlerCreateViewModel
            {
                RegisterViewModel = registerViewModel,
                SelectedRoles = await IdentityRepository.RoleManager.Roles.ToListAsync()
            };

            var user = new ApplicationUser { UserName = registerViewModel.Username, Email = registerViewModel.Email };

            IdentityResult userResult = await IdentityRepository.UserManager.CreateAsync(user, registerViewModel.Password);

            if (!userResult.Succeeded)
            {
                AddErrors(userResult);
                return View(model);
            }

            // Confirm email manualy
            string token = await IdentityRepository.UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            await IdentityRepository.UserManager.ConfirmEmailAsync(user.Id, token);

            //await IdentityRepository.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            //string code = await IdentityRepository.UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            //string callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            //await IdentityRepository.UserManager.SendEmailAsync(user.Id, "Confirm your account", callbackUrl);

            if (selectedRoles != null)
            {
                IdentityResult rolesResult = await IdentityRepository.UserManager.AddToRolesAsync(user.Id, selectedRoles);

                if (!rolesResult.Succeeded)
                {
                    ModelState.AddModelError("", rolesResult.Errors.First());
                    return View(model);
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> CreatePrepare()
        {
            return View(new UsersHandlerCreatePrepareViewModel { SelectedRoles = await IdentityRepository.RoleManager.Roles.ToListAsync() });
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePrepare(UserPersonalDataViewModel userPersonalData, params string[] selectedRoles)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index");

            var model = new UsersHandlerCreatePrepareViewModel
            {
                UserPersonalData = userPersonalData,
                SelectedRoles = await IdentityRepository.RoleManager.Roles.ToListAsync()
            };

            var user = new ApplicationUser { UserName = userPersonalData.Username, Email = userPersonalData.Email };

            // Add password manualy
            IdentityResult userResult = await IdentityRepository.UserManager.CreateAsync(user, "P@ssw0rd");

            if (!userResult.Succeeded)
            {
                AddErrors(userResult);
                return View(model);
            }

            // Confirm email manualy
            string token = await IdentityRepository.UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            await IdentityRepository.UserManager.ConfirmEmailAsync(user.Id, token);

            string code = await IdentityRepository.UserManager.GeneratePasswordResetTokenAsync(user.Id);
            string callbackUrl = Url.Action("CompleteRegistration", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

            await IdentityRepository.UserManager.SendEmailAsync(user.Id, "Create password for your account", callbackUrl);

            if (selectedRoles != null)
            {
                IdentityResult rolesResult = await IdentityRepository.UserManager.AddToRolesAsync(user.Id, selectedRoles);

                if (!rolesResult.Succeeded)
                {
                    ModelState.AddModelError("", rolesResult.Errors.First());
                    return View(model);
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ApplicationUser user = await IdentityRepository.UserManager.FindByIdAsync(id);

            if (user == null)
                return HttpNotFound();

            IList<string> userRoles = await IdentityRepository.UserManager.GetRolesAsync(user.Id);

            IQueryable<SelectListItem> rolesListForSelectedUser = IdentityRepository.RoleManager.Roles.Select(x => new SelectListItem
            {
                Selected = userRoles.Contains(x.Name),
                Text = x.Name,
                Value = x.Name
            });

            var model = new UsersHandlerEditViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                ResetPassword = false,
                RolesList = rolesListForSelectedUser
            };

            return View(model);
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UsersHandlerEditViewModel model, params string[] selectedRole)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Model is not valid!");
                return View();
            }

            ApplicationUser user = await IdentityRepository.UserManager.FindByIdAsync(model.Id);

            if (user == null) return HttpNotFound();

            user.UserName = model.UserName;
            user.Email = model.Email;

            IList<string> userRoles = await IdentityRepository.UserManager.GetRolesAsync(user.Id);

            selectedRole = selectedRole ?? new string[] { };

            IdentityResult result = await IdentityRepository.UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray());

            IsResultNotSucceededAddModelError(result);

            result = await IdentityRepository.UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray());

            IsResultNotSucceededAddModelError(result);

            if (model.ResetPassword)
            {
                if (!ModelState.IsValid) return View(model);

                string code = await IdentityRepository.UserManager.GeneratePasswordResetTokenAsync(user.Id);
                string callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                await IdentityRepository.UserManager.SendEmailAsync(user.Id, "Reset Password", callbackUrl);
            }

            if (result.Succeeded) return RedirectToAction("Index");

            return View();
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ApplicationUser user = await IdentityRepository.UserManager.FindByIdAsync(id);

            if (user == null) return HttpNotFound();

            var viewModelItem = new UsersHandlerIndexViewModel { ApplicationUser = user };

            IEnumerable<string> roles = await IdentityRepository.UserManager.GetRolesAsync(user.Id);

            if (roles.Any())
                foreach (string role in roles)
                    viewModelItem.RolesList.Add(role);


            return View(viewModelItem);
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (!ModelState.IsValid) return View();

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ApplicationUser user = await IdentityRepository.UserManager.FindByIdAsync(id);

            if (user == null) return HttpNotFound();

            IdentityResult result = await IdentityRepository.UserManager.DeleteAsync(user);

            IsResultNotSucceededAddModelError(result);

            if (result.Succeeded) return RedirectToAction("Index");

            return View();
        }

        #endregion
    }
}