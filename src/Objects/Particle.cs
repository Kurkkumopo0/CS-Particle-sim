using SFML.System;
using SFML.Graphics;

namespace Objects;
public class Particle : ISimObject
{
    public float Mass { get; init; }
    public float Restitution { get; init; }
    public CircleShape Shape { get; init; }
    public Vector2f Velocity { get; set; }
    public float Radius { get => Shape.Radius; }
    public Vector2f Position
    {
        get => Shape.Position;
        set => Shape.Position = value;
    }

    public Particle(Vector2f position, float radius, Vector2f velocity, float mass = 1.0F, float collosionDamping = 0.7F)
    {
        Shape = new CircleShape(radius)
        {
            FillColor = Color.White,
            Origin = new Vector2f(radius, radius),
            Position = position
        };

        Velocity = velocity;
        Mass = mass;
        Restitution = collosionDamping;
    }

    public Vector2f Pos() => Position;
    public void SetPos(Vector2f position) => Position = position;
    public void SetColor(Color color) => Shape.FillColor = color;
    public void Draw(RenderWindow window) => window.Draw(Shape);


    public void ApplyGravity(Vector2f gravity, float dt)
    {
        Velocity += gravity * dt;
    }

    public void Update(float dt)
    {
        // Update position
        Vector2f newPosition = Pos() + Velocity * dt;
        SetPos(newPosition);

    }
}
