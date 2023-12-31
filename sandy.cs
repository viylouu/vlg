﻿using ImGuiNET;
using NAudio.Wave;
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
    static float updtime = 1 / 60f;
    static float time = updtime;
    static float fps = 0;
    static float avdur = 1;
    static float avprog = 1;
    static float mcs = 1; //smooth ver
    static int mc = 1;

    static int sel = 0;
    static cell[] cells = null;

    static bool settings = false;

    static bool wu = true;

    public static void takeover() {
        Program.curUpdate = () => Rend(Program.curCanv);
        Program.current = false;

        Init();
    }

    static void Init() {
        mp = Mouse.Position;

        int width = 640, height = 360;
        Simulation.SetFixedResolution(width, height, Color.Black, false, false, false);
        map = new cell[width, height];
        maptex = Graphics.CreateTexture(width, height);

        cells = new cell[] { 
            sandycells.sand,
            sandycells.water,
            sandycells.stone
        };
    }

    public static void Rend(ICanvas canv) {
        draw(canv);
        updvars();
    }

    static void place() {
        Rectangle bounds = new(0, 0, Window.Width - 1, Window.Height - 1);

        if (Mouse.IsButtonDown(MouseButton.Left)) {
            if (bounds.ContainsPoint(mp)) {
                if (map[(int)mp.X, (int)(Window.Height - mp.Y)] == null && mc == 1) {
                    map[(int)mp.X, (int)(Window.Height - mp.Y)] = cells[sel];

                    maptex.GetPixel((int)mp.X, (int)(Window.Height - mp.Y)) = cells[sel].col;

                    maptex.ApplyChanges();

                    return;
                }
            }

            for (int x = -(int)mcs / 2; x < mcs / 2; x++) {
                for (int y = -(int)mcs / 2; y < mcs / 2; y++) {
                    if (bounds.ContainsPoint(new Vector2((int)mp.X + x, (int)(Window.Height - mp.Y) + y))) {
                        if (map[(int)mp.X + x, (int)(Window.Height - mp.Y) + y] == null) {
                            map[(int)mp.X + x, (int)(Window.Height - mp.Y) + y] = cells[sel];

                            maptex.GetPixel((int)mp.X + x, (int)(Window.Height - mp.Y) + y) = cells[sel].col;
                        }
                    }
                }
            }

            maptex.ApplyChanges();
        }

        if (Mouse.IsButtonDown(MouseButton.Middle)) {
            if(bounds.ContainsPoint(mp)) {
                if (map[(int)mp.X, (int)(Window.Height - mp.Y)] != null && mc == 1) {
                    map[(int)mp.X, (int)(Window.Height - mp.Y)] = null;

                    maptex.GetPixel((int)mp.X, (int)(Window.Height - mp.Y)) = Color.Transparent;

                    maptex.ApplyChanges();

                    return;
                }
            }

            for (int x = -(int)mcs / 2; x < mcs / 2; x++) {
                for (int y = -(int)mcs / 2; y < mcs / 2; y++) {
                    if (bounds.ContainsPoint(new Vector2((int)mp.X + x, (int)(Window.Height - mp.Y) + y))) {
                        if (map[(int)mp.X + x, (int)(Window.Height - mp.Y) + y] != null) {
                            map[(int)mp.X + x, (int)(Window.Height - mp.Y) + y] = null;

                            maptex.GetPixel((int)mp.X + x, (int)(Window.Height - mp.Y) + y) = Color.Transparent;
                        }
                    }
                }
            }

            maptex.ApplyChanges();
        }
    }

    static void updvars() {
        mp += m.twn2(mp, Mouse.Position, ms);

        if (Keyboard.IsKeyDown(Key.LeftControl)) {
            mc += (int)Mouse.ScrollWheelDelta;
            mc = (int)m.clmp(mc, 1, m.inf);
        } else {
            sel += (int)m.abs(Mouse.ScrollWheelDelta);
            sel = sel % cells.Length;
        }
        
        mcs += m.twn(mcs, Mouse.IsButtonDown(MouseButton.Left) || Mouse.IsButtonDown(MouseButton.Middle) || Keyboard.IsKeyDown(Key.LeftControl) ? mc : 1, 4);

        if (!wu) { process(); } 
        else {
            time -= Time.DeltaTime;

            if (time <= 0) {
                process();
                time = updtime;
            }
        }

        avprog -= Time.DeltaTime;

        if (avprog <= 0) {
            avprog = avdur;
            fps = m.rnd(1 / Time.DeltaTime);
        }

        place();

        if (Keyboard.IsKeyPressed(Key.C)) {
            map = new cell[Window.Width, Window.Height];

            maptex.Dispose();
            maptex = Graphics.CreateTexture(Window.Width, Window.Height);
        }

        if (Keyboard.IsKeyPressed(Key.Tab)) { settings = !settings; }

        if (settings) {
            ImGui.Begin("settings");

            ImGui.Checkbox("fixed upd", ref wu);

            ImGui.End();
        }
    }

    static void draw(ICanvas canv) {
        canv.Clear(new Color(44, 45, 53));

        canv.Scale(1, -1);
        canv.DrawTexture(maptex, new Vector2(0, 0), new Vector2(canv.Width, canv.Height), Alignment.BottomLeft);
        canv.ResetState();

        canv.Fill(Mouse.IsButtonDown(MouseButton.Left)? new Color(105, 255, 112) : Color.White);
        canv.DrawRect(m.rnd(mp.X - mcs / 2), m.rnd(mp.Y - (mcs / 2)), mcs, mcs, Alignment.TopLeft);

        canv.Fill(Color.White);
        canv.DrawText($"fps: {fps}", Vector2.One * 5, Alignment.TopLeft);
        canv.DrawText($"cell: {cells[sel].name}", new Vector2(Window.Width - 5, 5), Alignment.TopRight);
    }

    static void process() {
        bool upd = false;

        cellupd = new bool[map.GetLength(0), map.GetLength(1)];

        for (int x = 0; x < map.GetLength(0); x++) {
            for (int y = 0; y < map.GetLength(1); y++) {
                if (map[x, y] != null && cellupd[x, y] == false) {
                    if (map[x, y].move) {

                        if (y > 0) {
                            if (map[x, y - 1] == null) {
                                movecell(ref upd, 0, -1, x, y, ref cellupd, false);
                                continue;
                            }
                        }

                        if (y > 0 && x > 0) {
                            if (map[x - 1, y - 1] == null && map[x - 1, y] == null) {
                                movecell(ref upd, -1, -1, x, y, ref cellupd, false);
                                continue;
                            }
                        }

                        if (y > 0 && x < map.GetLength(0) - 1) {
                            if (map[x + 1, y - 1] == null && map[x + 1, y] == null) {
                                movecell(ref upd, 1, -1, x, y, ref cellupd, false);
                                continue;
                            }
                        }

                        if (map[x, y].liquid) {
                            int dir = m.rando(0, 2);

                            if (dir == 2) { dir = 1; }

                            if (dir == 0) {
                                if (x > 0) {
                                    if (map[x - 1, y] == null) {
                                        movecell(ref upd, -1, 0, x, y, ref cellupd, false);
                                        continue;
                                    } else { dir = 1; }
                                }
                            }

                            if (dir == 1) { 
                                if (x > map.GetLength(0) - 1) {
                                    if (map[x + 1, y] == null) {
                                        movecell(ref upd, 1, 0, x, y, ref cellupd, false);
                                        continue;
                                    } else { dir = 0; }
                                }
                            }

                            if (dir == 0) {
                                if (x > 0) {
                                    if (map[x - 1, y] == null) {
                                        movecell(ref upd, -1, 0, x, y, ref cellupd, false);
                                        continue;
                                    }
                                }
                            }

                        }

                    }
                }
            }
        }

        if (upd) { maptex.ApplyChanges(); }
    }

    static void movecell(ref bool upd, int x, int y, int curx, int cury, ref bool[,] cellupd, bool swap) { 
        cell cur = map[curx, cury];
        cell prev = map[curx + x, cury + y];
        map[curx, cury] = swap? prev : null;
        map[curx + x, cury + y] = cur;
        cellupd[curx + x, cury + y] = true;

        if(swap)
            cellupd[curx, cury] = true;

        maptex.GetPixel(curx, cury) = swap? map[curx, cury].col : Color.Transparent;
        maptex.GetPixel(curx + x, cury + y) = map[curx + x, cury + y].col;

        upd = true;
    }

    public class cell { 
        public Color col { get; set; }
        public string name { get; set; }

        //rules
        public bool move { get; set; }
        public bool liquid { get; set; }
    }
}
