using SFML.Graphics;
using SFML.Window;

class Program
{
    static int Main()
    {
        GUI.MainWindow window = new GUI.MainWindow("Hello World");
        window.Exec();

        return 0;
    }
}
