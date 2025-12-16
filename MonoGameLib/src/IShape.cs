using Microsoft.Xna.Framework;

namespace TheBackyard.MonoGameLib;

/// <summary>
/// Defines a rough outline of a shape.
/// </summary>
public interface IShape
{
    /// <summary>
    /// The amount of pixels that lie within this <see cref="IShape"/>.
    /// </summary>
    float Area {get;}

    /// <summary>
    /// The center position of this <see cref="IShape"/>
    /// </summary>
    Point Center {get;}

    /// <summary>
    /// The position of this <see cref="IShape"/>
    /// </summary>
    Point Position {get;}
}