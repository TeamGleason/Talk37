using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Input.Preview;

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction.Device
{
    public class GazeDevice : IGazeDevice
    {
        private readonly GazeDeviceWatcherPreview _watcher = GazeInputSourcePreview.CreateWatcher();
        private readonly List<GazeDevicePreview> _devices = new List<GazeDevicePreview>();
        private GazeInputSourcePreview _input;

        public static GazeDevice Instance => _instance.Value;
        private static ThreadLocal<GazeDevice> _instance = new ThreadLocal<GazeDevice>(() => new GazeDevice());

        public bool IsAvailable => _devices.Count != 0;

        public event EventHandler IsAvailableChanged
        {
            add => _isDeviceAvailableChanged += value;
            remove => _isDeviceAvailableChanged -= value;
        }
        EventHandler _isDeviceAvailableChanged;

        public event EventHandler GazeEntered
        {
            add => _gazeEntered += value;
            remove => _gazeEntered -= value;
        }
        private EventHandler _gazeEntered;

        public event EventHandler<GazeMovedEventArgs> GazeMoved
        {
            add => _gazeMoved += value;
            remove => _gazeMoved -= value;
        }
        private EventHandler<GazeMovedEventArgs> _gazeMoved;

        public event EventHandler GazeExited
        {
            add => _gazeExited += value;
            remove => _gazeExited -= value;
        }
        private EventHandler _gazeExited;

        private GazeDevice()
        {
            _watcher.Added += OnDeviceAdded;
            _watcher.Removed += OnDeviceRemoved;
            _watcher.Start();
        }

        ~GazeDevice()
        {
            _watcher.Added -= OnDeviceAdded;
            _watcher.Removed -= OnDeviceRemoved;
        }

        /// <summary>
        /// Run device calibration.
        /// </summary>
        /// <returns>Task that returns true, if calibration completes successfully; otherwise, false.</returns>
        public Task<bool> RequestCalibrationAsync()
        {
            return _devices.Count == 1 ?
                _devices[0].RequestCalibrationAsync().AsTask() :
                Task.FromResult(false);
        }

        private void OnDeviceAdded(GazeDeviceWatcherPreview sender, GazeDeviceWatcherAddedPreviewEventArgs args)
        {
            _devices.Add(args.Device);

            if (_devices.Count == 1)
            {
                _input = GazeInputSourcePreview.GetForCurrentView();
                _input.GazeEntered += OnGazeEntered;
                _input.GazeMoved += OnGazeMoved;
                _input.GazeExited += OnGazeExited;

                _isDeviceAvailableChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnDeviceRemoved(GazeDeviceWatcherPreview sender, GazeDeviceWatcherRemovedPreviewEventArgs args)
        {
            var index = 0;
            while (index < _devices.Count && _devices[index].Id != args.Device.Id)
            {
                index++;
            }

            if (index < _devices.Count)
            {
                _devices.RemoveAt(index);
            }
            else
            {
                _devices.RemoveAt(0);
            }

            if (_devices.Count == 0)
            {
                _input.GazeExited += OnGazeExited;
                _input.GazeMoved += OnGazeMoved;
                _input.GazeEntered += OnGazeEntered;
                _input = GazeInputSourcePreview.GetForCurrentView();

                _isDeviceAvailableChanged?.Invoke(null, null);
            }
        }

        private void OnGazeEntered(GazeInputSourcePreview sender, GazeEnteredPreviewEventArgs args)
        {
            _gazeEntered?.Invoke(this, EventArgs.Empty);
        }

        private void OnGazeMoved(GazeInputSourcePreview sender, GazeMovedPreviewEventArgs args)
        {
            var handler = _gazeMoved;
            if (handler != null)
            {
                var wrappedArgs = new UniversalGazeMovedArgs(args);
                handler.Invoke(this, wrappedArgs);
            }
        }

        private void OnGazeExited(GazeInputSourcePreview sender, GazeExitedPreviewEventArgs args)
        {
            _gazeExited?.Invoke(this, EventArgs.Empty);
        }
    }
}
