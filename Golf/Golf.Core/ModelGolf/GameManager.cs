using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysicsDemos;
using BEPUutilities;
using Golf.Core.ModelGolf.Cam;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using MathHelper = BEPUutilities.MathHelper;
using Matrix = BEPUutilities.Matrix;
using Vector3 = BEPUutilities.Vector3;

namespace Golf.Core.ModelGolf
{
    public class GameManager
    {
        public LinkedList<Player> Players { get; }
        public List<Player> Level { get; }
        public Space Space { get; }
        private MiniGolf game;
        public ChaseCameraControlScheme Camera;


        public GameManager(MiniGolf game)
        {
            this.game = game;
            Players = new LinkedList<Player>();

            //Creating and configuring space
            Space = new Space();
            Space.ForceUpdater.Gravity = new Vector3(-0, -9.81f, 0);
            
        }


       public void AddPlayer(Player player)
       {
            Players.AddLast(player);
       }

       public void LoadGame()
        {
            LoadLevel(new Level(game, "StageTest"));
            loadBall();
        }

       private void loadBall()
        {
            foreach(Player player in Players)
            {
                Ball ball = player.Ball;
                Space.Add(ball.Form);
                if (ball.Model != null)
                {
                    Matrix scaling = Matrix.CreateScale(ball.Form.Radius, ball.Form.Radius, ball.Form.Radius);
                    EntityModel model = new EntityModel(ball.Form, ball.Model, scaling, game);
                    game.Components.Add(model);

                }
            }

        }

       private void LoadLevel(Level level)
        {
            Vector3[] vertices;
            int[] indices;
            ModelDataExtractor.GetVerticesAndIndicesFromModel(level.ModelLevel, out vertices, out indices);
            var mesh = new StaticMesh(vertices, indices, new AffineTransform(new Vector3(0, -40, 0)));
            Space.Add(mesh);
            game.Components.Add(new StaticModel(level.ModelLevel, mesh.WorldTransform.Matrix, game));
        }

    }
}
