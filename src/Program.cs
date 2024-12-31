using SFML.Graphics;
using SFML.Window;

class Program
{
    static int Main()
    {
        GUI.SimulationWindow window = new("Hello World");
        Simulator.Simulation2 sim = new(window);
        sim.Run();

        return 0;
    }
}
