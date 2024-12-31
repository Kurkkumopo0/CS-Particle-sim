
using System.Numerics;
using GUI;
using SFML.System;

namespace Simulator;
class Simulation2
{
    private float _maxRadius = 5.0F;
    private int _subSteps = 4;
    private float _dt = 1.0f / 60.0F;
    private float _subDt;
    private Vector2f _gravity = new Vector2f(0F, 1000F);

    private SimulationWindow _window;
    private LinkedList<Particle2> _particles;
    private SpatialHash2 _spatialHash;

    public Simulation2(SimulationWindow window)
    {
        _window = window;
        _subDt = _dt / _subSteps;
        _particles = new();
        _spatialHash = new(_window.Size, _maxRadius);
    }

    public void AddParticle(Vector2f position, float radius, Vector2f velocity)
    {
        Particle2 p = new(position, radius, velocity);
        _particles.AddLast(p);
        _spatialHash.AddParticle(p);
    }

    public void RemoveParticle(Particle2 particle)
    {
        _particles.Remove(particle);
        _spatialHash.RemoveParticle(particle);
    }

    public void RemoveAllParticles()
    {
        foreach (Particle2 particle in _particles)
        {
            RemoveParticle(particle);
        }
        _particles.Clear();
    }


    // Main loop of the simulation
    public void Run()
    {
        AddParticle(new Vector2f(_maxRadius, _maxRadius), _maxRadius, new Vector2f(1F, 0));
        while (_window.IsOpen)
        {
            _window.HandleEvent();
            _window.Clear();

            for (int i = 0; i < _subSteps; i++)
            {
                _ApplyGravity();
                _HandleCollisions();
                _UpdateParticles();

            }

            foreach (Particle2 p in _particles)
            {
                _window.Draw(p.Shape);
            }
            _window.Display();
        }
    }
    private void _HandleCollisions()
    {
        foreach (Particle2 p in _particles)
        {
            Vector2u boundary = _window.Size;
            float restitution = p.Restitution;
            float x = p.Pos.X;
            float y = p.Pos.Y;

            // Handle border collision on the X-axis
            if (x - p.Shape.Radius < 0)
            {
                p.Velocity = new Vector2f(restitution * -p.Velocity.X, p.Velocity.Y);
                p.Pos = new Vector2f(p.Shape.Radius, p.Pos.Y);
            }
            else if (x + p.Shape.Radius > boundary.X)
            {
                p.Velocity = new Vector2f(restitution * -p.Velocity.X, p.Velocity.Y);
                p.Pos = new Vector2f(boundary.X - p.Shape.Radius, p.Pos.Y);
            }

            // Handle border collision on the Y-axis
            if (y - p.Shape.Radius < 0)
            {
                p.Velocity = new Vector2f(p.Velocity.X, restitution * -p.Velocity.Y);
                p.Pos = new Vector2f(p.Pos.X, p.Shape.Radius);

            }
            else if (y + p.Shape.Radius > boundary.Y)
            {
                p.Velocity = new Vector2f(p.Velocity.X, restitution * -p.Velocity.Y);
                p.Pos = new Vector2f(p.Pos.X, boundary.Y - p.Shape.Radius);
            }

        }
    }

    private void _ApplyGravity()
    {
        foreach (Particle2 p in _particles)
        {
            p.ApplyGravity(_gravity, _subDt);
        }

    }

    private void _UpdateParticles()
    {
        foreach (Particle2 p in _particles)
        {
            p.Update(_subDt);
        }

    }

}