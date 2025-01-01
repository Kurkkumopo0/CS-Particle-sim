
using System.Numerics;
using Objects;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Simulator;
class Simulation
{
    private float _maxRadius = 5.0F;
    private int _subSteps = 8;
    private float _dt = 1.0F / 60.0F;
    private float _subDt;
    private Vector2f _gravity = new Vector2f(0F, 1000F);
    private RenderWindow _window;
    private List<ISimObject> _objects;
    private SpatialHash _spatialHash;

    public Simulation(RenderWindow window)
    {
        _window = window;
        _subDt = _dt / _subSteps;
        _objects = new();
        _spatialHash = new(spacing: _maxRadius * 2, tableSize: 1024);
        _window.SetFramerateLimit(60);
        _window.Closed += (sender, e) => window.Close();
        _window.MouseButtonPressed += (sender, e) => _HandleMouseEvent(e);
        _window.KeyReleased += (sender, e) => _HandleKeyEvent(e);
    }

    public void AddParticle(Vector2f position, float radius, Vector2f velocity)
    {
        Particle p = new(position, radius, velocity);
        _objects.Add(p);
    }

    public void RemoveParticle(ISimObject obj)
    {
        _objects.Remove(obj);
    }

    public void RemoveAllParticles()
    {
        _objects.Clear();
    }


    // Main loop of the simulation
    public void Run()
    {

        while (_window.IsOpen)
        {
            AddParticle(new Vector2f(_maxRadius, _maxRadius), _maxRadius, new Vector2f(0, 0));
            _window.DispatchEvents();

            for (int i = 0; i < _subSteps; i++)
            {
                _ApplyGravity();
                _HandleCollisions();
                _UpdateParticles();
            }

            _window.Clear();
            _DrawObjects();
            _window.Display();
        }
    }
    private void _HandleCollisions()
    {
        foreach (Particle obj in _objects)
        {
            Vector2u boundary = _window.Size;
            ParticlePhysicsSolver.CollideWithBorder(obj, boundary);
        }

        _spatialHash.Create(_objects);

        foreach (Particle obj in _objects)
        {
            List<int> nearIndexes = _spatialHash.GetNearby(obj.Position, _maxRadius);
            foreach (int index in nearIndexes)
            {
                Particle near = (Particle)_objects[index];
                if (!ReferenceEquals(obj, near))
                {
                    ParticlePhysicsSolver.CheckCollision(obj, near, _subDt);
                }
            }
        }
    }

    private void _ApplyGravity()
    {
        foreach (Particle obj in _objects)
        {
            obj.ApplyGravity(_gravity, _subDt);
        }

    }

    private void _UpdateParticles()
    {
        foreach (Particle obj in _objects)
        {
            obj.Update(_subDt);
        }

    }

    private void _DrawObjects()
    {
        foreach (var obj in _objects)
        {
            obj.Draw(_window);
        }
    }

    private void _HandleMouseEvent(MouseButtonEventArgs e)
    {
        if (e.Button == Mouse.Button.Left)
        {
            Vector2f pos = new(e.X, e.Y);
            AddParticle(pos, _maxRadius, new Vector2f(0, 0));
        }
    }

    private void _HandleKeyEvent(KeyEventArgs e)
    {
        if (e.Code == Keyboard.Key.R)
        {
            RemoveAllParticles();
        }
    }

}