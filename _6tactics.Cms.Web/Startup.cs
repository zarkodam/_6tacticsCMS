using _6tactics.AspIdentity.Initialization;
using _6tactics.Cms.Web;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace _6tactics.Cms.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            IdentityConfig.ConfigureAuth(app);
        }
    }
}
