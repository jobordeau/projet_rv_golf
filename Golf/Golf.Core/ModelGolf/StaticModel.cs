
using ConversionHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Matrix = BEPUutilities.Matrix;

namespace Golf.Core.ModelGolf
{
    /// <summary>
    /// Component that draws a model.
    /// </summary>
    public class StaticModel : DrawableGameComponent
    {
        Model _model;
        /// <summary>
        /// Base transformation to apply to the model.
        /// </summary>
        public Matrix Transform;
        Microsoft.Xna.Framework.Matrix[] _boneTransforms;


        /// <summary>
        /// Creates a new StaticModel.
        /// </summary>
        /// <param name="model">Graphical representation to use for the entity.</param>
        /// <param name="transform">Base transformation to apply to the model before moving to the entity.</param>
        /// <param name="game">Game to which this component will belong.</param>
        public StaticModel(Model model, Matrix transform, Game game)
            : base(game)
        {
            this._model = model;
            this.Transform = transform;

            //Collect any bone transformations in the model itself.
            //The default cube model doesn't have any, but this allows the StaticModel to work with more complicated shapes.
            _boneTransforms = new Microsoft.Xna.Framework.Matrix[model.Bones.Count];
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _model.CopyAbsoluteBoneTransformsTo(_boneTransforms);
            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = _boneTransforms[mesh.ParentBone.Index] * MathConverter.Convert(Transform);
                    effect.View = MathConverter.Convert((Game as MiniGolf).CameraClassic.ViewMatrix);
                    effect.Projection = MathConverter.Convert((Game as MiniGolf).CameraClassic.ProjectionMatrix);
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
