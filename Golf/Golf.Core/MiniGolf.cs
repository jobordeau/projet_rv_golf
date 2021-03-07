using Golf.Core.ModelGolf;
using Golf.Core.ModelGolf.Cam;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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

        Vector3 charge=Vector3.Zero;
        Vector3 chargeMax = new Vector3(0, 0, -10);

        MouseState lastMouseState;

        public MiniGolf()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            modelManager = new ModelManager();
            
        }

        // Called when the game should load its content
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ball = new Ball(this, spriteBatch, graphics, new ModelRender(Content.Load<Model>("ball_red"), new Vector3(0, 400, 0), Vector3.Zero, new Vector3(250f)));
            Ball ball2 = new Ball(this, spriteBatch, graphics, new ModelRender(Content.Load<Model>("ball_red"), new Vector3(200, 400, 0), Vector3.Zero, new Vector3(250f)));
            level = new Level(this, spriteBatch, graphics, new ModelRender(Content.Load<Model>("StageTest"), Vector3.Zero, Vector3.Zero, new Vector3(50f)));
            modelManager.AddModel(ball);
            modelManager.AddModel(ball2);
            modelManager.AddModel(level);


            camera = new ChaseCamera(new Vector3(0, 1000, 4000), new Vector3(0, 200, 0), new Vector3(0, 0, 0), GraphicsDevice);
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
            modelManager.elements[0]._model.TargetRotation += rotChange * .025f;
            // Determine what direction to move in
            Matrix rotation = Matrix.CreateFromYawPitchRoll(modelManager.elements[0]._model.TargetRotation.Y, modelManager.elements[0]._model.TargetRotation.X, modelManager.elements[0]._model.TargetRotation.Z);
            // If space isn't down, the ship shouldn't move
            MouseState mouseState = Mouse.GetState();

            Ball ball = (Ball)modelManager.elements[0];
            if (mouseState.LeftButton == ButtonState.Released && ball.Moving == false)
            {
                if(lastMouseState.LeftButton == ButtonState.Pressed)
                {
                    ball.Velocity = Vector3.Transform(charge, rotation) * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 4;
                    ball.Velocity = Vector3.Round(ball.Velocity);
                    charge = Vector3.Zero;
                }
                
            }

            if(mouseState.LeftButton == ButtonState.Pressed)
            {
                if (!ball.Moving)
                {
                    if (!charge.Equals(chargeMax))
                    {
                        charge += new Vector3(0, 0, -0.2f);
                    }
                }
                
            }

            lastMouseState = mouseState;
                



        }



        // Called when the game should draw itself
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            modelManager.Draw(gameTime, camera);

            base.Draw(gameTime);
        }
    }
}
