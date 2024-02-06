﻿using SimulationFramework;
using SimulationFramework.Drawing;
using System.IO;
using System.Numerics;

partial class ruok {
    static chr[] chars = Array.Empty<chr>();
    static weap[] weaps = Array.Empty<weap>();

    static ITexture tex = Graphics.LoadTexture(@"Assets\Ruok\tiles.png");

    public static void takeover() {
        Program.curUpdate = () => Rend(Program.curCanv);
        Program.current = false;

        Init();
    }

    static void Init() {
        Simulation.SetFixedResolution(640, 360, Color.Black, false, false, false);

        weaps = new weap[Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Assets\Ruok\weaps\", "*.json").Length];

        string[] weaponFilePaths = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Assets\Ruok\weaps\", "*.json");
        for (int w = 0; w < weaps.Length; w++)
        {
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
        for (int c = 0; c < chars.Length; c++)
        {
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

        canv.DrawTexture(
            tex,
            new Rectangle(
                chars[0].tlpos,
                new Vector2((chars[0].brpos.X - chars[0].tlpos.X) / 9, chars[0].brpos.Y - chars[0].tlpos.Y),
                Alignment.TopLeft
            ),
            new Rectangle(
                new Vector2(canv.Width / 2, canv.Height / 2),
                new Vector2((chars[0].brpos.X - chars[0].tlpos.X) / 9, chars[0].brpos.Y - chars[0].tlpos.Y),
                Alignment.Center
            )
        );
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