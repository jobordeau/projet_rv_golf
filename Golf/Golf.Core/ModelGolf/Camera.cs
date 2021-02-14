using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Core.ModelGolf
{
    public class Camera
    {
        public Vector3 CamTarget { get; set; }
        public Vector3 CamPosition { get; set; }
        public Matrix ProjectionMatrix { get; set; }
        public Matrix ViewMatrix { get; set; }
        public Matrix WorldMatrix { get; set; }
        private static Camera _instance;

        public Camera(GraphicsDeviceManager graphics)
        {
            CamTarget = new Vector3(0f, 0f, 0f);
            CamPosition = new Vector3(0f, 0f, -5);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                               MathHelper.ToRadians(45f), graphics.
                               GraphicsDevice.Viewport.AspectRatio,
                1f, 1000f);
            ViewMatrix = Matrix.CreateLookAt(CamPosition, CamTarget,
                         new Vector3(0f, 1f, 0f));// Y up
            WorldMatrix = Matrix.CreateWorld(CamTarget, Vector3.
                          Forward, Vector3.Up);
        }

        public static Camera GetCamera(GraphicsDeviceManager graphics)
        {
            if (_instance == null)
            {
                _instance = new Camera(graphics);
            }
            return _instance;
        }

        
    }
}
