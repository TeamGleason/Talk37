using System;
using System.Diagnostics;

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction
{
    public class GazeHitTestArgs : EventArgs
    {
        public bool Handled => _target != null;

        public double X { get; private set; }

        public double Y { get; private set; }

        internal GazeTargetItem Target => _target;
        private GazeTargetItem _target;

        internal GazeHitTestArgs(double x, double y)
        {
            X = x;
            Y = y;
        }

        public void SetTarget(GazeTargetItem target)
        {
            Debug.Assert(_target == null);
            _target = target;
        }
    }
}
