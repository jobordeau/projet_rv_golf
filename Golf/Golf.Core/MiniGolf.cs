using System;
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
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
using MathHelper = BEPUutilities.MathHelper;
using Matrix = BEPUutilities.Matrix;
using Vector3 = BEPUutilities.Vector3;

namespace Golf.Core
{
    public class MiniGolf: Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Space space;
        Model level;
        private Model ball;
        private Vector3[] vertices;
        private int[] indices;
        public KeyboardState KeyboardState;
        public MouseState MouseState;

        public ChaseCameraControlScheme Camera;
        public Camera CameraClassic;

        public MiniGolf()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }



        protected override void LoadContent()
        {
            //loading models
            level = Content.Load<Model>("StageTest");
            ball = Content.Load<Model>("ball_red");

            //Creating and configuring space and adding elements
            space = new Space();
            space.ForceUpdater.Gravity = new Vector3(-0, -9.81f, 0);
            Sphere Balle = new Sphere(new Vector3(0, 0, 0), 1, 1);
            space.Add(Balle);

            ModelDataExtractor.GetVerticesAndIndicesFromModel(level, out vertices, out indices);
            var mesh = new StaticMesh(vertices, indices, new AffineTransform(new Vector3(0,-40,0)));
            space.Add(mesh);
            Components.Add(new StaticModel(level, mesh.WorldTransform.Matrix, this));

            //Hook an event handler to an entity to handle some game logic.
            //Refer to the Entity Events documentation for more information.
            //Box deleterBox = new Box(new Vector3(5, 2, 0), 3, 3, 3);
            //space.Add(deleterBox);
            //deleterBox.CollisionInformation.Events.InitialCollisionDetected += HandleCollision;

            foreach (Entity e in space.Entities)
            {
                Sphere box = e as Sphere;
                if(box != null)
                {
                    Matrix scaling = Matrix.CreateScale(box.Radius, box.Radius, box.Radius);
                    EntityModel model = new EntityModel(e, ball, scaling, this);
                    Components.Add(model);

                }
            }

            
            CameraClassic = new Camera(BEPUutilities.Vector3.Zero, 0, 0, BEPUutilities.Matrix.CreatePerspectiveFieldOfViewRH(MathHelper.PiOver4, graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight, .1f, 10000));

            Camera = new ChaseCameraControlScheme(space.Entities[0], new Vector3(0, 7, 0), false,50f, CameraClassic,this);
            //Camera = new FreeCameraControlScheme(10,CameraClassic,this);
        }

        /// <summary>
        /// Used to handle a collision event triggered by an entity specified above.
        /// </summary>
        /// <param name="sender">Entity that had an event hooked.</param>
        /// <param name="other">Entity causing the event to be triggered.</param>
        /// <param name="pair">Collision pair between the two objects in the event.</param>
        void HandleCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            //This type of event can occur when an entity hits any other object which can be collided with.
            //They aren't always entities; for example, hitting a StaticMesh would trigger this.
            //Entities use EntityCollidables as collision proxies; see if the thing we hit is one.
            var otherEntityInformation = other as EntityCollidable;
            if (otherEntityInformation != null)
            {
                //We hit an entity! remove it.
                space.Remove(otherEntityInformation.Entity);
                //Remove the graphics too.
                Components.Remove((EntityModel)otherEntityInformation.Entity.Tag);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();

            Camera.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
                return;
            }

            if(MouseState.LeftButton == ButtonState.Pressed)
            {
                
            }

            space.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSkyBlue);
            base.Draw(gameTime);
        }
    }
}