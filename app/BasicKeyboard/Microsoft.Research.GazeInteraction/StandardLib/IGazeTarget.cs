using System.Collections.Generic;
using System.Drawing;

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction
{
    public interface IGazeTarget<TElement>
        where TElement : class
    {
        void LoadSettings(IDictionary<string, object> settings);

        bool IsCursorVisible { get; set; }

        bool IsGazeEntered { get; set; }

        PointF Position { get; set; }

        TElement ActiveCursor { get; set; }

        TElement DefaultCursor { get; }

        GazeTargetItem<TElement> MissedGazeTargetItem { get; }

        /// <summary>
        /// Get existing item at specificified point.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        GazeTargetItem<TElement> GetOrCreateItem(double x, double y);
    }
}
