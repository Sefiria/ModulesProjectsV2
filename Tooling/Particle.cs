using System.Drawing;

namespace Tooling
{
    public class Particle
    {
        public static readonly float Gravity = 0.05f;
        public bool Exists = true;
        public vecf Position, Look;
        public float Size, Speed;
        public int ArgbColor, Duration, Ticks;
        public bool ApplyGravity = false;
        public delegate void AfterUpdateHandler(Particle context);
        public event AfterUpdateHandler OnAfterUpdate;

        public Particle(){}
        public Particle(vecf position, float size, vecf look, float force, int color, int duration, AfterUpdateHandler onAfterUpdate = null)
        {
            Position = position;
            Size = size;
            Look = look;
            Speed = RandomThings.rnd1() * force;
            ArgbColor = color;
            Duration = duration;
            Ticks = 0;
            if (onAfterUpdate != null)
                OnAfterUpdate += onAfterUpdate;
        }
        public void Update()
        {
            if (!Exists)
                return;

            Position += Look * Speed;
            if (Position.x - Size < ParticlesManager.Bounds.X || Position.y - Size < ParticlesManager.Bounds.Y || Position.x + Size > ParticlesManager.Bounds.Width || Position.y + Size > ParticlesManager.Bounds.Height)
                Exists = false;
            if(ApplyGravity)
                Look.y += Gravity;

            Ticks++;
            if (Ticks > Duration)
                Exists = false;

            if (Exists)
                OnAfterUpdate?.Invoke(this);
        }
        public void Draw(Graphics g, PointF? Offset = null)
        {
            g.FillRectangle(new SolidBrush(Color.FromArgb((byte)((Duration - Ticks) / (float)Duration * byte.MaxValue), Color.FromArgb(ArgbColor))), Position.x + Offset?.X ?? 0, Position.y + Offset?.Y ?? 0, Size, Size);
        }
    }
}
