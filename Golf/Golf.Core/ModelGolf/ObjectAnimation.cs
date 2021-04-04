using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Core.ModelGolf
{
    /// <summary>
    /// A partial implementation of the rolling animation of the ball
    /// </summary>
    class ObjectAnimation
    {
        Vector3 _startPosition, _endPosition, _startRotation, _endRotation;
        TimeSpan _duration;
        bool _loop;

        TimeSpan _elapsedTime = TimeSpan.FromSeconds(0);

        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }

        public ObjectAnimation(Vector3 startPosition, Vector3 endPosition, Vector3 startRotation, Vector3 endRotation, TimeSpan duration, bool loop)
        {
            this._startPosition = startPosition;
            this._endPosition = endPosition;
            this._startRotation = startRotation;
            this._endRotation = endRotation;
            this._duration = duration;
            this._loop = loop;
            Position = _startPosition;
            Rotation = _startRotation;
        }

        public void Update(TimeSpan elapsed)
        {
            // Update the time
            this._elapsedTime += elapsed;

            // Determine how far along the duration value we are (0 to 1)
            float amt = (float)_elapsedTime.TotalSeconds / (float)_duration.TotalSeconds;

            if (_loop)
                while (amt > 1) // Wrap the time if we are looping
                    amt -= 1;
            else // Clamp to the end value if we are not
                amt = MathHelper.Clamp(amt, 0, 1);

            // Update the current position and rotation
            Position = Vector3.Lerp(_startPosition, _endPosition, amt);
            Rotation = Vector3.Lerp(_startRotation, _endRotation, amt);
        }
    }
}
