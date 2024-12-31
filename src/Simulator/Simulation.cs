
using GUI;
using SFML.System;

namespace Simulator;
class Simulation
{
    private float _maxRadius = 5.0F;
    private SimulationWindow _window;
    private SpatialHash _spatialHash;
    private List<Particle> _particles;
    public int NumOfParticles { get => _particles.Count; }

    public Simulation(SimulationWindow window)
    {
        _window = window;
        Vector2f[] bounds = [new Vector2f(0, 0), (Vector2f)window.Size];
        _spatialHash = new SpatialHash(bounds, SpatialHash.GetDimension(window.Size, _maxRadius));
        _particles = new();
    }

    public void AddParticle(Vector2f position, float radius, Vector2f velocity)
    {
        SpatialHash.Client newClient = _spatialHash.NewClient(position, new Vector2f(2 * radius, 2 * radius));
        _particles.Add(new Particle(newClient, radius, velocity));
    }

    public void RemoveParticle(Particle particle)
    {
        _spatialHash.Remove(particle.Client);
        _particles.Remove(particle);
    }

    public void RemoveAllParticles()
    {
        foreach (Particle particle in _particles)
        {
            RemoveParticle(particle);
        }
        _particles.Clear();
    }


    // Main loop of the simulation
    public void Run()
    {
        const int subSteps = 8;
        float subDt = _window.FPSinverse / subSteps;

        uint ii = 0;
        while (_window.IsOpen)
        {
            if (ii % 16 == 0)
            {
                AddParticle(new Vector2f(_maxRadius, _maxRadius), _maxRadius, new Vector2f(0.1F, 0));
            }

            _window.HandleEvent();
            _window.Clear();

            for (int i = 0; i < subSteps; i++)
            {
                this._Update(subDt);
            }

            foreach (Particle p in _particles)
            {
                _window.Draw(p.Shape);
            }
            _window.Display();
            ii++;
        }
    }

    private void _Update(float dt)
    {
        foreach (Particle particle in _particles)
        {
            particle.ApplyGravity(new Vector2f(0, 1000F), dt);
            ParticlePhysicsSolver.CollideWithBorder(particle, _window.Size);
            var nearClients = _spatialHash.FindNear(particle.Shape.Position, particle.Client.Dimensions);
            particle.Update(dt);
            _spatialHash.UpdateClient(particle.Client);

        }
    }
}