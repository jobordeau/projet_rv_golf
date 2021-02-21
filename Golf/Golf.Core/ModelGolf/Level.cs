using Golf.Core.ModelGolf.Cam;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Core.ModelGolf
{
    class Level : GameObject
    {

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

        }




    }
}
