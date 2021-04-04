using Golf.Core.ModelGolf.Cam;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Golf.Core.ModelGolf
{
    /// <summary>
    /// Class defining an object that will be showed in the game
    /// </summary>
    public abstract class GameObject : DrawableGameComponent
    {
        /// <summary>
        /// Current graphic device 
        /// </summary>
        protected readonly GraphicsDeviceManager Graphics;
        /// <summary>
        /// The current SpriteBatch
        /// </summary>
        protected readonly SpriteBatch SpriteBatch;

        /// <summary>
        /// The constructor of a game object
        /// </summary>
        /// <param name="game">the current game</param>
        /// <param name="spriteBatch">the current sprite batch</param>
        /// <param name="graphics">the current graphic device</param>
        public GameObject(Game game, SpriteBatch spriteBatch, GraphicsDeviceManager graphics) : base(game)
        {
            Graphics = graphics;
            SpriteBatch = spriteBatch;
        }

        /// <summary>
        /// Method defining the update method of the object
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract override void Update(GameTime gameTime);

        /// <summary>
        /// Method defining the drawing method of the object
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract override void Draw(GameTime gameTime);


        //First implementation trying to get the bounding box of the model
        /* public abstract void HandleModelCollision(GameObject otherModel);
         //public abstract void Draw(GameTime gameTime, Camera camera);

         public List<BoundingBox> GetBounds()
         {
             Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
             Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
             List<BoundingBox> boxes = new List<BoundingBox>();

             foreach (ModelMesh mesh in this._model.Model.Meshes.Where(elt => elt.Name.Contains("plant")))
             {

                 var bounding = CreateBoundingBox(_model.Model, mesh);
                 boxes.Add(bounding);
             }
             return boxes;
         }

         private static BoundingBox CreateBoundingBox(Model model, ModelMesh mesh)
         {

             Matrix[] boneTransforms = new Matrix[model.Bones.Count];
             model.CopyAbsoluteBoneTransformsTo(boneTransforms);

             BoundingBox result = new BoundingBox();
             result.Min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
             result.Max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

             foreach (ModelMeshPart meshPart in mesh.MeshParts)
             {
                 BoundingBox? meshPartBoundingBox = GetBoundingBox(meshPart, boneTransforms[mesh.ParentBone.Index]);
                 if (meshPartBoundingBox != null)
                     result = BoundingBox.CreateMerged(result, meshPartBoundingBox.Value);
             }
             result = new BoundingBox(result.Min, result.Max);
             return result;
         }
         private static BoundingBox? GetBoundingBox(ModelMeshPart meshPart, Matrix transform)
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
             return BoundingBox.CreateFromPoints(transformedPositions);
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
         }*/

    }
}
