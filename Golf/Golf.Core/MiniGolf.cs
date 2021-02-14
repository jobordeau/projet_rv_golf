using Golf.Core.ModelGolf;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Golf.Core
{
    public class MiniGolf : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //Camera
        Camera camera;
        //Geometric info
        Model model;
        //Orbit
        bool orbit = false;

        //Model
        BallManager ballManager;
        GameManager scoreManager;
        
        //test model
        Player player1;
        public MiniGolf()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            base.Initialize();
            //Setup Camera
            camera = Camera.GetCamera(graphics);

            //test du modèle 
            player1 = new Player(this, spriteBatch, graphics, "jojo");
            scoreManager = new GameManager();
            scoreManager.AddPlayer(player1);
            ballManager = new BallManager(scoreManager.Players);

        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Content = new ContentManager(this.Services,"Content");
            model = Content.Load<Model>("StageTest");
        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                Keys.Escape))
                Exit();

            Vector3 camPosition = camera.CamPosition;
            Vector3 camTarget = camera.CamTarget;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                camPosition.X -= 0.1f;
                camTarget.X -= 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                camPosition.X += 0.1f;
                camTarget.X += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                camPosition.Y -= 0.1f;
                camTarget.Y -= 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                camPosition.Y += 0.1f;
                camTarget.Y += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
            {
                camPosition.Z += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            {
                camPosition.Z -= 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                orbit = !orbit;
            }
            if (orbit)
            {
                Matrix rotationMatrix = Matrix.CreateRotationY(
                                        MathHelper.ToRadians(1f));
                camPosition = Vector3.Transform(camPosition,
                              rotationMatrix);
            }
            camera.CamTarget = camTarget;
            camera.CamPosition = camPosition;
            camera.ViewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
                         Vector3.Up);
            ballManager.Update(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();
                    effect.AmbientLightColor = new Vector3(1f, 0, 0);
                    effect.View = camera.ViewMatrix;
                    effect.World = camera.WorldMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
            ballManager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
