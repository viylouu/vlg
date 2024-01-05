using SimulationFramework;
using SimulationFramework.Drawing;

partial class farmlight {
    static ITexture baset = null;
    static ITexture bgt = null;
    static ITexture charst = null;
    static ITexture farmt = null;
    static ITexture seedt = null;

    //static ISound theme = Audio.LoadSound(@"Assets\Farmlight\theme.wav");
    //static SoundPlayback themeplayback = null;

    

    public static void takeover() {
        Program.curUpdate = () => Rend(Program.curCanv);
        Program.current = false;

        Init();
    }

    static void Init() {
        /*themeplayback = theme.Play();

        baset = Graphics.LoadTexture(@"Assets\Farmlight\base.png");
        bgt = Graphics.LoadTexture(@"Assets\Farmlight\bg.png");
        charst = Graphics.LoadTexture(@"Assets\Farmlight\chars.png");
        farmt = Graphics.LoadTexture(@"Assets\Farmlight\farm.png");
        seedt = Graphics.LoadTexture(@"Assets\Farmlight\seed.png");*/
    }

    static void Rend(ICanvas canv) {
        canv.Clear(Color.Black);
    }
}