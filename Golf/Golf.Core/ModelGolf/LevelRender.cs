using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Golf.Core.ModelGolf
{
    /// <summary>
    /// The first implementation of the level renderer of the game
    /// </summary>
    public class LevelRender
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public Model Model { get; private set; }
        private Matrix[] _modelTransforms;
        private BoundingBox _boundingBox;
        public BoundingBox BoundingBox
        {
            get
            {
                // No need for rotation, as this is a sphere
                Matrix worldTransform = Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);

                BoundingBox transformed = _boundingBox;

                return transformed;
            }
        }
        public LevelRender(Model model, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            this.Model = model;

            _modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(_modelTransforms);

            this.Position = position;
            this.Scale = scale;

            BuildBoundingRectangle();
        }

        private void BuildBoundingRectangle()
        {
        }



        public void Draw(Matrix view, Matrix projection)
        {
            // Calculate the base transformation by combining
            // translation, rotation, and scaling
            Matrix baseWorld = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) * Matrix.CreateTranslation(Position);

            foreach (ModelMesh mesh in Model.Meshes)
            {
                Matrix localWorld = _modelTransforms[mesh.ParentBone.Index] * baseWorld;

                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    BasicEffect effect = (BasicEffect)meshPart.Effect;

                    effect.World = localWorld;
                    effect.View = view;
                    effect.Projection = projection;

                    //effect.EnableDefaultLighting();
                }

                mesh.Draw();
            }
        }
    }
}

