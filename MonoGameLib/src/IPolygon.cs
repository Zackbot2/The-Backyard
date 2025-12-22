using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;

namespace TheBackyard.MonoGameLib;

/// <summary>
/// Represents a polygon, enabling collision with other polygons with all sharing the same logic.
/// </summary>
public interface IPolygon : IShape
{
    /// <summary>
    /// The origin of <see cref="Rotation"/>, relative to <see cref="IShape.Position"/>.
    /// </summary>
    Vector2 Origin {get; set;}

    /// <summary>
    /// The rotation of this <see cref="IPolygon"/>.
    /// </summary>
    double Rotation {get; set;}

    /// <summary>
    /// The distance from <see cref="IShape.Position"/> that fully encapsulates the dimensions of this <see cref="IPolygon"/>.
    /// </summary>
    int CollidableRange {get;}

    /// <summary>
    /// This <see cref="IPolygon"/>, as a list of <see cref="Vector2"/>.
    /// </summary>
    /// <returns>
    /// Returns a list of type <see cref="Vector2"/>. May be clockwise or counter-clockwise, but <see cref="GetNormals"/> must account for this.
    /// </returns>
    List<Vector2> ToVectors();

    /// <summary>
    /// This <see cref="IPolygon"/>, as a list of <see cref="Point"/>s.
    /// Each point represents one vertex of the polygon.
    /// Should account for the <see cref="IShape.Position"/>.
    /// </summary>
    /// <returns></returns>
    List<Point> ToPoints();

    /// <summary>
    /// Get the normal of each <see cref="Vector2"/> in <see cref="ToVectors"/>.
    /// </summary>
    /// <returns>
    /// Returns a list of type <see cref="Vector2"/>
    /// </returns>
    List<Vector2> GetNormals()
    {
        List<Vector2> vectors = ToVectors();
        List<Vector2> normals = [];
        // IMPORTANT: we need to ensure we're actually going clockwise or counter-clockwise around the polygon. we currently do NOT do this.
        // for now, let's assume clockwise.

        foreach (Vector2 vector in vectors)
        {
            // left normal
            normals.Add(new(-vector.Y, vector.X));
        }
        return normals;
    }
    
    /// <summary>
    /// Get the distance between this <see cref="IPolygon"/> and <paramref name="poly2"/>.
    /// </summary>
    /// <param name="poly2"></param>
    /// <returns></returns>
    float GetDistanceFrom(IPolygon poly2)
    {
        #region calculate normals
        // find the normals to calculate for each box
        
        List<Vector2> normals = GetNormals();
        normals.AddRange(poly2.GetNormals());

        // eliminate any normals that are exactly equal or opposite

        List<Vector2> trimmedNormals = [];

        // using double foreach instead of linq for performance.
        // i agree this looks pretty disgusting, but it allows for 1 less sequence than something like .Where + .Contains.

        foreach (Vector2 normal in normals)
        {
            bool addThisNormal = true;
            foreach (Vector2 trimmedNormal in trimmedNormals)
            {
                if (trimmedNormal == normal || trimmedNormal == normal * -1)
                {
                    addThisNormal = false;
                    break;
                }
            }
            if (addThisNormal)
                trimmedNormals.Add(normal);
        }

        normals = trimmedNormals;
        #endregion calculate normals

        List<Vector2> poly1Points = ToPoints().ConvertAll(p => p.ToVector2());
        List<Vector2> poly2Points = poly2.ToPoints().ConvertAll(p => p.ToVector2());

        float? gap = null;

        // 2. For each one of these normals:
        foreach (Vector2 normal in normals)
        {
            normal.Normalize();
            
            float? newGap = null;

            // find the min and max dot product of each point and the current normal

            float poly1Min = Vector2.Dot(poly1Points[0], normal);
            float poly1Max = poly1Min;
            foreach(Vector2 point in poly1Points)
            {
                float pointDot = Vector2.Dot(point, normal);
                if (poly1Min > pointDot)
                {
                    poly1Min = pointDot;
                }
                if (poly1Max < pointDot)
                {
                    poly1Max = pointDot;
                }
            }

            // do the same with poly2
            float poly2Min = Vector2.Dot(poly2Points[0], normal);
            float poly2Max = poly2Min;
            foreach(Vector2 point in poly2Points)
            {
                float pointDot = Vector2.Dot(point, normal);
                if (poly2Min > pointDot)
                {
                    poly2Min = pointDot;
                }
                if (poly2Max < pointDot)
                {
                    poly2Max = pointDot;
                }
            }

            // determine if there's a gap

            newGap = MathF.Max(poly2Min - poly1Max, poly1Min - poly2Max);
                
            // remember this gap. the distance between the polygons is the maximum gap found.
            if (newGap != null && (gap == null || newGap > gap))
                gap = newGap;

        }
        //  3. If you get to this point and there's still no gap found, the boxes are overlapping.
        return gap??= 0;
    }

