using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Objects;

namespace CS_Particle_Sim_tests;
public class TestObject : ISimObject
{
    private CircleShape _shape;

    public TestObject(Vector2f position, float radius, Color color)
    {
        _shape = new CircleShape(radius)
        {
            Position = position,
            FillColor = color,
            Origin = new Vector2f(radius, radius)
        };
    }

    public Vector2f Pos() => _shape.Position;

    public void SetPos(Vector2f position)
    {
        _shape.Position = position;
    }

    public void SetColor(Color color)
    {
        _shape.FillColor = color;
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(_shape);
    }
}

// Test class for SpatialHash
class SpatialHash_Test
{
    private const int ObjectCount = 500;
    private const float ObjectRadius = 5f;
    private const float SearchRadius = 50f;


    static void DrawGrid(RenderWindow window, float spacing)
    {
        // Get the size of the window
        Vector2u windowSize = window.Size;

        // Set up the grid color
        Color gridColor = new Color(200, 200, 200, 50);

        // Draw vertical lines
        for (float x = 0; x < windowSize.X; x += spacing)
        {
            Vertex[] line =
            [
                new Vertex(new Vector2f(x, 0), gridColor),
                new Vertex(new Vector2f(x, windowSize.Y), gridColor)
            ];
            window.Draw(line, PrimitiveType.Lines);
        }

        // Draw horizontal lines
        for (float y = 0; y < windowSize.Y; y += spacing)
        {
            Vertex[] line = new Vertex[]
            {
                new Vertex(new Vector2f(0, y), gridColor),
                new Vertex(new Vector2f(windowSize.X, y), gridColor)
            };
            window.Draw(line, PrimitiveType.Lines);
        }
    }

    public static void Main()
    {
        // Create the window
        RenderWindow window = new RenderWindow(VideoMode.DesktopMode, "Spatial Hash Test");
        window.SetFramerateLimit(60);
        window.Closed += (sender, e) => window.Close();

        // Create spatial hash
        SpatialHash spatialHash = new SpatialHash(spacing: 50f, tableSize: 1024);

        // Create objects at random positions
        List<ISimObject> objects = new();
        Random random = new Random();
        for (int i = 0; i < ObjectCount; i++)
        {
            Vector2f position = new Vector2f(
                (float)random.NextDouble() * window.Size.X,
                (float)random.NextDouble() * window.Size.Y);
            objects.Add(new TestObject(position, ObjectRadius, Color.White));
        }

        // Create the hash
        spatialHash.Create(objects);

        // Create mouse hover indicator
        CircleShape mouseCircle = new CircleShape(SearchRadius)
        {
            FillColor = new Color(255, 255, 255, 50),
            OutlineColor = Color.Blue,
            OutlineThickness = 2,
            Origin = new Vector2f(SearchRadius, SearchRadius)
        };

        // Main loop
        while (window.IsOpen)
        {
            window.DispatchEvents();

            // Clear window
            window.Clear(Color.Black);

            // Update mouse position and circle
            Vector2i mousePos = Mouse.GetPosition(window);
            Vector2f mouseWorldPos = new Vector2f(mousePos.X, mousePos.Y);
            mouseCircle.Position = mouseWorldPos;

            // Reset color
            foreach (var obj in objects)
                obj.SetColor(Color.White);

            // Find nearby objects and color them
            List<int> nearbyIndices = spatialHash.GetNearby(mouseWorldPos, SearchRadius);
            foreach (int index in nearbyIndices)
                objects[index].SetColor(Color.Red);

            // Draw objects, mouse circle and grid
            foreach (var obj in objects)
                obj.Draw(window);

            window.Draw(mouseCircle);
            DrawGrid(window, SearchRadius);

            // Display window
            window.Display();
        }
    }
}
