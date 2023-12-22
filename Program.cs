using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using System.Numerics;
using vlg;

Simulation sim = Simulation.Create(Init, Rend);
sim.Run(new DesktopPlatform());

partial class Program {
    static int yscroll = 0;
    static float smoothyscroll = 0;
    static readonly float smooth = 5;

    static ITexture[] gameimages = new ITexture[] { Graphics.LoadTexture(@"Assets\Menu\sandy.png") };
    static readonly float tilesmooth = 15;

    static ISound ambience = Audio.LoadSound(@"Assets\Menu\ambience.wav");
    static SoundPlayback ambientPlayback = null;

    static ISound click = Audio.LoadSound(@"Assets\Menu\click.wav");
    static ISound scroll = Audio.LoadSound(@"Assets\Menu\scroll.wav");
    static ISound hover = Audio.LoadSound(@"Assets\Menu\hover.wav");

    static int stscroll = 0;

    static float menutitley = 0;

    static void Init() {
        Audio.MasterVolume = 0.0125f;

        Window.Title = "vlg";
        Simulation.SetFixedResolution(1920, 1080, Color.Black, false, false, false);

        menutitley = -1080 * 2;

        ambientPlayback = ambience.Play();
    }

    static void Rend(ICanvas canv) {
        drawBG(canv);
        drawGames(canv);

        audio();

        updVars();
    }

    static void audio() {
        if (ambientPlayback.IsStopped) 
        { ambientPlayback = ambience.Play(); }

        if (stscroll != (int)Mouse.ScrollWheelDelta) { 
            stscroll = (int)Mouse.ScrollWheelDelta;
            scroll.Play();
        }

        if (Mouse.IsButtonPressed(MouseButton.Left) || Mouse.IsButtonPressed(MouseButton.Right) || Mouse.IsButtonPressed(MouseButton.Middle)) 
        { click.Play(); }
    }

    static void drawGames(ICanvas canv) {
        canv.Translate(new Vector2(canv.Width / 2f - 128, canv.Height + canv.Height / 2f - smoothyscroll - 128));
        canv.Fill(gameimages[0]);
        canv.DrawRoundedRect(new Vector2(128, 128), Vector2.One * 256, 10, Alignment.Center);
        canv.ResetState();
    }

    static void updVars() {
        ICanvas canv = Graphics.GetOutputCanvas();

        yscroll += -(int)Mouse.ScrollWheelDelta * 36;
        yscroll = Math.Clamp(yscroll, 0, canv.Height);

        smoothyscroll += (yscroll - smoothyscroll) / (smooth * (1 / (Time.DeltaTime * 30)));
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
}