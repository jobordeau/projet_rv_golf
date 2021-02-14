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
        public GameObject(Game game, SpriteBatch spriteBatch, GraphicsDeviceManager graphics) : base(game)
        {
            _graphics = graphics;
            _spriteBatch = spriteBatch;
        }
  
    }
}
