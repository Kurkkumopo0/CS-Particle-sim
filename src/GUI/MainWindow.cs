using SFML.Graphics;
using SFML.Window;
using SFML.System;
using Simulator;

namespace GUI;
class MainWindow : RenderWindow
{
    static MainWindow()
    {
        WindowSize = new Vector2u(VideoMode.DesktopMode.Width, VideoMode.DesktopMode.Height);
    }

    // Constructor
    public MainWindow(string title) : base(VideoMode.DesktopMode, title)
    {
        this.SetFramerateLimit(60u);
        this._simulation.InitParticles(100);
    }
    // Executes the simulation
    public void Exec()
    {
        while (this.IsOpen)
        {
            this.HandleEvent();
            this.Clear();

            foreach (Particle particle in _simulation)
            {
                this.Draw(particle);
            }

            this.Display();
        }
    }

    // Handles MainWindow events
    private void HandleEvent()
    {
        for (Event ev; this.PollEvent(out ev);)
        {
            if (ev.Type == EventType.Closed)
            {
                this.Close();
            }
            else if (ev.Type == EventType.KeyPressed)
            {
                if (ev.Key.Code == Keyboard.Key.Escape)
                {
                    this.Close();
                }
            }
        }
    }

    public static Vector2u WindowSize { get; private set; }

    private readonly Simulation _simulation = new();
}

