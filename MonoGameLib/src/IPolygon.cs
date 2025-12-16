using Microsoft.Xna.Framework;

namespace TheBackyard.MonoGameLib;

/// <summary>
/// Represents a polygon, enabling collision with other polygons with all sharing the same logic.
/// </summary>
public interface IPolygon : IShape
{
    /// <summary>
    /// This <see cref="IPolygon"/>, as a List of <see cref="Vector2"/>.
    /// </summary>
    /// <returns>
    /// Returns a list of type <see cref="Vector2"/>. May be clockwise or counter-clockwise, but <see cref="GetNormals"/> must account for this.
    /// </returns>
    List<Vector2> ToVectors();

    /// <summary>
    /// Get the normal of each <see cref="Vector2"/> in <see cref="ToVectors"/>.
    /// </summary>
    /// <returns>
    /// Returns a list of type <see cref="Vector2"/>
    /// </returns>
    List<Vector2> GetNormals();
}