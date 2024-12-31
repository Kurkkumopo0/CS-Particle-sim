using SFML.System;

namespace Simulator;
public class SpatialHash
{
    private readonly List<Node>[,] _cells;
    private readonly Vector2u _dimensions;
    private readonly Vector2f[] _bounds;
    private int _queryIdCounter;

    public SpatialHash(Vector2f[] bounds, Vector2u dimensions)
    {
        _dimensions = dimensions;
        _bounds = bounds;
        _cells = new List<Node>[dimensions.X, dimensions.Y];

        for (int x = 0; x < dimensions.X; x++)
        {
            for (int y = 0; y < dimensions.Y; y++)
            {
                _cells[x, y] = new List<Node>();
            }
        }
        _queryIdCounter = 0;
    }

    private (int, int) GetCellIndex(Vector2f position)
    {
        double xNormalized = Math.Clamp((position.X - _bounds[0].X) / (_bounds[1].X - _bounds[0].X), 0, 1);
        double yNormalized = Math.Clamp((position.Y - _bounds[0].Y) / (_bounds[1].Y - _bounds[0].Y), 0, 1);

        int xIndex = (int)(xNormalized * (_dimensions.X - 1));
        int yIndex = (int)(yNormalized * (_dimensions.Y - 1));

        return (xIndex, yIndex);
    }

    public Client NewClient(Vector2f position, Vector2f dimensions)
    {
        var client = new Client(position, dimensions);
        Insert(client);
        return client;
    }

    private void Insert(Client client)
    {
        (int minX, int minY) = GetCellIndex(0.5F * (client.Position - client.Dimensions));
        (int maxX, int maxY) = GetCellIndex(0.5F * (client.Position + client.Dimensions));

        client.MinCell = new Vector2u((uint)minX, (uint)minY);
        client.MaxCell = new Vector2u((uint)maxX, (uint)maxY);

        for (int xi = minX; xi <= maxX; xi++)
        {
            for (int yi = minY; yi <= maxY; yi++)
            {
                var node = new Node(client);
                _cells[xi, yi].Add(node);
                client.Nodes.Add(node);
            }
        }
    }

    public void UpdateClient(Client client)
    {
        (int minX, int minY) = GetCellIndex(0.5F * (client.Position - client.Dimensions));
        (int maxX, int maxY) = GetCellIndex(0.5F * (client.Position + client.Dimensions));

        if (client.MinCell.X == minX && client.MinCell.Y == minY &&
            client.MaxCell.X == maxX && client.MaxCell.Y == maxY)
        {
            return;
        }

        Remove(client);
        Insert(client);
    }

    public List<Client> FindNear(Vector2f position, Vector2f bounds)
    {
        (int xi, int yi) = GetCellIndex(0.5F * (position - bounds));
        (int xend, int yend) = GetCellIndex(0.5F * (position + bounds));

        List<Client> result = new List<Client>();
        int queryId = _queryIdCounter++;

        for (; xi <= xend; xi++)
        {
            for (; yi <= yend; yi++)
            {
                foreach (var node in _cells[xi, yi])
                {
                    if (node.Client.QueryId != queryId)
                    {
                        node.Client.QueryId = queryId;
                        result.Add(node.Client);
                    }
                }
            }
        }

        return result;
    }

    public void Remove(Client client)
    {
        foreach (var node in client.Nodes)
        {
            _cells[node.CellX, node.CellY].Remove(node);
        }
        client.Nodes.Clear();
    }

    public class Client
    {
        public Vector2f Position { get; private set; }
        public Vector2f Dimensions { get; private set; }
        public Vector2u MinCell { get; set; }
        public Vector2u MaxCell { get; set; }
        public List<Node> Nodes { get; private set; }
        public int QueryId { get; set; }

        public Client(Vector2f position, Vector2f dimensions)
        {
            Position = position;
            Dimensions = dimensions;
            Nodes = new List<Node>();
            QueryId = -1;
        }
    }
    public class Node
    {
        public Client Client { get; }
        public int CellX { get; }
        public int CellY { get; }

        public Node(Client client)
        {
            Client = client;
        }
    }

    public static Vector2u GetDimension(Vector2u windowSize, float maxRadius)
    {
        uint dimensionX = (uint)Math.Ceiling(windowSize.X / (2 * maxRadius));
        uint dimensionY = (uint)Math.Ceiling(windowSize.Y / (2 * maxRadius));
        return new Vector2u(dimensionX, dimensionY);

    }
}
