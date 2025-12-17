using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheBackyard.MonoGameLib;

/// <summary>
/// Extension methods for pre-existing datatypes such as <see cref="Microsoft.Xna.Framework.Vector2"/> and <see cref="Microsoft.Xna.Framework.Point"/> allowing them to be drawn using a <see cref="SpriteBatch"/>.
/// </summary>
public static class DrawExtensions
{
    /// <summary>
    /// Draw <paramref name="vect"/>.
    /// </summary>
    /// <param name="vect"></param>
    /// <param name="spriteBatch"></param>
    public static void Draw(this Vector2 vect, SpriteBatch spriteBatch)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Draw <paramref name="point"/>.
    /// </summary>
    /// <param name="point"></param>
    /// <param name="spriteBatch"></param>
    public static void Draw(this Point point, SpriteBatch spriteBatch)
    {
        throw new NotImplementedException();
    }
}