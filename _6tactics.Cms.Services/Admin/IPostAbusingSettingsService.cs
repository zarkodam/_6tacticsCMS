using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Core.Models.Admin;

namespace _6tactics.Cms.Services.Admin
{
    public interface IPostAbusingSettingsService
    {
        IPostAbusingSettings Get(PostAbusingSettingsFor chooseSetting);
    }
}