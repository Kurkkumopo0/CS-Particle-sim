using SFML.System;
using SFML.Graphics;
class Particle : CircleShape
{
    public Particle(uint radius) : base(radius)
    {
        this.Position = new Vector2f(0, 0);
        this.Origin = new Vector2f(Radius, Radius);
    }

}