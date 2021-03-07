using Golf.Core.ModelGolf.Cam;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Text;

namespace Golf.Core.ModelGolf
{
    class Level : GameObject
    {
        private string _nom { get; set; }
        private int par { get; set; }

        private List<Microsoft.Xna.Framework.BoundingBox> boundingBoxes;


        public Level(Game game, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, ModelRender model) : base(game, spriteBatch, graphics, model)
        {
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

        }

        public override void HandleModelCollision(GameObject otherModel)
        {
            if (otherModel is Ball)
            {
                Ball otherBall = (Ball)otherModel;
                foreach (var box in _model.BoundingBoxes)
                {
                    if (box.Intersects(otherBall._model.BoundingSphere))
                    {
                        
                    }
                }
               
            }

        }




    }
}
