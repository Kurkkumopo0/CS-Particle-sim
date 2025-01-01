using SFML.Graphics;
using SFML.Window;

namespace CS_Particle_Sim;
class Program
{
    static int Main()
    {
        RenderWindow window = new(VideoMode.DesktopMode, "Sim");
        Simulator.Simulation sim = new(window);
        sim.Run();

        return 0;
    }
}
