using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;

namespace TheBackyard.MonoGameLib;

/// <summary>
/// <see cref="GameObject"/>s serve as a general object within a game, with the ability to move and process gravity.
/// </summary>
public class GameObject
{
    #region properties
    private Vector2 _position;
    public required Vector2 Position
    {
        get => _position;
        set
        {
            _position = value;
            _boundingBox = null;
        } 
    }

    /// <summary>
    /// The center of <see cref="BoundingBox"/>
    /// </summary>
    public Point CenterPoint => BoundingBox.Center;

    /// <summary>
    /// The <see cref="Vector2.X"/> component of <see cref="Position"/>.
    /// </summary>
    public float XPosition
    {
        get => _position.X;
        set => _position.X = value;
    }

    /// <summary>
    /// The <see cref="Vector2.Y"/> component of <see cref="Position"/>.
    /// </summary>
    public float YPosition
    {
        get => _position.Y;
        set => _position.Y = value;
    }
    
    #region velocity and direction
    private Vector2 _velocity = Vector2.Zero;

    /// <summary>
    /// Defaults to <see cref="Vector2.Zero"/>.
    /// </summary>
    public Vector2 Velocity
    {
        get => _velocity;
        set => _velocity = value;
    }

    /// <summary>
    /// <see cref="Vector2.X"/> component of <see cref="Velocity"/>.
    /// </summary>
    public float XVelocity
    {
        get => _velocity.X;
        set => _velocity.X = value;
    }

    /// <summary>
    /// <see cref="Vector2.Y"/> component of <see cref="Velocity"/>.
    /// </summary>
    public float YVelocity
    {
        get => _velocity.Y;
        set => _velocity.Y = value;
    }

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

    /// <summary>
    /// The gravity factor this <see cref="GameObject"/> will experience.
    /// </summary>
    public float Gravity { get; set; } = 0;
    #endregion velocity and direction

    #region dimensions
    /// <summary>
    /// The width of <see cref="BoundingBox"/>.
    /// </summary>
    public int Width
    {
        get; 
        set 
        {
            // a width/height of 0 will cause Rectangle to throw an excepiton.
            ArgumentOutOfRangeException.ThrowIfEqual(value, 0);
            field = value;
            _boundingBox = null;
        }
    } = 1;

    /// <summary>
    /// The width of <see cref="BoundingBox"/>.
    /// </summary>
    public int Height
    {
        get; 
        set 
        {
            // a width/height of 0 will cause Rectangle to throw an excepiton.
            ArgumentOutOfRangeException.ThrowIfEqual(value, 0);
            field = value;
            _boundingBox = null;
        }
    } = 1;

    /// <summary>
    /// Cache the value of <see cref="BoundingBox"/>, and only recalculate when something changes.
    /// </summary>
    private Rectangle? _boundingBox = null;

    /// <summary>
    /// A <see cref="Rectangle"/> with <see cref="Position"/>, <see cref="Width"/> and <see cref="Height"/>.
    /// </summary>
    public Rectangle BoundingBox
    {
        get
        {
            if (_boundingBox == null)
                _boundingBox = new((int)Position.X, (int)Position.Y, Width, Height);
            
            return (Rectangle)_boundingBox;
        }
        set
        {
            XPosition = value.X;
            YPosition = value.Y;
            Width = value.Width;
            Height = value.Height;
        }
    }
    #endregion dimensions
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

    #region methods
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
        Position += Velocity * Utilities.DeltaTime(gameTime);
    }

    /// <summary>
    /// Add <see cref="Gravity"/> to <see cref="YVelocity"/>
    /// </summary>
    public void ApplyGravity()
    {
        YVelocity += Gravity;
    }
    #endregion methods
}