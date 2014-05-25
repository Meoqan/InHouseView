using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using Kirche.inc.toolbox;

namespace Kirche.inc.image
{
    class holder
    {
        public Texture2D tex = null;
        public Vector2 from = new Vector2();
        public Vector2 to = new Vector2();
        public bool clicked = false;
        public List<Texture2D> chain = new List<Texture2D>();
    }
}
