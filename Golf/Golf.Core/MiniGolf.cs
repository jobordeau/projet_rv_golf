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
using Mouse = Microsoft.Xna.Framework.Input.Mouse;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = BEPUutilities.Vector3;

namespace Golf.Core
{
    /// <summary>
    /// Class defining the game logic
    /// </summary>
    public class MiniGolf : Game
    {
        /// <summary>
        /// The current graphic device
        /// </summary>
        public GraphicsDeviceManager Graphics;

        public SpriteBatch SpriteBatch;
        public Camera CameraClassic { get; private set; }
        public ChaseCameraControlScheme Camera { get; private set; }
        public MouseState MouseState { get; private set; }
        private KeyboardState _keyboardState;
        private MouseState _lastMouseState;
        private GameManager _manager;
        private SpriteFont _hudFont;
        private readonly Vector2 _baseScreenSize = new Vector2(1080, 720);
        private Texture2D _background;
        private bool _launched = false;
        private ChargeBar _chargeBar;
        private IMGUI _ui;
        private SoundManager _sound;

        public MiniGolf()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Graphics.ToggleFullScreen();
            base.Initialize();
        }


        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            //Create the different managers
            _chargeBar = new ChargeBar(this, SpriteBatch, Graphics, new Vector2((1900 * Window.ClientBounds.Width) / 1980, (1100 * Window.ClientBounds.Height) / 1080));

            _sound = new SoundManager(this);

            _manager = new GameManager(this);
            _manager.AddPlayer(new Player(this, "jojo", "ball_red", new Vector3(0, -20, 0)));
            _manager.LoadGame(1);

            //Create cameras responsible of following the ball
            CameraClassic = new Camera(Vector3.Zero, 0, 0,
                BEPUutilities.Matrix.CreatePerspectiveFieldOfViewRH(MathHelper.PiOver4,
                    Graphics.PreferredBackBufferWidth / (float)Graphics.PreferredBackBufferHeight, .1f, 10000));
            Camera = new ChaseCameraControlScheme(_manager.Space.Entities[0], new Vector3(0, 7, 0), false, 50f, CameraClassic,
                this);

            //Adding event for sound managing
            _manager.MainPlayer.Ball.Form.CollisionInformation.Events.ContactCreated += Events_ContactCreated;
            _manager.MainPlayer.Ball.Form.CollisionInformation.Events.DetectingInitialCollision += Events_DetectingInitialCollision;
            _manager.MainPlayer.Ball.Form.CollisionInformation.Events.CollisionEnded += Events_CollisionEnded;

            //Loading font and background for HUD
            FontSystem fontSystem = FontSystemFactory.Create(GraphicsDevice, 2048, 2048);
            fontSystem.AddFont(TitleContainer.OpenStream($"{Content.RootDirectory}/font-file.ttf"));

            _background = Content.Load<Texture2D>("Sprites/background_menu");

            //Setup the HUD
            GuiHelper.Setup(this, fontSystem);

            _hudFont = Content.Load<SpriteFont>("Hud");

