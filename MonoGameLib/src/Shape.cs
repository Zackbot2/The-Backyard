using Microsoft.Xna.Framework;

namespace TheBackyard.MonoGameLib;

public interface IShape
{
    float Area {get;}
    Point Center {get;}
}