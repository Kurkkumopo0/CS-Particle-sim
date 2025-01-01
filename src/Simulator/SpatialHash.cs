using System.Security.Cryptography;
using Objects;
using SFML.System;
public class SpatialHash
{
    public float Spacing { get; private set; }
    private int _tableSize;
    private int[] _cellStart;
    private int[] _cellEntries;
    public int Count { get; set; }
    private float _maxLoadFactor = 0.75F;

    public SpatialHash(float spacing, int tableSize)
    {
        Spacing = spacing;
        Count = 0;
        _tableSize = tableSize;
        _cellStart = new int[tableSize + 1];
        _cellEntries = [];
    }

    // Double the hashtable size
    private void _Resize()
    {
        _tableSize *= 2;
        _cellStart = new int[_tableSize + 1];
    }

    // Objects amount change
    private void _UpdateCount(int newCount)
    {
        int oldCount = Count;
        Count = newCount;
        if (newCount > oldCount) while (_LoadFactor() > _maxLoadFactor) _Resize();
        _cellEntries = new int[Count];
    }

    private float _LoadFactor() => Count / (float)_tableSize;

    // Get index to hashtable
    private int _HashKey(int xi, int yi)
    {
        unchecked
        {
            const int seed1 = (int)0x9E3779B1;
            const int seed2 = (int)0x85EBCA77;

            return (((xi * seed1) ^ (yi * seed2)) & 0x7FFFFFFF) % _tableSize;
        }
    }

    // Get index to hashtable
    private int _HashPos(Vector2f pos)
    {
        return _HashKey(
            _GridCoord(pos.X),
            _GridCoord(pos.Y)
        );
    }

    private int _GridCoord(float coord)
    {
        return (int)Math.Floor(coord / Spacing);
    }

    // Creates/updates this data structure from a given list of simulation objects
    public void Create(List<ISimObject> objects)
    {
        // Update the datastructure
        if (objects.Count != Count) _UpdateCount(objects.Count);
        Array.Clear(_cellStart, 0, _cellStart.Length);
        Array.Clear(_cellEntries, 0, _cellEntries.Length);

        // Add positions to hashtable
        for (int i = 0; i < Count; i++)
        {
            int h = _HashPos(objects[i].Pos());
            _cellStart[h]++;
        }

        // Determine cell starts
        int start = 0;
        for (int i = 0; i < _tableSize; i++)
        {
            start += _cellStart[i];
            _cellStart[i] = start;
        }
        _cellStart[_tableSize] = start; // Add guard

        // Fill in object IDs
        for (int i = 0; i < Count; i++)
        {
            int h = _HashPos(objects[i].Pos());
            _cellStart[h]--;
            _cellEntries[_cellStart[h]] = i;
        }
    }

    // Get a list of indexes to nearby SimObjects
    public List<int> GetNearby(Vector2f position, float searchDist)
    {
        List<int> nearbyParticles = new();

        // Determine the bounding box
        int xMin = _GridCoord(position.X - searchDist);
        int yMin = _GridCoord(position.Y - searchDist);
        int xMax = _GridCoord(position.X + searchDist);
        int yMax = _GridCoord(position.Y + searchDist);

        // Iterate over all grid cells in the bounding box
        for (int xi = xMin; xi <= xMax; xi++)
        {
            for (int yi = yMin; yi <= yMax; yi++)
            {
                // Get the hash for the current cell
                int h = _HashKey(xi, yi);

                // Retrieve the start and end indices for objects in this cell
                int start = _cellStart[h];
                int end = _cellStart[h + 1];

                // Add all objects in this cell to the nearby list
                for (int i = start; i < end; i++)
                {
                    nearbyParticles.Add(_cellEntries[i]);
                }
            }
        }

        return nearbyParticles;
    }

}