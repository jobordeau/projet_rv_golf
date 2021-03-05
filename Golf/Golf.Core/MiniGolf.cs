using Golf.Core.ModelGolf;
using Golf.Core.ModelGolf.Cam;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Golf.Core.ModelGolf.BoundingBox;
using Myra;
using Myra.Graphics2D.UI;

namespace Golf.Core
{
    public class MiniGolf : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<ModelRender> models = new List<ModelRender>();
        ModelManager modelManager;
        ChaseCamera camera;
        Ball ball;
        Level level;
        private SpriteFont hudFont;

        bool charge;

        private Desktop _desktop;

        MouseState lastMouseState;

        public MiniGolf()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            

            modelManager = new ModelManager();
            
        }

        // Called when the game should load its content
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            ball = new Ball(this, spriteBatch, graphics, new ModelRender(Content.Load<Model>("ball_red"), new Vector3(0, 400, 0), Vector3.Zero, new Vector3(250f),this));
            Ball ball2 = new Ball(this, spriteBatch, graphics, new ModelRender(Content.Load<Model>("ball_red"), new Vector3(200, 400, 0), Vector3.Zero, new Vector3(250f), this));
            level = new Level(this, spriteBatch, graphics, new ModelRender(Content.Load<Model>("StageTest"), Vector3.Zero, Vector3.Zero, new Vector3(50f), this));
            modelManager.AddModel(ball);
            modelManager.AddModel(ball2);
            modelManager.AddModel(level);
            
            

            hudFont = Content.Load<SpriteFont>("Fonts/Hud");


            MyraEnvironment.Game = this;

            var grid = new Grid
            {
                RowSpacing = 8,
                ColumnSpacing = 8
            };

            grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));

            var helloWorld = new Label
            {
                Id = "label",
                Text = "Hello, World!"
            };
            grid.Widgets.Add(helloWorld);

            // ComboBox
            var combo = new ComboBox
            {
                GridColumn = 1,
                GridRow = 0
            };

            combo.Items.Add(new ListItem("Red", Color.Red));
            combo.Items.Add(new ListItem("Green", Color.Green));
            combo.Items.Add(new ListItem("Blue", Color.Blue));
            grid.Widgets.Add(combo);

            // Button
            var button = new TextButton
            {
                GridColumn = 0,
                GridRow = 1,
                Text = "Show"
            };

            button.Click += (s, a) =>
            {
                var messageBox = Dialog.CreateMessageBox("Message", "Some message!");
                messageBox.ShowModal(_desktop);
            };

            grid.Widgets.Add(button);

            // Spin button
            var spinButton = new SpinButton
            {
                GridColumn = 1,
                GridRow = 1,
                Width = 100,
                Nullable = true
            };
            grid.Widgets.Add(spinButton);

            // Add it to the desktop
            _desktop = new Desktop();
            _desktop.Root = grid;

            
        }

        // Called when the game should update itself
        protected override void Update(GameTime gameTime)
        {
            
            
            modelManager.Update(gameTime);
            modelManager.HandleModelCollision(modelManager.elements[0]);
            updateModel(gameTime);
            camera.Update(ball);
            base.Update(gameTime);
        }

        

        void updateModel(GameTime gameTime)
        {
            // Get the new keyboard and mouse state

            

            KeyboardState keyState = Keyboard.GetState();
            Vector3 rotChange = new Vector3(0, 0, 0);
            // Determine on which axes the ship should be rotated on, if any
            if (keyState.IsKeyDown(Keys.Q))
                rotChange += new Vector3(0, 1, 0);
            if (keyState.IsKeyDown(Keys.D))
                rotChange += new Vector3(0, -1, 0);
            modelManager.elements[0]._model.Rotation += rotChange * .025f;
            // Determine what direction to move in
            Matrix rotation = Matrix.CreateFromYawPitchRoll(modelManager.elements[0]._model.Rotation.Y, modelManager.elements[0]._model.Rotation.X, modelManager.elements[0]._model.Rotation.Z);
            // If space isn't down, the ship shouldn't move
            MouseState mouseState = Mouse.GetState();

            Ball ball = (Ball)modelManager.elements[0];
            if (mouseState.LeftButton == ButtonState.Pressed && ball.Moving == false)
            {
                ball.Velocity = Vector3.Transform(Vector3.Forward, rotation) * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 4;
                ball.Velocity = Vector3.Round(ball.Velocity);
            }
                



        }



        // Called when the game should draw itself
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            modelManager.Draw(gameTime, camera);

            
            
            //GraphicsDevice.Clear(Color.Black);
            // _desktop.Render();
            
            //DrawHud();

            base.Draw(gameTime);

           
        }

        private void DrawHud()
        {
            Rectangle titleSafeArea = GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 hudLocation = new Vector2(titleSafeArea.X, titleSafeArea.Y);
            Vector2 center = new Vector2(titleSafeArea.X + titleSafeArea.Width / 2.0f,
                                       titleSafeArea.Y + titleSafeArea.Height / 2.0f);




            string timeString = "Position " + ball._model.Position.ToString();
            Color timeColor = Color.Black;
            
            DrawShadowedString(hudFont, timeString, hudLocation, timeColor);

        }

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, color);
            spriteBatch.End();
        }

        protected override void Initialize()
        {
            camera = new ChaseCamera(new Vector3(0, 400, 1500), new Vector3(0, 200, 0), new Vector3(0, 0, 0), GraphicsDevice);
            Services.AddService(typeof(ICameraService), camera);
            //Components.Add(new ModelComponent(this, "ball_red"));
            Components.Add(new BoundingBoxComponent(this, "StageTest"));
            base.Initialize();
        }
    }
}
