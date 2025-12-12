using Microsoft.Xna.Framework;

namespace TheBackyard.MonoGameLib;

internal static class ShapeUtils
{
    #region intersects
    public static bool Intersects(Rectangle rect1, Rectangle rect2)
    {
        return 
            rect1.Left < rect2.Right && rect1.Right > rect2.Left 
            && rect1.Top < rect2.Bottom && rect1.Bottom > rect2.Top;
    }
    #endregion intersects
}