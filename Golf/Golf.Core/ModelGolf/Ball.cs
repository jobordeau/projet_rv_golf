using Golf.Core.ModelGolf.Cam;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Core.ModelGolf
{
    public class Ball : GameObject
    {
        
        public Vector3 Velocity { get; set; }
        private Vector3 lastVelocity;
        public bool Moving { get; set; }

        public Ball(Game game, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, ModelRender model) : base(game, spriteBatch, graphics,model)
        {
            Velocity = Vector3.Zero;
            lastVelocity = Vector3.Zero;
            Moving = false;
            LoadContent();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime, Camera camera)
        {
            if (camera.BoundingVolumeIsInView(_model.BoundingSphere))
                _model.Draw(camera.View, camera.Projection);
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
            _model.Position = Vector3.Add(_model.Position, Velocity);
            if (Velocity != Vector3.Zero)
            {
                if (!Moving)
                {
                    lastVelocity = Velocity;
                    Moving = true;
                }
                
                Velocity -= lastVelocity /100;
                Velocity = new Vector3((float)Math.Round(Velocity.X,2), (float)Math.Round(Velocity.Y,2), (float)Math.Round(Velocity.Z,2));
                Console.WriteLine(Velocity);
            }
            else
            {
                Moving = false;
            }
                
            
            /*Tester si la balle est hors du terrain*/
        }

        public override void HandleModelCollision(GameObject otherModel)
        {
            if(otherModel is Ball)
            {
                Ball otherBall = (Ball)otherModel;
                if (_model.BoundingSphere.Intersects(otherBall._model.BoundingSphere))
                {

                    otherBall.Velocity = Vector3.Negate(otherBall.Velocity) + Velocity/2;
                    otherBall.lastVelocity = Vector3.Negate(otherBall.lastVelocity);
                    otherBall.Velocity = Vector3.Round(otherBall.Velocity);

                    Velocity = Vector3.Negate(Velocity);
                    lastVelocity = Vector3.Negate(lastVelocity);

                    
                }
            }
            
        }




    }

}
