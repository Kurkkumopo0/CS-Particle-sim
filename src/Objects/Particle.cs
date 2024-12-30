using SFML.System;
using SFML.Graphics;
using Objects;
public class Particle : CircleShape
{
    public Vector2f PreviousPosition
    {
        get => _previousPosition;
        set => _previousPosition = value;
    }
    public Vector2f Acceleration
    {
        get => _acceleration;
        set => _acceleration = value;
    }
    public Vector2f VelocityDt { get; set; }

    public float Mass { get; init; }
    public float Restitution { get; init; }
    public int? HashKey { get; set; }


    private Vector2f _acceleration;
    private Vector2f _previousPosition;

    public Particle(float radius, float mass = 1.0F, float restitution = 0.75F) : base(radius)
    {
        this.Origin = new Vector2f(radius, radius);
        this.Position = new Vector2f(radius, radius);
        this.PreviousPosition = new Vector2f(radius - 0.01F, radius);
        this.VelocityDt = new Vector2f(0F, 0F);
        this.Acceleration = new Vector2f(0F, 0F);

        this.Mass = mass;
        this.Restitution = restitution;
    }

    public void ResetAcceleration()
    {
        _acceleration.X = 0; _acceleration.Y = 0;
    }

    public void Accelerate(float x, float y)
    {
        _acceleration.X += x; _acceleration.Y += y;
    }

    public void Accelerate(Vector2f acc)
    {
        _acceleration += acc;
    }

    public void SetPos(float? x = null, float? y = null)
    {
        float newX = Position.X;
        float newY = Position.Y;

        if (x != null) { newX = (float)x; }
        if (y != null) { newY = (float)y; }

        Position = new Vector2f(newX, newY);
    }

    public void SetPrevPos(float? x = null, float? y = null)
    {
        if (x != null) { _previousPosition.X = (float)x; }
        if (y != null) { _previousPosition.Y = (float)y; }
    }

    public void UpdatePos(float dt)
    {
        VelocityDt = Position - PreviousPosition;
        PreviousPosition = Position;
        Position += VelocityDt + 0.5F * Acceleration * dt * dt;
        VelocityDt = Position - PreviousPosition;
        this.ResetAcceleration();
    }

}