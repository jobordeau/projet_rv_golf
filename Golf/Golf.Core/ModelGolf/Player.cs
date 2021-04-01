using Microsoft.Xna.Framework;
using Vector3 = BEPUutilities.Vector3;
namespace Golf.Core.ModelGolf
{
    public class Player
    {
        public string Name { get; set; }

        public Ball Ball { get; set; }
        public Player(MiniGolf game, string name, string ball_model_name, Vector3 position )
        {
            Name = name;
            Ball = new Ball(game, ball_model_name, position);
        }
        
    }
}
