using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.CollisionTests;
using BEPUphysics.Entities;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysicsDemos;
using Golf.Core.ModelGolf;
using Golf.Core.ModelGolf.Cam;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BoundingBox = BEPUutilities.BoundingBox;
using MathHelper = BEPUutilities.MathHelper;
using Apos.Gui;
using FontStashSharp;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = BEPUutilities.Vector3;

namespace Golf.Core
{
    public class MiniGolf : Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public Camera CameraClassic { get; private set; }
        public ChaseCameraControlScheme Camera { get; private set; }

        public MouseState MouseState { get; private set; }
        KeyboardState keyboardState;
        MouseState lastMouseState;

        GameManager manager;


        SpriteFont hudFont;
        Vector2 baseScreenSize = new Vector2(1080, 720);
        Texture2D background;

        bool _launched = false;
        

        ChargeBar chargeBar;
        IMGUI _ui;

        SoundManager sound;

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
            spriteBatch = new SpriteBatch(GraphicsDevice);

            chargeBar = new ChargeBar(this, spriteBatch, graphics, new Vector2((1900 * Window.ClientBounds.Width) / 1980, (1100 * Window.ClientBounds.Height) / 1080));

            sound = new SoundManager(this);

            manager = new GameManager(this);
            manager.AddPlayer(new Player(this, "jojo", "ball_red", new Vector3(0, -20, 0)));
            manager.LoadGame(1);
            CameraClassic = new Camera(Vector3.Zero, 0, 0,
                BEPUutilities.Matrix.CreatePerspectiveFieldOfViewRH(MathHelper.PiOver4,
                    graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight, .1f, 10000));
            Camera = new ChaseCameraControlScheme(manager.Space.Entities[0], new Vector3(0, 7, 0), false, 50f, CameraClassic,
                this);

            manager.MainPlayer.Ball.Form.CollisionInformation.Events.ContactCreated += Events_ContactCreated;
            manager.MainPlayer.Ball.Form.CollisionInformation.Events.DetectingInitialCollision += Events_DetectingInitialCollision;
            manager.MainPlayer.Ball.Form.CollisionInformation.Events.CollisionEnded += Events_CollisionEnded;

            FontSystem fontSystem = FontSystemFactory.Create(GraphicsDevice, 2048, 2048);
            fontSystem.AddFont(TitleContainer.OpenStream($"{Content.RootDirectory}/font-file.ttf"));

            //hudFont = Content.Load<SpriteFont>("font-file");
            background = Content.Load<Texture2D>("Sprites/background_menu");

            GuiHelper.Setup(this, fontSystem);

            hudFont = Content.Load<SpriteFont>("Hud");

