using Microsoft.HandsFree.Sensors;
using StandardLib;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction.Device
{
    internal class LegacyGazePointerGazeMovedEventArgs : GazeMovedEventArgs
    {
        private GazeEventArgs _args;

        public LegacyGazePointerGazeMovedEventArgs(GazeEventArgs e)
        {
            _args = e;
        }

        public override IEnumerable<GazePoint> GetGazePoints()
        {
            var position = new PointF((float)_args.Screen.X, (float)_args.Screen.Y);
            var timestamp = new TimeSpan(10000 * _args.Timestamp);
            var gazePoint = new GazePoint(timestamp, position);
            yield return gazePoint;
        }
    }
}