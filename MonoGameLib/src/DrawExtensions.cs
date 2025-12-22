using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheBackyard.MonoGameLib;

/// <summary>
/// Extensions of pre-existing classes/structs allowing them to be drawn using a <see cref="SpriteBatch"/>.
/// </summary>
public static class DrawExtensions
{
    #region Vector2
    /// <summary>
    /// Draw <paramref name="vect"/>, using <paramref name="graphicsDevice"/> to create a pixel texture.
    /// </summary>
    /// <param name="vect">The <see cref="Vector2"/> to draw.</param>
    /// <param name="spriteBatch"></param>
    /// <param name="graphicsDevice"></param>
    /// <param name="color"></param>
    /// <param name="position"></param>
    public static void Draw(this Vector2 vect, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Vector2? position = null, Color? color = null)
    {
        color ??= Color.White;
        position??= Vector2.Zero;
        
        // create a rectangle texture, place it at the tail of the vector, and rotate it to match the vector direction
        Rectangle vectorRect = new((Vector2)position, (int)vect.Length(), 3)
        {
            // get rotation of the vector
            Rotation = MathF.Atan2(vect.Y, vect.X)
        };

        vectorRect.Draw(spriteBatch, graphicsDevice, color);
    }

    /// <summary>
    /// Draw <paramref name="vect"/>, using a scaled <paramref name="pixelTexture"/>.
    /// </summary>
    /// <param name="vect">The <see cref="Vector2"/> to draw.</param>
    /// <param name="spriteBatch"></param>
    /// <param name="pixelTexture"></param>
    /// <param name="color"></param>
    /// <param name="position"></param>
    public static void Draw(this Vector2 vect, SpriteBatch spriteBatch, Texture2D pixelTexture, Vector2? position = null, Color? color = null)
    {
        color ??= Color.White;
        position??= Vector2.Zero;
        
        // create a rectangle texture, place it at the tail of the vector, and rotate it to match the vector direction
        Rectangle vectorRect = new((Vector2)position, (int)vect.Length(), 3)
        {
            // get rotation of the vector
            Rotation = MathF.Atan2(vect.Y, vect.X)
        };

        vectorRect.Draw(spriteBatch, pixelTexture, color);
    }
    #endregion Vector2

    #region Point
    /// <summary>
    /// Draw <paramref name="point"/>, using <paramref name="graphicsDevice"/> to create a pixel texture.
    /// </summary>
    /// <param name="point">The <see cref="Point"/> to draw.</param>
    /// <param name="spriteBatch"></param>
    /// <param name="graphicsDevice"></param>
    /// <param name="color"></param>
    public static void Draw(this Point point, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Color? color = null)
    {
        color ??= Color.White;

        Rectangle rect = new(point.ToVector2(), 3, 3)
        {
            Origin = new(2, 2)
        };
        
        rect.CenterOn(point);
        rect.Draw(spriteBatch, graphicsDevice, color);
    }

    /// <summary>
    /// Draw <paramref name="point"/>, using a scaled <paramref name="pixelTexture"/>.
    /// </summary>
    /// <param name="point">The <see cref="Point"/> to draw.</param>
    /// <param name="spriteBatch"></param>
    /// <param name="pixelTexture"></param>
    /// <param name="color"></param>
    public static void Draw(this Point point, SpriteBatch spriteBatch, Texture2D pixelTexture, Color? color = null)
    {
        color ??= Color.White;

        Rectangle rect = new(point.ToVector2(), 3, 3)
        {
            Origin = new(2, 2)
        };

        rect.CenterOn(point);
        rect.Draw(spriteBatch, pixelTexture, color);
    }
    #endregion Point

    #region Rectangle
    /// <summary>
    /// Draw <paramref name="rect"/>, using <paramref name="graphicsDevice"/> to create a pixel texture.
    /// </summary>
    /// <param name="rect">The <see cref="Rectangle"/> to draw.</param>
    /// <param name="spriteBatch"></param>
    /// <param name="graphicsDevice"></param>
    /// <param name="color"></param>
    public static void Draw(this Rectangle rect, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Color? color = null)
    {
        color ??= Color.White;

        spriteBatch.Draw(
            TextureManager.CreatePixelTexture(graphicsDevice), 
            rect.Position.ToVector2() + rect.Origin,  // position
            rect.ToXna(),
            (Color)color,
            (float)rect.Rotation,   // rotation
            rect.Origin,    // origin
            Vector2.One,    // scale
            SpriteEffects.None, 
            0f  // layer depth
            );
    }

    /// <summary>
    /// Draw <paramref name="rect"/>, using a scaled <paramref name="pixelTexture"/>.
    /// </summary>
    /// <param name="rect">The <see cref="Rectangle"/> to draw.</param>
    /// <param name="spriteBatch"></param>
    /// <param name="pixelTexture"></param>
    /// <param name="color"></param>
    public static void Draw(this Rectangle rect, SpriteBatch spriteBatch, Texture2D pixelTexture, Color? color = null)
    {
        color ??= Color.White;

        spriteBatch.Draw(
            pixelTexture, 
            rect.Position.ToVector2() + rect.Origin,  // position
            rect.ToXna(),
            (Color)color,
            (float)rect.Rotation,   // rotation
            rect.Origin,    // origin
            Vector2.One,    // scale
            SpriteEffects.None, 
            0f  // layer depth
            );
    }
    #endregion Rectangle
}