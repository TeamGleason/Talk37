// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Windows.UI.Xaml.Controls.Primitives;

#if WINDOWS_UWP
namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction
#else
namespace FrameworkLib;
#endif
{
    internal class GazeFeedbackPopupFactory
    {
        private readonly List<Popup> _cache = new List<Popup>();

        public Popup Get()
        {
            Popup popup;
            Windows.UI.Xaml.Shapes.Rectangle rectangle;

            if (_cache.Count != 0)
            {
                popup = _cache[0];
                _cache.RemoveAt(0);

                rectangle = popup.Child as Windows.UI.Xaml.Shapes.Rectangle;
            }
            else
            {
                popup = new Popup();

                rectangle = new Windows.UI.Xaml.Shapes.Rectangle
                {
                    IsHitTestVisible = false
                };

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
