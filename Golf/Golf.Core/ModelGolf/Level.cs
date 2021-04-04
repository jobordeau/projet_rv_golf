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
    /// <summary>
    /// Class defining a course in the game
    /// </summary>
    public class Level : IDisposable
    {
        /// <summary>
        /// The model of the level
        /// </summary>
        public Model ModelLevel { get; }
        /// <summary>
        /// The model of the arrival
        /// </summary>
        public Model ModelArrival { get; }
        /// <summary>
        /// The current game
        /// </summary>
        public Game Game { get; }

        /// <summary>
        /// The bounding box of the level
        /// </summary>
        public BoundingBox BoundingLevel { get; private set; }
        /// <summary>
        /// The bounding box of the arrival
        /// </summary>
        public BoundingBox BoundingArrive { get; private set; }

        
        /// <summary>
        /// The constructor of a level
        /// </summary>
        /// <param name="game">the current game</param>
        /// <param name="levelIndex">the index of the level</param>
        public Level(Game game,int levelIndex=1)
        {
            Game = game;
            string levelPath = $"Levels/{levelIndex}";
            ModelLevel = game.Content.Load<Model>(levelPath);
            string arrivePath = $"Arrive/{levelIndex}";
            ModelArrival = game.Content.Load<Model>(arrivePath);

        }

        /// <summary>
        /// Method defining the loading of the model in our space
        /// </summary>
        /// <param name="space">the current space</param>
        /// <param name="game">the current game</param>
        public void Load(Space space, Game game)
        {
            //Using the ModelDataExtractor in order to extract the bounding of the model and activate the collisions
            ModelDataExtractor.GetVerticesAndIndicesFromModel(ModelLevel, out Vector3[] vertices, out int[] indices);
            var meshLevel = new StaticMesh(vertices, indices, new AffineTransform(new Vector3(0, -40, 0)));
            BoundingLevel = meshLevel.BoundingBox;
            space.Add(meshLevel);
            game.Components.Add(new StaticModel(ModelLevel, meshLevel.WorldTransform.Matrix, game));

            ModelDataExtractor.GetVerticesAndIndicesFromModel(ModelArrival, out vertices, out indices);
            var meshArrival = new StaticMesh(vertices, indices, new AffineTransform(new Vector3(0, -40, 0)));
            BoundingArrive = meshArrival.BoundingBox;
        }

        /// <summary>
        /// Method in order to dispose the current object
        /// </summary>
        public void Dispose()
        {
            Game.Content.Dispose();
        }

    }
}
