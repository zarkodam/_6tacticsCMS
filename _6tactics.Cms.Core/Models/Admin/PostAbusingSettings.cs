namespace _6tactics.Cms.Core.Models.Admin
{
    public class PostAbusingSettings : IPostAbusingSettings
    {
        public int NumberOfAllowedAttempts { get; set; }
        public int TimePerAllowedAttemptsInHours { get; set; }
        public int MaxBlockingTimeInHours { get; set; }

        public PostAbusingSettings()
        {
            NumberOfAllowedAttempts = 5;
            TimePerAllowedAttemptsInHours = 1;
            MaxBlockingTimeInHours = 24;
        }
    }
}
