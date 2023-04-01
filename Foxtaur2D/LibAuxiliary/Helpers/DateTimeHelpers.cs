namespace LibAuxiliary.Helpers;

/// <summary>
/// Useful stuff to work with date time
/// </summary>
public static class DateTimeHelpers
{
    public static string ToDateTimeString(this DateTime dateTime)
    {
        return dateTime.ToString("g");
    }
}