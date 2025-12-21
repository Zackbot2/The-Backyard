using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;

namespace TheBackyard.MonoGameLib;

/// <summary>
/// A <see cref="IShape"/> representing a circle.
/// </summary>
public struct Circle : IShape
{
    private const double PI = Math.PI;

    #region properties

    #region inherited
    /// <summary>
    /// The center point of this <see cref="Circle"/>.
    /// </summary>
    public Point Center => Position;
    /// <summary>
    /// The number of pixels that lie within this <see cref="Circle"/>.
    /// </summary>
    public readonly float Area => (float)Math.Pow(PI * Radius, 2);
    private Point _position;
    /// <summary>
    /// The position of this <see cref="Circle"/>.
    /// </summary>
    public required Point Position
    {
        get => _position;
        set => _position = value;
    }
    #endregion inherited
    
    /// <summary>
    /// The radius of this <see cref="Circle"/>, in pixels.
    /// </summary>
    public required int Radius {get; set;}

    /// <summary>
    /// The X <see cref="Position"/> of this <see cref="Circle"/>.
    /// </summary>
    public int X
    {
        get => _position.X;
        set => _position.X = value;
    }
    /// <summary>
    /// The Y <see cref="Position"/> of this <see cref="Circle"/>.
    /// </summary>

    public int Y
    {
        get => _position.Y;
        set => _position.Y = value;
    }
    #endregion properties

    #region constructors
    /// <summary>
    /// Default <see cref="Circle"/> constructor. No funny business.
    /// </summary>
    public Circle() {  }

    /// <summary>
    /// Create a <see cref="Circle"/> with a given <paramref name="position"/> and <paramref name="radius"/>.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="radius"></param>
    [SetsRequiredMembers]
    public Circle(Point position, int radius)
    {
        Position = position;
        Radius = radius;
    }
    #endregion constructors

    #region distance
    #region static
    /// <summary>
    /// Get the exact distance between two <see cref="Circle"/>s.
    /// </summary>
    /// <param name="circle1"></param>
    /// <param name="circle2"></param>
    /// <returns>Returns the distance BETWEEN each <see cref="Circle"/>. Will be negative if they are overlapping.</returns>
    public static int Distance(Circle circle1, Circle circle2)
    {
        Point distancePoint = circle1.Position - circle2.Position;

        // we only want the positive version of the distance.
        // sadly, Point.Absolute() isn't a thing, so we have to do it manually.
        if (distancePoint.X < 0)
            distancePoint.X *= -1;
        if (distancePoint.Y < 0)
            distancePoint.Y *= -1;
        
        // use trig to calculate the effective distance, and then subtract each circle's radius because the insides of the circles don't count.
        return (int)MathF.Sqrt(distancePoint.X * distancePoint.X + distancePoint.Y * distancePoint.Y) - (circle1.Radius + circle2.Radius);
    }
    #endregion static

    /// <summary>
    /// Get the exact distance <b>between</b> this <see cref="Circle"/> and <paramref name="circle"/>.
    /// </summary>
    /// <param name="circle"></param>
    /// <returns>Returns the distance <b>between</b> each <see cref="Circle"/>. Will be negative if they are overlapping.</returns>
    public readonly int Distance(Circle circle)
    {
        return Distance(this, circle);
    }
    #endregion distance
}