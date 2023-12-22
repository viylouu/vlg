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
    static float smooth = 5;

    static void Init() {
        Window.Title = "vlg";
        Simulation.SetFixedResolution(1920, 1080, Color.Black, false, false, false);
    }

    static void Rend(ICanvas canv) {
        canv.Clear(new Color(14, 14, 15));

        Gradient bggradl = new LinearGradient(canv.Width / 2, -smoothyscroll, canv.Width / 2, canv.Height - smoothyscroll, new Color[] { new Color(34, 35, 36), new Color(14, 14, 15) });
        Gradient bggradd = new LinearGradient(canv.Width / 2, -smoothyscroll + canv.Height, canv.Width / 2, canv.Height - smoothyscroll + canv.Height, new Color[] { new Color(14, 14, 15), new Color(8, 8, 8) });

        canv.Fill(bggradl); //drawbg
        canv.DrawRect(new Vector2(0, -smoothyscroll), new Vector2(canv.Width, canv.Height));
        canv.Fill(bggradd);
        canv.DrawRect(new Vector2(0, -smoothyscroll + canv.Height), new Vector2(canv.Width, canv.Height));

        yscroll += -(int)Mouse.ScrollWheelDelta * 36; //apply y scroll
        yscroll = Math.Clamp(yscroll, 0, canv.Height);

        smoothyscroll += (yscroll - smoothyscroll) / (smooth * (1 / (Time.DeltaTime * 30))); //smooth y scroll and apply it to variable
    }

    static ITexture gaussianblur(ITexture image, int blurSize) {
        ITexture blurred = Graphics.CreateTexture(image.Width, image.Height);

        for (int xx = 0; xx < image.Width; xx++) {
            for (int yy = 0; yy < image.Height; yy++) {
                float avgR = 0, avgG = 0, avgB = 0, avgA = 0;
                int blurPixelCount = 0;

                for (int x = xx; (x < xx + blurSize && x < image.Width) ; x++) {
                    for (int y = yy; (y < yy + blurSize && y < image.Height) ; y++) {
                        Color pixel = image.GetPixel(x, y);

                        avgR += pixel.R; avgG += pixel.G; avgB += pixel.B; avgA += pixel.A;

                        blurPixelCount++;
                    }
                }

                avgR = avgR / blurPixelCount;
                avgG = avgG / blurPixelCount;
                avgB = avgB / blurPixelCount;
                avgA = avgA / blurPixelCount;

                for (int x = xx; x < xx + blurSize && x < image.Width; x++) {
                    for (int y = yy; y < yy + blurSize && y < image.Height; y++) {
                        blurred.GetPixel(x, y) = new Color(avgR, avgG, avgB, avgA);
                    }
                }
            }
        }

        blurred.ApplyChanges();
        return blurred;
    }
}