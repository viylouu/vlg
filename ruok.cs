using SimulationFramework;
using SimulationFramework.Drawing;

partial class ruok {
    public static void takeover() {
        Program.curUpdate = () => Rend(Program.curCanv);
        Program.current = false;

        Init();
    }

    static void Init() {
        Simulation.SetFixedResolution(640, 360, Color.Black, false, false, false);
    }

    public static void Rend(ICanvas canv) {
        canv.Clear(Color.Black);
    }
}