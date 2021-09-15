// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
#if WINDOWS_UWP
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Shapes;
#else
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;
#endif

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction
{
    internal class GazeFeedbackPopupFactory
    {
        private readonly List<Popup> _cache = new List<Popup>();

        public Popup Get()
        {
            Popup popup;
            Rectangle rectangle;

            if (_cache.Count != 0)
            {
                popup = _cache[0];
                _cache.RemoveAt(0);

                rectangle = popup.Child as Rectangle;
            }
            else
            {
                popup = new Popup();

                rectangle = new Rectangle
                {
                    IsHitTestVisible = false
                };

#if WINDOWS_UWP
#else
                popup.AllowsTransparency = true;
                popup.Placement = PlacementMode.Absolute;
#endif

                popup.Child = rectangle;
            }

            rectangle.StrokeThickness = GazeTargetItem.GazeInput_DwellFeedbackStrokeThickness;

            return popup;
        }

        public void Return(Popup popup)
        {
            popup.IsOpen = false;
            _cache.Add(popup);
        }
    }
}
