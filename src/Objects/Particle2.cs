using SFML.System;
using SFML.Graphics;
using Simulator;
public class Particle2
{
    public float Mass { get; init; }
    public float Restitution { get; init; }
    public CircleShape Shape { get; init; }
    public Vector2f Velocity { get; set; }
    public Vector2f Pos
    {
        get => Shape.Position;
        set => Shape.Position = value;
    }
    public int Key { get; set; }

    public Particle2(Vector2f position, float radius, Vector2f velocity, float mass = 1.0F, float collosionDamping = 0.7F)
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

    public void ApplyGravity(Vector2f gravity, float dt)
    {
        Velocity += gravity * dt;
    }

    public void Update(float dt)
    {
        // Update position
        Vector2f newPosition = Pos + Velocity * dt;
        Pos = newPosition;

    }
}
