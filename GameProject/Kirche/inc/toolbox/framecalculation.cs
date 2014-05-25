using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Toolkit;

namespace Kirche.inc.toolbox
{
    class framecalculation
    {
        private const float PI = (float)Math.PI;

        private static float timemsec = 0;

        public static void set_timemsec(float time)
        {
            timemsec = time;
        }

        public static float move(float speed)
        {
            return speed * timemsec / 100.0f;
        }

        public static Vector2 move(float speed, float rad, Vector2 position)
        {
            position.X += (float)Math.Cos(DegreeToRadian(rad - 90.0f)) * move(speed);
            position.Y += (float)Math.Sin(DegreeToRadian(rad - 90.0f)) * move(speed);
            return position;
        }

        public static float DegreeToRadian(float angle)
        {
            return PI * angle / 180.0f;
        }
        public static float RadianToDegree(float angle)
        {
            return angle * (180.0f / PI);
        }
    }
}
