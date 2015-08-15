using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Reflection; //Let's take some time to reflect...

namespace SonicSharp
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        //Graphics-related variables
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static Font font;
        public static Color bgcolor = Color.Black;
        public static int scalemodifier = 2;
        public static bool fullscreen = false;
        
        private int virtualscreenwidth = 800, virtualscreenheight = 600;
        private Point windowstartpos;
        private Vector3 scale;

        //Other variables
        public static GameState gamestate = GameState.loading;
        public static KeyboardState kbst, prevkbst; //TODO: Add controller support
        public static List<Player> players = new List<Player>();
        public static string versionstring = "DEV 2.1", startdir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public enum GameState { loading, inlevel }

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "SoniC#";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            windowstartpos = Window.Position;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = new Font(Content.Load<Texture2D>("Sprites\\HUD\\HUDfont"), new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ':', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', ' ', '.' }, new Rectangle[] { new Rectangle(2,0,6,10), new Rectangle(14,0,4,11), new Rectangle(24,0,7,11), new Rectangle(35,0,7,11), new Rectangle(46,0,7,11), new Rectangle(57,0,7,11), new Rectangle(68,0,7,11), new Rectangle(79,0,7,11), new Rectangle(90,0,7,11), new Rectangle(101,0,7,11), new Rectangle(114,0,3,11), new Rectangle(123,0,7,11), new Rectangle(134,0,7,11), new Rectangle(145,0,7,11), new Rectangle(156,0,7,11), new Rectangle(167,0,7,11), new Rectangle(178,0,7,11), new Rectangle(189,0,7,11), new Rectangle(200,0,7,11), new Rectangle(213,0,3,11), new Rectangle(222,0,7,11), new Rectangle(232,0,8,11), new Rectangle(244,0,6,11), new Rectangle(253,0,10,11), new Rectangle(265,0,9,11), new Rectangle(277,0,7,11), new Rectangle(288,0,7,11), new Rectangle(299,0,7,11), new Rectangle(310,0,7,11), new Rectangle(321,0,7,11), new Rectangle(332,0,7,11), new Rectangle(343,0,7,11), new Rectangle(354,0,7,11), new Rectangle(363,0,10,11), new Rectangle(375,0,8,11), new Rectangle(387,0,7,11), new Rectangle(397,0,8,11), new Rectangle(407,0,11,11), new Rectangle(422,0,3,11) });

            Thread loadcontentthread = new Thread(new ThreadStart(LoadContentAsync));
            loadcontentthread.Start();
        }

        /// <summary>
        /// A function which loads the game's content asynchronously.
        /// </summary>
        private void LoadContentAsync()
        {
            players.Add(new Sonic(20,20)); //TODO: Remove this temporary line!
            Level.Load("Angel Island Zone","AI1.tmx"); //TODO: Remove this temporary line!
            gamestate = GameState.inlevel;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            kbst = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kbst.IsKeyDown(Keys.Escape)) Exit(); //TODO: Remove this line

            if (gamestate == GameState.inlevel)
            {
                Level.Update();
            }

            if (kbst.IsKeyDown(Keys.F11) && !prevkbst.IsKeyDown(Keys.F11))
            {
                fullscreen = !fullscreen;
                Window.IsBorderless = fullscreen;
                Window.Position = (fullscreen) ? Point.Zero : windowstartpos;
                graphics.PreferredBackBufferWidth = (fullscreen) ? graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width:800;
                graphics.PreferredBackBufferHeight = (fullscreen) ? graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height:480;
                graphics.ApplyChanges();
            }

            prevkbst = kbst;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(bgcolor);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: GetDrawingMatrix());

            if (gamestate == GameState.loading)
            {
                font.Draw("LOADING...", Camera.pos.X+(Program.game.Window.ClientBounds.Width-font.GetWidth("LOADING... ")*scalemodifier)/scalemodifier, Camera.pos.Y+(Program.game.Window.ClientBounds.Height-font.GetHeight("LOADING...")/scalemodifier)/scalemodifier);
            }
            else if (gamestate == GameState.inlevel)
            {
                Level.Draw();
                font.Draw("CAMERA POSITION: " + Camera.pos.X.ToString() + " : " + Camera.pos.Y.ToString(),Camera.pos.X/scalemodifier,Camera.pos.Y/scalemodifier);
            }

            virtualscreenwidth = Window.ClientBounds.Width;
            virtualscreenheight = Window.ClientBounds.Height;

            var scaleX = GraphicsDevice.Viewport.Width / virtualscreenwidth;
            var scaleY = GraphicsDevice.Viewport.Height / virtualscreenheight;
            scale = new Vector3(scaleX * scalemodifier, scaleY * scalemodifier, 1.0f);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Gets the Matrix used for drawing to the screen.
        /// </summary>
        Matrix GetDrawingMatrix()
        {
            return Matrix.Multiply(Matrix.CreateScale(scale), Matrix.CreateTranslation(Camera.pos.X * -1, Camera.pos.Y * -1, 0));
        }
    }
}
