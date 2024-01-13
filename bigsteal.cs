using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using System.IO;
using System.Numerics;
using System.Text.Json.Serialization;

partial class bigsteal {
    static cardata[] cardatas = null;
    static car[] cars = null;

    static Vector2 camp = Vector2.Zero;

    static string[,] map = new string[,] {
        { "r","r","r" },
        { "r","f","r" },
        { "r","r","r" }
    };

    static ITexture maptex = null;

    static ITexture tiles = Graphics.LoadTexture(@"Assets\Big Steal\stuff\tilemap.png");

    public static void takeover() {
        Program.curUpdate = () => Rend(Program.curCanv);
        Program.current = false;

        Init();
    }

    static void Init() {
        cardatas = new cardata[Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Assets\Big Steal\", "*.png").Length];
        cars = new car[cardatas.Length];

        for (int i = 0; i < cardatas.Length; i++) {
            cardatas[i] = new cardata();
            cars[i] = new car();

            cardatas[i].name = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Assets\Big Steal\", "*.png")[i];
            cardatas[i].name = cardatas[i].name.Remove(0, (Directory.GetCurrentDirectory() + @"\Assets\Big Steal\").Length);

            cardatas[i].tex = Graphics.LoadTexture(@"Assets\Big Steal\" + cardatas[i].name);

            cardatas[i].name = cardatas[i].name.Remove(cardatas[i].name.Length - 4, 4);

            cars[i].type = cardatas[i];

            string path = Directory.GetCurrentDirectory() + @$"\Assets\Big Steal\{cardatas[i].name + " data"}.json";

            if(Path.Exists(path)) {
                string content = null;

                using (StreamReader sr = new StreamReader(path))
                    content = sr.ReadToEnd();

                jsondata datagot = Newtonsoft.Json.JsonConvert.DeserializeObject<jsondata>(content);
                cardatas[i].data = datagot;
            } else {
                jsondata dataser = new jsondata();

                string content = Newtonsoft.Json.JsonConvert.SerializeObject(dataser);

                using (StreamWriter sw = new StreamWriter(path))
                    sw.Write(content);

                cardatas[i].data = dataser;
            }

            //cars[i].idlesound = Audio.LoadSound(@"Assets\Big Steal\enginesound.wav");
            //cars[i].idlesoundpb = cars[i].idlesound.Play();

            cons.dbg.log("LOADED CAR: " + cardatas[i].name);
        }

        loadmap();

        Simulation.SetFixedResolution(640, 360, Color.Black, false, false, false);
    }

    static void Rend(ICanvas canv) {
        canv.Clear(Color.Black);

        if (Mouse.IsButtonDown(MouseButton.Left)) 
            camp -= Mouse.DeltaPosition;

        //canv.DrawTexture(maptex, -camp, new Vector2(maptex.Width, maptex.Height));

        for (int i = 0; i < cars.Length; i++) {
            for (int l = 0; l < cars[i].type.tex.Width / 16; l++) {
                canv.Translate(new Vector2(i * 32, canv.Height / 2 - l) - camp);
                canv.Rotate(Angle.ToRadians(cars[i].rot));

                canv.DrawTexture(
                    cars[i].type.tex,
                    new Rectangle(
                        new Vector2(l * 16, 0),
                        Vector2.One * 16,
                        Alignment.TopLeft
                    ),
                    new Rectangle( 
                        Vector2.Zero,
                        Vector2.One * 16,
                        Alignment.Center
                    )
                );

                canv.ResetState();
            }

            cars[i].rot += Time.DeltaTime * 30;
            cars[i].rot = cars[i].rot % 360;

            //if (cars[i].idlesoundpb.IsStopped) {
            //    cars[i].idlesoundpb = cars[i].idlesound.Play();
            //}
        }
    }

    static void loadmap() {
        for (int x = 0; x < map.GetLength(0); x++) {
            for (int y = 0; y < map.GetLength(1); y++) {
                if (map[x, y] == "r")
                { }
                    
            }
        }
    }

    static void puttileontex(Rectangle source) { 
        
    }

    class jsondata { 
        public float maxspeed { get; set; }
        public float turnspeed { get; set; }
        public float friction { get; set; }
        public float accel { get; set; }
        public float deccel { get; set; }
        public float turnaccel { get; set; }
        public float turndeccel { get; set; }
        public float armor { get; set; }
        public int cost { get; set; }
    }

    class cardata { 
        public string name { get; set; }
        public ITexture tex { get; set; }

        public jsondata data { get; set; }
    }

    class car { 
        public cardata type { get; set; }
        public float rot { get; set; }
        public ISound idlesound { get; set; }
        public SoundPlayback idlesoundpb { get; set; }
    }
}