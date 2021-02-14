using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Core.ModelGolf
{
    public class Ball : GameObject
    {
        private Model _texture;
        public BoundingSphere BoundingSphere { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }

        public Ball(Game game, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Vector3 position) : base(game, spriteBatch, graphics)
        {
            BoundingSphere = new BoundingSphere(new Vector3(Position.X, Position.Y, Position.Z),5); //comment obtenir le rayon d'un modèle 3D ?
            Position = position;
            Velocity = Vector3.Zero;
            LoadContent();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _texture = Game.Content.Load<Model>("ball_red");
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Camera camera = Camera.GetCamera(_graphics);
            foreach (ModelMesh mesh in _texture.Meshes)
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
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Position = Vector3.Add(Position, Velocity);

            /*Tester si la balle est hors du terrain*/
        }

        public void HandleBallCollision(Ball otherBall)
        {
            if (BoundingSphere.Intersects(otherBall.BoundingSphere))
            {
                Velocity = Vector3.Negate(Velocity);
                otherBall.Velocity = Vector3.Negate(otherBall.Velocity);
            }
        }




    }
}
