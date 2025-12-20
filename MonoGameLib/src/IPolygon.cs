using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace TheBackyard.MonoGameLib;

/// <summary>
/// Represents a polygon, enabling collision with other polygons with all sharing the same logic.
/// </summary>
public interface IPolygon : IShape
{
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
        List<Vector2> normals = [];
        List<Vector2> vectors = ToVectors();
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
    /// <exception cref="NotImplementedException"></exception>
    float GetDistanceFrom(IPolygon poly2)
    {
        Debug.WriteLine("Getting distance between polygons.");
        // 1. Find the normals to calculate for each box
        List<Vector2> normals = GetNormals();
        normals.AddRange(poly2.GetNormals());

        Debug.WriteLine($"Found {normals.Count} normals.");
        // - Eliminate any normals that are exactly equal or opposite
        List<Vector2> poly1Points = ToPoints().ConvertAll(p => p.ToVector2());
        List<Vector2> poly2Points = poly2.ToPoints().ConvertAll(p => p.ToVector2());

        float? gap = null;

        // 2. For each one of these normals:
        foreach (Vector2 normal in normals)
        {
            normal.Normalize();
            // 1. find `box1.min`, `box1.max`, `box2.min` and `box2.max` using the dot product of each point and the normal
            float? newGap = null;

            // get the min and max values for poly1. we don't need to remember the point location or anything, just the dot value.

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

            Debug.WriteLine($"Poly1: ({poly1Min}, {poly1Max}), Poly2: ({poly2Min}, {poly2Max})");
            
            // 2. Determine if there's a gap (`box1.max` < `box2.min`) or (`box2.max` > `box1.min`)

            newGap = MathF.Max(poly2Min - poly1Max, poly1Min - poly2Max);
                
            // 3. If there is a gap, these boxes are not colliding and you can immediately exit.
                // - If you want to find the exact distance, you would need to continue and return the smallest value achieved at this point in the loop once finished.
            if (newGap != null && (gap == null || newGap > gap))
                gap = newGap;

            Debug.WriteLine($"THIS NORMAL'S GAP: {newGap}");
        }
        //  3. If you get to this point and there's still no gap found, the boxes are overlapping.
        Debug.WriteLine(gap);
        return gap??= 0;
    }

    /// <summary>
    /// Is this <see cref="IPolygon"/> overlapping with <paramref name="poly2"/>?
    /// </summary>
    /// <param name="poly2"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    bool Intersects(IPolygon poly2)
    {
        throw new NotImplementedException();
    }
}