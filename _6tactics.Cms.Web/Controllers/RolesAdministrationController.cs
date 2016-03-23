using _6tactics.AspIdentity.Repositories;
using _6tactics.AspIdentity.ViewModels.RolesHandler;
using _6tactics.Cms.Core.Attributes;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _6tactics.Cms.Web.Controllers
{
    [Authorizer(Roles = "Administrators,AdminReadOnly")]
    public class RolesAdministrationController : IdentityBaseController
    {
        #region Constructors

        public RolesAdministrationController(IIdentityRepository identityRepository)
            : base(identityRepository)
        { }

        #endregion

        #region Actions

        public ActionResult Index()
        {
            return View(IdentityRepository.RoleManager.Roles.OrderBy(i => i.Name));
        }

        public ActionResult Create()
        {
            return View();
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleViewModel roleViewModel)
        {
            if (!ModelState.IsValid) return View();

            var role = new IdentityRole(roleViewModel.Name);

            IdentityResult result = await IdentityRepository.RoleManager.CreateAsync(role);

            IsResultNotSucceededAddModelError(result);

            if (result.Succeeded) return RedirectToAction("Index");

            return View();
        }

        public async Task<ActionResult> Edit(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var role = await IdentityRepository.RoleManager.FindByIdAsync(id);

            if (role == null) return HttpNotFound();

            RoleViewModel roleModel = new RoleViewModel { Id = role.Id, Name = role.Name };

            return View(roleModel);
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoleViewModel roleModel)
        {
            if (!ModelState.IsValid) return View();

            IdentityRole role = await IdentityRepository.RoleManager.FindByIdAsync(roleModel.Id);

            role.Name = roleModel.Name;

            await IdentityRepository.RoleManager.UpdateAsync(role);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            IdentityRole role = await IdentityRepository.RoleManager.FindByIdAsync(id);

            if (role == null) return HttpNotFound();

            return View(role);
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id, string deleteUser)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            IdentityRole role = await IdentityRepository.RoleManager.FindByIdAsync(id);

            if (role == null) return HttpNotFound();

            IdentityResult result = await IdentityRepository.RoleManager.DeleteAsync(role);

            IsResultNotSucceededAddModelError(result);

            if (result.Succeeded) return RedirectToAction("Index");

            return View();
        }

        #endregion
    }
}