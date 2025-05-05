using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tooling
{
    public class vecf
    {
        public float x { get; set; }
        public float y { get; set; }
        public byte UserData { get; set; } = 0;
        public vecf() { x = y = 0F; }
        public vecf(vecf v) { x = v.x; y = v.y; }
        public vecf(int x, int y) { this.x = x; this.y = y; }
        public vecf(float x, float y, byte usrdt = 0) { this.x = x; this.y = y; UserData = usrdt; }
        [JsonIgnore] public vec i => new vec(x, y);
        [JsonIgnore] public Point ipt => new Point((int)x, (int)y);
        [JsonIgnore] public PointF pt => new PointF(x, y);
        [JsonIgnore] public Size isz => new Size((int)x, (int)y);
        [JsonIgnore] public SizeF sz => new SizeF(x, y);

        [JsonIgnore] public static vecf Zero => new vecf(0F, 0F);

        public vecf tile(int sz) => new vecf((int)(x / sz), (int)(y / sz));
        public vecf snap(int sz) => new vecf((int)(x / sz) * sz, (int)(y / sz) * sz);
        public vecf snap() => new vecf((int)(x / Core.TSZ) * Core.TSZ, (int)(y / Core.TSZ) * Core.TSZ);

        public override bool Equals(object obj)
        {
            return obj is vecf vecf &&
                        x == vecf.x &&
                        y == vecf.y;
        }

        public override int GetHashCode()
        {
            int hashCode = 1954509452;
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(vecf a, vecf b) => a?.x == b?.x && a?.y == b?.y;
        public static bool operator !=(vecf a, vecf b) => a?.x != b?.x || a?.y != b?.y;
        public static vecf operator +(vecf a, vecf b) => new vecf((a?.x + b?.x).Value, (a?.y + b?.y).Value);
        public static vecf operator +(vecf a, int v) => new vecf((a?.x + v).Value, (a?.y + v).Value);
        public static vecf operator +(vecf a, float v) => new vecf((a?.x + v).Value, (a?.y + v).Value);
        public static vecf operator -(vecf a, vecf b) => new vecf((a?.x - b?.x).Value, (a?.y - b?.y).Value);
        public static vecf operator -(vecf a, int v) => new vecf((a?.x - v).Value, (a?.y - v).Value);
        public static vecf operator -(vecf a, float v) => new vecf((a?.x - v).Value, (a?.y - v).Value);
        public static vecf operator -(float a, vecf v) => new vecf(a - v?.x ?? 0F, a- v?.y ?? 0F);
        public static vecf operator *(vecf a, vecf b) => new vecf((a?.x * b?.x).Value, (a?.y * b?.y).Value);
        public static vecf operator *(vecf a, int b) => new vecf((a?.x * b).Value, (a?.y * b).Value);
        public static vecf operator *(vecf a, float b) => new vecf((a?.x * b).Value, (a?.y * b).Value);
        public static vecf operator /(vecf a, vec b) => new vecf((a?.x / b?.x).Value, (a?.y / b?.y).Value);
        public static vecf operator /(vecf a, vecf b) => new vecf((a?.x / b?.x).Value, (a?.y / b?.y).Value);
        public static vecf operator /(vecf a, int b) => b == 0 ? a : new vecf((a?.x / b).Value, (a?.y / b).Value);
        public static vecf operator /(vecf a, float b) => Math.Round(b, 4) == 0F ? a : new vecf((a?.x / b).Value, (a?.y / b).Value);
        public static vecf operator %(vecf a, float b) => new vecf((a?.x % b).Value, (a?.y % b).Value);
        public static vecf operator %(vecf a, vecf b) => new vecf((a?.x % b?.x).Value, (a?.y % b?.y).Value);

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }
}
