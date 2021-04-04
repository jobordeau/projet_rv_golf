using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Golf.Core.ModelGolf
{
    /// <summary>
    /// Class defining the charging bar
    /// </summary>
    class ChargeBar : GameObject
    {
        /// <summary>
        /// Current charge of the bar
        /// </summary>
        public float Charge { get; set; }
        /// <summary>
        /// The maximum charge you can reach
        /// </summary>
        public readonly float ChargeMax;
        /// <summary>
        /// The current color of the bar
        /// </summary>
        private Color _color;
        /// <summary>
        /// The position of the charging bar
        /// </summary>
        private Vector2 _pos;
        /// <summary>
        /// The ratio of the charge
        /// </summary>
        private int _ratio;

        /// <summary>
        /// Constructor of the charge bar
        /// </summary>
        /// <param name="game">The current Game</param>
        /// <param name="spriteBatch">The current spriteBatch</param>
        /// <param name="graphics">The current graphic device</param>
        /// <param name="pos">The position the bar will be draw</param>
        public ChargeBar(Game game, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Vector2 pos) : base(game,spriteBatch,graphics)
        {
            Charge = 0;
            ChargeMax = 200;
            this._pos = pos;
            _ratio = (int)((Charge * 400) / ChargeMax);
        }

        /// <summary>
        /// Method defining the update of the bar
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Update(GameTime gameTime)
        {
            //Showing a different color according to the charge of the bar
            _ratio = (int)((Charge * 400) / ChargeMax);

            if (_ratio < 400 / 3)
            {
                _color = Color.Green;
            }
            else
            {
                if (_ratio < (400 / 3) * 2)
                {
                    _color = Color.Orange;
                }
                else
                {
                    _color = Color.Red;
                }
            }
        }

        /// <summary>
        /// Method defining the drawing of the element
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Draw(GameTime gameTime)
        {
            //We draw the element only if it have a charge
            if(_ratio > 0)
            {
                Texture2D rect = new Texture2D(Graphics.GraphicsDevice, 30, _ratio);
                Color[] data = new Color[30 * _ratio];
                for (int i = 0; i < data.Length; i++) data[i] = _color;
                rect.SetData(data);
                SpriteBatch.Draw(rect, _pos,null, Color.White, MathHelper.Pi, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
                
            }
        }
    }


}

