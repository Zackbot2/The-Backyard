using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;

namespace TheBackyard.MonoGameLib;

/// <summary>
/// Represents a rectangle.
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
    public Point Position
    {
        readonly get => new(X, Y);
        set
        {
            X = value.X;
            Y = value.Y;
        }
    }
    
    /// <summary>
    /// The rotation of this <see cref="Rectangle"/>, in radians.
    /// </summary>
    public double Rotation {get; set;} = 0;

    /// <summary>
    /// The <see cref="Origin"/> relative to the dimensions of this <see cref="Rectangle"/>.
    /// Takes priority over <see cref="Origin"/> if it has a value.
    /// </summary>
    public Vector2? RelativeOrigin {get; set;} = null;

    /// <summary>
    /// The origin of rotation, in local space.
    /// If <see cref="RelativeOrigin"/> has a value, <see cref="Origin"/> will be calculated based on that.
    /// Otherwise, uses its stored value.
    /// </summary>
    public Vector2 Origin 
    { 
        readonly get => RelativeOrigin != null ? new(RelativeOrigin.Value.X * Width, RelativeOrigin.Value.Y * Height) : field;
        set;
    } = Vector2.Zero;

    /// <summary>
    /// The distance from <see cref="Position"/> that fully encapsulates the dimensions of this <see cref="Rectangle"/>
    /// </summary>
    public readonly int CollidableRange => Width + Height;
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

    #region sides
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
    #endregion sides

    #endregion properties

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

    /// <summary>
    /// Set the <see cref="Origin"/> to <paramref name="origin"/>.
    /// Pass a null <paramref name="origin"/> to reset <see cref="Origin"/> to default.
    /// </summary>
    /// <param name="origin"></param>
    public void SetOrigin(Vector2? origin)
    {
        origin ??= Vector2.Zero;

        Origin = (Vector2)origin;
    }

    #region translation
    /// <summary>
    /// Center this <see cref="Rectangle"/> on <paramref name="point"/>.
    /// </summary>
    /// <param name="point"></param>
    public void CenterOn(Point point)
    {
        Position = new Point(point.X - Width/2, point.Y - Height/2);
    }

    /// <summary>
    /// Position this <see cref="Rectangle"/> such that the <see cref="Origin"/> lies on <paramref name="point"/>.
    /// </summary>
    /// <param name="point"></param>
    public void OriginOn(Point point)
    {
        Position = point - Origin.ToPoint();
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
            point -= Position + Origin.ToPoint();

            Point translatedPoint;

            translatedPoint.X = (int)(point.X * xAxis.X + point.Y * yAxis.X);
            translatedPoint.Y = (int)(point.X * xAxis.Y + point.Y * yAxis.Y);

            translatedPoint += Position + Origin.ToPoint();

            point = translatedPoint;
        }
        return
            point.X >= Left && point.X <= Right 
            && point.Y >= Top && point.Y <= Bottom;
    }

    #endregion interaction
    #region xna
    /// <summary>
    /// Convert <paramref name="rect"/> into a <see cref="Microsoft.Xna.Framework.Rectangle"/>.
    /// </summary>
    /// <param name="rect"></param>
    /// <returns></returns>
    public static Microsoft.Xna.Framework.Rectangle ToXna(Rectangle rect) => new(rect.X, rect.Y, rect.Width, rect.Height);

    /// <summary>
    /// Convert this <see cref="Rectangle"/> into a <see cref="Microsoft.Xna.Framework.Rectangle"/>.
    /// </summary>
    /// <returns></returns>
    public readonly Microsoft.Xna.Framework.Rectangle ToXna() => ToXna(this);

    /// <summary>
    /// Convert <paramref name="rect"/> <see cref="Microsoft.Xna.Framework.Rectangle"/> into a <see cref="Rectangle"/>.
    /// </summary>
    /// <param name="rect"></param>
    /// <returns></returns>
    public static Rectangle FromXna(Microsoft.Xna.Framework.Rectangle rect) => new(rect.X, rect.Y, rect.Width, rect.Height);
    #endregion xna

    #region inherited
    /// <summary>
    /// Get this <see cref="Rectangle"/> as <see cref="Vector2"/>s, going clockwise from the top left position.
    /// Does account for <see cref="Rotation"/>.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public readonly List<Vector2> ToVectors() => [
            Vector2.Rotate(new(Right, 0), (float)Rotation),
            Vector2.Rotate(new(0, Bottom), (float)Rotation),
            Vector2.Rotate(new(-Right, 0), (float)Rotation),
            Vector2.Rotate(new(0, -Bottom), (float)Rotation)
        ];

    /// <summary>
    /// Get this <see cref="Rectangle"/> as <see cref="Point"/>s, going clockwise from the top left position.
    /// Does account for <see cref="Rotation"/>.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public readonly List<Point> ToPoints() => [
            Vector2.RotateAround(new Point(Left, Top).ToVector2(), Origin + Position.ToVector2(), (float)Rotation).ToPoint(),
            Vector2.RotateAround(new Point(Right, Top).ToVector2(), Origin + Position.ToVector2(), (float)Rotation).ToPoint(),
            Vector2.RotateAround(new Point(Right, Bottom).ToVector2(), Origin + Position.ToVector2(), (float)Rotation).ToPoint(),
            Vector2.RotateAround(new Point(Left, Bottom).ToVector2(), Origin + Position.ToVector2(), (float)Rotation).ToPoint(),
        ];

    #endregion inherited

    /// <summary>
    /// Get the distance between this <see cref="Rectangle"/> and <paramref name="rect"/>.
    /// </summary>
    /// <param name="rect"></param>
    /// <returns></returns>
    public readonly float GetDistanceFrom(Rectangle rect) => ((IPolygon)this).GetDistanceFrom(rect);

    /// <summary>
    /// Determine if this <see cref="Rectangle"/> intersects <paramref name="rect"/>.
    /// </summary>
    /// <param name="rect"></param>
    /// <returns></returns>
    public readonly bool Intersects(Rectangle rect) => ((IPolygon)this).Intersects(rect);
    #endregion methods

    #region operators
    #endregion operators
}