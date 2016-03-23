using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace _6tactics.AspIdentity.Handlers
{
    public interface IApplicationRoleHandler
    {
        void Dispose();
        Task<IdentityResult> CreateAsync(IdentityRole role);
        Task<IdentityResult> UpdateAsync(IdentityRole role);
        Task<IdentityResult> DeleteAsync(IdentityRole role);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityRole> FindByIdAsync(string roleId);
        Task<IdentityRole> FindByNameAsync(string roleName);
        IIdentityValidator<IdentityRole> RoleValidator { get; set; }
        IQueryable<IdentityRole> Roles { get; }
    }
}