using Microsoft.Xna.Framework;

namespace TheBackyard.MonoGameLib;

/// <summary>
/// Extension methods adding flexibility with translations. (Movement, rotation, etc.)
/// Do note that NONE of these extensions modify the structs/classes they're called on.
/// </summary>
public static class TranslationExtensions
{
    #region rotation
    /// <summary>
    /// Rotate <paramref name="point"/> around a given <paramref name="origin"/> by <paramref name="radians"/> and return the result.
    /// </summary>
    /// <param name="point"></param>
    /// <param name="origin">The origin of rotation.</param>
    /// <param name="radians"></param>
    /// <returns>
    /// Returns the rotated point.
    /// </returns>
    public static Point RotateAround(this Point point, Vector2 origin, float radians)
    {
        point -= origin.ToPoint();
        point = point.Rotate(radians);
        return point + origin.ToPoint();
    }

    /// <summary>
    /// Rotate <paramref name="point"/> around a given <paramref name="origin"/> by <paramref name="radians"/> and return the result.
    /// </summary>
    /// <param name="point"></param>
    /// <param name="origin">The origin of rotation.</param>
    /// <param name="radians"></param>
    /// <returns>
    /// Returns the rotated point.
    /// </returns>
    public static Point RotateAround(this Point point, Point origin, float radians)
    {
        point -= origin;
        point = point.Rotate(radians);
        return point + origin;
    }

    /// <summary>
    /// Rotate <paramref name="point"/> by <paramref name="radians"/> and return the result.
    /// </summary>
    /// <param name="point"></param>
    /// <param name="radians"></param>
    /// <returns>
    /// Returns the rotated point.
    /// </returns>
    public static Point Rotate(this Point point, float radians)
    {
        float cos = MathF.Cos(radians);
        float sin = MathF.Sin(radians);
        int x = point.X;
        point.X = (int)(point.X * cos - point.Y * sin);
        point.Y = (int)(x * sin + point.Y * cos);
        return point;
    }

    /// <summary>
    /// Rotate <paramref name="point"/> by <paramref name="radians"/> and return the result.
    /// </summary>
    /// <param name="point"></param>
    /// <param name="radians"></param>
    /// <returns>
    /// Returns the rotated point.
    /// </returns>
    public static Point Rotate(this Point point, double radians)
    {
        double cos = Math.Cos(radians);
        double sin = Math.Sin(radians);
        int x = point.X;
        point.X = (int)(point.X * cos - point.Y * sin);
        point.Y = (int)(x * sin + point.Y * cos);
        return point;
    }
    #endregion rotation
}