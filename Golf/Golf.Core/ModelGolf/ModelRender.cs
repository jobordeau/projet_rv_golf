using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golf.Core.ModelGolf.Cam;

namespace Golf.Core.ModelGolf
{
    public class ModelRender : DrawableGameComponent
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public Model Model { get; private set; }
        private Matrix[] modelTransforms;
        private BoundingSphere boundingSphere;
        private BoundingSphere arrive;

        private List<Microsoft.Xna.Framework.BoundingBox> boundingBoxes = new List<Microsoft.Xna.Framework.BoundingBox>();
        public BoundingSphere BoundingSphere
        {
            get
            {
                // No need for rotation, as this is a sphere
                Matrix worldTransform = Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);

                BoundingSphere transformed = boundingSphere;
                transformed = transformed.Transform(worldTransform);

                return transformed;
            }
        }
        public ModelRender(Model Model, Vector3 Position, Vector3 Rotation, Vector3 Scale, Game game)
        :base(game)
        {
            this.Model = Model;

            modelTransforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            this.Position = Position;
            this.Rotation = Rotation;
            this.Scale = Scale;


            BuildBoundingSphere();
            BuildBoundingBox();
        }

        private void BuildBoundingBox()
        {
            Microsoft.Xna.Framework.BoundingBox box;
            ModelMeshCollection.Enumerator md;

            md=Model.Meshes.GetEnumerator();

            ModelMeshPart mesh;
            VertexBuffer vertex;

            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            while (md.MoveNext())
            {
                var meshPart = md.Current.MeshParts[0];
                // Vertex buffer parameters
                int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                int vertexBufferSize = meshPart.NumVertices * vertexStride;

                // Get vertex data as float
                float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                meshPart.VertexBuffer.GetData<float>(vertexData);

                // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                {
                    Vector3 transformedPosition = new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]);

                    min = Vector3.Min(min, transformedPosition);
                    max = Vector3.Max(max, transformedPosition);
                }
                /*
                if (md.Current.Name.Contains("plantTest"))
                {
                    mesh = md.Current.MeshParts[0];
                    float[] tab = new float[mesh.VertexBuffer.VertexCount];
                   
                    vertex = mesh.VertexBuffer;
                    vertex.GetData(tab);
                    box = new BoundingBox(new Vector3(tab[0], tab[1], tab[2]), new Vector3(tab[0], tab[1], tab[2]));
                    boundingBoxes.Add(box);
                }

                if (md.Current.Name.Contains("arrive"))
                {
                    float[] tab = new float[] { };
                    mesh = md.Current.MeshParts[0];
                    vertex = mesh.VertexBuffer;
                    vertex.GetData(tab);
                    arrive = new BoundingSphere(new Vector3(tab[0], tab[1], tab[2]), tab[3]);
                }
                */
            }

        }

        private void BuildBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere(Vector3.Zero, 0);

            // Merge all the model's built in bounding spheres
            foreach (ModelMesh mesh in Model.Meshes)
            {
                BoundingSphere transformed = mesh.BoundingSphere.Transform(
                   modelTransforms[mesh.ParentBone.Index]);

                sphere = BoundingSphere.CreateMerged(sphere, transformed);
            }

            this.boundingSphere = sphere;
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
