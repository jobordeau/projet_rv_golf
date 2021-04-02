using System;
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.CollisionTests;
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
using BoundingBox = BEPUutilities.BoundingBox;
using MathHelper = BEPUutilities.MathHelper;

using Vector3 = BEPUutilities.Vector3;

namespace Golf.Core
{
    public class MiniGolf : Game
    {
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public KeyboardState KeyboardState;
        public MouseState MouseState;
        public MouseState LastMouseState;
        public GameManager manager;
        public ChaseCameraControlScheme Camera;
        public Camera CameraClassic;
        private int nbHits=0;
        private ChargeBar chargeBar;

        public MiniGolf()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.ToggleFullScreen();
            base.Initialize();
        }



        protected override void LoadContent()
        {
           
            spriteBatch = new SpriteBatch(GraphicsDevice);

            chargeBar = new ChargeBar(this, spriteBatch, graphics, new Microsoft.Xna.Framework.Vector2((1900 * Window.ClientBounds.Width) / 1980, (1100 * Window.ClientBounds.Height) / 1080));

            manager = new GameManager(this);
            manager.AddPlayer(new Player(this, "jojo", "ball_red", new Vector3(0, 0, 0)));
            manager.LoadGame();
            CameraClassic = new Camera(Vector3.Zero, 0, 0, BEPUutilities.Matrix.CreatePerspectiveFieldOfViewRH(MathHelper.PiOver4, graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight, .1f, 10000));
            Camera = new ChaseCameraControlScheme(manager.Space.Entities[0], new Vector3(0, 7, 0), false, 50f, CameraClassic, this);
        }

        /// <summary>
        /// Used to handle a collision event triggered by an entity specified above.
        /// </summary>
        /// <param name="sender">Entity that had an event hooked.</param>
        /// <param name="other">Entity causing the event to be triggered.</param>
        /// <param name="pair">Collision pair between the two objects in the event.</param>
        /*void HandleCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
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
        }*/

        protected override void Update(GameTime gameTime)
        {
            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
            Entity mainEntity = manager.MainPlayer.Ball.Form;

            Camera.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            if (KeyboardState.IsKeyDown(Keys.Escape))
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
                if(LastMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (MouseState.LeftButton == ButtonState.Released)
                    {
                        mainEntity.LinearVelocity += Camera.Camera.ViewDirection * chargeBar.Charge;
                        nbHits++;
                    }
                }

            }
            else
            {
                chargeBar.Charge = 0;
            }
            LastMouseState = MouseState;

            if (mainEntity.CollisionInformation.BoundingBox.Intersects(manager.MainLevel.BoundingArrive))
            {
                Exit();
                return;
            }

            if (mainEntity.Position.Y < -50f || KeyboardState.IsKeyDown(Keys.R))
            {
                mainEntity.LinearVelocity = Vector3.Zero;
                mainEntity.Position = Vector3.Zero;
            }

            chargeBar.Update(gameTime);
            manager.Space.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSkyBlue);
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            base.Draw(gameTime);
            chargeBar.Draw(gameTime);

        }
    }
}
