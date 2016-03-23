using _6tactics.Cms.Core.Entities;
using _DataAccess.Contexts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace _DataAccess.Repositories
{
    public class UserActivityTrackingRepository : IUserActivityTrackingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TrackedUser> _dbSet;

        public UserActivityTrackingRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TrackedUser>();
        }

        public void AddUserOnTracking(string ip, string dns)
        {
            _dbSet.Add(new TrackedUser { UsersIp = ip, UsersDns = dns });
        }

        public void AddUserOnTrackingWithActivity(string ip, string dns)
        {
            if (_dbSet.Any(i => i.UsersIp.Equals(ip)))
                _dbSet.First(i => i.UsersIp.Equals(ip)).UserActivityTrackings.Add(new UserActivityTracking { Time = DateTime.Now });
            else
            {
                var trackedUser = new TrackedUser
                {
                    UsersIp = ip,
                    UsersDns = dns,
                    UserActivityTrackings = new List<UserActivityTracking>
                    {
                        new UserActivityTracking { Time =DateTime.Now }
                    }
                };

                _dbSet.Add(trackedUser);
            }

            _context.SaveChanges();
        }

        public bool IsTrackedUserByIpExist(string ip)
        {
            return _dbSet.Any(i => i.UsersIp.Equals(ip));
        }

        public bool IsTrackedUserByDnsExist(string dns)
        {
            return _dbSet.Any(i => i.UsersIp.Equals(dns));
        }

        public IQueryable<TrackedUser> GetTrackedUserByIp(string ip)
        {
            return _dbSet.Where(i => i.UsersIp.Equals(ip)).Include(i => i.UserActivityTrackings);
        }

        public IQueryable<TrackedUser> GetTrackedUserByDns(string dns)
        {
            return _dbSet.Where(i => i.UsersIp.Equals(dns)).Include(i => i.UserActivityTrackings);
        }

        private int GetUserLastActivityInHours(TrackedUser userByIp)
        {
            if (userByIp == null || !userByIp.UserActivityTrackings.Any()) return 0;

            DateTime lastUserActivity = userByIp.UserActivityTrackings.Last().Time;
            DateTime timeDifference = DateTime.Now.Subtract(lastUserActivity.TimeOfDay);

            return timeDifference.Hour;
        }

        public int TimePassedFromLastActivityByIpInHours(string ip)
        {
            return GetUserLastActivityInHours(_dbSet.FirstOrDefault(i => i.UsersIp.Equals(ip)));
        }

        public int TimePassedFromActivityByDnsInHours(string dns)
        {
            return GetUserLastActivityInHours(_dbSet.FirstOrDefault(i => i.UsersIp.Equals(dns)));
        }


        public void RemoveUserByIp(string ip)
        {
            if (!_dbSet.Any(i => i.UsersIp.Equals(ip))) return;

            _dbSet.Remove(_dbSet.First(i => i.UsersIp.Equals(ip)));
            _context.SaveChanges();
        }

        public void RemoveUserByDns(string dns)
        {
            if (!_dbSet.Any(i => i.UsersIp.Equals(dns))) return;

            _dbSet.Remove(_dbSet.First(i => i.UsersIp.Equals(dns)));
            _context.SaveChanges();
        }
    }
}
