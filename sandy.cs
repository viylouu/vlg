using ImGuiNET;
using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using System.Numerics;

partial class sandy {
    static Vector2 mp = new Vector2(0, 0);
    static float ms = 5;
    static cell[,] map = null;
    static bool[,] cellupd = null;
    static ITexture maptex = null;
    static float updtime = 1;
    static float time = updtime;
    static float fps = 0;
    static float avdur = 1;
    static float avprog = 1;

    static cell sand = new cell() {
        col = new Color(237, 188, 90, 255),
        move = true
    };

    public static void takeover() {
        Program.curUpdate = () => Rend(Program.curCanv);
        Program.current = false;

        Init();
    }

    static void Init() {
        mp = Mouse.Position;

        Simulation.SetFixedResolution(640, 360, Color.Black, false, false, false);
        map = new cell[Window.Width, Window.Height];
        maptex = Graphics.CreateTexture(Window.Width, Window.Height);
    }

    public static void Rend(ICanvas canv) {
        draw(canv);
        updvars();
    }

    static void updvars() {
        mp += m.twn2(mp, Mouse.Position, ms);

        if (Mouse.IsButtonDown(MouseButton.Left) && mp.X >= 0 && Window.Height - mp.Y >= 0 && mp.X >= Window.Width && Window.Height - mp.Y < Window.Height) { 
            map[(int)mp.X, (int)(Window.Height - mp.Y)] = sand;

            maptex.GetPixel((int)mp.X, (int)(Window.Height - mp.Y)) = sand.col;
        }

        time -= Time.DeltaTime;

        if (time <= 0) {
            process();
            time = updtime;
        }

        avprog -= Time.DeltaTime;

        if (avprog <= 0) {
            avprog = avdur;
            fps = m.rnd(1 / Time.DeltaTime);
        }
    }

    static void draw(ICanvas canv) {
        canv.Clear(new Color(44, 45, 53));

        canv.Scale(1, 1);
        canv.DrawTexture(maptex, Vector2.Zero, new Vector2(canv.Width, canv.Height), Alignment.TopLeft);
        canv.ResetState();

        canv.Fill(Color.White);
        canv.DrawRect(m.rnd(mp.X), m.rnd(mp.Y), 1, 1, Alignment.TopLeft);

        canv.DrawText($"FPS: {fps}", Vector2.One * 5, Alignment.TopLeft);
    }

    static void process() {
        bool upd = false;

        cellupd = new bool[map.GetLength(0), map.GetLength(1)];

        for (int x = 0; x < map.GetLength(0); x++) {
            for (int y = 0; y < map.GetLength(1); y++) {
                if (map[x, y] != null && cellupd[x, y] == false) {
                    if (map[x, y].move) {
                        bool moved = false;

                        if (y > 0) {
                            movecell(ref moved, 0, -1, x, y, ref cellupd, ref upd, false);
                        }
                    }
                }
            }
        }

        if (upd) { maptex.ApplyChanges(); }
    }

    static void movecell(ref bool moved, int x, int y, int curx, int cury, ref bool[,] cellupd, ref bool upd, bool swap) { 
        cell cur = map[curx, cury];
        cell prev = map[curx + x, cury + y];
        map[curx, cury] = swap? prev : null;
        map[curx + x, cury + y] = cur;
        cellupd[curx + x, cury + y] = true;
        moved = true;

        maptex.GetPixel(curx, cury) = swap? map[curx, cury].col : Color.Transparent;
        maptex.GetPixel(curx + x, cury + y) = map[curx + x, cury + y].col;

        upd = true;
    }

    class cell { 
        public Color col { get; set; }

        //rules
        public bool move { get; set; }
    }
}
