using SFML.System;
using SFML.Graphics;
using Simulator;
public class Particle
{
    public float Mass { get; init; }
    public float Restitution { get; init; }
    public CircleShape Shape { get; init; }
    public Vector2f Velocity { get; set; }
    public SpatialHash.Client Client { get; set; }

    public Particle(SpatialHash.Client client, float radius, Vector2f velocity, float mass = 1.0F, float collosionDamping = 0.5F)
    {
        Client = client;
        Shape = new CircleShape(radius)
        {
            FillColor = Color.White,
            Origin = new Vector2f(radius, radius),
            Position = client.Position
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
        Vector2f newPosition = Shape.Position + Velocity * dt;
        Shape.Position = newPosition;

    }
}
