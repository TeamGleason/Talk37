using System;
using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction
{
    public abstract class GazeMovedEventArgs : EventArgs
    {
        public abstract IEnumerable<GazePoint> GetGazePoints();
    }
}
