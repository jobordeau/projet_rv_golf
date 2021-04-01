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

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Space space;
        Model level;
        private Model ball;
        private BoundingBox boundingArrive;
        private Vector3[] vertices;
        private int[] indices;
        public KeyboardState KeyboardState;
        public MouseState MouseState;

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
            base.Initialize();
        }



        protected override void LoadContent()
        {
            manager = new GameManager(this);
            manager.AddPlayer(new Player(this, "jojo", "ball_red", new Vector3(0, -20, 0)));
            manager.LoadGame();
            var model = Content.Load<Model>("arrive");
            ModelDataExtractor.GetVerticesAndIndicesFromModel(model, out vertices, out indices);
            var mesh = new StaticMesh(vertices, indices, new AffineTransform(new Vector3(0, -40, 0)));
            boundingArrive = mesh.BoundingBox;
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
            if (_launched)
            {
                Camera.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

                if (KeyboardState.IsKeyDown(Keys.Escape))
                {
                    Exit();
                    return;
                }

                //On peut taper uniquement quand la vitesse de la balle est basse
                if (manager.Space.Entities[0].LinearVelocity.Length() < 50)
                {
                    if (MouseState.LeftButton == ButtonState.Pressed)
                    {
                        manager.Space.Entities[0].LinearVelocity += Camera.Camera.ViewDirection;
                    }

                    if (MouseState.RightButton == ButtonState.Pressed)
                    {
                        manager.Space.Entities[0].LinearVelocity -= Camera.Camera.ViewDirection;
                    }

                    nbHits++;
                }

                if (manager.Space.Entities[0].CollisionInformation.BoundingBox.Intersects(boundingArrive))
                {
                    Exit();
                    return;
                }

                if (manager.Space.Entities[0].Position.Y < -50f || KeyboardState.IsKeyDown(Keys.R))
                {
                    manager.Space.Entities[0].LinearVelocity = Vector3.Zero;
                    manager.Space.Entities[0].Position = Vector3.Zero;
                }

                manager.Space.Update();
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
