﻿using System.Collections.Generic;
using System.Drawing;

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction
{
    public interface IGazeCursor
    {
        void LoadSettings(IDictionary<string, object> settings);

        bool IsCursorVisible { get; set; }

        bool IsGazeEntered { get; set; }

        PointF Position { get; set; }
    }
}