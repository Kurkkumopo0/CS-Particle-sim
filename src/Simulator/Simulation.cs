using System.Collections;
using System.Net.NetworkInformation;
using GUI;
using Objects;
using SFML.Window;

namespace Simulator;
class Simulation : IEnumerable
{
    public SimulationWindow SimulationWindow { get; init; }

    public HashGrid Grid { get; init; }

    private readonly LinkedList<Particle> _particles = new();

    public int NumOfParticles
    {
        get => _particles.Count;
    }

    public Simulation(SimulationWindow window)
    {
        SimulationWindow = window;
        Grid = new HashGrid(window.Size);
    }

    public IEnumerator GetEnumerator()
    {
        foreach (Particle particle in _particles)
        {
            yield return particle;
        }
    }

    public void AddParticle(Particle particle)
    {
        _particles.AddLast(particle);
        Grid.NewParticle(particle);
    }

    public void RemoveParticle(Particle particle)
    {
        if (_particles.Remove(particle))
        {
            if (Grid.RemoveParticle(particle) != null)
            {
                Console.Write("Particle removed.");
            }
            else
            {
                Console.Write("Error: Client couldn't be removed");
            }
        }
        else
        {
            Console.Write("Error: Particle not found");
        }
    }

    // Testing method.
    public void InitParticles(int numOf)
    {
        if (NumOfParticles < 1000)
        {
            for (int i = 0; i < numOf; i++)
            {
                this.AddParticle(new Particle(10.0F));
            }
        }
    }

    public void RemoveAllParticles()
    {
        _particles.Clear();
    }


    // Main loop of the simulation
    public void Exec()
    {
        uint ii = 0;
        while (SimulationWindow.IsOpen)
        {
            if (ii % 10 == 0) this.InitParticles(1);
            const int subSteps = 8;
            float subDt = SimulationWindow.FPSinverse / subSteps;

            SimulationWindow.HandleEvent();
            SimulationWindow.Clear();
            for (int i = 0; i < subSteps; i++)
            {
                this._Update(subDt);
            }
            foreach (Particle p in _particles)
            {
                SimulationWindow.Draw(p);
            }
            SimulationWindow.Display();
            ii++;
        }
    }

    private void _Update(float dt)
    {
        foreach (Particle particle in _particles)
        {
            ParticlePhysicsSolver.ApplyGravity(particle, 1000.0F);
            ParticlePhysicsSolver.CollideWithBorder(particle, SimulationWindow.Size, dt);

            List<Particle> nearbyParticles = Grid.GetNearbyObjects(particle);
            foreach (Particle nearParticle in nearbyParticles)
            {
                ParticlePhysicsSolver.HandleCollision(particle, nearParticle, dt);
            }
            particle.UpdatePos(dt);
            Grid.Update(particle);
        }

    }
}