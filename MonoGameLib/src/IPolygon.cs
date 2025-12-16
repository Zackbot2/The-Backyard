using Microsoft.Xna.Framework;

namespace TheBackyard.MonoGameLib;

public interface IPolygon
{
    List<Vector2> ToVectors();
    List<Vector2> GetNormals();
}