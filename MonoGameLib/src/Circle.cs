using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;

namespace TheBackyard.MonoGameLib;

public struct Circle : IShape
{
    private const double PI = Math.PI;

    #region properties

    #region inherited
    public Point Center => Position;
    public readonly float Area => (float)Math.Pow(PI * Radius, 2);
    private Point _position;
    public required Point Position
    {
        get => _position;
        set => _position = value;
    }
    #endregion inherited
    

    public required int Radius {get; set;}

    public int X
    {
        get => _position.X;
        set => _position.X = value;
    }

    public int Y
    {
        get => _position.Y;
        set => _position.Y = value;
    }
    #endregion properties

    #region constructors
    public Circle() {  }

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
    /// <param name="circle"></param>
    /// <returns>Returns the distance BETWEEN each <see cref="Circle"/>. Will be negative if they are overlapping.</returns>
    public static int Distance(Circle circle1, Circle circle2)
    {
        return ShapeUtils.GetMinimumDistance(circle1, circle2);
    }
    #endregion static

    /// <summary>
    /// Get the exact distance <b>between</b> <see cref="this"/> <see cref="Circle"/> and <paramref name="circle"/>.
    /// </summary>
    /// <param name="circle"></param>
    /// <returns>Returns the distance <b>between</b> each <see cref="Circle"/>. Will be negative if they are overlapping.</returns>
    public readonly int Distance(Circle circle)
    {
        return ShapeUtils.GetMinimumDistance(this, circle);
    }
    #endregion distance
}