using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using SonicSharp.Players;
using System;
using System.Collections.Generic;
using System.IO;

namespace SonicSharp
{
    public class GameWindow : Game
    {
        // Variables/Constants
        public static GraphicsDeviceManager Graphics;
        public static SpriteBatch SpriteBatch;
        public static Stage CurrentStage;
        public static Player[] LocalPlayers = new Player[MaxLocalPlayers];

        public static Camera2D[] Cameras = new Camera2D[MaxLocalPlayers];
        public static Viewport[] Viewports = new Viewport[MaxLocalPlayers];
        public static Dictionary<string, Input> Inputs = new Dictionary<string, Input>();

        public string ContentDirectory
        {
            get => Path.Combine(Program.StartupPath, Content.RootDirectory);
        }

        public static Random Rand = new Random();
        public static int LocalPlayerCount { get; private set; } = 0;
        public static KeyboardState KeyState, PrevKeyState;
        public static GamePadState[] PadStates = new GamePadState[MaxLocalPlayers],
            PrevPadStates = new GamePadState[MaxLocalPlayers];

        public const int MaxLocalPlayers = 4;

        // Constructors
        public GameWindow()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        // Methods
        public static void AddLocalPlayer(Player localPlayer)
        {
            if (LocalPlayerCount < MaxLocalPlayers)
            {
                Cameras[LocalPlayerCount] = new Camera2D(Program.Window.GraphicsDevice);
                LocalPlayers[LocalPlayerCount++] = localPlayer;
                UpdateViewports();
            }
        }

        public static void UpdateViewports()
        {
            int x = 0, y = 0;
            int sectionWidth = Graphics.PreferredBackBufferWidth /
                ((LocalPlayerCount > 2) ? 2 : 1);

            int sectionHeight = Graphics.PreferredBackBufferHeight /
                ((LocalPlayerCount > 1) ? 2 : 1);

            for (int i = 0; i < LocalPlayerCount; i++)
            {
                Viewports[i] = new Viewport(x, y, sectionWidth, sectionHeight);
                x += sectionWidth;

                if (x >= Graphics.PreferredBackBufferWidth)
                {
                    x = 0;
                    y += sectionHeight;
                }
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Setup Inputs
            Inputs.Add("Up", new Input(Keys.W, Buttons.LeftThumbstickUp));
            Inputs.Add("Left", new Input(Keys.A, Buttons.LeftThumbstickLeft));
            Inputs.Add("Down", new Input(Keys.S, Buttons.LeftThumbstickDown));
            Inputs.Add("Right", new Input(Keys.D, Buttons.LeftThumbstickRight));

            Inputs.Add("AltUp", new Input(Keys.Up, Buttons.DPadUp));
            Inputs.Add("AltLeft", new Input(Keys.Left, Buttons.DPadLeft));
            Inputs.Add("AltDown", new Input(Keys.Down, Buttons.DPadDown));
            Inputs.Add("AltRight", new Input(Keys.Right, Buttons.DPadRight));

            Inputs.Add("Jump", new Input(Keys.Space, Buttons.A));
            Inputs.Add("Start", new Input(Keys.Enter, Buttons.Start));
            Inputs.Add("Back", new Input(Keys.Escape, Buttons.B));

            // TODO: Add More Inputs

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO
            var plr = new Sonic();
            plr.Init();
            AddLocalPlayer(plr);

            // TODO: REMOVE THIS
            plr.Position.X = 4;
            plr.Position.Y = -400;
            CurrentStage = new Stage(Content.Load<Texture2D>("Stages\\TestZone\\TileMap"));
            CurrentStage.Load(Path.Combine(ContentDirectory, "Stages",
                "TestZone", $"TestZone{Stage.Extension}"));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            // Update Input States
            KeyState = Keyboard.GetState();
            for (int i = 0; i < MaxLocalPlayers; ++i)
            {
                PadStates[i] = GamePad.GetState(i);
            }

            // DEBUG
            if (KeyState.IsKeyDown(Keys.Escape) ||
                PadStates[0].Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }

            if (KeyState.IsKeyDown(Keys.P) && !PrevKeyState.IsKeyDown(Keys.P))
            {
                var plr = new Sonic((Input.Devices)LocalPlayerCount,
                    new Color(Rand.Next(0, 256), Rand.Next(0, 256),
                    Rand.Next(0, 256), 255));

                plr.Init();
                AddLocalPlayer(plr);
            }

            // Update Players
            for (int i = 0; i < LocalPlayerCount; ++i)
            {
                LocalPlayers[i].Update();
            }

            // Update Previous Input States
            PrevKeyState = KeyState;
            for (int i = 0; i < MaxLocalPlayers; ++i)
            {
                PrevPadStates[i] = PadStates[i];
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw Players
            for (int i = 0; i < LocalPlayerCount; ++i)
            {
                var cam = Cameras[i];
                var vp = Viewports[i];
                var plr = LocalPlayers[i];

                // Move Camera
                // TODO: Make this better lol
                var camPos = cam.Position;
                if (plr.Position.X >= (camPos.X + vp.Width) - (vp.Width / 2))
                {
                    float dif = plr.Position.X - ((camPos.X + vp.Width) - (vp.Width / 2));
                    camPos.X += Math.Min(dif, 16);
                }
                else if (plr.Position.X <= camPos.X + (vp.Width / 2.22f))
                {
                    float dif = (camPos.X + (vp.Width / 2.22f)) - plr.Position.X;
                    camPos.X -= Math.Min(dif, 16);
                }

                if (plr.IsFalling)
                {
                    if (plr.Position.Y >= (camPos.Y + vp.Height) - (vp.Height / 1.75f))
                    {
                        float dif = plr.Position.Y - ((camPos.Y +
                            vp.Height) - (vp.Height / 1.75f));

                        camPos.Y += Math.Min(dif, 16);
                    }
                    else if (plr.Position.Y <= camPos.Y + (vp.Height / 3.5f))
                    {
                        float dif = (camPos.Y + (vp.Height / 3.5f)) - plr.Position.Y;
                        camPos.Y -= Math.Min(dif, 16);
                    }
                }
                else
                {
                    float dif = plr.Position.Y - (camPos.Y + (vp.Height / 2.33f));
                    if (dif != 0)
                    {
                        camPos.Y += Math.Min(Math.Abs(dif), (plr.YSpeed <= 6) ?
                            6 : 16) * Math.Sign(dif);
                    }
                }

                cam.Position = camPos;

                // Draw Everything
                GraphicsDevice.Viewport = vp;
                SpriteBatch.Begin(transformMatrix: cam.GetViewMatrix(),
                    samplerState: SamplerState.PointClamp);

                if (CurrentStage != null)
                    CurrentStage.Draw(cam);

                SpriteBatch.End();
            }
        }
    }
}