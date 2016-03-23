using _6tactics.Cms.Core.Models.Admin;

namespace _6tactics.Cms.Services.Admin
{
    public interface IUserActivityTrackingService
    {
        bool IsRequestComingFromPossibleBot(string ip, string dns, IPostAbusingSettings settings);
    }
}