namespace LibAuxiliary.Helpers;

/// <summary>
/// Useful stuff to use with foxes
/// </summary>
public static class FoxHelpers
{
    /// <summary>
    /// Format fox frequency
    /// </summary>
    public static string FormatFoxFrequency(double frequency)
    {
        return $"{(frequency / 1000000):.00MHz}";
    }
}