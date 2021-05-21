namespace CowinAvailabilityTracker.Constants
{
    public class Enums
    {
        public enum RunMode
        {
            District,
            RunAllForOnce
        }

        public enum VaccineSearchMode
        {
            DEFAULT,
            ALL,
            COVISHIELD,
            COVAXIN
        }

        public enum NotifyOn
        {
            Telegram,
            GoogleChat
        }
    }
}
