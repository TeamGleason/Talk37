using System;
using System.Collections.Generic;
using System.Drawing;
using Windows.Devices.Input.Preview;

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction.Device
{
    internal class UniversalGazeMovedArgs : GazeMovedEventArgs
    {
        private readonly GazeMovedPreviewEventArgs _args;

        public UniversalGazeMovedArgs(GazeMovedPreviewEventArgs args)
        {
            _args = args;
        }

        public override IEnumerable<GazePoint> GetGazePoints()
        {
            var intermediatePoints = _args.GetIntermediatePoints();
            foreach (var point in intermediatePoints)
            {
                var position = point.EyeGazePosition;
                if (position.HasValue)
                {
                    var timestamp = new TimeSpan(10 * (long)point.Timestamp);
                    var value = position.Value;
                    var pointF = new PointF((float)value.X, (float)value.Y);
                    yield return new GazePoint(timestamp, pointF);
                }
            }
        }
    }
}