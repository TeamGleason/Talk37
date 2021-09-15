using System;
using System.Drawing;

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction
{
    public struct GazePoint
    {
        public GazePoint(TimeSpan timestamp, PointF position)
        {
            Timestamp = timestamp;
            Position = position;
        }

        public TimeSpan Timestamp { get; }

        public PointF Position { get; }
    }
}
