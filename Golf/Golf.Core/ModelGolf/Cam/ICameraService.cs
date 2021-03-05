using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Golf.Core.ModelGolf.Cam
{
    public interface ICameraService
    {
        Matrix View { get; }
        Matrix Projection { get; }
    }
}
