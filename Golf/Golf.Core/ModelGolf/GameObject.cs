using Golf.Core.ModelGolf.Cam;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Golf.Core.ModelGolf
{
    public abstract class GameObject : DrawableGameComponent
    {
        protected readonly GraphicsDeviceManager _graphics;
        protected readonly SpriteBatch _spriteBatch;
        public  ModelRender _model;
        public GameObject(Game game, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, ModelRender model) : base(game)
        {
            _graphics = graphics;
            _spriteBatch = spriteBatch;
            _model = model;
            _model.BoundingBoxes = GetBounds();
        }
        public abstract void HandleModelCollision(GameObject otherModel);
        public abstract void Draw(GameTime gameTime, Camera camera);

        public List<Microsoft.Xna.Framework.BoundingBox> GetBounds()
        {
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            List<Microsoft.Xna.Framework.BoundingBox> boxes = new List<Microsoft.Xna.Framework.BoundingBox>();

            foreach (ModelMesh mesh in this._model.Model.Meshes.Where(elt => elt.Name.Contains("plant")))
            {

                var bounding = CreateBoundingBox(_model.Model, mesh);
                boxes.Add(bounding);
            }
            return boxes;
        }

        private static Microsoft.Xna.Framework.BoundingBox CreateBoundingBox(Model model, ModelMesh mesh)
        {

            Matrix[] boneTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            Microsoft.Xna.Framework.BoundingBox result = new Microsoft.Xna.Framework.BoundingBox();
            result.Min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            result.Max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (ModelMeshPart meshPart in mesh.MeshParts)
            {
                Microsoft.Xna.Framework.BoundingBox? meshPartBoundingBox = GetBoundingBox(meshPart, boneTransforms[mesh.ParentBone.Index]);
                if (meshPartBoundingBox != null)
                    result = Microsoft.Xna.Framework.BoundingBox.CreateMerged(result, meshPartBoundingBox.Value);
            }
            result = new Microsoft.Xna.Framework.BoundingBox(result.Min, result.Max);
            return result;
        }
        private static Microsoft.Xna.Framework.BoundingBox? GetBoundingBox(ModelMeshPart meshPart, Matrix transform)
        {
            if (meshPart.VertexBuffer == null)
                return null;

            Vector3[] positions = VertexElementExtractor.GetVertexElement(meshPart, VertexElementUsage.Position);
            if (positions == null)
                return null;

            Vector3[] transformedPositions = new Vector3[positions.Length];
            Vector3.Transform(positions, ref transform, transformedPositions);

            for (int i = 0; i < transformedPositions.Length; i++)
            {
                Console.WriteLine(" " + transformedPositions[i]);
            }
            return Microsoft.Xna.Framework.BoundingBox.CreateFromPoints(transformedPositions);
        }


        public static class VertexElementExtractor
        {
            public static Vector3[] GetVertexElement(ModelMeshPart meshPart, VertexElementUsage usage)
            {
                VertexDeclaration vd = meshPart.VertexBuffer.VertexDeclaration;
                VertexElement[] elements = vd.GetVertexElements();

                Func<VertexElement, bool> elementPredicate = ve => ve.VertexElementUsage == usage && ve.VertexElementFormat == VertexElementFormat.Vector3;
                if (!elements.Any(elementPredicate))
                    return null;

                VertexElement element = elements.First(elementPredicate);

                Vector3[] vertexData = new Vector3[meshPart.NumVertices];
                meshPart.VertexBuffer.GetData((meshPart.VertexOffset * vd.VertexStride) + element.Offset,
                    vertexData, 0, vertexData.Length, vd.VertexStride);

                return vertexData;
            }
        }

    }
}
