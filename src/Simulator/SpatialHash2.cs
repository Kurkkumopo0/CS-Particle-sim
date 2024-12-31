using SFML.System;

namespace Simulator;
public class SpatialHash2
{
    public int[] Dimensions { get; private set; }
    public float Spacing { get; private set; }
    private LinkedList<Particle2>[] _particles;

    public SpatialHash2(Vector2u windowSize, float maxRadius)
    {
        Spacing = 2 * maxRadius;
        int gx = (int)Math.Ceiling(windowSize.X / Spacing);
        int gy = (int)Math.Ceiling(windowSize.Y / Spacing);
        Dimensions = [gx, gy];
        _particles = new LinkedList<Particle2>[gx * gy];
    }

    public int[] GridPos(Vector2f pos)
    {
        int xi = Math.Min((int)Math.Floor(pos.X / Spacing), Dimensions[0] - 1);
        int yi = Math.Min((int)Math.Floor(pos.X / Spacing), Dimensions[1] - 1);

        return [xi, yi];
    }

    private int _NewKey(int gx, int gy)
    {
        return gy + gx * Dimensions[1];
    }

    public void AddParticle(Particle2 particle)
    {
        int[] gridPos = GridPos(particle.Pos);
        int key = _NewKey(gridPos[0], gridPos[1]);
        particle.Key = key;

        if (_particles[key] == null)
        {
            _particles[key] = new LinkedList<Particle2>();
        }
        _particles[key].AddLast(particle);

    }

    public void RemoveParticle(Particle2 particle)
    {
        _particles[particle.Key]?.Remove(particle);
    }

    public void UpdateBoundary(Vector2u windowSize, float maxRadius)
    {
        Spacing = 2 * maxRadius;
        int gx = (int)Math.Ceiling(windowSize.X / Spacing);
        int gy = (int)Math.Ceiling(windowSize.Y / Spacing);
        Dimensions = [gx, gy];
        _particles = new LinkedList<Particle2>[gx * gy];
    }

    public List<Particle2> GetNearbyObjects(Particle particle)
    {
        List<Particle2> list = [];

        return list;
    }
}