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
}