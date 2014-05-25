using System;
using System.Collections.Generic;

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Kirche.inc.engine
{
    class GameObject
    {
        private static List<GameObject> renderlist = new List<GameObject>();

        public Texture2D texture;
        
        public Vector2 position = new Vector2(0.0f, 0.0f);
        public Vector2 origin = new Vector2(0.0f, 0.0f);
        public Vector2 scale = new Vector2(1.0f, 1.0f);
        public float layer_depth = 0.5f;
        public SpriteEffects effect = SpriteEffects.None;
        public float rotation = 0.0f;
        public Rectangle sourceRectangle = new Rectangle(0, 0, 1, 1);
        public Color color = Color.White;
        public float alpha = 1.0f;
        public bool visible = true;

        public GameObject()
        {
            renderlist.Add(this);
        }

        public void dispose()
        {
            renderlist.Remove(this);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameObject go in renderlist)
            {
                if (go.visible)
                {
                    
                    Color col = go.color * go.alpha;
                    spriteBatch.Draw(go.texture, Camera.calc_pos_inscreen(go.position), go.sourceRectangle, col, toolbox.framecalculation.DegreeToRadian(go.rotation), go.origin, go.scale, go.effect, go.layer_depth);
                    
                }
            }
        }
    }
}
