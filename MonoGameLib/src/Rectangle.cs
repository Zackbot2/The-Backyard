using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;

namespace TheBackyard.MonoGameLib;

public struct Rectangle : IShape
{
    #region properties
    #region inherited
    public readonly float Area => Width * Height;
    public readonly Point Center => new(X + Width/2, Y + Height/2);
    #endregion inherited
    public required int X {get;set;}
    public required int Y {get;set;}
    public required int Width {get;set;}
    public required int Height {get;set;}

    public readonly int Top => Y;
    public readonly int Bottom => Y + Height;
    public readonly int Left => X;
    public readonly int Right => X + Width;
    #endregion

    #region constructors

    public Rectangle() {  }

    [SetsRequiredMembers]
    public Rectangle(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    [SetsRequiredMembers]
    public Rectangle(Microsoft.Xna.Framework.Rectangle rect)
    {
        this = FromXna(rect);
    }
    #endregion constructors

    #region methods
    #region translation

    #endregion translation
    #region interaction
    public readonly bool Contains(Point point)
    {
        return point.X > Left && point.X < Right && point.Y > Top && point.Y < Bottom;
    }

    public readonly bool Overlaps(Rectangle other)
    {
        bool overlap = false;

        if (Left < other.Right && Right > other.Left
            && Top < other.Bottom && Bottom > other.Top)
            overlap = true;

        return overlap;
    }

    public readonly bool Overlaps(Microsoft.Xna.Framework.Rectangle other)
    {
        return Overlaps(FromXna(other));
    }
    #endregion interaction
    #region xna
    public static Microsoft.Xna.Framework.Rectangle ToXna(Rectangle rect)
    {
        return new(rect.X, rect.Y, rect.Width, rect.Height);
    }

    public readonly Microsoft.Xna.Framework.Rectangle ToXna()
    {
        return ToXna(this);
    }
    
    public static Rectangle FromXna(Microsoft.Xna.Framework.Rectangle rect)
    {
        return new(rect.X, rect.Y, rect.Width, rect.Height);
    }
    #endregion xna
    #endregion methods

    #region operators
    #endregion operators
}