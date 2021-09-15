using System;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction
{
    public interface IGazeDevice
    {
        bool IsAvailable { get; }

        event EventHandler IsAvailableChanged;

        event EventHandler GazeEntered;

        event EventHandler GazeExited;

        event EventHandler<GazeMovedEventArgs> GazeMoved;

        Task<bool> RequestCalibrationAsync();
    }
}
