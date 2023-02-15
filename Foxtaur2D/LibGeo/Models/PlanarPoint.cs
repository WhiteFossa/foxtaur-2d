namespace LibGeo.Models;

/// <summary>
/// Point with planar coordinates
/// </summary>
public class PlanarPoint
{
    /// <summary>
    /// X
    /// </summary>
    public double X { get; private set; }

    /// <summary>
    /// Y
    /// </summary>
    public double Y { get; private set; }

    public PlanarPoint(double x, double y)
    {
        if (x < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(x));
        }

        if (y < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(y));
        }

        X = x;
        Y = y;
    }
}