            _ui = new IMGUI();
        }

        private void Events_CollisionEnded(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            sound.RollStop();
        }

        private void Events_DetectingInitialCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            sound.Impact(sender.Entity);
        }

        private void Events_ContactCreated(EntityCollidable sender, Collidable other, CollidablePairHandler pair, ContactData contact)
        {
            if (sender.Entity.CollisionInformation.BoundingBox.Intersects(manager.MainLevel.BoundingLevel) && manager.MainPlayer.Ball.IsMoving())
            {
                sound.Roll(sender.Entity);
            }
             
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
            Entity mainEntity = manager.MainPlayer.Ball.Form;

            if (_launched)
            {
                Camera.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
                return;
            }
            
            if (!manager.MainPlayer.Ball.IsMoving())
            {
                if (MouseState.LeftButton == ButtonState.Pressed)
                {
                    if (chargeBar.Charge <= chargeBar.CHARGE_MAX)
                    {
                        chargeBar.Charge += 0.1f * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    }
                    if (chargeBar.Charge >= chargeBar.CHARGE_MAX)
                    {
                        chargeBar.Charge = chargeBar.CHARGE_MAX;
                    }
                }
                if(lastMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (MouseState.LeftButton == ButtonState.Released)
                    {
                        mainEntity.LinearVelocity += Camera.Camera.ViewDirection * chargeBar.Charge;
                        sound.Hit(chargeBar);
                        chargeBar.Charge = 0;
                        manager.NbHits++;
                    }
                }
            }
            else
            {
                chargeBar.Charge = 0;
            }
            lastMouseState = MouseState;

                if (mainEntity.CollisionInformation.BoundingBox.Intersects(manager.MainLevel.BoundingArrive) && !manager.Loading)
                {
                    sound.Success();
                    BoundingBox box = new BoundingBox(new Vector3(-1, -21, -1), new Vector3(1, -19, 1));
                    mainEntity.CollisionInformation.BoundingBox = box;
                    mainEntity.Position = Vector3.Zero;
                    mainEntity.LinearVelocity = Vector3.Zero;
                    manager.LoadNextLevel();
                }

                if (mainEntity.Position.Y < -50f || keyboardState.IsKeyDown(Keys.R))
                {
                    sound.Out();
                    mainEntity.LinearVelocity = Vector3.Zero;
                    mainEntity.Position = Vector3.Zero;
                }

                chargeBar.Update(gameTime);
                manager.Space.Update();
                base.Update(gameTime);
            }
            else if (!_launched && !manager.Ended )
            {
                GuiHelper.UpdateSetup();
                _ui.UpdateAll(gameTime);

                // Create your UI.
                Panel.Push().XY = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
                Label.Put("MiniGolf");
                if (Button.Put("Launch Game").Clicked)
                {
                    IsMouseVisible = false;
                    _launched = true;
                    sound.PlayAmbiant();
                }
                if (Button.Put("Quit").Clicked)
                {
                    Exit();
                }
                Panel.Pop();


                // Call UpdateCleanup at the end.
                GuiHelper.UpdateCleanup();
            }

            if (manager.Ended)
            {
                int i = 1;
                GuiHelper.UpdateSetup();
                _ui.UpdateAll(gameTime);

                // Create your UI.
                Panel.Push().XY = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
                Label.Put("Course finished");
                Label.Put("Player : " + manager.MainPlayer.Name);
                foreach (var score in manager.MainPlayer.Score)
                {
                    Label.Put($"Level {i} : "+ score.ToString());
                    i++;
                }
                Label.Put("Press Echap to quit");
                if (Button.Put("Quit").Clicked)
                {
                    Exit();
                    return;
                }
                Panel.Pop();

                // Call UpdateCleanup at the end.
                GuiHelper.UpdateCleanup();

                if (keyboardState.IsKeyDown(Keys.Escape))
                {
                    Exit();
                    return;
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.Clear(Color.LightSkyBlue);
            spriteBatch.Begin(SpriteSortMode.BackToFront, null);
            if (!_launched && !manager.Ended)
            {
                spriteBatch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.0f);
                spriteBatch.End();
                spriteBatch.Begin();
                _ui.Draw(gameTime);
            }
            else
            {
                DrawHud();
                base.Draw(gameTime);
                chargeBar.Draw(gameTime);
            }
            if (manager.Ended)
            {
                sound.Clap();
                _launched = false;
                IsMouseVisible = true;
                _ui.Draw(gameTime);
                sound.StopAmbiant();
            }
            spriteBatch.End();
        }

        private void DrawHud()
        {
            Rectangle titleSafeArea = GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 hudLocation = new Vector2(titleSafeArea.X, titleSafeArea.Y);
            //Vector2 center = new Vector2(titleSafeArea.X + titleSafeArea.Width / 2.0f,
            //                             titleSafeArea.Y + titleSafeArea.Height / 2.0f);

            Vector2 center = new Vector2(baseScreenSize.X / 2, baseScreenSize.Y / 2);

            // Draw time remaining. Uses modulo division to cause blinking when the
            // player is running out of time.
            string timeString = "Niveau: " + manager.LevelIndex;
            Color timeColor;
            timeColor= Color.Black;
            DrawShadowedString(hudFont, timeString, hudLocation, timeColor);

            // Draw score
            float timeHeight = hudFont.MeasureString(timeString).Y;
            DrawShadowedString(hudFont, "Coups: " + manager.NbHits.ToString(), hudLocation + new Vector2(0.0f, timeHeight * 1.2f), Color.Yellow);
        }

        

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, color);
        }

    }
}

