using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysicsDemos;
using BEPUutilities;
using Golf.Core.ModelGolf.Cam;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MathHelper = BEPUutilities.MathHelper;
using Vector3 = BEPUutilities.Vector3;

namespace Golf.Core.ModelGolf
{
    public class Level
    {

        public Model ModelLevel { get; }
        public Model ModelFinish { get; }

        public Level(Game game, string modelName)
        {

            ModelLevel = game.Content.Load<Model>(modelName);

        }

        public void Load(Space space, MiniGolf game)
        {
            ModelDataExtractor.GetVerticesAndIndicesFromModel(ModelLevel, out Vector3[] vertices, out int[] indices);
            var mesh = new StaticMesh(vertices, indices, new AffineTransform(new Vector3(0, -40, 0)));
            space.Add(mesh);
            game.Components.Add(new StaticModel(ModelLevel, mesh.WorldTransform.Matrix, game));
        }

    }
}
