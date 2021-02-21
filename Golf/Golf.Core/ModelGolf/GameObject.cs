using Golf.Core.ModelGolf.Cam;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;


namespace Golf.Core.ModelGolf
{
    public abstract class GameObject : DrawableGameComponent
    {
        protected readonly GraphicsDeviceManager _graphics;
        protected readonly SpriteBatch _spriteBatch;
        public  ModelRender _model;
        public GameObject(Game game, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, ModelRender model) : base(game)
        {
            _graphics = graphics;
            _spriteBatch = spriteBatch;
            _model = model;
        }
        public abstract void HandleModelCollision(GameObject otherModel);
        public abstract void Draw(GameTime gameTime, Camera camera);

    }
}
