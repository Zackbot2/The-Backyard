using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;

namespace TheBackyard.MonoGameLib;

public class GameObject
{
    #region properties
    public required Vector2 Position {get; set;}
    
    /// <summary>
    /// Defaults to <see cref="Vector2.Zero"/>.
    /// </summary>
    public Vector2 Velocity {get; set;} = Vector2.Zero;

    #region expression body definitions
    /// <summary>
    /// The magnitude of <see cref="Velocity"/>.
    /// </summary>
    public float Speed => Velocity.Length();

    /// <summary>
    /// The direction of <see cref="Velocity"/>.
    /// </summary>
    public Vector2 Direction
    {
        get => Vector2.Normalize(Velocity);
        set
        {
            // when setting the direction, the velocity should update to match the direction, but keep the same magnitude.
            
            // if speed or the input value are 0, Vector2.Normalize can return NaNs. prevent this by checking first.
            if (Speed == 0 || value == Vector2.Zero)
                Velocity = Vector2.Zero;
            
            else
                Velocity = Vector2.Normalize(value) * Speed;
        }
    }
    #endregion expression body definitions
    #endregion properties

    #region constructors
    /// <summary>
    /// Create a new <see cref="GameObject"/>.
    /// </summary>
    public GameObject() {  }

    /// <summary>
    /// Create a new <see cref="GameObject"/> at a <paramref name="position"/> with an optional <paramref name="velocity"/>.
    /// </summary>
    /// <param name="position">The <see cref="Position"/> at which this <see cref="GameObject"/> will be created.</param>
    /// <param name="velocity">The <see cref="Velocity"/> this <see cref="GameObject"/> will have upon creation (optional).</param>
    [SetsRequiredMembers]
    public GameObject (Vector2 position, Vector2? velocity = null)
    {
        Position = position;

        if (velocity.HasValue)
            Velocity = (Vector2)velocity;
    }
    #endregion constructors

    /// <summary>
    /// Update the position of this <see cref="GameObject"/>. Does not account for delta time.
    /// </summary>
    public void UpdatePosition()
    {
        Position += Velocity;
    }

    /// <summary>
    /// Update the position of this <see cref="GameObject"/>, with respect to <see cref="gameTime"/>.
    /// </summary>
    /// <param name="gameTime"></param>
    public void UpdatePosition(GameTime gameTime)
    {
        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}