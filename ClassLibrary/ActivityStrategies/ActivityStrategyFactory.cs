namespace DeviceSimulator
{
    public class ActivityStrategyFactory
    {
        public IActivityStrategy CreateStrategy(ActivityType activityType)
        {
            return activityType switch
            {
                ActivityType.Gaming => new GamingStrategy(),
                ActivityType.Working => new WorkingStrategy(),
                ActivityType.Chatting => new ChattingStrategy(),
                ActivityType.WatchingVideo => new WatchingVideoStrategy(),
                ActivityType.ListeningMusic => new ListeningMusicStrategy(),
                ActivityType.Printing => new PrintingStrategy(),
                _ => throw new ArgumentException($"Unsupported activity type: {activityType}")
            };
        }
    }
}