
using BEPUphysics.Entities.Prefabs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector3 = BEPUutilities.Vector3;
using Matrix = BEPUutilities.Matrix;
using BEPUphysics;
using System;

namespace Golf.Core.ModelGolf
{
    public class Ball
    {
        public Model Model { get; }
        public Sphere Form { get; }

        public Ball(Game game, string modelName, Vector3  position)
        {

            Model = game.Content.Load<Model>(modelName);
            Form = new Sphere(position, 1, 10);
         
        }

        public void Load(Space space, MiniGolf game)
        {
            space.Add(Form);
            if (Model != null)
            {
                Matrix scaling = Matrix.CreateScale(Form.Radius, Form.Radius, Form.Radius);
                EntityModel model = new EntityModel(Form, Model, scaling, game);
                game.Components.Add(model);

            }
            else
            {
                space.Remove(Form);
                throw new Exception("Load on model null");
            }
        }

    }
    /*public class Ball : GameObject
    {
        
        public Vector3 Velocity { get; set; }
        private Vector3 lastVelocity;
        public bool Moving { get; set; }

        ObjectAnimation anim;

        public Ball(Game game, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, ModelRender model) : base(game, spriteBatch, graphics,model)
        {
            Velocity = Vector3.Zero;
            lastVelocity = Vector3.Zero;
            Moving = false;
            
            LoadContent();
            GetBounds();
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
            }
            else
            {
                Moving = false;
            }

            anim = new ObjectAnimation(_model.Position, 
                                       _model.Position,
                                       _model.Rotation,
                                       _model.Rotation+new Vector3(10,0,0), TimeSpan.FromSeconds(2), true);
            anim.Update(gameTime.ElapsedGameTime);
            _model.Rotation = anim.Rotation;
            /*Tester si la balle est hors du terrain*/
    /*}

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

}*/

}
