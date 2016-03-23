using _6tactics.Cms.Core.Entities;
using System.Linq;

namespace _DataAccess.Repositories
{
    public interface IUserActivityTrackingRepository
    {
        void AddUserOnTracking(string ip, string dns);
        void AddUserOnTrackingWithActivity(string ip, string dns);
        bool IsTrackedUserByIpExist(string ip);
        IQueryable<TrackedUser> GetTrackedUserByIp(string ip);
        IQueryable<TrackedUser> GetTrackedUserByDns(string dns);
        int TimePassedFromLastActivityByIpInHours(string ip);
        int TimePassedFromActivityByDnsInHours(string dns);
        void RemoveUserByIp(string ip);
        void RemoveUserByDns(string dns);
    }
}