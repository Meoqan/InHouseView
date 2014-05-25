using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using Kirche.inc.image;

namespace Kirche.inc.engine
{
    class Window : Game
    {
        private readonly Stopwatch fpsClock;
        private GraphicsDeviceManager graphicsDeviceManager;
        private SpriteBatch spriteBatch;
        private SpriteFont arial16BMFont;


        private Texture2D OutsideTexture;
        private Texture2D selectx;

        private Texture2D rechtsTexture;
        private Texture2D untenTexture;
        private Texture2D linksTexture;
        private Texture2D obenTexture;

        private int frameCount;
        private string fpsText;

        private KeyboardManager keyboarManager;
        private KeyboardState keyboardState;
        private MouseManager mouseManager;
        private MouseState mouseState;

        image.image image = null;

        image.image rechts = null;
        image.image links = null;
        image.image unten = null;
        image.image oben = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteBatchAndFontGame" /> class.
        /// </summary>
        public Window()
        {
            Kirche.inc.main.loading.Show();
            Kirche.inc.main.loading.Update();

            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            
            
            //this.GraphicsDevice.MainDevice.Presenter.Resize(1024, 768, SharpDX.DXGI.Format.Unknown);

            //this.Window.BeginScreenDeviceChange(true);
            //this.Window.AllowUserResizing = true;
            //graphicsDeviceManager.DeviceCreationFlags = new SharpDX.Direct3D11.DeviceCreationFlags();

            // Create the keyboard manager
            keyboarManager = new KeyboardManager(this);
            mouseManager = new MouseManager(this);

            // Force no vsync and use real timestep to print actual FPS
            graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";

            // Variable used for FPS
            fpsClock = new Stopwatch();
            fpsText = string.Empty;

            IsMouseVisible = true;
        }

        public Texture2D LoadTexture(string filename)
        {
            return Texture2D.Load(graphicsDeviceManager.GraphicsDevice, "Content\\" + filename);
        }

        protected override void LoadContent()
        {
            OutsideTexture = Texture2D.Load(graphicsDeviceManager.GraphicsDevice, "Content\\outside03.jpg");
            selectx = Texture2D.Load(graphicsDeviceManager.GraphicsDevice, "Content\\select.dds");
            //test_001_LTexture = Texture2D.Load(graphicsDeviceManager.GraphicsDevice, "Content\\test-001-L.dds");
            //test_001_RTexture = Texture2D.Load(graphicsDeviceManager.GraphicsDevice, "Content\\test-001-R.dds");
            //test_002Texture = Texture2D.Load(graphicsDeviceManager.GraphicsDevice, "Content\\test-002.dds");

            rechtsTexture = Texture2D.Load(graphicsDeviceManager.GraphicsDevice, "Content\\rechts.dds");
            untenTexture = Texture2D.Load(graphicsDeviceManager.GraphicsDevice, "Content\\unten.dds");
            linksTexture = Texture2D.Load(graphicsDeviceManager.GraphicsDevice, "Content\\links.dds");
            obenTexture = Texture2D.Load(graphicsDeviceManager.GraphicsDevice, "Content\\oben.dds");

            


            image = new image.image();
            image.texture = OutsideTexture;
            image.sourceRectangle = new Rectangle(0, 0, OutsideTexture.Width, OutsideTexture.Height);
            image.scale = new Vector2((float)GraphicsDevice.BackBuffer.Width / (float)OutsideTexture.Width, (float)GraphicsDevice.BackBuffer.Height / (float)OutsideTexture.Height);
            image.origin = new Vector2(OutsideTexture.Width / 2.0f, OutsideTexture.Height / 2.0f);

            arial16BMFont = Content.Load<SpriteFont>("Arial16");
            
            spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();

            Camera.screensizechanged(new Vector2(GraphicsDevice.BackBuffer.Width, GraphicsDevice.BackBuffer.Height));

            inc.image.contentloader.scan(graphicsDeviceManager);

            rechts = new image.image();
            //rechts.layer_depth = 1.0f;
            rechts.texture = rechtsTexture;
            rechts.scale = new Vector2(0.75f, 0.75f);
            rechts.sourceRectangle = new Rectangle(0, 0, rechtsTexture.Width, rechtsTexture.Height);
            rechts.origin = new Vector2(rechtsTexture.Width / 2.0f, rechtsTexture.Height / 2.0f);

            unten = new image.image();
            //unten.layer_depth = 0.0f;
            unten.texture = untenTexture;
            unten.scale = new Vector2(0.75f, 0.75f);
            unten.sourceRectangle = new Rectangle(0, 0, untenTexture.Width, untenTexture.Height);
            unten.origin = new Vector2(untenTexture.Width / 2.0f, untenTexture.Height / 2.0f);

            links = new image.image();
            //links.layer_depth = 1.0f;
            links.texture = linksTexture;
            links.scale = new Vector2(0.75f, 0.75f);
            links.sourceRectangle = new Rectangle(0, 0, linksTexture.Width, linksTexture.Height);
            links.origin = new Vector2(linksTexture.Width / 2.0f, linksTexture.Height / 2.0f);

            oben = new image.image();
            //oben.layer_depth = 1.0f;
            oben.texture = obenTexture;
            oben.scale = new Vector2(0.75f, 0.75f);
            oben.sourceRectangle = new Rectangle(0, 0, obenTexture.Width, obenTexture.Height);
            oben.origin = new Vector2(obenTexture.Width / 2.0f, obenTexture.Height / 2.0f);

            Kirche.inc.main.loading.Close();
        }

