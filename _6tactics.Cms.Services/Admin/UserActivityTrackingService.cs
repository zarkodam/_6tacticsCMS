using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Core.Models.Admin;
using _DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _6tactics.Cms.Services.Admin
{
    public class UserActivityTrackingService : IUserActivityTrackingService
    {
        private readonly IUserActivityTrackingRepository _activityTracking;

        public UserActivityTrackingService(IUserActivityTrackingRepository activityTracking)
        {
            _activityTracking = activityTracking;
        }

        private int TimeHolderCalculator(List<UserActivityTracking> trackingsPerIp, IPostAbusingSettings settings)
        {
            int blockingTime = (trackingsPerIp.Count / settings.NumberOfAllowedAttempts) * settings.TimePerAllowedAttemptsInHours;
            return Math.Min(blockingTime, settings.MaxBlockingTimeInHours);
        }

        private bool IsUserAllowedToTryAgain(List<UserActivityTracking> trackingsPerIp, int delayInHours)
        {
            return delayInHours != 0 && trackingsPerIp.Last().Time.AddHours(delayInHours) <= DateTime.Now;
        }

        public bool IsRequestComingFromPossibleBot(string ip, string dns, IPostAbusingSettings settings)
        {
            bool isBoot = true;

            if (_activityTracking.IsTrackedUserByIpExist(ip))
            {
                List<UserActivityTracking> trackings = _activityTracking.GetTrackedUserByIp(ip).First().UserActivityTrackings.ToList();

                int delayTimeInHours = TimeHolderCalculator(trackings, settings);

                // If max time passes remove ip from db
                if (IsUserAllowedToTryAgain(trackings, delayTimeInHours))
                {
                    _activityTracking.RemoveUserByIp(ip);
                    isBoot = false;
                }

                if (delayTimeInHours == 0)
                    isBoot = false;
            }
            else
                isBoot = false;

            _activityTracking.AddUserOnTrackingWithActivity(ip, dns);

            return isBoot;
        }
    }
}
