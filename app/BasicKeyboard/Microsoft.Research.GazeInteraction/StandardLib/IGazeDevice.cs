using System;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction
{
    public interface IGazeDevice
    {
        bool IsAvailable { get; }

        TimeSpan EyesOffDelay { get; set; }

        event EventHandler IsAvailableChanged;

        event EventHandler GazeEntered;

        event EventHandler<GazeMovedEventArgs> GazeMoved;

        event EventHandler GazeExited;

        event EventHandler EyesOff;

        Task<bool> RequestCalibrationAsync();
    }
}
