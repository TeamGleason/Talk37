using System;
using System.Collections.Generic;

namespace StandardLib
{
    public abstract class GazeMovedEventArgs : EventArgs
    {
        public abstract IEnumerable<GazePoint> GetGazePoints();
    }
}
