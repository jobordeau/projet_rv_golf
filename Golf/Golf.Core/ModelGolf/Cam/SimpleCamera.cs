using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Golf.Core.ModelGolf.Cam
{
	public class SimpleCamera : DrawableGameComponent, ICameraService
    {
        public SimpleCamera(Game game)
            : base(game)
        {

        }

        public override void Update(GameTime gameTime)
        {
            View = Matrix.CreateLookAt(new Vector3(2, 1.8f, -2), new Vector3(0, -1, 0), Vector3.Up);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                GraphicsDevice.Viewport.AspectRatio,
                1.0f, 5000.0f);

            base.Update(gameTime);
        }

        public Matrix View { get; private set; }
        public Matrix Projection { get; private set; }
    }
}
