using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;

namespace TheBackyard.MonoGameLib;

public struct Circle : IShape
{
    private const double PI = Math.PI;

    #region properties

    #region inherited
    public readonly Point Center => Position;
    public readonly float Area => (float)Math.Pow(PI * Radius, 2);
    #endregion inherited
    public required Point Position;

    public required int Radius {get; set;}

    public int X
    {
        get => Position.X;
        set => Position.X = value;
    }

    public int Y
    {
        get => Position.Y;
        set => Position.Y = value;
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