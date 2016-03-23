using System.Collections.Generic;

namespace _6tactics.Cms.Core.Entities
{
    public class TrackedUser
    {
        public int Id { get; set; }
        public string UsersIp { get; set; }
        public string UsersDns { get; set; }
        public ICollection<UserActivityTracking> UserActivityTrackings { get; set; }
    }
}
