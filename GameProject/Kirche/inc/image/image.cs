using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using Kirche.inc.toolbox;

namespace Kirche.inc.image
{
    class image : engine.GameObject
    {

        private static image image_sin = null;

        public image()
        {
            image_sin = this;
        }

        public void update(MouseState mouseState, List<Keys> keys, GameTime gameTime)
        { 
            /*
            foreach (Keys key in keys)
            {
                if (key == Keys.W)
                {
                    player_speed += framecalculation.move(10.0f);
                }

                if (key == Keys.S)
                {
                    player_speed += framecalculation.move(-10.0f);
                }

                if (key == Keys.A)
                {
                    player_rotation += framecalculation.move(-20.0f);
                }

                if (key == Keys.D)
                {
                    player_rotation += framecalculation.move(20.0f);
                }
            }
            */
        }

    }
}
