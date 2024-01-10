using ImGuiNET;
using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using System.Numerics;

try {
    Simulation sim = Simulation.Create(Init, Rend);
    sim.Run(new DesktopPlatform());
}
catch (Exception ex) {
    Console.WriteLine(ex.Message);
    Console.ReadKey();
}

partial class Program {
    static int yscroll = 0;
    static float smoothyscroll = 0;
    static int yscrollACT = 0;
    static float smoothyscrollACT = 0;
    static readonly float smooth = 5;

    static ISound ambience = Audio.LoadSound(@"Assets\Menu\ambience.wav");
    static SoundPlayback ambientPlayback = null;

    static ISound click = Audio.LoadSound(@"Assets\Menu\click.wav");
    static ISound scroll = Audio.LoadSound(@"Assets\Menu\scroll.wav");

    static int stscroll = 0;

    static float menutitley = 0;
    static float menutitleyACT = 0;

    static bool hasUsername = false;

    static Vector2 windowdims = Vector2.One;

    static string _username = "";

    static Color[,] cursorblank = new Color[6, 6];

    static Vector2 mousepos1 = Vector2.Zero;

    public static bool current = true;

    static Color blac = new Color(27, 17, 44);
    static Color whit = new Color(255, 247, 255);

    static float _vol = 1;

    static game[] games = new game[] {
        new game() { dn = "sandy", to = sandy.takeover },
        new game() { dn = "farmlight", to = farmlight.takeover },
        new game() { dn = "big steal", to = bigsteal.takeover }
    };

    //game running logic
    public static Action curUpdate = null;
    public static ICanvas curCanv = null;

    static void load() {
        windowdims = new Vector2(ImGui.GetMainViewport().Size.X, ImGui.GetMainViewport().Size.Y);

        Window.Title = "vlg";
        Simulation.SetFixedResolution(1920, 1080, Color.Black, false, false, false);

        menutitley = -1080 * 2;
        menutitleyACT = -windowdims.Y * 2;

        loaddata();

        Audio.MasterVolume = _vol;

        makecursor();

        ambientPlayback = ambience.Play();
    }

    static void loaddata() { 
        string content = null;
        string path = Directory.GetCurrentDirectory() + @"\Assets\Saves\userdata.json";

        using (StreamReader sr = new StreamReader(path)) {
            content = sr.ReadToEnd();
        }

        userData readData = Newtonsoft.Json.JsonConvert.DeserializeObject<userData>(content);
        _username = readData.username;
        hasUsername = readData.hasname;
        _vol = readData.vol;
    }

    static void makecursor() { 
        for (int x = 0; x < 6; x++) {
            for (int y = 0; y < 6; y++) {
                cursorblank[x, y] = blac;
            }
        }

        for (int x = 1; x < 5; x++) {
            for (int y = 1; y < 5; y++) {
                cursorblank[x, y] = whit;
            }
        }

        cursorblank[1, 1] = blac;
        cursorblank[1, 4] = blac;
        cursorblank[4, 4] = blac;
        cursorblank[4, 1] = blac;

        cursorblank[0, 0] = Color.Transparent;
        cursorblank[0, 5] = Color.Transparent;
        cursorblank[5, 5] = Color.Transparent;
        cursorblank[5, 0] = Color.Transparent;

        Mouse.SetCursor(cursorblank, Alignment.Center);
    }

    static void Init() { load(); }

    static void Rend(ICanvas canv) {
        cons.dbg.now = DateTime.Now;
        cons.dbg.frame += 1;

        if (current) {
            draw(canv);
            audio();
            vars();

            return;
        }

        curCanv = canv;

        curUpdate();
    }

    static void draw(ICanvas canv) {
        drawBG(canv);
        drawGames(canv);
        drawOther(canv);
    }

    static void audio() {
        if (ambientPlayback.IsStopped) 
        { ambientPlayback = ambience.Play(); }

        if (stscroll != (int)Mouse.ScrollWheelDelta && hasUsername) { 
            stscroll = (int)Mouse.ScrollWheelDelta;
            scroll.Play();
        }

        if (Keyboard.PressedKeys.Any() || Mouse.IsButtonPressed(MouseButton.Left) || Mouse.IsButtonPressed(MouseButton.Right) || Mouse.IsButtonPressed(MouseButton.Middle)) 
        { click.Play(); }
    }

    static void drawGames(ICanvas canv) {
        ImGui.Begin("games");

        ImGui.SetWindowSize(new Vector2(336, 140));
        ImGui.SetWindowPos(new Vector2(windowdims.X / 2 - 336 / 2f, windowdims.Y * 1.45f - smoothyscrollACT));

        for (int i = 0; i < games.Length; i++) {
            if (i % 5 != 0)
                ImGui.SameLine();
            if (ImGui.Button(games[i].dn))
                games[i].to();
        }

        ImGui.SameLine();

        ImGui.End();
    }

