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
        return ShapeUtils.GetMinimumDistance(circle1, circle2);
    }
    #endregion static

    /// <summary>
    /// Get the exact distance <b>between</b> this <see cref="Circle"/> and <paramref name="circle"/>.
    /// </summary>
    /// <param name="circle"></param>
    /// <returns>Returns the distance <b>between</b> each <see cref="Circle"/>. Will be negative if they are overlapping.</returns>
    public readonly int Distance(Circle circle)
    {
        return ShapeUtils.GetMinimumDistance(this, circle);
    }
    #endregion distance
}