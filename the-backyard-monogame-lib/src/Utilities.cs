using Microsoft.Xna.Framework;

namespace TheBackyard.MonogameLib;

public static class Utilities
{
    /// <summary>
    /// Return the delta time from a given <paramref name="gameTime"/>.
    /// </summary>
    /// <param name="gameTime">the <see cref="GameTime"/></param>
    /// <returns>Returns the number of seconds since the last call to <see cref="Game.Update(GameTime)"/></returns>
    public static float DeltaTimeFromGameTime(GameTime gameTime)
    {
        return (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}