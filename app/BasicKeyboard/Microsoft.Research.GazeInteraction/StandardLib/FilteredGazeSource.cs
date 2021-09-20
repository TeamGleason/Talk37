using System;
using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction
{
    public class FilteredGazeSource : IGazeSource
    {
        private readonly IGazeDevice _device;
        private readonly IGazeFilter _filter;

        public FilteredGazeSource(IGazeDevice device, IGazeFilter filter)
        {
            _device = device;
            _filter = filter;
        }

        event EventHandler IGazeSource.GazeEntered
        {
            add => _device.GazeEntered += value;
            remove => _device.GazeEntered -= value;
        }

        event EventHandler<GazeMovedEventArgs> IGazeSource.GazeMoved
        {
            add
            {
                if (_gazeMoved == null)
                {
                    _device.GazeMoved += OnGazeMoved;
                }

                _gazeMoved += value;
            }
            remove
            {
                _gazeMoved -= value;

                if (_gazeMoved == null)
                {
                    _device.GazeMoved -= value;
                }
            }
        }

        private class UpdatedGazeMovedEventArgs : GazeMovedEventArgs
        {
            private readonly IEnumerable<GazePoint> _points;

            internal UpdatedGazeMovedEventArgs(IEnumerable<GazePoint> points)
            {
                _points = points;
            }

            public override IEnumerable<GazePoint> GetGazePoints() => _points;
        }

        private void OnGazeMoved(object sender, GazeMovedEventArgs e)
        {
            var points = new List<GazePoint>(1);
            foreach (var unfiltered in e.GetGazePoints())
            {

                var filteredPosition = _filter.Update(unfiltered.Timestamp, unfiltered.Position);
                var filtered = new GazePoint(unfiltered.Timestamp, filteredPosition);
                points.Add(filtered);
            }
            var updatedArgs = new UpdatedGazeMovedEventArgs(points);

            _gazeMoved?.Invoke(sender, updatedArgs);
        }
        EventHandler<GazeMovedEventArgs> _gazeMoved;

        event EventHandler IGazeSource.GazeExited
        {
            add => _device.GazeExited += value;
            remove => _device.GazeExited -= value;
        }

        event EventHandler IGazeSource.EyesOff
        {
            add => _device.EyesOff += value;
            remove => _device.EyesOff -= value;
        }
    }
}
