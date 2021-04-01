using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golf.Core;
using Golf.Core.ModelGolf.Cam;
using Microsoft.Xna.Framework.Input;

namespace Golf.Core.ModelGolf.Cam
{
    /// <summary>
    /// Superclass of implementations which control the behavior of a camera.
    /// </summary>
    public abstract class CameraControlScheme
    {
        MouseState lastMousState;
        /// <summary>
        /// Gets the game associated with the camera.
        /// </summary>
        public MiniGolf Game { get; private set; }

        /// <summary>
        /// Gets the camera controlled by this control scheme.
        /// </summary>
        public Camera Camera { get; private set; }

        protected CameraControlScheme(Camera camera, MiniGolf game)
        {
            Camera = camera;
            Game = game;
        }

        /// <summary>
        /// Updates the camera state according to the control scheme.
        /// </summary>
        /// <param name="dt">Time elapsed since previous frame.</param>
        public virtual void Update(float dt)
        {
#if XBOX360
            Yaw += Game.GamePadInput.ThumbSticks.Right.X * -1.5f * dt;
            Pitch += Game.GamePadInput.ThumbSticks.Right.Y * 1.5f * dt;
#else       
            MouseState mouse = Game.MouseState;

            if (!Game.IsMouseVisible && lastMousState != null)
            {
                Camera.Yaw((lastMousState.X - mouse.X) * dt * 0.4f);
            }
            Camera.Pitch((lastMousState.Y - mouse.Y) * dt * 0.4f);
            lastMousState = mouse;
#endif
        }
    }
}
