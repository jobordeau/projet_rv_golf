using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Golf.Core.ModelGolf
{
    public class LevelRender
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public Model Model { get; private set; }
        private Matrix[] modelTransforms;
        private BoundingBox boundingBox;
        public BoundingBox BoundingBox
        {
            get
            {
                // No need for rotation, as this is a sphere
                Matrix worldTransform = Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);

                BoundingBox transformed = boundingBox;

                return transformed;
            }
        }
        public LevelRender(Model Model, Vector3 Position, Vector3 Rotation, Vector3 Scale)
        {
            this.Model = Model;

            modelTransforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            this.Position = Position;
            this.Scale = Scale;

            BuildBoundingRectangle();
        }

        private void BuildBoundingRectangle()
        {
        }



        public void Draw(Matrix View, Matrix Projection)
        {
            // Calculate the base transformation by combining
            // translation, rotation, and scaling
            Matrix baseWorld = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) * Matrix.CreateTranslation(Position);

            foreach (ModelMesh mesh in Model.Meshes)
            {
                Matrix localWorld = modelTransforms[mesh.ParentBone.Index] * baseWorld;

                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    BasicEffect effect = (BasicEffect)meshPart.Effect;

                    effect.World = localWorld;
                    effect.View = View;
                    effect.Projection = Projection;

                    //effect.EnableDefaultLighting();
                }

                mesh.Draw();
            }
        }
    }
}

