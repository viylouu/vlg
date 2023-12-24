using ImGuiNET;
using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using System.Numerics;

Simulation sim = Simulation.Create(Init, Rend);
sim.Run(new DesktopPlatform());

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

    //game running logic
    public static Action curUpdate = null;
    public static ICanvas curCanv = null; 

    static void Init() {
        Audio.MasterVolume = 0.0125f;

        windowdims = new Vector2(ImGui.GetMainViewport().Size.X, ImGui.GetMainViewport().Size.Y);

        Window.Title = "vlg";
        Simulation.SetFixedResolution(1920, 1080, Color.Black, false, false, false);

        menutitley = -1080 * 2;
        menutitleyACT = -windowdims.Y * 2;

        ambientPlayback = ambience.Play();

        string content = null;
        string path = Directory.GetCurrentDirectory() + @"\Assets\Saves\userdata.json";

        using (StreamReader sr = new StreamReader(path)) { 
            content = sr.ReadToEnd();
        }

        userData readData = Newtonsoft.Json.JsonConvert.DeserializeObject<userData>(content);
        _username = readData.username;
        hasUsername = readData.hasname;

        for (int x = 0; x < 6; x++) {
            for (int y = 0; y < 6; y++) {
                cursorblank[x, y] = new Color(0, 0, 0);
            }
        }

        for (int x = 1; x < 5; x++) {
            for (int y = 1; y < 5; y++) {
                cursorblank[x, y] = new Color(255, 255, 255);
            }
        }

        cursorblank[1, 1] = new Color(0, 0, 0);
        cursorblank[1, 4] = new Color(0, 0, 0);
        cursorblank[4, 4] = new Color(0, 0, 0);
        cursorblank[4, 1] = new Color(0, 0, 0);

        cursorblank[0, 0] = new Color(0, 0, 0, 0);
        cursorblank[0, 5] = new Color(0, 0, 0, 0);
        cursorblank[5, 5] = new Color(0, 0, 0, 0);
        cursorblank[5, 0] = new Color(0, 0, 0, 0);

        Mouse.SetCursor(cursorblank, Alignment.Center);
    }

    static void Rend(ICanvas canv) {
        if (current) {
            drawBG(canv);
            drawGames(canv);
            drawOther(canv);

            audio();

            updVars();

            return;
        }

        curCanv = canv;

        curUpdate();
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
        
        if (ImGui.Button("sandy")) {
            sandy.takeover();
        }

        ImGui.SameLine();

        ImGui.End();
    }

    static void drawOther(ICanvas canv) {
        canv.Fill(Color.White);
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
                    username = _username
                };
                
                saveusername(userdata);

                Environment.Exit(0);
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
                username = ""
            };

            saveusername(userdata);

            Environment.Exit(0);
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

    static void updVars() {
        ICanvas canv = Graphics.GetOutputCanvas();

        if (hasUsername) {
            yscroll += -(int)Mouse.ScrollWheelDelta * 36;
            yscroll = (int)m.clmp(yscroll, 0, canv.Height);

            yscrollACT += -(int)(Mouse.ScrollWheelDelta * (windowdims.Y / 30));
            yscrollACT = (int)m.clmp(yscrollACT, 0, windowdims.Y);

            smoothyscroll += (yscroll - smoothyscroll) / (smooth * (1 / (Time.DeltaTime * 30)));
            smoothyscrollACT += (yscrollACT - smoothyscrollACT) / (smooth * (1 / (Time.DeltaTime * 30)));
        }

        menutitley += (canv.Height / 2f - menutitley) / (smooth * 1.25f * (1 / (Time.DeltaTime * 30)));
        menutitleyACT += (windowdims.X / 2f - menutitleyACT) / (smooth * 1.25f * (1 / (Time.DeltaTime * 30)));

        mousepos1 += (Mouse.Position - (mousepos1-new Vector2(0, smoothyscroll))) / (smooth / 2f * (1 / (Time.DeltaTime * 30)));

        windowdims = new Vector2(ImGui.GetMainViewport().Size.X, ImGui.GetMainViewport().Size.Y);
    }

    static void drawBG(ICanvas canv) {
        canv.Clear(new Color(14, 14, 15));

        Gradient bgGradU = new LinearGradient(canv.Width / 2f, -smoothyscroll, canv.Width / 2f,
            canv.Height - smoothyscroll, new Color[] { new Color(34, 35, 36), new Color(14, 14, 15) });
        Gradient bgGradB = new LinearGradient(canv.Width / 2f, -smoothyscroll + canv.Height, canv.Width / 2f,
            canv.Height - smoothyscroll + canv.Height, new Color[] { new Color(14, 14, 15), new Color(8, 8, 8) });

        canv.Fill(bgGradU);
        canv.DrawRect(new Vector2(0, -smoothyscroll), new Vector2(canv.Width, canv.Height));
        canv.Fill(bgGradB);
        canv.DrawRect(new Vector2(0, -smoothyscroll + canv.Height), new Vector2(canv.Width, canv.Height));
    }

    class userData { 
        public bool hasname { get; set; }
        public string username { get; set; }
    }

    static ITexture gaussian(ITexture image, int blurAmt) {
        ITexture img = Graphics.CreateTexture(image.Width, image.Height);

        for (int x = 0; x < image.Width; x++) {
            for (int y = 0; y < image.Height; y++) {
                img.GetPixel(x, y) = calcblurcol(image, new Vector2(x, y), blurAmt);
            }
        }

        image.Dispose();
        img.ApplyChanges();
        return img;
    }

    static Color calcblurcol(ITexture image, Vector2 pos, int blurAmt) {
        int totalR = 0, totalG = 0, totalB = 0, count = 0;

        for (int i = -blurAmt; i <= blurAmt; i++) {
            for (int j = -blurAmt; j <= blurAmt; j++) {
                int newX = (int)m.max(0, m.min(image.Width - 1, (int)pos.X + i));
                int newY = (int)m.max(0, m.min(image.Height - 1, (int)pos.Y + j));

                totalR += image.GetPixel(newX, newY).R;
                totalG += image.GetPixel(newX, newY).G;
                totalB += image.GetPixel(newX, newY).B;

                count++;
            }
        }

        return new Color(totalR / count, totalG / count, totalB / count);
    }
}