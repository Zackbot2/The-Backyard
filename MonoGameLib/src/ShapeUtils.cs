using Microsoft.Xna.Framework;

namespace TheBackyard.MonoGameLib;

internal static class ShapeUtils
{
    #region intersects

    #endregion intersects
    #region distance
    /// <summary>
    /// Get the distance between the <see cref="IShape.Position"/> of <paramref name="shape1"/> and the <see cref="IShape.Position"/> of <paramref name="shape2"/>.
    /// </summary>
    /// <param name="shape1"></param>
    /// <param name="shape2"></param>
    /// <returns></returns>
    /// <remarks>This is not useful for accurate distance calculation, as the name suggests. Every <see cref="IShape.Position"/> means something different, so this calculation loses meaning the closer the shapes are to eachother.
    /// It is, however, much cheaper than doing precise calculations on all <see cref="IShape"/>s regardless of their actual position.</remarks>
    public static int GetRoughDistance(IShape shape1, IShape shape2)
    {
        Point distancePoint = shape1.Position - shape2.Position;

        // we only want the positive version of the distance.
        // sadly, Point.Absolute() isn't a thing, so we have to do it manually.
        if (distancePoint.X < 0)
            distancePoint.X *= -1;
        if (distancePoint.Y < 0)
            distancePoint.Y *= -1;

        return (int)MathF.Sqrt(distancePoint.X * distancePoint.X + distancePoint.Y * distancePoint.Y);
    }
    
    #endregion
}