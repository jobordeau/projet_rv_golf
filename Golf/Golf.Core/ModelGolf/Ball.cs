
using BEPUphysics.Entities.Prefabs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector3 = BEPUutilities.Vector3;
using Matrix = BEPUutilities.Matrix;
using BEPUphysics;
using System;
using Microsoft.Xna.Framework.Content;

namespace Golf.Core.ModelGolf
{
    /// <summary>
    /// Class representing the golf ball
    /// </summary>
    public class Ball : IDisposable
    {
        /// <summary>
        /// The graphic model of the ball
        /// </summary>
        public Model Model { get; }
        /// <summary>
        /// The logic model of the ball using Bepu sphere
        /// </summary>
        public Sphere Form { get; }
        /// <summary>
        /// The current game
        /// </summary>
        public Game Game { get; }
        
        /// <summary>
        /// Constructor of the ball
        /// </summary>
        /// <param name="game">Current game</param>
        /// <param name="modelName">Model of the ball</param>
        /// <param name="position">Starting position of the ball</param>
        public Ball(Game game, string modelName, Vector3  position)
        {
            Game = game;
            Model = game.Content.Load<Model>(modelName);
            Form = new Sphere(position, 1, 10);
        }
        /// <summary>
        /// Constructor of the ball
        /// </summary>
        /// <param name="ball">the other ball we want to copy</param>
        public Ball(Ball ball)
        {
            Game = ball.Game;
            Model = ball.Model;
            Form = ball.Form;
        }

        /// <summary>
        /// Method for loading the ball into the game
        /// </summary>
        /// <param name="space">Space the ball will be added</param>
        /// <param name="game">The current game</param>
        public void Load(Space space, Game game)
        {
            space.Add(Form);
            if (Model != null)
            {
                Matrix scaling = Matrix.CreateScale(Form.Radius, Form.Radius, Form.Radius);
                EntityModel entityModel = new EntityModel(Form, Model, scaling, game);
                game.Components.Add(entityModel);

            }
            else
            {
                space.Remove(Form);
                throw new Exception("Load on model null");
            }
        }

        /// <summary>
        /// Method in order to check if the ball is moving
        /// </summary>
        /// <returns>mobility state of the ball</returns>
        public bool IsMoving()
        {
            if (Form.LinearVelocity.Length() < 10)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Method in order to dispose the model in the current game
        /// </summary>
        public void Dispose()
        {
            Game.Content.Dispose();
        }

        /// <summary>
        /// Method to remove the object from the space
        /// </summary>
        /// <param name="space">Current space</param>
        public void RemoveFromSpace(Space space)
        {
            space.Remove(Form);
        }
    }

    //First implementation of the ball
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
