using Objects;
using SFML.System;

namespace Simulator;
public class HashGrid
{
    public Vector2u Dimensinons { get; private set; }

    public float Spacing { get; private set; }

    private LinkedList<Particle>[] _clients;

    public HashGrid(Vector2u dimensions)
    {
        Dimensinons = dimensions;
        Spacing = 0;
        _clients = new LinkedList<Particle>[Dimensinons.X * Dimensinons.Y];
    }

    public void NewParticle(Particle particle)
    {
        Spacing = Math.Max(Spacing, 2 * particle.Radius);
        int key = _GetHash(GetGridPos(particle.Position));

        _AddParticle(particle, key);
    }

    private void _AddParticle(Particle particle, int key)
    {
        particle.HashKey = key;
        if (_clients[key] == null)
        {
            _clients[key] = new LinkedList<Particle>();
        }
        _clients[key].AddLast(particle);
    }

    public Particle? RemoveParticle(Particle particle)
    {
        if (particle.HashKey != null)
        {
            _clients[(int)particle.HashKey].Remove(particle);
            particle.HashKey = null;
            return particle;
        }
        return null;
    }

    public bool Update(Particle particle)
    {
        if (RemoveParticle(particle) != null)
        {
            int newKey = _GetHash(GetGridPos(particle.Position));
            _AddParticle(particle, newKey);
            return true;
        }
        return false;
    }

    public void Clear()
    {
        Array.Clear(_clients, 0, _clients.Length);
    }

    private int _GetHash(int gridX, int gridY)
    {
        return Math.Min(gridY + (int)Dimensinons.X * gridX, (int)(Dimensinons.X * Dimensinons.Y - 1));

    }
    private int _GetHash((int, int) gridPos)
    {
        (int gridX, int gridY) = gridPos;
        return _GetHash(gridX, gridY);

    }

    public (int, int) GetGridPos(Vector2f pos)
    {
        int gridX = (int)(pos.X / Spacing);
        int gridY = (int)(pos.Y / Spacing);
        return (gridX, gridY);
    }

    public void UpdateDimensions(Vector2u newDimensions)
    {
        Dimensinons = newDimensions;
        _clients = new LinkedList<Particle>[Dimensinons.X * Dimensinons.Y];
    }

    public List<Particle> GetNearbyObjects(Particle particle)
    {
        List<Particle> list = [];
        List<int> cellHashs;

        (int gridX, int gridY) = GetGridPos(particle.Position);
        cellHashs = _GetNearbyCells(gridX, gridY);

        foreach (int i in cellHashs)
        {
            LinkedList<Particle> nearParticles = _clients[i];
            if (nearParticles != null)
            {
                foreach (Particle near in nearParticles)
                {
                    if (!ReferenceEquals(near, particle))
                    {
                        list.Add(near);
                    }
                }
            }
        }
        return list;
    }

    private List<int> _GetNearbyCells(int gridX, int gridY)
    {
        List<int> indexes = [];
        int i = Math.Max(gridX - 1, 0);
        int endI = Math.Min(gridX + 1, (int)Dimensinons.X);

        int j = Math.Max(gridY - 1, 0);
        int endJ = Math.Min(gridY + 1, (int)Dimensinons.Y);

        for (; i <= endI; i++)
        {
            for (; j <= endJ; j++)
            {
                indexes.Add(_GetHash(i, j));
            }

        }
        return indexes;
    }

}