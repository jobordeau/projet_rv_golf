using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Golf.Core.ModelGolf
{
    class ChargeBar : GameObject
    {
        public float Charge { get; set; }
        public readonly float CHARGE_MAX;
        private Color color;
        private Vector2 pos;
        private int ratio;
        public ChargeBar(Game game, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Vector2 pos) : base(game,spriteBatch,graphics)
        {
            Charge = 0;
            CHARGE_MAX = 200;
            this.pos = pos;
            ratio = (int)((Charge * 400) / CHARGE_MAX);
        }

        public override void Update(GameTime gameTime)
        {
            ratio = (int)((Charge * 400) / CHARGE_MAX);

            if (ratio < 400 / 3)
            {
                color = Color.Green;
            }
            else
            {
                if (ratio < (400 / 3) * 2)
                {
                    color = Color.Orange;
                }
                else
                {
                    color = Color.Red;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if(ratio > 0)
            {
                Texture2D rect = new Texture2D(_graphics.GraphicsDevice, 30, ratio);
                Color[] data = new Color[30 * ratio];
                for (int i = 0; i < data.Length; i++) data[i] = color;
                rect.SetData(data);
                _spriteBatch.Draw(rect, pos,null, Color.White, MathHelper.Pi, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
                
            }
        }
    }


}

