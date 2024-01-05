using System.Numerics;

partial class v {
    public struct vec2 {
        public float x, y;

        public vec2(float _x, float _y) { x = _x;  y = _y; }
        public vec2(float val) { x = val; y = val; }

        public static vec2 zero { get => default; }
        public static vec2 one { get => new vec2(1); }

        public static vec2 x1 { get => new vec2(1, 0); }
        public static vec2 y1 { get => new vec2(0, 1); }

        public static vec2 operator +(vec2 l, vec2 r) { return new vec2(l.x + r.x, l.y + r.y); }
        public static vec2 operator -(vec2 l, vec2 r) { return new vec2(l.x - r.x, l.y - r.y); }
        public static vec2 operator *(vec2 l, vec2 r) { return new vec2(l.x * r.x, l.y * r.y); }
        public static vec2 operator /(vec2 l, vec2 r) { return new vec2(l.x / r.x, l.y / r.y); }

        public static vec2 operator -(vec2 val) { return zero - val; }

        public static vec2 operator *(float l, vec2 r) { return new vec2(l * r.x, l * r.y); }
        public static vec2 operator /(float l, vec2 r) { return new vec2(l / r.x, l / r.y); }
        public static vec2 operator *(vec2 l, float r) { return new vec2(l.x * r, l.y * r); }
        public static vec2 operator /(vec2 l, float r) { return new vec2(l.x / r, l.y / r); }

        public static bool operator ==(vec2 l, vec2 r) { return l.x == r.x && l.y == r.y; }
        public static bool operator !=(vec2 l, vec2 r) { return l.x != r.x && l.y != r.y; }
        public static bool operator >(vec2 l, vec2 r) { return l.x > r.x && l.y > r.y; }
        public static bool operator <(vec2 l, vec2 r) { return l.x < r.x && l.y < r.y; }
        public static bool operator >=(vec2 l, vec2 r) { return l.x >= r.x && l.y >= r.y; }
        public static bool operator <=(vec2 l, vec2 r) { return l.x <= r.x && l.y <= r.y; }

        public static Vector2 tosyst(vec2 val) { return new Vector2(val.x, val.y); }
    }
}