using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using System.Numerics;

partial class sandy {
    static Vector2 mp = new Vector2(0, 0);
    static float ms = 5;

    public static void takeover() { 
        Program.curUpdate = () => Rend(Program.curCanv);
        Program.current = false;

        Init();
    }

    static void Init() {
        mp = Mouse.Position;

        Simulation.SetFixedResolution(640, 360, Color.Black, false, false, false);
    }

    public static void Rend(ICanvas canv) {
        draw(canv);
        updvars();
    }

    static void updvars() {
        mp += m.twn2(mp, Mouse.Position, ms);
    }

    static void draw(ICanvas canv) {
        canv.Clear(new Color(41, 41, 46));

        canv.Fill(Color.White);
        canv.DrawRect(m.rnd(mp.X), m.rnd(mp.Y), 1, 1, Alignment.TopLeft);
    }
}
