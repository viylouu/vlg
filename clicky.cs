/*

    INFO

    this game was made in 1 hr as requested by Lucas-Code
    it was not very hard to make and the main gameplay
    only took around 30 mins, so half of the time was
    spent on polishing. so yeah!

*/

using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using System.Numerics;
using System.Xml.Linq;

partial class clicky {
    static int score = 0;
    static float btnscl = 120;
    static float upgrbtnscl = 240;
    static float upgrcost = 15;
    static float upgrcostmult = 1.25f;
    static int scrmult = 1;

    static Color white = new Color(255, 247, 255);
    static Color black = new Color(27, 17, 44);

    //static ISound click = Audio.LoadSound(@"Assets\Clicky\click.wav");

    //static ISound song = Audio.LoadSound(@"Assets\Clicky\song.wav");
    //static SoundPlayback songplayback = null;

    public static void takeover() {
        Program.curUpdate = () => Rend(Program.curCanv);
        Program.current = false;

        Init();
    }

    static void Init() {
        Simulation.SetFixedResolution(1920, 1080, Color.Black, false, false, false);

        //songplayback = song.Play();
    }

    static void Rend(ICanvas canv) {
        canv.Clear(black);

        canv.Fill(white);
        canv.DrawCircle(canv.Width / 2, canv.Height / 2, btnscl, Alignment.Center);

        canv.FontSize(50);
        canv.DrawText(score + "", canv.Width / 2, 5, Alignment.TopCenter);

        canv.Fill(black);
        canv.FontSize(btnscl / 1.5f);
        canv.DrawText("click", canv.Width / 2, canv.Height / 2, Alignment.Center);

        if (m.dist2(Mouse.Position, new Vector2(canv.Width / 2, canv.Height / 2)) < 120 && Mouse.IsButtonReleased(MouseButton.Left))
        { score += scrmult; /*click.Play();*/ }
        else if (m.dist2(Mouse.Position, new Vector2(canv.Width / 2, canv.Height / 2)) < 120 && Mouse.IsButtonDown(MouseButton.Left))
            btnscl += m.twn(btnscl, 90, 5);
        else
            btnscl += m.twn(btnscl, 120, 5);

        canv.Fill(white);
        canv.DrawRoundedRect(new Vector2(canv.Width / 2, canv.Height / 2 + 200), new Vector2(upgrbtnscl, upgrbtnscl / 2), 25, Alignment.Center);

        canv.Fill(black);
        canv.FontSize(upgrbtnscl / 8);
        canv.DrawText("upgrade for " + upgrcost, canv.Width / 2, canv.Height / 2 + 200, Alignment.Center);

        Rectangle upgrbtn = new Rectangle(new Vector2(canv.Width / 2, canv.Height / 2 + 200), new Vector2(upgrbtnscl, upgrbtnscl / 2), Alignment.Center);

        if (upgrbtn.ContainsPoint(Mouse.Position) && Mouse.IsButtonReleased(MouseButton.Left)) {
            if (score >= upgrcost) {
                scrmult++;
                score -= (int)upgrcost;
                upgrcost *= upgrcostmult;
                upgrcost = m.rnd(upgrcost);
                //click.Play();
            }
        }
        else if (upgrbtn.ContainsPoint(Mouse.Position) && Mouse.IsButtonDown(MouseButton.Left))
            upgrbtnscl += m.twn(upgrbtnscl, 225, 5);
        else
            upgrbtnscl += m.twn(upgrbtnscl, 240, 5);

        //if (songplayback.IsStopped)
        //    songplayback = song.Play();

        if (Keyboard.IsKeyPressed(Key.S))
            save();
        if (Keyboard.IsKeyPressed(Key.L))
            load();
    }

    static void save() {
        data dat = new data();
        dat.upgrcost = upgrcost;
        dat.score = score;
        dat.scrmult = scrmult;

        string path = Directory.GetCurrentDirectory() + @$"\Assets\Clicky\savedata.json";

        string content = Newtonsoft.Json.JsonConvert.SerializeObject(dat);

        using (StreamWriter sw = new StreamWriter(path)) { 
            sw.Write(content);
        }
    }

    static void load() {
        string path = Directory.GetCurrentDirectory() + @$"\Assets\Clicky\savedata.json";

        if (Path.Exists(path)) {
            string content = null;

            using (StreamReader sr = new StreamReader(path)) { 
                content = sr.ReadToEnd();
            }

            data dat = Newtonsoft.Json.JsonConvert.DeserializeObject<data>(content);
            upgrcost = dat.upgrcost;
            scrmult = dat.scrmult;
            score = dat.score;
        }
    }

    class data { 
        public int score { get; set; }
        public float upgrcost { get; set; }
        public int scrmult { get; set; }
    }
}