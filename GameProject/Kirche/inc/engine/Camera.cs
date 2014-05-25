using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Toolkit;
using Kirche.inc.toolbox;

namespace Kirche.inc.engine
{
    static class Camera
    {
        public static Vector2 position = new Vector2(0.0f, 0.0f);
        public static float rotation = 0.0f;
        public static float zoom = 1.0f;
        public static Vector2 screensize = new Vector2(1024.0f, 768.0f);


        public static bool is_inview(Vector2 pos)
        {
            if ((((pos.X - position.X) / zoom) + (screensize.X / 2.0f)) > screensize.X) return false;
            if ((((pos.X - position.X) / zoom) + (screensize.X / 2.0f)) < 0.0f) return false;

            if ((((pos.Y - position.Y) / zoom) + (screensize.Y / 2.0f)) > screensize.Y) return false;
            if ((((pos.Y - position.Y) / zoom) + (screensize.Y / 2.0f)) < 0.0f) return false;

            return true;
        }


        public static Vector2 calc_pos_inscreen(Vector2 pos)
        {
            return ((pos - position) / zoom) + (screensize / 2.0f);
        }


        public static void set_position(Vector2 pos)
        {
            position = pos;
        }

        public static void add_position(Vector2 pos)
        {
            position += pos;
        }

        public static void smooth_to_position(Vector2 pos)
        {
            position.X += framecalculation.move((pos.X - position.X) / 1.0f);
            position.Y += framecalculation.move((pos.Y - position.Y) / 1.0f);
        }

        public static void screensizechanged(Vector2 size)
        {
            screensize = size;
        }
    }
}
