using SFML.Graphics;
using SFML.Window;

class Program
{
    static int Main()
    {
        GUI.SimulationWindow window = new("Hello World");
        Simulator.Simulation sim = new(window);
        sim.Exec();

        return 0;
    }
}
