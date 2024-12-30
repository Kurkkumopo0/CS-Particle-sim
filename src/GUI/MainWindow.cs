using SFML.Graphics;
using SFML.Window;
using SFML.System;
using Simulator;

namespace GUI;
class SimulationWindow : RenderWindow
{
    static SimulationWindow()
    {
        WindowSize = new Vector2u(VideoMode.DesktopMode.Width, VideoMode.DesktopMode.Height);
    }

    // Constructor
    public SimulationWindow(string title) : base(VideoMode.DesktopMode, title)
    {
        this.SetFramerateLimit(60u);
    }

    public void HandleEvent()
    {
        for (Event ev; this.PollEvent(out ev);)
        {
            switch (ev.Type)
            {
                case EventType.Closed:
                    this.Close();
                    break;
                case EventType.KeyPressed when ev.Key.Code == Keyboard.Key.Escape:
                    this.Close();
                    break;
            }
        }
    }

    public override void SetFramerateLimit(uint limit)
    {
        this.FPS = limit;
        this.FPSinverse = 1.0F / limit;
        base.SetFramerateLimit(limit);
    }

    public uint FPS { get; private set; }

    public float FPSinverse { get; private set; }

    public static Vector2u WindowSize { get; private set; }
}

