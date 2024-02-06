using SimulationFramework;
using SimulationFramework.Drawing;
using System.Numerics;

partial class ballbounce {
    static ITexture stufft = Graphics.LoadTexture(@"Assets\Ball Bounce\stuff.png");
    static ITexture chart = Graphics.LoadTexture(@"Assets\Ball Bounce\char.png");

    public static void takeover() {
        Program.curUpdate = () => Rend(Program.curCanv);
        Program.current = false;

        Init();
    }

    static void Init() {
        Simulation.SetFixedResolution(320, 180, Color.Black, false, false, false);
    }

    static void Rend(ICanvas canv) {
        for (int x = 0; x < canv.Width / 16; x++) {
            for (int y = 0; y < canv.Height / 16 + 1; y++) {
                canv.DrawTexture(
                    stufft,
                    new Rectangle(
                        new Vector2(16, 0),
                        Vector2.One * 16,
                        Alignment.TopLeft
                    ),
                    new Rectangle(
                        new Vector2(x * 16, y * 16),
                        Vector2.One * 16,
                        Alignment.TopLeft
                    )
                );

                if (y == canv.Height / 16) {
                    canv.DrawTexture(
                        stufft,
                        new Rectangle(
                            Vector2.Zero,
                            Vector2.One * 16,
                            Alignment.TopLeft
                        ),
                        new Rectangle(
                            new Vector2(x * 16, canv.Height),
                            Vector2.One * 16,
                            Alignment.BottomLeft
                        )
                    );
                }
            }
        }


    }
}
