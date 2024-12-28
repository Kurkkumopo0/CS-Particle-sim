using System.Collections;

namespace Simulator;
class Simulation : IEnumerable
{
    public Simulation()
    {
        PhysicsSolver = new PhysicsSolver();
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
    }

    public void RemoveParticle(ref Particle particle)
    {
        if (_particles.Remove(particle))
            Console.Write("Particle removed.");
        else
            Console.Write("Error: Particle not found");
    }

    // Testing method.
    public void InitParticles(int numOf)
    {
        for (int i = 0; i < numOf; i++)
        {
            this.AddParticle(new Particle(10));
        }
    }

    public void RemoveAllParticles()
    {
        _particles.Clear();
    }

    public int NumOfParticles
    {
        get => _particles.Count;
    }

    public PhysicsSolver PhysicsSolver { get; init; }

    private readonly LinkedList<Particle> _particles = new();
}