using SimulationFramework;
using SimulationFramework.Drawing;

partial class farmlight {
    public static void takeover() {
        Program.curUpdate = () => Rend(Program.curCanv);
        Program.current = false;

        Init();
    }

    static void Init() { 
    
    }

    static void Rend(ICanvas canv) {
        canv.Clear(Color.Black);
    }
}