            _ui = new IMGUI();
        }

        private void Events_CollisionEnded(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            _sound.RollStop();
        }

        private void Events_DetectingInitialCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            _sound.Impact(sender.Entity);
        }

        private void Events_ContactCreated(EntityCollidable sender, Collidable other, CollidablePairHandler pair, ContactData contact)
        {
            if (sender.Entity.CollisionInformation.BoundingBox.Intersects(_manager.MainLevel.BoundingLevel) && _manager.MainPlayer.Ball.IsMoving())
            {
                _sound.Roll(sender.Entity);
            }
             
        }

        protected override void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
            Entity mainEntity = _manager.MainPlayer.Ball.Form;

            //Managing the different interactions following the current state of the game
            if (_launched)
            {
                //Updating the camera
                Camera.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            if (_keyboardState.IsKeyDown(Keys.Escape))
            {
                //Quit using Escape
                Exit();
                return;
            }
            
            //Managing the loading of the shot
            if (!_manager.MainPlayer.Ball.IsMoving())
            {
                if (MouseState.LeftButton == ButtonState.Pressed)
                {
                    if (_chargeBar.Charge <= _chargeBar.ChargeMax)
                    {
                        _chargeBar.Charge += 0.1f * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    }
                    if (_chargeBar.Charge >= _chargeBar.ChargeMax)
                    {
                        _chargeBar.Charge = _chargeBar.ChargeMax;
                    }
                }
                if(_lastMouseState.LeftButton == ButtonState.Pressed)
                {
                    //Hitting the ball
                    if (MouseState.LeftButton == ButtonState.Released)
                    {
                        mainEntity.LinearVelocity += Camera.Camera.ViewDirection * _chargeBar.Charge;
                        _sound.Hit(_chargeBar);
                        _chargeBar.Charge = 0;
                        _manager.NbHits++;
                    }
                }
            }
            else
            {
                _chargeBar.Charge = 0;
            }
            _lastMouseState = MouseState;

                //Managing the hole and the finish of the course
                if (mainEntity.CollisionInformation.BoundingBox.Intersects(_manager.MainLevel.BoundingArrive) && !_manager.Loading)
                {
                    _sound.Success();
                    BoundingBox box = new BoundingBox(new Vector3(-1, -21, -1), new Vector3(1, -19, 1));
                    mainEntity.CollisionInformation.BoundingBox = box;
                    mainEntity.Position = Vector3.Zero;
                    mainEntity.LinearVelocity = Vector3.Zero;
                    _manager.LoadNextLevel();
                }

                //Managing a fall and the reset of position in case of bug
                if (mainEntity.Position.Y < -50f || _keyboardState.IsKeyDown(Keys.R))
                {
                    _sound.Out();
                    mainEntity.LinearVelocity = Vector3.Zero;
                    mainEntity.Position = Vector3.Zero;
                }

                _chargeBar.Update(gameTime);
                _manager.Space.Update();
                base.Update(gameTime);
            }
            //If the game is not launched or ended the start HUD is showed
            else if (!_launched && !_manager.Ended )
            {
                //Using Apos.GUI to show the HUD
                GuiHelper.UpdateSetup();
                _ui.UpdateAll(gameTime);

                // Creating the HUD
                Panel.Push().XY = new Vector2(Graphics.PreferredBackBufferWidth / 2, Graphics.PreferredBackBufferHeight / 2);
                Label.Put("MiniGolf");
                if (Button.Put("Launch Game").Clicked)
                {
                    IsMouseVisible = false;
                    _launched = true;
                    _sound.PlayAmbiant();
                }
                if (Button.Put("Quit").Clicked)
                {
                    Exit();
                }

                Panel.Pop();


                GuiHelper.UpdateCleanup();
            }

            //Showing the score board at the end using Apos.GUI
            if (_manager.Ended)
            {
                int i = 1;
                GuiHelper.UpdateSetup();
                _ui.UpdateAll(gameTime);

                // Create your UI.
                Panel.Push().XY = new Vector2(Graphics.PreferredBackBufferWidth / 2, Graphics.PreferredBackBufferHeight / 2);
                Label.Put("Course finished");
                Label.Put("Player : " + _manager.MainPlayer.Name);
                foreach (var score in _manager.MainPlayer.Score)
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

                if (_keyboardState.IsKeyDown(Keys.Escape))
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
            //Define options to draw on the sprite
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.Clear(Color.LightSkyBlue);

            //Define the start of the draw on the sprite with a specific order
            SpriteBatch.Begin(SpriteSortMode.BackToFront, null);
            //If not launched we draw the start HUD
            if (!_launched && !_manager.Ended)
            {
                SpriteBatch.Draw(_background, new Rectangle(0, 0, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.0f);
                SpriteBatch.End();
                SpriteBatch.Begin();
                _ui.Draw(gameTime);
            }
            //If launched we draw the HUD, the charge bar and the game
            else
            {
                DrawHud();
                base.Draw(gameTime);
                _chargeBar.Draw(gameTime);
            }
            //If the game is finished we draw the score board
            if (_manager.Ended)
            {
                _sound.Clap();
                _launched = false;
                IsMouseVisible = true;
                _ui.Draw(gameTime);
                _sound.StopAmbiant();
            }
            SpriteBatch.End();
        }

        /// <summary>
        /// Drawing the HUD when the game is started
        /// </summary>
        private void DrawHud()
        {
            //Getting the hud locations
            Rectangle titleSafeArea = GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 hudLocation = new Vector2(titleSafeArea.X, titleSafeArea.Y);
            Vector2 center = new Vector2(_baseScreenSize.X / 2, _baseScreenSize.Y / 2);

            //Showing the current level
            string levelString = "Level: " + _manager.LevelIndex;
            Color levelColor;
            levelColor = Color.Black;
            DrawShadowedString(_hudFont, levelString, hudLocation, levelColor);

            //Showing the current number of hits
            float levelHeight = _hudFont.MeasureString(levelString).Y;
            DrawShadowedString(_hudFont, "Hits : " + _manager.NbHits.ToString(), hudLocation + new Vector2(0.0f, levelHeight * 1.2f), Color.Yellow);
        }

        
        /// <summary>
        /// Method to draw a string with a shadow
        /// </summary>
        /// <param name="font">the current font</param>
        /// <param name="value">string to draw</param>
        /// <param name="position">the position you want to draw</param>
        /// <param name="color">color of the string</param>
        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color)
        {
            SpriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            SpriteBatch.DrawString(font, value, position, color);
        }

    }
}

