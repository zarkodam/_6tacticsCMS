namespace _6tactics.Cms.Core.Models.Admin
{
    public interface IPostAbusingSettings
    {
        int NumberOfAllowedAttempts { get; set; }
        int TimePerAllowedAttemptsInHours { get; set; }
        int MaxBlockingTimeInHours { get; set; }
    }
}