    static void drawOther(ICanvas canv) {
        canv.Fill(whit);
        canv.FontSize(24);
        canv.DrawText(!hasUsername? "welcome to vlg" : $"welcome back {_username}", new Vector2(canv.Width / 2, menutitley - smoothyscroll), Alignment.Center);

        //cursor
        canv.DrawCircle(new Vector2(mousepos1.X, mousepos1.Y - smoothyscroll), 8, Alignment.Center);

        if (!hasUsername) {
            ImGui.Begin("menu");

            ImGui.SetWindowSize(new Vector2(336, 140));
            ImGui.SetWindowPos(new Vector2(windowdims.X / 2 - 336 / 2f, (menutitleyACT - windowdims.Y / 2 + 120) - smoothyscrollACT));

            ImGui.Text("insert username");
            ImGui.InputText("username", ref _username, 30);

            if (ImGui.Button("apply") && _username != "") {
                userData userdata = new userData() { 
                    hasname = true,
                    username = _username,
                    vol = _vol
                };
                
                saveusername(userdata);

                Environment.Exit(0);
            }

            float __vol2 = _vol;

            ImGui.SliderFloat("volume", ref _vol, 0, 1);

            if (__vol2 != _vol)
            { Audio.MasterVolume = _vol; }

            if (ImGui.Button("save")) {
                userData userdata = new userData() {
                    hasname = true,
                    username = _username,
                    vol = _vol
                };

                saveusername(userdata);
            }

            ImGui.End();

            return;
        }

        ImGui.Begin("menu");

        ImGui.SetWindowSize(new Vector2(336, 140));
        ImGui.SetWindowPos(new Vector2(windowdims.X / 2 - 336 / 2f, (menutitleyACT - windowdims.Y / 2 + 120) - smoothyscrollACT));
        
        if (ImGui.Button("clear username")) {
            userData userdata = new userData() {
                hasname = false,
                username = "",
                vol = _vol
            };

            saveusername(userdata);

            Environment.Exit(0);
        }

        float __vol = _vol;

        ImGui.SliderFloat("volume", ref _vol, 0, 1);

        if (__vol != _vol)
        { Audio.MasterVolume = _vol; }

        if (ImGui.Button("save")) {
            userData userdata = new userData() {
                hasname = true,
                username = _username,
                vol = _vol
            };

            saveusername(userdata);
        }

        ImGui.End();
    }

    static void saveusername(userData data) {
        var serObj = Newtonsoft.Json.JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);

        string path = Directory.GetCurrentDirectory() + @"\Assets\Saves\userdata.json";

        using (StreamWriter sw = new StreamWriter(path)) {
            sw.Write(serObj);
        }
    }

    static void vars() {
        if (hasUsername) {
            yscroll += -(int)Mouse.ScrollWheelDelta * 36;
            yscroll = (int)m.clmp(yscroll, 0, Window.Height);

            yscrollACT += -(int)(Mouse.ScrollWheelDelta * (windowdims.Y / 30));
            yscrollACT = (int)m.clmp(yscrollACT, 0, windowdims.Y);

            smoothyscroll += (yscroll - smoothyscroll) / (smooth * (1 / (Time.DeltaTime * 30)));
            smoothyscrollACT += (yscrollACT - smoothyscrollACT) / (smooth * (1 / (Time.DeltaTime * 30)));
        }

        menutitley += (Window.Height / 2f - menutitley) / (smooth * 1.25f * (1 / (Time.DeltaTime * 30)));
        menutitleyACT += (windowdims.X / 2f - menutitleyACT) / (smooth * 1.25f * (1 / (Time.DeltaTime * 30)));

        mousepos1 += (Mouse.Position - (mousepos1-new Vector2(0, smoothyscroll))) / (smooth / 2f * (1 / (Time.DeltaTime * 30)));

        windowdims = new Vector2(ImGui.GetMainViewport().Size.X, ImGui.GetMainViewport().Size.Y);
    }

    static void drawBG(ICanvas canv) {
        canv.Clear(new Color(14, 14, 15));

        Gradient bgGradU = new LinearGradient(canv.Width / 2f, -smoothyscroll, canv.Width / 2f,
            canv.Height - smoothyscroll, new Color[] { new Color(84, 62, 84), new Color(65, 48, 71) });
        Gradient bgGradB = new LinearGradient(canv.Width / 2f, -smoothyscroll + canv.Height, canv.Width / 2f,
            canv.Height - smoothyscroll + canv.Height, new Color[] { new Color(65, 48, 71), blac });

        canv.Fill(bgGradU);
        canv.DrawRect(new Vector2(0, -smoothyscroll), new Vector2(canv.Width, canv.Height));
        canv.Fill(bgGradB);
        canv.DrawRect(new Vector2(0, -smoothyscroll + canv.Height), new Vector2(canv.Width, canv.Height));
    }

    class userData { 
        public bool hasname { get; set; }
        public string username { get; set; }
        public float vol { get; set; }
    }

    class game { 
        public string dn { get; set; }
        public Action to { get; set; }
    }
}