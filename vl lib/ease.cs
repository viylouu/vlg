//a whole bunch of easing functions

partial class ease {
    public static float isine(float t) { return 1 - m.cos(t * m.pi / 2); }
    public static float osine(float t) { return m.sin(t * m.pi / 2); }
    public static float iosine(float t) { return -(m.cos(m.pi * t) - 1) / 2; }

    public static float iquad(float t) { return m.sqr(t); }
    public static float oquad(float t) { return 1 - m.sqr(1 - t); }
    public static float ioquad(float t) { return t < 0.5f ? 2 * m.sqr(t) : 1 - m.sqr(-2 * t + 2) / 2; }

    public static float icube(float t) { return m.cbe(t); }
    public static float ocube(float t) { return 1 - m.cbe(1 - t);}
    public static float iocube(float t) { return t < 0.5f ? 4 * m.cbe(t) : 1 - m.cbe(-2 * t + 2) / 2; }

    public static float iquart(float t) { return m.qrt(t); }
    public static float oquart(float t) { return 1 - m.qrt(1 - t); }
    public static float ioquart(float t) { return t < 0.5f ? 8 * m.qrt(t) : 1 - m.qrt(-2 * t + 2) / 2; }

    public static float iquint(float t) { return m.qnt(t); }
    public static float oquint(float t) { return 1 - m.qnt(1 - t); }
    public static float ioquint(float t) { return t < 0.5f ? 16 * m.qnt(t) : 1 - m.qnt(-2 * t + 2) / 2; }

    public static float iexpo(float t) { return t == 0 ? 0 : m.pwr(2, 10 * t - 10); }
    public static float oexpo(float t) { return t == 1 ? 1 : 1 - m.pwr(2, -10 * t); }
    public static float ioexpo(float t) { return t == 0 ? 0 : t == 1 ? 1 : t < 0.5f ? m.pwr(2, 20 * t - 10) / 2 : (2 - m.pwr(2, -20 * t + 10)) / 2; }

    public static float icirc(float t) { return 1 - m.sqrt(1 - m.pwr(t, 2)); }
    public static float ocirc(float t) { return m.sqrt(1 - m.sqr(t - 1)); }
    public static float iocirc(float t) { return t < 0.5f ? (1 - m.sqrt(1 - m.sqr(2 * t))) / 2 : (m.sqrt(1 - m.sqr(-2 * t + 2)) + 1) / 2; }

    public static float iback(float t) { return 2.70158f * m.cbe(t) - 1.70158f * m.sqr(t); }
    public static float oback(float t) { return 1 + 2.70158f * m.cbe(t - 1) + 1.70158f * m.sqr(t - 1); }
    public static float ioback(float t) { return t < 0.5f ? (m.sqr(2 * t) * ((2.5949095f + 1) * 2 * t - 2.5949095f)) : (m.sqr(2 * t - 2) * ((2.5949095f + 1) * (t * 2 - 2) + 2.5949095f) + 2) / 2; }
}