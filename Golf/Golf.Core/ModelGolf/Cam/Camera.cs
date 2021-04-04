
using System;
using BEPUutilities;
using Microsoft.Xna.Framework.Input;

namespace Golf.Core.ModelGolf.Cam
{
    /// <summary>
    /// Basic camera class supporting mouse/keyboard/gamepad-based movement.
    /// </summary>
    public class Camera
    {

        public Matrix3x3 Rotation { get; set; }

        /// <summary>
        /// Gets or sets the position of the camera.
        /// </summary>
        public Vector3 Position { get; set; }


        /// <summary>
        /// Gets or sets the projection matrix of the camera.
        /// </summary>
        public Matrix ProjectionMatrix { get; set; }


        /// <summary>
        /// Gets the view matrix of the camera.
        /// </summary>
        public Matrix ViewMatrix
        {
            get { return Matrix.CreateViewRH(Position, _viewDirection, _lockedUp); }
        }

        /// <summary>
        /// Gets the world transformation of the camera.
        /// </summary>
        public Matrix WorldMatrix
        {
            get { return Matrix.CreateWorldRH(Position, _viewDirection, _lockedUp); }
        }

        private Vector3 _viewDirection = Vector3.Forward;

        /// <summary>
        /// Gets or sets the view direction of the camera.
        /// </summary>
        public Vector3 ViewDirection
        {
            get { return _viewDirection; }
            set
            {
                float lengthSquared = value.LengthSquared();
                if (lengthSquared > Toolbox.Epsilon)
                {
                    Vector3.Divide(ref value, (float) Math.Sqrt(lengthSquared), out value);
                    //Validate the input. A temporary violation of the maximum pitch is permitted as it will be fixed as the user looks around.
                    //However, we cannot allow a view direction parallel to the locked up direction.
                    float dot;
                    Vector3.Dot(ref value, ref _lockedUp, out dot);
                    if (Math.Abs(dot) > 1 - Toolbox.BigEpsilon)
                    {
                        //The view direction must not be aligned with the locked up direction.
                        //Silently fail without changing the view direction.
                        return;
                    }

                    _viewDirection = value;
                }
            }
        }

        private float _maximumPitch = MathHelper.PiOver2 * 0.99f;

        /// <summary>
        /// Gets or sets how far the camera can look up or down in radians.
        /// </summary>
        public float MaximumPitch
        {
            get { return _maximumPitch; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Maximum pitch corresponds to pitch magnitude; must be positive.");
                if (value >= MathHelper.PiOver2)
                    throw new ArgumentException("Maximum pitch must be less than Pi/2.");
                _maximumPitch = value;
            }
        }

        private Vector3 _lockedUp = Vector3.Up;

        /// <summary>
        /// Gets or sets the current locked up vector of the camera.
        /// </summary>
        public Vector3 LockedUp
        {
            get { return _lockedUp; }
            set
            {
                var oldUp = _lockedUp;
                float lengthSquared = value.LengthSquared();
                if (lengthSquared > Toolbox.Epsilon)
                {
                    Vector3.Divide(ref value, (float) Math.Sqrt(lengthSquared), out _lockedUp);
                    //Move the view direction with the transform. This helps guarantee that the view direction won't end up aligned with the up vector.
                    Quaternion rotation;
                    Quaternion.GetQuaternionBetweenNormalizedVectors(ref oldUp, ref _lockedUp, out rotation);
                    Quaternion.Transform(ref _viewDirection, ref rotation, out _viewDirection);
                }

                //If the new up vector was a near-zero vector, silently fail without changing the up vector.
            }
        }


        /// <summary>
        /// Creates a camera.
        /// </summary>
        /// <param name="position">Initial position of the camera.</param>
        /// <param name="pitch">Initial pitch angle of the camera.</param>
        /// <param name="yaw">Initial yaw value of the camera.</param>
        /// <param name="projectionMatrix">Projection matrix used.</param>
        public Camera(Vector3 position, float pitch, float yaw, Matrix projectionMatrix)
        {
            Position = position;
            Yaw(yaw);
            Pitch(pitch);
            ProjectionMatrix = projectionMatrix;
        }



        /// <summary>
        /// Moves the camera forward.
        /// </summary>
        /// <param name="distance">Distance to move.</param>
        public void MoveForward(float distance)
        {
            Position += WorldMatrix.Forward * distance;
        }

        /// <summary>
        /// Moves the camera to the right.
        /// </summary>
        /// <param name="distance">Distance to move.</param>
        public void MoveRight(float distance)
        {
            Position += WorldMatrix.Right * distance;
        }

        /// <summary>
        /// Moves the camera up.
        /// </summary>
        /// <param name="distance">Distance to move.</param>
        public void MoveUp(float distance)
        {
            Position += new Vector3(0, distance, 0);
        }


        /// <summary>
        /// Rotates the camera around its locked up vector.
        /// </summary>
        /// <param name="radians">Amount to rotate.</param>
        public void Yaw(float radians)
        {
            //Rotate around the up vector.
            Matrix3x3 rot;
            Matrix3x3.CreateFromAxisAngle(ref _lockedUp, radians, out rot);
            Matrix3x3.Transform(ref _viewDirection, ref rot, out _viewDirection);

            //Avoid drift by renormalizing.
            _viewDirection.Normalize();

            Rotation = rot;
        }

        /// <summary>
        /// Rotates the view direction up or down relative to the locked up vector.
        /// </summary>
        /// <param name="radians">Amount to rotate.</param>
        public void Pitch(float radians)
        {
            //Do not allow the new view direction to violate the maximum pitch.
            float dot;
            Vector3.Dot(ref _viewDirection, ref _lockedUp, out dot);

            //While this could be rephrased in terms of dot products alone, converting to actual angles can be more intuitive.
            //Consider +Pi/2 to be up, and -Pi/2 to be down.
            float currentPitch = (float) Math.Acos(MathHelper.Clamp(-dot, -1, 1)) - MathHelper.PiOver2;
            //Compute our new pitch by clamping the current + change.
            float newPitch = MathHelper.Clamp(currentPitch + radians, -_maximumPitch, _maximumPitch);
            float allowedChange = newPitch - currentPitch;

            //Compute and apply the rotation.
            Vector3 pitchAxis;
            Vector3.Cross(ref _viewDirection, ref _lockedUp, out pitchAxis);
            //This is guaranteed safe by all interaction points stopping viewDirection from being aligned with lockedUp.
            pitchAxis.Normalize();
            Matrix3x3 rotation;
            Matrix3x3.CreateFromAxisAngle(ref pitchAxis, allowedChange, out rotation);
            Matrix3x3.Transform(ref _viewDirection, ref rotation, out _viewDirection);

            //Avoid drift by renormalizing.
            _viewDirection.Normalize();
        }
    }
}
