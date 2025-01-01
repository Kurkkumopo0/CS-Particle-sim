using Objects;
using SFML.System;

namespace Simulator;
static class ParticlePhysicsSolver
{

    public static void CheckCollision(Particle particle, Particle other, float dt)
    {
        // Calculate the collision axis
        Vector2f collisionAxis = particle.Pos() - other.Pos();

        if (collisionAxis.X == 0 && collisionAxis.Y == 0)
        {
            // Generate a random collision axis
            Random rand = new Random();
            float randomX = (float)(rand.NextDouble() * 2 - 1);
            float randomY = (float)(rand.NextDouble() * 2 - 1);
            collisionAxis = new Vector2f(randomX, randomY);
        }

        float distanceEnergy = MathF.Pow(collisionAxis.X, 2) + MathF.Pow(collisionAxis.Y, 2);
        float threshold = particle.Radius + other.Radius;

        if (distanceEnergy < threshold * threshold)
        {
            // Update positions to prevent overlap
            float distance = MathF.Sqrt(distanceEnergy);
            float delta = distance - threshold;
            Vector2f normalized = collisionAxis / distance;  // Normalize the collision axis

            // Move particles apart along the collision axis
            particle.Position -= 0.5F * normalized * delta;
            other.Position += 0.5F * normalized * delta;

            // Calculate velocities along the collision axis
            float r = Math.Min(particle.Restitution, other.Restitution);

            Vector2f v1 = particle.Velocity;
            Vector2f v2 = other.Velocity;

            float m1 = particle.Mass;
            float m2 = other.Mass;

            // Compute relative velocity along the collision axis using dot product
            float v1AlongAxis = v1.X * normalized.X + v1.Y * normalized.Y;
            float v2AlongAxis = v2.X * normalized.X + v2.Y * normalized.Y;

            // Calculate the end relative velocity scalars along the collision axis
            float impulse = (1 + r) * (v2AlongAxis - v1AlongAxis) * m1 * m2 / (m1 + m2);
            Vector2f impulseVector = impulse * normalized;

            // Update velocities along the collision axis
            particle.Velocity += impulseVector / m1;
            other.Velocity -= impulseVector / m2;
        }
    }



    public static void CollideWithBorder(Particle p, Vector2u boundary)
    {
        float restitution = p.Restitution;
        float x = p.Pos().X;
        float y = p.Pos().Y;

        // Handle border collision on the X-axis
        if (x - p.Shape.Radius < 0)
        {
            p.Velocity = new Vector2f(restitution * -p.Velocity.X, p.Velocity.Y);
            p.SetPos(new Vector2f(p.Shape.Radius, p.Pos().Y));
        }
        else if (x + p.Shape.Radius > boundary.X)
        {
            p.Velocity = new Vector2f(restitution * -p.Velocity.X, p.Velocity.Y);
            p.SetPos(new Vector2f(boundary.X - p.Shape.Radius, p.Pos().Y));
        }

        // Handle border collision on the Y-axis
        if (y - p.Shape.Radius < 0)
        {
            p.Velocity = new Vector2f(p.Velocity.X, restitution * -p.Velocity.Y);
            p.SetPos(new Vector2f(p.Pos().X, p.Shape.Radius));

        }
        else if (y + p.Shape.Radius > boundary.Y)
        {
            p.Velocity = new Vector2f(p.Velocity.X, restitution * -p.Velocity.Y);
            p.SetPos(new Vector2f(p.Pos().X, boundary.Y - p.Shape.Radius));
        }
    }
}
