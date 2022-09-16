namespace MemberCoupon
{
    public static class Constants
    {
        public static TimeSpan TimeZone = new TimeSpan(8, 0, 0);

        public static DateTime CurrentTime()
        {
            return DateTimeOffset.UtcNow.ToOffset(Constants.TimeZone).DateTime;
        }
    }
}
