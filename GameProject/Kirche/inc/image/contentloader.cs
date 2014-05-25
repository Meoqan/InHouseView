using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using Kirche.inc.toolbox;

namespace Kirche.inc.image
{
    class contentloader
    {
        public static List<contentloader> LIST = new List<contentloader>();
        public static List<contentloader> DETAILLIST = new List<contentloader>();

        public Texture2D tex = null;
        public string name = string.Empty;

        public static void scan(GraphicsDeviceManager graphicsDeviceManager)
        {
            string[] array1 = Directory.GetFiles(@"scan\", "*.jpg");


            logwriter.add("--- Files: ---", 0);
            foreach (string name in array1)
            {
                logwriter.add(name, 0);

                contentloader cont = new contentloader();
                cont.tex = Texture2D.Load(graphicsDeviceManager.GraphicsDevice, name);
                cont.name = name.Split('\\')[1].Split('.')[0];
                LIST.Add(cont);

            }

            string[] array2 = Directory.GetFiles(@"detail\", "*.jpg");


            logwriter.add("--- DETAILLIST Files: ---", 0);
            foreach (string name in array2)
            {
                logwriter.add(name, 0);

                contentloader cont = new contentloader();
                cont.tex = Texture2D.Load(graphicsDeviceManager.GraphicsDevice, name);
                cont.name = name.Split('\\')[1].Split('.')[0];
                DETAILLIST.Add(cont);

            }
        }

        public static Texture2D get_by_filename(string name)
        {
            foreach (contentloader cont in LIST)
            {
                if (cont.name == name) return cont.tex;
            }

            return null;
        }

        public static holder DETAIL_get_by_filename(string filename, string view, float fx, float tx, float fy, float ty,string nowview,float mx,float my, string TEXCHAIN = "NONE")
        {
            if (nowview != view) return null;
            if (mx > fx && mx < tx && my > fy && my < ty)
            {
                foreach (contentloader cont in DETAILLIST)
                {
                    if (cont.name == filename)
                    {
                        holder tex = new holder();
                        tex.tex = cont.tex;
                        tex.from = new Vector2(fx, fy);
                        tex.to = new Vector2(tx, ty);
                        tex.clicked = true;

                        if (TEXCHAIN != "NONE")
                        {
                            if (TEXCHAIN.Contains(";"))
                            {
                                string[] parts = TEXCHAIN.Split(';');
                                foreach (string p in parts)
                                {
                                    foreach (contentloader next in DETAILLIST)
                                    {
                                        if (next.name == p)
                                        {
                                            tex.chain.Add(next.tex);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (contentloader next in DETAILLIST)
                                {
                                    if (next.name == TEXCHAIN)
                                    {
                                        tex.chain.Add(next.tex);
                                    }
                                }
                                
                            }
                        }

                        return tex;
                    }
                }
            }
            return null;
        }

    }
}
