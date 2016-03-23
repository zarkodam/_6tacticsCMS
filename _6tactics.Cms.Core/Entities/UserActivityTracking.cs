using System;

namespace _6tactics.Cms.Core.Entities
{
    public class UserActivityTracking
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }

        public int TrackedUserId { get; set; }
        public TrackedUser TrackedUser { get; set; }
    }
}
