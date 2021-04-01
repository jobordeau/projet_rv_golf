using System;
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.CollisionTests;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysicsDemos;
using BEPUutilities;
using Golf.Core.ModelGolf;
using Golf.Core.ModelGolf.Cam;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BoundingBox = BEPUutilities.BoundingBox;
using MathHelper = BEPUutilities.MathHelper;
using Apos;
using Apos.Gui;
using FontStashSharp;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = BEPUutilities.Vector3;

namespace Golf.Core
{
    public class MiniGolf : Game
    {
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public KeyboardState KeyboardState;
        public MouseState MouseState;
        public MouseState LastMouseState;
        public GameManager manager;
        public ChaseCameraControlScheme Camera;
        public Camera CameraClassic;
        private int nbHits=0;
        private Vector3 lastPosition;
        IMGUI _ui;
        private bool _launched = false;

        public MiniGolf()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.ToggleFullScreen();
            base.Initialize();
        }



        protected override void LoadContent()
        {
            manager = new GameManager(this);
            manager.AddPlayer(new Player(this, "jojo", "ball_red", new Vector3(0, -20, 0)));
            manager.LoadGame();
            CameraClassic = new Camera(Vector3.Zero, 0, 0,
                BEPUutilities.Matrix.CreatePerspectiveFieldOfViewRH(MathHelper.PiOver4,
                    graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight, .1f, 10000));
            Camera = new ChaseCameraControlScheme(manager.Space.Entities[0], new Vector3(0, 7, 0), false, 50f, CameraClassic,
                this);
            
            FontSystem fontSystem = FontSystemFactory.Create(GraphicsDevice, 2048, 2048);
            fontSystem.AddFont(TitleContainer.OpenStream($"{Content.RootDirectory}/font-file.ttf"));

            GuiHelper.Setup(this, fontSystem);

            _ui = new IMGUI();
        }


        protected override void Update(GameTime gameTime)
        {
            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
            Entity mainEntity = manager.MainPlayer.Ball.Form;

            if (_launched)
            {
                Camera.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

                if (KeyboardState.IsKeyDown(Keys.Escape))
                {
                    Exit();
                    return;
                }

                if (!manager.MainPlayer.Ball.IsMoving())
                {
                    if (MouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (manager.Charge <= manager.CHARGE_MAX)
                        {
                            manager.Charge += 0.1f * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        }
                        if (manager.Charge >= manager.CHARGE_MAX)
                        {
                            manager.Charge = manager.CHARGE_MAX;
                        }
                    }
                    if (LastMouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (MouseState.LeftButton == ButtonState.Released)
                        {
                            mainEntity.LinearVelocity += Camera.Camera.ViewDirection * manager.Charge;
                            nbHits++;
                        }
                    }

                }
                else
                {
                    manager.Charge = 0;
                }
                LastMouseState = MouseState;

                if (mainEntity.CollisionInformation.BoundingBox.Intersects(manager.MainLevel.BoundingArrive))
                {
                    Exit();
                    return;
                }

                if (mainEntity.Position.Y < -50f || KeyboardState.IsKeyDown(Keys.R))
                {
                    mainEntity.LinearVelocity = Vector3.Zero;
                    mainEntity.Position = Vector3.Zero;
                }

                manager.Space.Update();
                base.Update(gameTime);
            }


            GuiHelper.UpdateSetup();
            _ui.UpdateAll(gameTime);

            // Create your UI.
            Panel.Push().XY = new Vector2(700,300);
            Label.Put("MiniGolf");
            if (Button.Put("Launch Game").Clicked)
            {
                IsMouseVisible = false;
                _launched = true;
            }
            if (Button.Put("Quit").Clicked)
            {
                Exit();
            }
            Panel.Pop();


            // Call UpdateCleanup at the end.
            GuiHelper.UpdateCleanup();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSkyBlue);

            if (!_launched)
            { 
                _ui.Draw(gameTime);
            }
            else
            {
                base.Draw(gameTime);
            }
        }
    }
}
