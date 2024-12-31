using System.Numerics;
using SFML.Graphics.Glsl;
using SFML.System;

namespace Simulator;
static class ParticlePhysicsSolver
{
    /*
    public static void HandleCollision(Particle particle, Particle other, float dt)
    {
        Vector2f collisionAxis = particle.Position - other.Position;
        //if (collisionAxis.X == 0) collisionAxis.X = 1e-6F;
        //if (collisionAxis.Y == 0) collisionAxis.Y = 1e-6F;

        float distanceEnergy = MathF.Pow(collisionAxis.X, 2) + MathF.Pow(collisionAxis.Y, 2);
        float threshold = particle.Radius + other.Radius;
        float thresholdEnergy = MathF.Pow(threshold, 2);

        if (distanceEnergy < thresholdEnergy)
        {
            // Update position
            float distance = MathF.Sqrt(distanceEnergy);
            float delta = distance - threshold;
            Vector2f normalized = collisionAxis / distance;  // Normalize the collision axis

            particle.Position -= 0.5F * normalized * delta;
            other.Position += 0.5F * normalized * delta;
            
            particle.SetPrevPos(particle.Position.X, particle.Position.Y);
            other.SetPrevPos(other.Position.X, other.Position.Y);

            // Update acceleration based on impulse
            float restitution = Math.Min(particle.Restitution, other.Restitution);
            Vector2f v1 = particle.VelocityDt / dt;
            Vector2f v2 = other.VelocityDt / dt;

            float m1 = particle.Mass;
            float m2 = other.Mass;

            Vector2f u1 = (m1 * v1 + m2 * v2 - m2 * restitution * (v1 - v2)) / (m1 + m2);

            Vector2f impulse = m1 * (u1 - v1);
            Vector2f force = impulse / dt;

            particle.Accelerate(force / m1);
            other.Accelerate(-force / m2);
            
        }
    }
    */


    public static void CollideWithBorder(Particle particle, Vector2u boundary)
    {
        float restitution = particle.Restitution;
        float x = particle.Shape.Position.X;
        float y = particle.Shape.Position.Y;

        // Handle collision on the X-axis
        if (x - particle.Shape.Radius < 0 || x + particle.Shape.Radius > boundary.X)
        {
            particle.Velocity = new Vector2f(restitution * -particle.Velocity.X, restitution * particle.Velocity.Y);
        }

        // Handle collision on the Y-axis
        if (y - particle.Shape.Radius < 0 || y + particle.Shape.Radius > boundary.Y)
        {
            particle.Velocity = new Vector2f(restitution * particle.Velocity.X, restitution * -particle.Velocity.Y);
        }
    }
}