    /// <summary>
    /// Get the gaps between this <see cref="IPolygon"/> and <paramref name="poly2"/>.
    /// </summary>
    /// <param name="poly2"></param>
    /// <returns></returns>
    List<(Vector2 normal, float gap)> GetGaps(IPolygon poly2)
    {
        #region calculate normals
        // find the normals to calculate for each box
        
        List<Vector2> normals = GetNormals();
        normals.AddRange(poly2.GetNormals());

        // eliminate any normals that are exactly equal or opposite

        List<Vector2> trimmedNormals = [];

        // using double foreach instead of linq for performance.
        // i agree this looks pretty disgusting, but it allows for 1 less sequence than something like .Where + .Contains.

        foreach (Vector2 normal in normals)
        {
            bool addThisNormal = true;
            foreach (Vector2 trimmedNormal in trimmedNormals)
            {
                if (trimmedNormal == normal || trimmedNormal == normal * -1)
                {
                    addThisNormal = false;
                    break;
                }
            }
            if (addThisNormal)
                trimmedNormals.Add(normal);
        }

        normals = trimmedNormals;
        #endregion calculate normals

        List<Vector2> poly1Points = ToPoints().ConvertAll(p => p.ToVector2());
        List<Vector2> poly2Points = poly2.ToPoints().ConvertAll(p => p.ToVector2());

        List<(Vector2, float)> normalGaps = [];

        // 2. For each one of these normals:
        foreach (Vector2 normal in normals)
        {
            normal.Normalize();

            // find the min and max dot product of each point and the current normal

            float poly1Min = Vector2.Dot(poly1Points[0], normal);
            float poly1Max = poly1Min;
            foreach(Vector2 point in poly1Points)
            {
                float pointDot = Vector2.Dot(point, normal);
                if (poly1Min > pointDot)
                {
                    poly1Min = pointDot;
                }
                if (poly1Max < pointDot)
                {
                    poly1Max = pointDot;
                }
            }

            // do the same with poly2
            float poly2Min = Vector2.Dot(poly2Points[0], normal);
            float poly2Max = poly2Min;
            foreach(Vector2 point in poly2Points)
            {
                float pointDot = Vector2.Dot(point, normal);
                if (poly2Min > pointDot)
                {
                    poly2Min = pointDot;
                }
                if (poly2Max < pointDot)
                {
                    poly2Max = pointDot;
                }
            }

            // remember the gap
            normalGaps.Add((normal, MathF.Max(poly2Min - poly1Max, poly1Min - poly2Max)));
        }
        //  3. If you get to this point and there's still no gap found, the boxes are overlapping.
        return normalGaps;
    }

    /// <summary>
    /// Is this <see cref="IPolygon"/> overlapping with <paramref name="poly2"/>?
    /// </summary>
    /// <param name="poly2"></param>
    /// <returns></returns>
    /// <remarks>
    /// This code is a slightly tweaked version of <see cref="GetDistanceFrom"/>.
    /// It returns false as soon as any gap is found, making it much lighter.
    /// </remarks>
    bool Intersects(IPolygon poly2)
    {
        // exit early if the polygons are too far apart to collide
        if (!IsInCollidableRange(poly2))
            return false;

        #region calculate normals
        // find the normals to calculate for each box
        
        List<Vector2> normals = GetNormals();
        normals.AddRange(poly2.GetNormals());

        // eliminate any normals that are exactly equal or opposite

        List<Vector2> trimmedNormals = [];

        // using double foreach instead of linq for performance.
        // i agree this looks pretty disgusting, but it allows for 1 less sequence than something like .Where + .Contains.

        foreach (Vector2 normal in normals)
        {
            bool addThisNormal = true;
            foreach (Vector2 trimmedNormal in trimmedNormals)
            {
                if (trimmedNormal == normal || trimmedNormal == normal * -1)
                {
                    addThisNormal = false;
                    break;
                }
            }
            if (addThisNormal)
                trimmedNormals.Add(normal);
        }

        normals = trimmedNormals;
        #endregion calculate normals

        List<Vector2> poly1Points = ToPoints().ConvertAll(p => p.ToVector2());
        List<Vector2> poly2Points = poly2.ToPoints().ConvertAll(p => p.ToVector2());

        foreach (Vector2 normal in normals)
        {
            normal.Normalize();

            // find the min and max dot product of each point and the current normal

            float poly1Min = Vector2.Dot(poly1Points[0], normal);
            float poly1Max = poly1Min;
            foreach(Vector2 point in poly1Points)
            {
                float pointDot = Vector2.Dot(point, normal);
                if (poly1Min > pointDot)
                {
                    poly1Min = pointDot;
                }
                if (poly1Max < pointDot)
                {
                    poly1Max = pointDot;
                }
            }

            // do the same with poly2
            float poly2Min = Vector2.Dot(poly2Points[0], normal);
            float poly2Max = poly2Min;
            foreach(Vector2 point in poly2Points)
            {
                float pointDot = Vector2.Dot(point, normal);
                if (poly2Min > pointDot)
                {
                    poly2Min = pointDot;
                }
                if (poly2Max < pointDot)
                {
                    poly2Max = pointDot;
                }
            }

            // determine if there's a gap
            if (poly2Min > poly1Max || poly1Min > poly2Max)
                // there is a gap, these boxes are not colliding. return false immediately to save performance.
                return false;
        }
        //  if you get to this point and there's still no gap found, the boxes are overlapping.
        return true;
    }

    /// <summary>
    /// Determine if this <see cref="IPolygon"/> is within collidable range of <paramref name="poly"/>.
    /// </summary>
    /// <param name="poly"></param>
    /// <returns></returns>
    bool IsInCollidableRange(IPolygon poly)
    {
        Point realPosition = Position.RotateAround(Position.ToVector2() + Origin, (float)Rotation);
        Point polyRealPosition = poly.Position.RotateAround(poly.Position.ToVector2() + poly.Origin, (float)poly.Rotation);
        int differenceX = Math.Abs(realPosition.X - polyRealPosition.X);
        int differenceY = Math.Abs(realPosition.Y - polyRealPosition.Y);

        return differenceX + differenceY < CollidableRange + poly.CollidableRange;
    }
}