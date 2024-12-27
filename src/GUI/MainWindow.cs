using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace GUI;
class MainWindow : RenderWindow
{
    // Constructor
    public MainWindow(string title) : base(VideoMode.DesktopMode, title)
    {
        this.SetFramerateLimit(60u);
    }

    // Executes the simulation
    public void Exec()
    {
        while (this.IsOpen)
        {
            this.HandleEvent();
            this.Clear();
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

}

