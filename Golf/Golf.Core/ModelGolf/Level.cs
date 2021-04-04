using System;
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysicsDemos;
using BEPUutilities;
using Golf.Core.ModelGolf.Cam;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BoundingBox = BEPUutilities.BoundingBox;
using MathHelper = BEPUutilities.MathHelper;
using Vector3 = BEPUutilities.Vector3;

namespace Golf.Core.ModelGolf
{
    public class Level : IDisposable
    {

        public Model ModelLevel { get; }
        public Model ModelArrival { get; }
        
        public Game Game { get; }

        public BoundingBox BoundingLevel { get; private set; }
        public BoundingBox BoundingArrive { get; private set; }

        

        public Level(Game game,int levelIndex=1)
        {
            Game = game;
            string levelPath = $"Levels/{levelIndex}";
            ModelLevel = game.Content.Load<Model>(levelPath);
            string arrivePath = $"Arrive/{levelIndex}";
            ModelArrival = game.Content.Load<Model>(arrivePath);

        }

        public void Load(Space space, Game game)
        {
            ModelDataExtractor.GetVerticesAndIndicesFromModel(ModelLevel, out Vector3[] vertices, out int[] indices);
            var meshLevel = new StaticMesh(vertices, indices, new AffineTransform(new Vector3(0, -40, 0)));
            BoundingLevel = meshLevel.BoundingBox;
            space.Add(meshLevel);
            game.Components.Add(new StaticModel(ModelLevel, meshLevel.WorldTransform.Matrix, game));

            ModelDataExtractor.GetVerticesAndIndicesFromModel(ModelArrival, out vertices, out indices);
            var meshArrival = new StaticMesh(vertices, indices, new AffineTransform(new Vector3(0, -40, 0)));
            BoundingArrive = meshArrival.BoundingBox;
        }

        public void Dispose()
        {
            Game.Content.Dispose();
        }

    }
}
