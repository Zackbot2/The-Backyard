using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheBackyard.MonoGameLib;

/// <summary>
/// 
/// </summary>
public struct Rectangle : IPolygon
{
    #region properties
    #region inherited
    /// <summary>
    /// The amount of pixels that lie within this <see cref="Rectangle"/>.
    /// </summary>
    public readonly float Area => Width * Height;
    /// <summary>
    /// The direct center of this <see cref="Rectangle"/>.
    /// </summary>
    public readonly Point Center => new(X + Width/2, Y + Height/2);
    /// <summary>
    /// The top left position of this <see cref="Rectangle"/>.
    /// </summary>
    public readonly Point Position => new(X, Y);
    #endregion inherited
    /// <summary>
    /// The x coordinate of this <see cref="Rectangle"/>.
    /// </summary>
    public required int X {get;set;}
    /// <summary>
    /// The y coordinate of this <see cref="Rectangle"/>.
    /// </summary>
    public required int Y {get;set;}
    /// <summary>
    /// The y coordinate of this <see cref="Rectangle"/>.
    /// </summary>
    public required int Width {get;set;}
    /// <summary>
    /// The height of this <see cref="Rectangle"/>.
    /// </summary>
    public required int Height {get;set;}

    /// <summary>
    /// The top of this <see cref="Rectangle"/>. Does not account for <see cref="Rotation"/>.
    /// </summary>
    public readonly int Top => Y;
    /// <summary>
    /// The bottom of this <see cref="Rectangle"/>. Does not account for <see cref="Rotation"/>.
    /// </summary>
    public readonly int Bottom => Y + Height;
    /// <summary>
    /// The left of this <see cref="Rectangle"/>. Does not account for <see cref="Rotation"/>.
    /// </summary>
    public readonly int Left => X;
    /// <summary>
    /// The right of this <see cref="Rectangle"/>. Does not account for <see cref="Rotation"/>.
    /// </summary>
    public readonly int Right => X + Width;

    /// <summary>
    /// The rotation of this <see cref="Rectangle"/>, in radians.
    /// </summary>
    public double Rotation {get; set;} = 0;

    /// <summary>
    /// The origin of rotation, in local space.
    /// </summary>
    public Vector2 Origin {get; set;} = Vector2.Zero;
    #endregion

    #region constructors

    /// <summary>
    /// Default <see cref="Rectangle"/> constructor.
    /// </summary>
    public Rectangle() {  }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    [SetsRequiredMembers]
    public Rectangle(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Instantiate a new <see cref="Rectangle"/>, using a <see cref="Vector2"/> for <paramref name="position"/>.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    [SetsRequiredMembers]
    public Rectangle(Vector2 position, int width, int height)
    {
        X = (int)position.X;
        Y = (int)position.Y;
        Width = width;
        Height = height;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rect"></param>
    [SetsRequiredMembers]
    public Rectangle(Microsoft.Xna.Framework.Rectangle rect)
    {
        this = FromXna(rect);
    }
    #endregion constructors

    #region methods
    #region translation
    /// <summary>
    /// Center this <see cref="Rectangle"/>
    /// </summary>
    /// <param name="point"></param>
    public void CenterOn(Point point)
    {
        X = (int)(point.X);
        Y = (int)(point.Y);
    }
    #endregion translation
    #region interaction
    /// <summary>
    /// Does <paramref name="point"/> lie within this <see cref="Rectangle"/>?
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public readonly bool ContainsPoint(Point point)
    {
        // immediately return false if the point is outside of collidable range
        if ((point.X - X + point.Y - Y) / 2 > Width + Height)
            return false;

        if (Rotation != 0 && Rotation != Math.PI)
        {
            Vector2 xAxis = new((float)Math.Cos(-Rotation), (float)Math.Sin(-Rotation));
            Vector2 yAxis = new(-xAxis.Y, xAxis.X);

            // offset from origin, which lies at the position + origin
            point -= (Position + Origin.ToPoint());

            Point translatedPoint;

            translatedPoint.X = (int)(point.X * xAxis.X + point.Y * yAxis.X);
            translatedPoint.Y = (int)(point.X * xAxis.Y + point.Y * yAxis.Y);

            translatedPoint += (Position + Origin.ToPoint());

            point = translatedPoint;
        }
        return
            point.X >= Left && point.X <= Right 
            && point.Y >= Top && point.Y <= Bottom;
    }

    /// <summary>
    /// Does this <see cref="Rectangle"/> overlap with <paramref name="other"/> <see cref="Rectangle"/>?
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public readonly bool Intersects(Rectangle other)
    {
        return ShapeUtils.Intersects(this, other);
    }

    /// <summary>
    /// Does this <see cref="Rectangle"/> overlap with <paramref name="other"/> <see cref="Microsoft.Xna.Framework.Rectangle"/>?
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public readonly bool Intersects(Microsoft.Xna.Framework.Rectangle other)
    {
        return ShapeUtils.Intersects(this, FromXna(other));
    }
    #endregion interaction
    #region xna
    /// <summary>
    /// Convert <paramref name="rect"/> into a <see cref="Microsoft.Xna.Framework.Rectangle"/>.
    /// </summary>
    /// <param name="rect"></param>
    /// <returns></returns>
    public static Microsoft.Xna.Framework.Rectangle ToXna(Rectangle rect)
    {
        return new(rect.X, rect.Y, rect.Width, rect.Height);
    }

    /// <summary>
    /// Convert this <see cref="Rectangle"/> into a <see cref="Microsoft.Xna.Framework.Rectangle"/>.
    /// </summary>
    /// <returns></returns>
    public readonly Microsoft.Xna.Framework.Rectangle ToXna()
    {
        return ToXna(this);
    }

    /// <summary>
    /// Convert <paramref name="rect"/> <see cref="Microsoft.Xna.Framework.Rectangle"/> into a <see cref="Rectangle"/>.
    /// </summary>
    /// <param name="rect"></param>
    /// <returns></returns>
    public static Rectangle FromXna(Microsoft.Xna.Framework.Rectangle rect)
    {
        return new(rect.X, rect.Y, rect.Width, rect.Height);
    }
    #endregion xna
    
    /// <summary>
    /// Get this <see cref="Rectangle"/> as <see cref="Vector2"/>s, going clockwise from the top left position.
    /// Does account for <see cref="Rotation"/>.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public readonly List<Vector2> ToVectors()
    {
        return [
            Vector2.Rotate(new(Right, 0), (float)Rotation),
            Vector2.Rotate(new(0, Bottom), (float)Rotation),
            //Vector2.Rotate(new(-Right, 0), (float)Rotation),
            //Vector2.Rotate(new(0, -Bottom), (float)Rotation)
        ];
    }

    /// <summary>
    /// Get this <see cref="Rectangle"/> as <see cref="Point"/>s, going clockwise from the top left position.
    /// Does account for <see cref="Rotation"/>.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public readonly List<Point> ToPoints()
    {
        return [
            Vector2.RotateAround(new Point(Left, Top).ToVector2(), Origin + Position.ToVector2(), (float)Rotation).ToPoint(),
            Vector2.RotateAround(new Point(Right, Top).ToVector2(), Origin + Position.ToVector2(), (float)Rotation).ToPoint(),
            Vector2.RotateAround(new Point(Right, Bottom).ToVector2(), Origin + Position.ToVector2(), (float)Rotation).ToPoint(),
            Vector2.RotateAround(new Point(Left, Bottom).ToVector2(), Origin + Position.ToVector2(), (float)Rotation).ToPoint(),
        ];
    }
    
    #endregion methods

    #region operators
    #endregion operators
}