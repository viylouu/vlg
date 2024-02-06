//i made this cuz i was bored so i implemented the math functions from the silly language
using System.Numerics;
using SimulationFramework;

partial class m {
    public static float abs(float inp) { return MathF.Abs(inp); }
    public static float acos(float inp) { return MathF.Acos(inp); }
    public static float acosh(float inp) { return MathF.Acosh(inp); }
    public static float asin(float inp) { return MathF.Asin(inp); }
    public static float asinh(float inp) { return MathF.Asinh(inp); }
    public static float atan(float inp) { return MathF.Atan(inp); }
    public static float atanh(float inp) { return MathF.Atanh(inp); }
    public static float atan2(float inp1, float inp2) { return MathF.Atan2(inp1, inp2); }
    public static float ceil(float inp) { return MathF.Ceiling(inp); }
    public static float cos(float inp) { return MathF.Cos(inp); }
    public static float clmp(float inp, float min, float max) { return Math.Clamp(inp, min, max); }
    public static float exp(float inp) { return MathF.Exp(inp); }
    public static float flr(float inp) { return MathF.Floor(inp); }
    public static float log(float inp) { return MathF.Log(inp); }
    public static float lgt(float inp) { return MathF.Log10(inp); }
    public static float max(float inp1, float inp2) { return MathF.Max(inp1, inp2); }
    public static float min(float inp1, float inp2) { return MathF.Min(inp1, inp2); }
    public static float pwr(float inp, float pow) { return MathF.Pow(inp, pow); }
    public static float rnd(float inp) { return MathF.Round(inp); }
    public static float sin(float inp) { return MathF.Sin(inp); }
    public static float sqrt(float inp) { return MathF.Sqrt(inp); }
    public static float cbrt(float inp) { return MathF.Cbrt(inp); }
    public static float tan(float inp) { return MathF.Tan(inp); }
    public static float sinh(float inp) { return MathF.Sinh(inp); }
    public static float tanh(float inp) { return MathF.Tanh(inp); }
    public static float sqr(float inp) { return inp * inp; }
    public static float cbe(float inp) { return inp * inp * inp; }
    public static float qrt(float inp) { return inp * inp * inp * inp; }
    public static float qnt(float inp) { return inp * inp * inp * inp * inp; }

    public static float dist2(Vector2 p1, Vector2 p2) { return sqrt(sqr(p2.X - p1.X) + sqr(p2.Y - p1.Y)); }
    public static float dist3(Vector3 p1, Vector3 p2) { return cbrt(sqr(p2.X - p1.X) + sqr(p2.Y - p1.Y) + sqr(p2.Z - p1.Z)); }

    public static Vector2 twn2(Vector2 pos, Vector2 target, float smooth) { return (target - pos) / (smooth * (1 / (Time.DeltaTime * 30))); }
    public static Vector3 twn3(Vector3 pos, Vector3 target, float smooth) { return (target - pos) / (smooth * (1 / (Time.DeltaTime * 30))); }
    public static float twn(float cur, float targ, float smooth) { return (targ - cur) / (smooth * (1 / (Time.DeltaTime * 30))); }

    public static int rando(int min, int max) { Random r = new Random(); return r.Next(min, max); }

    public static float pi = MathF.PI;
    public static float e = MathF.E;
    public static float inf = float.PositiveInfinity;
}