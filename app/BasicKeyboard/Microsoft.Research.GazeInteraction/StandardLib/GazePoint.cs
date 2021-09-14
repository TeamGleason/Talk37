using System;
using System.Drawing;

namespace StandardLib
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
