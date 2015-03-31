/*
 * soniC#
 *  A C#/MonoGame engine inspried by the Sonic the Hedgehog franchise!
 *  
 * This engine was created strictly for fun/educational use and should not be used
 * commercially. It is under the Creative Commons Attribution-Noncommercial-ShareAlike-3.0 License
 * (Found Here: http://creativecommons.org/licenses/by-nc-sa/3.0/) and as such legally dis-allows
 * commercial use, amoungst other things. For more information please refer to the LICENSE.txt file
 * provided with the project's official repository at the root as well as the Visual Studio solution.
 * 
 * Thanks for reading, and enjoy the engine! :)
*/

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace SonicSharp
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Game
    {
        #region Variable Declarations
        
        #region Public static variables

        //Graphics-drawing
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        
        //Screen Resolution/Size
        public static int virtualscreenwidth = 960;
        public static int virtualscreenheight = 540;
        public static bool fullscreen = false;

        #endregion

        #region Other variables

        Vector3 scale;
        Texture2D running;

        #endregion

        #endregion

        public Main(): base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Assets";
            Window.Title = "soniC#";
            Window.AllowUserResizing = true;

            graphics.PreferredBackBufferWidth = virtualscreenwidth;
            graphics.PreferredBackBufferHeight = virtualscreenheight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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

            running = Content.Load<Texture2D>("Sprites\\running1");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.F11))
            {
                fullscreen = !fullscreen;

                if (fullscreen)
                {
                    virtualscreenwidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    virtualscreenheight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                    Window.IsBorderless = true;
                    Window.Position = new Point(0, 0);
                }
                else
                {
                    virtualscreenwidth = 960;
                    virtualscreenheight = 450;
                    Window.IsBorderless = false;
                }

                graphics.PreferredBackBufferWidth = virtualscreenwidth;
                graphics.PreferredBackBufferHeight = virtualscreenheight;
                graphics.ApplyChanges();
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                Camera.campos.X++;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                Camera.campos.X--;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Matrix scaleMatrix = GetDrawingMatrix();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, scaleMatrix);
            spriteBatch.Draw(running,new Vector2(0,0),Color.White);
            spriteBatch.End();

            //Re-scale the window incase the user resized it.
            var scaleX = (float)GraphicsDevice.Viewport.Width / (float)virtualscreenwidth;
            var scaleY = (float)GraphicsDevice.Viewport.Height / (float)virtualscreenheight;
            scale = new Vector3(scaleX * 2, scaleY * 2, 1.0f);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Gets the Matrix used for drawing to the screen.
        /// </summary>
        Matrix GetDrawingMatrix()
        {
            return Matrix.Multiply(Matrix.CreateScale(scale), Matrix.CreateTranslation(Camera.campos.X * -1, Camera.campos.Y * -1, 0));
        }
    }
}
