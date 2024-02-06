﻿using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using System.IO;
using System.Numerics;

partial class ruok {
    static chr[] chars = Array.Empty<chr>();
    static weap[] weaps = Array.Empty<weap>();

    static ITexture tex = Graphics.LoadTexture(@"Assets\Ruok\tiles.png");

    static float wdir = 0;
    static float wdap = 0;
    static bool wd = false;

    static Vector2 charposWP;
    static Vector2 charposSP;
    static Vector2 campos;

    public static void takeover() {
        Program.curUpdate = () => Rend(Program.curCanv);
        Program.current = false;

        Init();
    }

    static void Init() {
        Simulation.SetFixedResolution(640, 360, Color.Black, false, false, false);

        weaps = new weap[Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Assets\Ruok\weaps\", "*.json").Length];

        string[] weaponFilePaths = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Assets\Ruok\weaps\", "*.json");
        for (int w = 0; w < weaps.Length; w++) {
            string content = null;

            string name = Path.GetFileNameWithoutExtension(weaponFilePaths[w]);

            using (StreamReader sr = new StreamReader(weaponFilePaths[w]))
                content = sr.ReadToEnd();

            weap datagot = Newtonsoft.Json.JsonConvert.DeserializeObject<weap>(content);
            weaps[w] = datagot;

            cons.dbg.log("LOADED WEAP: " + name);
        }

        chars = new chr[Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Assets\Ruok\chars\", "*.json").Length];

        string[] characterFilePaths = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Assets\Ruok\chars\", "*.json");
        for (int c = 0; c < chars.Length; c++) {
            string content = null;

            string name = Path.GetFileNameWithoutExtension(characterFilePaths[c]);

            using (StreamReader sr = new StreamReader(characterFilePaths[c]))
                content = sr.ReadToEnd();

            chr datagot = Newtonsoft.Json.JsonConvert.DeserializeObject<chr>(content);
            chars[c] = datagot;

            cons.dbg.log("LOADED CHAR: " + name);
        }
    }

    public static void Rend(ICanvas canv) {
        canv.Clear(Color.Black);

        charposWP = new Vector2(canv.Width / 2, canv.Height / 2);
        charposSP = charposWP - campos + new Vector2(canv.Width / 2, canv.Height / 2);

        campos += m.twn2(campos, charposWP, 5);

        drawplayer(canv, chars[0], charposSP, false);

        drawweapon(canv, weaps[0], new Vector2(charposSP.X + m.sin(m.deg2rad(wdir + m.rad2deg(m.atan2(Mouse.Position.Y - charposSP.Y, Mouse.Position.X - charposSP.X)))) * 14, charposSP.Y - m.cos(m.deg2rad(wdir + m.rad2deg(m.atan2(Mouse.Position.Y - charposSP.Y, Mouse.Position.X - charposSP.X)))) * 14), wdir * 2 - 90 + m.rad2deg(m.atan2(Mouse.Position.Y - charposSP.Y, Mouse.Position.X - charposSP.X)));

        if (Mouse.IsButtonPressed(MouseButton.Left)) {
            wd = !wd;
            wdap = 0;
        }

        wdap += Time.DeltaTime * 15;

        if (wdap >= 1) { wdap = 1; }

        wdir += m.twn(wdir, ease.oback(wdap) * 90 - (wd? -90 : 90), 1.2f);
    }




    static void drawplayer(ICanvas canv, chr data, Vector2 pos, bool l) {
        canv.DrawTexture(
            tex,
            new Rectangle(
                data.tlpos,
                new Vector2((data.brpos.X - data.tlpos.X) / 9, data.brpos.Y - data.tlpos.Y),
                Alignment.TopLeft
            ),
            new Rectangle(
                pos,
                new Vector2((data.brpos.X - data.tlpos.X) / 9, data.brpos.Y - data.tlpos.Y),
                Alignment.Center
            )
        );
    }

    static void drawweapon(ICanvas canv, weap data, Vector2 pos, float dir) {
        canv.Translate( pos.X, pos.Y );
        canv.Rotate(m.deg2rad(dir));
        canv.Translate(0, -(data.brpos.Y - data.tlpos.Y) / 2);

        canv.DrawTexture(
            tex,
            new Rectangle(
                data.tlpos,
                new Vector2((data.brpos.X - data.tlpos.X), data.brpos.Y - data.tlpos.Y),
                Alignment.TopLeft
            ),
            new Rectangle(
                Vector2.Zero,
                new Vector2((data.brpos.X - data.tlpos.X), data.brpos.Y - data.tlpos.Y),
                Alignment.Center
            )
        );

        canv.ResetState();
    }






    enum atktype {
        melee,
        magic,
        ranged,
        all
    }

    class chr { 
        public Vector2 tlpos { get; set; }
        public Vector2 brpos { get; set; }

        public float fat { get; set; }
        public atktype type { get; set; }
    }

    class weap {
        public Vector2 tlpos { get; set; }
        public Vector2 brpos { get; set; }

        public float wei { get; set; }
        public atktype type { get; set; }

        public ushort dmg { get; set; }
        public ushort hp { get; set; }
    }
}