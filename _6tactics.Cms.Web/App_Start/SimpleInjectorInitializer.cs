using _6tactics.AspIdentity.Repositories;
using _6tactics.Cms.Services.Admin;
using _6tactics.Cms.Services.Common;
using _6tactics.Cms.Services.Plugins;
using _6tactics.Cms.Services.Web;
using _DataAccess.Contexts;
using _DataAccess.Repositories;
using _DataAccess.UnitOfWork;

[assembly: WebActivator.PostApplicationStartMethod(typeof(_6tactics.Cms.Web.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace _6tactics.Cms.Web.App_Start
{
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using System.Reflection;
    using System.Web.Mvc;

    public static class SimpleInjectorInitializer
    {
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void InitializeContainer(Container container)
        {
            //Identity
            container.Register<IIdentityRepository, IdentityRepository>(Lifestyle.Scoped);

            container.Register<ApplicationDbContext>(Lifestyle.Scoped);

            // Unit of work
            container.Register<IUnitOfWork, UnitOfWork>(Lifestyle.Scoped);

            // Repositories
            container.Register<IContentItemRepository, ContentItemRepository>(Lifestyle.Scoped);
            container.Register<IUserActivityTrackingRepository, UserActivityTrackingRepository>(Lifestyle.Scoped);

            // Services
            container.Register<IContentItemsWithParentCountService, ContentItemsWithParentCountService>(Lifestyle.Scoped);
            container.Register<IAdminLogicService, AdminLogicService>(Lifestyle.Scoped);
            container.Register<IAdminViewModelBuilderService, AdminViewModelBuilderService>(Lifestyle.Scoped);
            container.Register<ISetupService, SetupService>(Lifestyle.Scoped);
            container.Register<IPluginsManagerService, PluginsManagerService>(Lifestyle.Scoped);
            container.Register<ISeoDataService, SeoDataService>(Lifestyle.Scoped);
            container.Register<ISiteMapService, SiteMapService>(Lifestyle.Scoped);
            container.Register<IUserActivityTrackingService, UserActivityTrackingService>(Lifestyle.Scoped);
            container.Register<ISimpleCaptchaService, SimpleCaptchaService>(Lifestyle.Scoped);
            container.Register<IPostAbusingSettingsService, PostAbusingSettingsService>(Lifestyle.Scoped);
        }
    }
}