        protected override void UnloadContent()
        {
            spriteBatch.Dispose();

            base.UnloadContent();
        }

        protected override void BeginRun()
        {
            // Starts the FPS clock
            fpsClock.Start();
            base.BeginRun();
        }


        protected override void OnExiting(object sender, EventArgs args)
        {
            inc.maintenance.control.stop();

            toolbox.logwriter.writelogtodisk();

            base.OnExiting(sender, args);
        }

        protected override void Initialize()
        {
            Window.Title = "Kirche";

            //Window.BeginScreenDeviceChange(false);
            Window.AllowUserResizing = true;
            //Window.EndScreenDeviceChange(1024, 768);
            
            this.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);

            Window.IsMouseVisible = true;

            base.Initialize();

            
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            Camera.screensizechanged(new Vector2(GraphicsDevice.BackBuffer.Width, GraphicsDevice.BackBuffer.Height));
            
        }


        int pos = 3;
        int look = 0;
        int lookup = 0;
        string view = "";
        int detailview = 0;
        Texture2D b4detailview = null;
        holder tex_detail = null;

        private readonly List<Keys> keys = new List<Keys>();
        protected override void Draw(GameTime gameTime)
        {
            //Camera.set_position(new Vector2(0.0f, 0.0f));
            // compute mouse position in screen coordinates
            RenderTarget2D backbuffer = GraphicsDevice.BackBuffer;
            int screenWidth = GraphicsDevice.BackBuffer.Width;
            int screenHeight = GraphicsDevice.BackBuffer.Height;

            if (detailview == 0)
            {
                tex_detail = null;
            }

            keyboardState.GetDownKeys(keys);

            image.update(mouseState,keys,gameTime);

            image.scale = new Vector2((float)GraphicsDevice.BackBuffer.Width / (float)image.texture.Width, (float)GraphicsDevice.BackBuffer.Height / (float)image.texture.Height);

            float mx = mouseState.X;
            float my = mouseState.Y;

            if (detailview == 0)
            {
                if (lookup == 0)
                {
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("1_S_01", "1_S", 0.33f, 0.66f, 0.66f, 0.98f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("1_S_02", "1_S", 0.05f, 0.25f, 0.55f, 0.95f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("1_S_03", "1_S", 0.3f, 0.7f, 0.15f, 0.5f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("1_S_04", "1_S", 0.3f, 0.7f, 0.5f, 0.66f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("2_S_01", "2_S", 0.1f, 0.9f, 0.1f, 0.9f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("3_O_01", "5_W", 0.18f, 0.65f, 0.12f, 0.57f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("3_O_01", "2_N", 0.25f, 0.3f, 0.47f, 0.59f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("3_O_01", "6_SW", 0.24f, 0.44f, 0.24f, 0.53f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("3_O_01", "7_S", 0.82f, 0.9f, 0.4f, 0.64f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("3_O_01", "4_W", 0.78f, 0.9f, 0.11f, 0.57f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("NEU_3_S", "3_S", 0.22f, 0.37f, 0.38f, 0.6f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("3_S_02", "4_S", 0.8f, 0.9f, 0.47f, 0.53f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_O_01", "3_W", 0.25f, 0.6f, 0.12f, 0.61f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_O_02", "3_W", 0.82f, 0.9f, 0.38f, 0.53f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_O_02", "4_W", 0.13f, 0.22f, 0.36f, 0.50f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_O_03", "4_W", 0.23f, 0.53f, 0.02f, 0.32f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_O_PUB", "4_N", 0.64f, 0.9f, 0.6f, 0.9f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_O_PUB", "4_O", 0.1f, 0.52f, 0.63f, 0.9f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_O_PUB", "2_N", 0.54f, 0.67f, 0.6f, 0.65f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_O_PUB", "3_N", 0.56f, 0.9f, 0.55f, 0.7f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_O_PUB", "5_O", 0.1f, 0.9f, 0.6f, 0.9f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_O_PUB", "6_SO", 0.65f, 0.9f, 0.51f, 0.72f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_O_PUB", "6_S", 0.1f, 0.42f, 0.54f, 0.62f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_O_PUB", "7_S", 0.15f, 0.43f, 0.6f, 0.74f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_W_01", "7_S", 0.2f, 0.26f, 0.44f, 0.6f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_W_01", "6_S", 0.1f, 0.18f, 0.33f, 0.52f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_W_01", "4_O", 0.39f, 0.75f, 0.13f, 0.58f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_W_01", "2_N", 0.76f, 0.82f, 0.48f, 0.65f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_W_PUB", "2_N", 0.3f, 0.46f, 0.59f, 0.65f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_W_PUB", "3_N", 0.1f, 0.42f, 0.54f, 0.68f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_W_PUB", "4_N", 0.1f, 0.38f, 0.59f, 0.9f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_W_PUB", "4_W", 0.38f, 0.9f, 0.59f, 0.9f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_W_PUB", "5_W", 0.1f, 0.9f, 0.61f, 0.9f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_W_PUB", "6_S", 0.55f, 0.9f, 0.53f, 0.64f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_W_PUB", "6_SW", 0.1f, 0.25f, 0.52f, 0.68f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("4_W_PUB", "7_S", 0.54f, 0.81f, 0.59f, 0.68f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_N_02", "5_N", 0.37f, 0.62f, 0.61f, 0.82f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_N_01", "5_N", 0.37f, 0.62f, 0.54f, 0.61f, pos + "_" + view, mx, my, "NEU_5_N_03;NEU_5_N_01;NEU_5_N_05;NEU_5_N_02");
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_O_PUB_01", "2_N", 0.55f, 0.84f, 0.66f, 0.81f, pos + "_" + view, mx, my, "5_O_PUB_02;5_O_PUB_03");
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_O_PUB_01", "3_O", 0.1f, 0.9f, 0.6f, 0.9f, pos + "_" + view, mx, my, "5_O_PUB_02;5_O_PUB_03");
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_O_PUB_01", "3_S", 0.1f, 0.33f, 0.61f, 0.9f, pos + "_" + view, mx, my, "5_O_PUB_02;5_O_PUB_03");
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_O_PUB_01", "3_N", 0.76f, 0.9f, 0.73f, 0.9f, pos + "_" + view, mx, my, "5_O_PUB_02;5_O_PUB_03");
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_O_PUB_01", "4_O", 0.8f, 0.9f, 0.6f, 0.9f, pos + "_" + view, mx, my, "5_O_PUB_02;5_O_PUB_03");
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_O_PUB_01", "4_S", 0.1f, 0.4f, 0.54f, 0.9f, pos + "_" + view, mx, my, "5_O_PUB_02;5_O_PUB_03");
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_O_PUB_01", "5_S", 0.1f, 0.43f, 0.51f, 0.58f, pos + "_" + view, mx, my, "5_O_PUB_02;5_O_PUB_03");
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_O_PUB_01", "6_S", 0.26f, 0.45f, 0.51f, 0.53f, pos + "_" + view, mx, my, "5_O_PUB_02;5_O_PUB_03");
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_O_PUB_01", "7_S", 0.31f, 0.46f, 0.55f, 0.59f, pos + "_" + view, mx, my, "5_O_PUB_02;5_O_PUB_03");
                    //                    if(tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_O_PUB_02", ",", ",", ", "BACK" , pos + "_" + view, mx, my);
                    //                    if(tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_O_PUB_03", ",", ",", ", "BACK" , pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_W_PUB_01", "2_N", 0.1f, 0.44f, 0.66f, 0.81f, pos + "_" + view, mx, my, "5_W_PUB_02;5_W_PUB_03");
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_W_PUB_01", "3_N", 0.1f, 0.24f, 0.72f, 0.9f, pos + "_" + view, mx, my, "5_W_PUB_02;5_W_PUB_03");
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_W_PUB_01", "3_W", 0.1f, 0.9f, 0.61f, 0.9f, pos + "_" + view, mx, my, "5_W_PUB_02;5_W_PUB_03");
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_W_PUB_01", "3_S", 0.65f, 0.9f, 0.61f, 0.9f, pos + "_" + view, mx, my, "5_W_PUB_02;5_W_PUB_03");
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_W_PUB_01", "4_W", 0.1f, 0.16f, 0.61f, 0.9f, pos + "_" + view, mx, my, "5_W_PUB_02;5_W_PUB_03");
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_W_PUB_01", "4_S", 0.57f, 0.9f, 0.53f, 0.9f, pos + "_" + view, mx, my, "5_W_PUB_02;5_W_PUB_03");
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_W_PUB_01", "5_S", 0.55f, 0.9f, 0.52f, 0.58f, pos + "_" + view, mx, my, "5_W_PUB_02;5_W_PUB_03");
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_W_PUB_01", "6_S", 0.53f, 0.72f, 0.51f, 0.52f, pos + "_" + view, mx, my, "5_W_PUB_02;5_W_PUB_03");
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_W_PUB_01", "7_S", 0.53f, 0.68f, 0.55f, 0.59f, pos + "_" + view, mx, my, "5_W_PUB_02;5_W_PUB_03");
                    //                    if(tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_W_PUB_02", ",", ",", ", "BACK" , pos + "_" + view, mx, my);
                    //                    if(tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("5_W_PUB_03", ",", ",", ", "BACK" , pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("6_N_01", "6_N", 0.36f, 0.63f, 0.1f, 0.25f, pos + "_" + view, mx, my);
                    //if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("6_N_01", "5_N", 0.73f, 0.9f, 0.1f, 0.53f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("6_NO_01", "6_O", 0.1f, 0.48f, 0.1f, 0.9f, pos + "_" + view, mx, my, "7_SO_01;7_SO_02");
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("6_N_02", "5_N", 0.38f, 0.6f, 0.1f, 0.3f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("6_O_01", "6_O", 0.46f, 0.58f, 0.55f, 0.84f, pos + "_" + view, mx, my);
                    //                    if(tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("6_O_02", ",", ",", ", "BACK" , pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("6_O_03", "6_O", 0.58f, 0.7f, 0.1f, 0.9f, pos + "_" + view, mx, my);
                    //                    if(tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("6_O_04", ",", ",", ", "BACK" , pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("6_SO_01", "6_O", 0.86f, 0.9f, 0.35f, 0.58f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("6_SO_01", "6_SO", 0.27f, 0.33f, 0.37f, 0.58f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("6_SW_05", "6_SW", 0.37f, 0.58f, 0.53f, 0.9f, pos + "_" + view, mx, my, "6_SW_04;6_SW_03;6_SW_02;6_SW_01");
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("6_SW_05", "5_N", 0.13f, 0.28f, 0.5f, 0.8f, pos + "_" + view, mx, my, "6_SW_04;6_SW_03;6_SW_02;6_SW_01");
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("6_SW_05", "7_S", 0.71f, 0.84f, 0.7f, 0.9f, pos + "_" + view, mx, my, "6_SW_04;6_SW_03;6_SW_02;6_SW_01");
                    //                    if(tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("6_SW_02", ",", ",", ", "BACK" , pos + "_" + view, mx, my);
                    //                    if(tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("6_SW_03", ",", ",", ", "BACK" , pos + "_" + view, mx, my);
                    //                    if(tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("6_SW_04", ",", ",", ", "BACK" , pos + "_" + view, mx, my);
                    //                    if(tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("6_SW_05", ",", ",", ", "BACK" , pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("6_S_01", "6_S", 0.28f, 0.71f, 0.1f, 0.33f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("6_S_01", "7_S", 0.31f, 0.68f, 0.1f, 0.39f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("6_S_02", "6_S", 0.1f, 0.9f, 0.67f, 0.9f, pos + "_" + view, mx, my, "NEU_6_S_03;NEU_6_S_02;NEU_6_S_01");
                    //                    if(tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("6_S_03", ",", ",", ", "BACK" , pos + "_" + view, mx, my);
                    //                    if(tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("7_NO_01", ",", ",", ", "BACK" , pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("7_N_06", "7_N", 0.44f, 0.56f, 0.5f, 0.57f, pos + "_" + view, mx, my, "7_N_02;7_N_01");
                    //                    if(tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("7_N_03", ",", ",", ", "BACK" , pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("7_N_04", "7_N", 0.44f, 0.56f, 0.38f, 0.5f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("7_N_05", "7_N", 0.28f, 0.7f, 0.1f, 0.31f, pos + "_" + view, mx, my);
                    //                    if(tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("7_N_06", ",", ",", ", "BACK" , pos + "_" + view, mx, my);
                    //                    if(tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("7_N_07", ",", ",", ", "BACK" , pos + "_" + view, mx, my);
                    //                    if(tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("7_SO_02", ",", ",", ", "BACK" , pos + "_" + view, mx, my);
                    //                    if(tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("outside01", ",", ",", ", "BACK" , pos + "_" + view, mx, my);
                    //                    if(tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("outside02", ",", ",", ", "BACK" , pos + "_" + view, mx, my);
                    //                    if(tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("outside03", ",", ",", ", "BACK" , pos + "_" + view, mx, my);
                    //                    if(tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("pos", ",", ",", ", "BACK" , pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("TAFEL", "6_SO", 0.34f, 0.41f, 0.31f, 0.52f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("TAFEL", "6_SW", 0.58f, 0.66f, 0.3f, 0.5f, pos + "_" + view, mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("TAFEL", "4_W", 0.54f, 0.63f, 0.27f, 0.52f, pos + "_" + view, mx, my);
                }
                if(lookup == 1)
                {
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("7_NO_01", "7_NO_U", 0.41f, 0.56f, 0.42f, 0.61f, pos + "_" + view + "_U", mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("7_NW_01", "7_NW_U", 0.42f, 0.57f, 0.39f, 0.64f, pos + "_" + view + "_U", mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("7_O_01", "7_O_U", 0.36f, 0.63f, 0.41f, 0.62f, pos + "_" + view + "_U", mx, my);
                    //if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("7_SO_01", "7_SO_U", 0.1f, 0.9f, 0.1f, 0.9f, pos + "_" + view + "_U", mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("7_W_01", "7_W_U", 0.37f, 0.66f, 0.39f, 0.62f, pos + "_" + view + "_U", mx, my);
                    if (tex_detail == null) tex_detail = inc.image.contentloader.DETAIL_get_by_filename("NEU_6_O_02", "6_W_U", 0.23f, 0.49f, 0.15f, 0.46f, pos + "_" + view + "_U", mx, my);

                }
            }

            if (mouseState.LeftButton.Pressed)
            {
                if (detailview == 1)
                {
                    if (tex_detail.chain.Count > 0)
                    {
                        image.texture = tex_detail.chain.First<Texture2D>();
                        tex_detail.chain.Remove(tex_detail.chain.First<Texture2D>());
                    }
                    else
                    {
                        tex_detail = null;
                        detailview = 0;
                        image.texture = b4detailview;
                    }
                    
                }
                else
                {

                    if (tex_detail == null)
                    {
                        if(pos < 6)
                        {
                            lookup = 0;
                        }

                        if (pos == 3 && look == 2)
                        {
                            if (mouseState.X < 0.6 && mouseState.X > 0.4 && mouseState.Y < 0.7 && mouseState.Y > 0.45) pos = 1;
                        }

                        if (pos == 1)
                        {
                            if (mouseState.X > 0.9 && mouseState.Y < 0.9) look++;
                            if (mouseState.X < 0.1 && mouseState.Y < 0.9) look--;
                            if (pos < 6)
                            {
                                if (look < 0) look = 3;
                                if (look > 3) look = 0;
                            }
                            if ((look == 0 && pos < 6))
                            {
                                if (mouseState.X < 0.9 && mouseState.X > 0.1 && mouseState.Y < 0.9 && mouseState.Y > 0.1) pos++;
                                if (mouseState.X < 0.9 && mouseState.X > 0.1 && mouseState.Y > 0.9) pos--;
                            }

                            if (pos > 7) pos = 7;
                            if (pos < 1) pos = 1;

                        }

                        if (pos > 1)
                        {
                            int lookb4 = look;
                            if (mouseState.X > 0.9 && mouseState.Y < 0.9) look++;
                            if (mouseState.X < 0.1 && mouseState.Y < 0.9) look--;
                            if (pos < 6 && lookup == 0)
                            {
                                if (look < 0) look = 3;
                                if (look > 3) look = 0;
                            }
                            if (pos == 6)
                            {
                                if (look < 0) look = 7;
                                if (look > 7) look = 0;
                            }
                            if (pos == 7)
                            {
                                if (look < 0)look = 5;
                                if (look > 5)look = 0;
                                if (lookb4 == 2 && look == 3 && lookup == 1) look = 4;
                                if (lookb4 == 4 && look == 3 && lookup == 1) look = 2;
                            }

                            if (look == 0 && lookup == 0)
                            {
                                if (mouseState.X < 0.9 && mouseState.X > 0.1 && mouseState.Y < 0.9 && mouseState.Y > 0.1) pos++;
                                if (mouseState.X < 0.9 && mouseState.X > 0.1 && mouseState.Y > 0.9) pos--;
                            }
                            if (((look == 2 && pos < 6) || (look == 4 && pos == 6) || (look == 3 && pos == 7)) && lookup == 0)
                            {
                                int b4 = pos;

                                if (mouseState.X < 0.9 && mouseState.X > 0.1 && mouseState.Y < 0.9 && mouseState.Y > 0.1) pos--;
                                if (mouseState.X < 0.9 && mouseState.X > 0.1 && mouseState.Y > 0.9) pos++;

                                //von 5 auf 6 -> 0
                                if (b4 == 5 && pos == 6)
                                {
                                    if (look == 0)
                                    {

                                    }
                                    else
                                    {
                                        look = 4;
                                    }

                                }
                                //von 6 auf 5 -> 4 zu 2
                                if (b4 == 6 && pos == 5)
                                {
                                    if(look == 0)
                                    {

                                    }
                                    else
                                    {
                                        look = 2;
                                    }
                                    
                                }

                                //von 6 auf 7 -> 0
                                if (b4 == 6 && pos == 7)
                                {
                                    if (look == 0)
                                    {

                                    }
                                    else
                                    {
                                        look = 3;
                                    }
                                }
                                //von 7 auf 6 -> 3 zu 4

                                if (b4 == 7 && pos == 6)
                                {
                                    if (look == 0)
                                    {

                                    }
                                    else
                                    {
                                        look = 4;
                                    }
                                }
                            }

                            if (pos > 7) pos = 7;
                            if (pos < 2) pos = 2;
                        }

                        if (pos < 6)
                        {
                            switch (look)
                            {
                                case 0:
                                    view = "N";
                                    break;
                                case 1:
                                    view = "O";
                                    break;
                                case 2:
                                    view = "S";
                                    break;
                                case 3:
                                    view = "W";
                                    break;
                            }
                        }

                        if (pos == 6)
                        {
                            switch (look)
                            {
                                case 0:
                                    view = "N";
                                    break;
                                case 1:
                                    view = "NO";
                                    break;
                                case 2:
                                    view = "O";
                                    break;
                                case 3:
                                    view = "SO";
                                    break;
                                case 4:
                                    view = "S";
                                    break;
                                case 5:
                                    view = "SW";
                                    break;
                                case 6:
                                    view = "W";
                                    break;
                                case 7:
                                    view = "NW";
                                    break;
                            }
                        }

                        if (pos == 7)
                        {
                            switch (look)
                            {
                                case 0:
                                    view = "N";
                                    break;
                                case 1:
                                    view = "NO";
                                    break;
                                case 2:
                                    view = "O";
                                    break;
                                case 3:
                                    view = "S";
                                    break;
                                case 4:
                                    view = "W";
                                    break;
                                case 5:
                                    view = "NW";
                                    break;
                            }
                        }

                        if ((pos == 6 || (pos == 7 && !(view == "S"))) && lookup == 0)
                        {
                            if (mouseState.Y < 0.1) lookup = 1;
                        }
                        if ((pos == 6 || pos == 7) && lookup == 1)
                        {
                            if (mouseState.Y > 0.9) lookup = 0;
                        }

                        if (lookup == 0) image.texture = inc.image.contentloader.get_by_filename(pos + "_" + view);
                        if (lookup == 1) image.texture = inc.image.contentloader.get_by_filename(pos + "_" + view +"_U");
                    }
                    else
                    {
                        
                        b4detailview = image.texture;
                        image.texture = tex_detail.tex;
                        detailview = 1;
                    }
                }
                image.sourceRectangle = new Rectangle(0, 0, image.texture.Width, image.texture.Height);
                image.scale = new Vector2((float)GraphicsDevice.BackBuffer.Width / (float)image.texture.Width, (float)GraphicsDevice.BackBuffer.Height / (float)image.texture.Height);
                image.origin = new Vector2(image.texture.Width / 2.0f, image.texture.Height / 2.0f);
            }




            if(detailview == 1 || view == "")
            {
                rechts.visible = false;
                unten.visible = false;
                links.visible = false;
                oben.visible = false;
            }
            else
            {
                rechts.visible = true;
                unten.visible = true;
                links.visible = true;

                if ((view == "N" || view == "S") && !(pos == 1 && view == "N") && !(pos == 1 && view == "S") && !(pos == 2 && view == "N") && !(pos == 7 && view == "S"))
                {
                    unten.visible = true;
                }
                else
                {
                    unten.visible = false;
                }

                if (lookup == 1)
                {
                    unten.visible = true;
                }

                if ((pos == 6 || (pos == 7 && view != "S")) && lookup == 0)
                {
                    oben.visible = true;
                }
                else
                {
                    oben.visible = false;
                }
                

                float mod =(1.0f + (float)Math.Cos(gameTime.TotalGameTime.TotalMilliseconds / 150)) * 8.0f;

                rechts.position = new Vector2((screenWidth / 2.0f) - 64.0f, 0.0f);
                unten.position = new Vector2(0.0f, (screenHeight / 2.0f) - 64.0f);
                links.position = new Vector2(-(screenWidth / 2.0f) + 64.0f, 0.0f);
                oben.position = new Vector2(0.0f, -(screenHeight / 2.0f) + 64.0f);

                if (mx > 0.9f) rechts.position = new Vector2((screenWidth / 2.0f) - 64.0f - mod, 0.0f);
                if (my > 0.9f) unten.position = new Vector2(0.0f, (screenHeight / 2.0f) - 64.0f - mod);
                if (mx < 0.1f) links.position = new Vector2(-(screenWidth / 2.0f) + 64.0f + mod, 0.0f);
                if (my < 0.1f) oben.position = new Vector2(0.0f, -(screenHeight / 2.0f) + 64.0f + mod);
            }





            spriteBatch.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.NonPremultiplied);  // Use NonPremultiplied, as this sprite texture is not premultiplied
            GraphicsDevice.Clear(Color.CornflowerBlue);


            Kirche.inc.engine.GameObject.Draw(spriteBatch);


            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.Additive);

            if (detailview == 0)
            {
                Color col = Color.White * 0.5f;
                if (tex_detail != null) spriteBatch.Draw(selectx, new Vector2(tex_detail.from.X * screenWidth, tex_detail.from.Y * screenHeight), new Rectangle(0, 0, 100, 100), col, toolbox.framecalculation.DegreeToRadian(0.0f), new Vector2(0, 0), new Vector2(((tex_detail.to.X - tex_detail.from.X) * screenWidth) / 100.0f, ((tex_detail.to.Y - tex_detail.from.Y) * screenHeight) / 100.0f), SpriteEffects.None, 0.5f);
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.NonPremultiplied);


            /*
            // Update the FPS text
            frameCount++;
            if (fpsClock.ElapsedMilliseconds > 1000.0f)
            {
                fpsText = string.Format("{0:F2} FPS\n" + gameTime.TotalGameTime.TotalMilliseconds, (float)frameCount * 1000 / fpsClock.ElapsedMilliseconds);
                frameCount = 0;
                fpsClock.Restart();
            }

            StringBuilder sb1 = new StringBuilder();
            if (tex_detail != null) sb1.AppendLine("Pressed keys:  " + tex_detail.ToString());

            foreach(Keys key in keys)
                sb1.AppendFormat("Key: {0}, Code: {1}\n", key, (int)key);

            */
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Left button  : {0}\n", mouseState.LeftButton);
            sb.AppendFormat("Middle button: {0}\n", mouseState.MiddleButton);
            sb.AppendFormat("Right button : {0}\n", mouseState.RightButton);
            sb.AppendFormat("XButton1     : {0}\n", mouseState.XButton1);
            sb.AppendFormat("XButton2     : {0}\n", mouseState.XButton2);

            // the mouse coordinates are in range [0; 1] relative to window.
            // any coordinates outside of the game window or control are clamped to this range
            // on Windows 8 platform it may not get to the values exactly 0 or 1 because of "active corners" feature of the OS.
            sb.AppendFormat("X            : {0}\n", mouseState.X);
            sb.AppendFormat("Y            : {0}\n", mouseState.Y);

            

            sb.AppendFormat("Screen X     : {0}\n", mouseState.X * screenWidth);
            sb.AppendFormat("Screen Y     : {0}\n", mouseState.Y * screenHeight);

            sb.AppendFormat("Wheel        : {0}\n", mouseState.WheelDelta);
            sb.AppendFormat("View         : " + pos+"_"+view+"\n");
            //spriteBatch.DrawString(arial16BMFont, sb.ToString(), new Vector2(8, 100), Color.White);

            /*
            // Render the text
            
            spriteBatch.DrawString(arial16BMFont, fpsText, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(arial16BMFont, sb1.ToString(), new Vector2(8, 32), Color.White);

            spriteBatch.DrawString(arial16BMFont, sb.ToString(), new Vector2(8, 400), Color.White);
             */
            spriteBatch.End();

            // Handle base.Draw
            base.Draw(gameTime);
        }



        static double framemilisec = 0;
        static double lastframemilisec = 0;
        static float newframemilisec = 0;
        protected override void Update(GameTime gameTime)
        {
            framemilisec = gameTime.TotalGameTime.TotalMilliseconds;
            if (framemilisec != lastframemilisec)
            {
                newframemilisec = (float)(framemilisec - lastframemilisec);
                lastframemilisec = framemilisec;
            }

            toolbox.framecalculation.set_timemsec(newframemilisec);
            // read the current keyboard state
            keyboardState = keyboarManager.GetState();
            mouseState = mouseManager.GetState();

            base.Update(gameTime);
        }
    }
}
