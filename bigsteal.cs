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

            string content = null;

            string path = Directory.GetCurrentDirectory() + @$"\Assets\Big Steal\{cardatas[i].name + " data"}.json";

            using (StreamReader sr = new StreamReader(path)) {
                content = sr.ReadToEnd();
            }

            jsondata datagot = Newtonsoft.Json.JsonConvert.DeserializeObject<jsondata>(content);
            cardatas[i].data = datagot;

            cons.dbg.log("LOADED CAR: " + cardatas[i].name);
        }

        jsondata data = new jsondata();
        data.maxspeed = 0;
        data.friction = 0;
        data.accel = 0;
        data.deccel = 0;
        data.maxhp = 0;

        Simulation.SetFixedResolution(640, 360, Color.Black, false, false, false);
    }

    static void Rend(ICanvas canv) {
        canv.Clear(Color.Black);

        if (Mouse.IsButtonDown(MouseButton.Left)) { camp -= Mouse.DeltaPosition; }

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
        }
    }

    class jsondata { 
        public float maxspeed { get; set; }
        public float friction { get; set; }
        public float accel { get; set; }
        public float deccel { get; set; }
        public int maxhp { get; set; }
    }

    class cardata { 
        public string name { get; set; }
        public ITexture tex { get; set; }

        public jsondata data { get; set; }
    }

    class car { 
        public cardata type { get; set; }
        public float rot { get; set; }
    }
}