using Golf.Core.ModelGolf.Cam;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
        }
        public abstract void HandleModelCollision(GameObject otherModel);
        public abstract void Draw(GameTime gameTime, Camera camera);

        public List<BoundingBox> GetBounds()
        {
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            List<BoundingBox> boxes = new List<BoundingBox>();

            foreach (ModelMesh mesh in this._model.Model.Meshes)
            {
                
                    foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    {
                        int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                        int vertexBufferSize = meshPart.NumVertices * vertexStride;

                        int vertexDataSize = vertexBufferSize / sizeof(float);
                        float[] vertexData = new float[vertexDataSize];
                        meshPart.VertexBuffer.GetData<float>(vertexData);

                        for (int i = 0; i < vertexDataSize; i += vertexStride / sizeof(float))
                        {
                            Vector3 vertex = new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]);
                            min = Vector3.Min(min, vertex);
                            max = Vector3.Max(max, vertex);
                        }
                        boxes.Add(new BoundingBox(min, max));
                        min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue); 
                        max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
                    
                    }
            }
            return boxes;
        }

    }
}
