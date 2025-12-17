using Microsoft.Xna.Framework;

namespace TheBackyard.MonoGameLib;

// todo: refactor this class. I am not happy with how it works currently, and the functionality is way too general.
internal static class ShapeUtils
{
    #region intersects

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rect1"></param>
    /// <param name="rect2"></param>
    /// <returns></returns>
    public static bool Intersects(Rectangle rect1, Rectangle rect2)
    {
        return 
            rect1.Left < rect2.Right && rect1.Right > rect2.Left 
            && rect1.Top < rect2.Bottom && rect1.Bottom > rect2.Top;
    }
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rect1"></param>
    /// <param name="rect2"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static int GetMinimumDistance(Rectangle rect1, Rectangle rect2)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="circ"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static int GetMinimumDistance(Rectangle rect, Circle circ)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get the minimum distance between two <see cref="Circle"/>s.
    /// </summary>
    /// <param name="circ1"></param>
    /// <param name="circ2"></param>
    /// <returns>The distance BETWEEN each circle. Will be negative if they are overlapping.</returns>
    public static int GetMinimumDistance(Circle circ1, Circle circ2)
    {
        Point distancePoint = circ1.Position - circ2.Position;

        // we only want the positive version of the distance.
        // sadly, Point.Absolute() isn't a thing, so we have to do it manually.
        if (distancePoint.X < 0)
            distancePoint.X *= -1;
        if (distancePoint.Y < 0)
            distancePoint.Y *= -1;
        
        // use trig to calculate the effective distance, and then subtract each circle's radius because the insides of the circles don't count.
        return (int)MathF.Sqrt(distancePoint.X * distancePoint.X + distancePoint.Y * distancePoint.Y) - (circ1.Radius + circ2.Radius);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rect1"></param>
    /// <param name="rect2"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static int GetMaximumDistance(Rectangle rect1, Rectangle rect2)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="circ"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static int GetMaximumDistance(Rectangle rect, Circle circ)
    {
        throw new NotImplementedException();
    }
    

    /// <summary>
    /// Get the minimum distance between two <see cref="Circle"/>s.
    /// </summary>
    /// <param name="circ1"></param>
    /// <param name="circ2"></param>
    /// <returns>The distance BETWEEN each circle. Will never be negative..</returns>
    public static int GetMaximumDistance(Circle circ1, Circle circ2)
    {
        Point distancePoint = circ1.Position - circ2.Position;

        // we only want the positive version of the distance.
        // sadly, Point.Absolute() isn't a thing, so we have to do it manually.
        if (distancePoint.X < 0)
            distancePoint.X *= -1;
        if (distancePoint.Y < 0)
            distancePoint.Y *= -1;
        
        // use trig to calculate the effective distance, and then subtract each circle's radius because the insides of the circles don't count.
        return (int)MathF.Sqrt(distancePoint.X * distancePoint.X + distancePoint.Y * distancePoint.Y) - (circ1.Radius + circ2.Radius);
    }
    
    #endregion
}