using System.Diagnostics;
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

    /// <summary>
    /// The rotation of this <see cref="Rectangle"/>, in radians.
    /// </summary>
    public double Rotation {get; set;} = 0;
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
    public void CenterOn(Point point)
    {
        X = point.X - Width;
        Y = point.Y - Height;
    }
    #endregion translation
    #region interaction
    public readonly bool ContainsPoint(Point point)
    {
        // immediately return false if the point is outside of collidable range
        if ((point.X - X + point.Y - Y) / 2 > Width + Height)
            return false;

        if (Rotation != 0 && Rotation != Math.PI)
        {
            Vector2 xAxis = new((float)Math.Cos(-Rotation), (float)Math.Sin(-Rotation));
            Vector2 yAxis = new(-xAxis.Y, xAxis.X);

            // offset from origin
            point -= Center;

            Point translatedPoint;

            translatedPoint.X = (int)(point.X * xAxis.X + point.Y * yAxis.X + Center.X);
            translatedPoint.Y = (int)(point.X * xAxis.Y + point.Y * yAxis.Y + Center.Y);

            point = translatedPoint;
        }
        return
            point.X >= Left && point.X <= Right 
            && point.Y >= Top && point.Y <= Bottom;
    }

    public readonly bool Intersects(Rectangle other)
    {
        return ShapeUtils.Intersects(this, other);
    }

    public readonly bool Intersects(Microsoft.Xna.Framework.Rectangle other)
    {
        return ShapeUtils.Intersects(this, FromXna(other));
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