using SFML.System;
using SFML.Graphics;

namespace Objects;
public interface ISimObject
{
    Vector2f Pos();
    void SetPos(Vector2f position);
    void SetColor(Color color);
    void Draw(RenderWindow window);
}