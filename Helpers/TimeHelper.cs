namespace AuthMvcApp.Helpers;

public static class TimeHelper
{
    private static readonly TimeZoneInfo IndianZone = ResolveIndianTimeZone();

    private static TimeZoneInfo ResolveIndianTimeZone()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        }
        catch
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata");
            }
            catch
            {
                return TimeZoneInfo.CreateCustomTimeZone("IST", TimeSpan.FromMinutes(330), "India Standard Time", "India Standard Time");
            }
        }
    }

    /// <summary>
    /// Returns the current DateTime in Indian Standard Time (IST, UTC+05:30)
    /// </summary>
    public static DateTime Now => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IndianZone);

    /// <summary>
    /// Returns the current Date in Indian Standard Time (IST, UTC+05:30)
    /// </summary>
    public static DateTime Today => Now.Date;

    /// <summary>
    /// Converts a DateTime to Indian Standard Time
    /// </summary>
    public static DateTime ToIndianTime(DateTime dt)
    {
        if (dt.Kind == DateTimeKind.Utc)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dt, IndianZone);
        }
        return dt;
    }
}
