namespace PlaybackDataServer.App.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string Format(this TimeSpan timeSpan)
        {
            return $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
        }
    }
}