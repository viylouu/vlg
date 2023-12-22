using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using System.Numerics;
using System.Threading;
using vlg;

Simulation sim = Simulation.Create(Init, Rend);
sim.Run(new DesktopPlatform());

partial class Program {
    static int yscroll = 0;
    static float smoothyscroll = 0;
    static readonly float smooth = 5;

    static bool debugDrawing = false;

    static ITexture[] gameimages = new ITexture[] { Graphics.LoadTexture(@"Assets\Menu\sandy.png") };
    static readonly float tilesmooth = 15;

    static void Init() {
        Window.Title = "vlg";
        Simulation.SetFixedResolution(1920, 1080, Color.Black, false, false, false);

        if (debugDrawing)
        { debug.good("finished init"); }
        else
        { debug.msg("debugging disabled"); }
    }

    static void Rend(ICanvas canv) {
        drawBG(canv);
        drawGames(canv);

        updVars();
    }

    static void drawGames(ICanvas canv) {
        if (debugDrawing)
        { debug.log("rendering game image 0"); }

        canv.Translate(new Vector2(canv.Width / 2f - 128, canv.Height + canv.Height / 2f - smoothyscroll - 128));
        canv.Fill(gameimages[0]);
        canv.DrawRoundedRect(new Vector2(128, 128), Vector2.One * 256, 10, Alignment.Center);
        canv.ResetState();

        if (debugDrawing)
        { debug.good("rendered game image 0"); }
    }

    static void updVars() {
        ICanvas canv = Graphics.GetOutputCanvas();

        if (debugDrawing) {
            debug.log("updating y scroll");
        }

        yscroll += -(int)Mouse.ScrollWheelDelta * 36;
        yscroll = Math.Clamp(yscroll, 0, canv.Height);

        if (debugDrawing) {
            debug.good("updated y scroll");
            debug.log("smoothing y scroll");
        }

        smoothyscroll += (yscroll - smoothyscroll) / (smooth * (1 / (Time.DeltaTime * 30)));

        if (debugDrawing) {
            debug.good("smoothed y scroll");
        }

        debug.frame++;
    }

    static void drawBG(ICanvas canv) {
        canv.Clear(new Color(14, 14, 15));

        if (debugDrawing) {
            debug.log("making gradients");
        }

        Gradient bgGradU = new LinearGradient(canv.Width / 2f, -smoothyscroll, canv.Width / 2f,
            canv.Height - smoothyscroll, new Color[] { new Color(34, 35, 36), new Color(14, 14, 15) });
        Gradient bgGradB = new LinearGradient(canv.Width / 2f, -smoothyscroll + canv.Height, canv.Width / 2f,
            canv.Height - smoothyscroll + canv.Height, new Color[] { new Color(14, 14, 15), new Color(8, 8, 8) });

        if (debugDrawing) {
            debug.good("made gradients");
            debug.log("drawing bg");
        }

        canv.Fill(bgGradU);
        canv.DrawRect(new Vector2(0, -smoothyscroll), new Vector2(canv.Width, canv.Height));
        canv.Fill(bgGradB);
        canv.DrawRect(new Vector2(0, -smoothyscroll + canv.Height), new Vector2(canv.Width, canv.Height));

        if (debugDrawing) {
            debug.good("finished drawing bg");
        }
    }
}