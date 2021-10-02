// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Drawing;
#if WINDOWS_UWP
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
#else
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
#endif

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction
{
    internal class GazeCursor : IGazeCursor<UIElement>
    {
        private const int DEFAULT_CURSOR_RADIUS = 5;
        private const bool DEFAULT_CURSOR_VISIBILITY = true;

#if WINDOWS_UWP
#else
        private readonly float _scalingX;
        private readonly float _scalingY;
#endif

        public void LoadSettings(IDictionary<string, object> settings)
        {
            if (settings.ContainsKey("GazeCursor.CursorVisibility"))
            {
                IsCursorVisible = (bool)settings["GazeCursor.CursorVisibility"];
            }
        }

        public bool IsCursorVisible
        {
            get
            {
                return _isCursorVisible;
            }

            set
            {
                _isCursorVisible = value;
                SetVisibility();
            }
        }

        public bool IsGazeEntered
        {
            get
            {
                return _isGazeEntered;
            }

            set
            {
                _isGazeEntered = value;
                SetVisibility();
            }
        }

        public PointF Position
        {
            get
            {
                return _cursorPosition;
            }

            set
            {
                _cursorPosition = value;
#if WINDOWS_UWP
                _gazePopup.HorizontalOffset = value.X;
                _gazePopup.VerticalOffset = value.Y;
#else
                _gazePopup.HorizontalOffset = value.X * _scalingX;
                _gazePopup.VerticalOffset = value.Y * _scalingY;
#endif
                SetVisibility();
            }
        }

        public UIElement ActiveCursor
        {
            get
            {
                return _gazePopup.Child;
            }

            set
            {
                _gazePopup.Child = value;
            }
        }

        public UIElement DefaultCursor { get; } = CreateDefaultCursor();

        internal GazeCursor()
        {
#if WINDOWS_UWP
#else
            var screenRect = System.Windows.Forms.Screen.PrimaryScreen;
            _scalingX = (float)(SystemParameters.PrimaryScreenWidth / screenRect.Bounds.Width);
            _scalingY = (float)(SystemParameters.PrimaryScreenHeight / screenRect.Bounds.Height);
#endif

            _gazePopup = new Popup
            {
                IsHitTestVisible = false,
                Child = DefaultCursor
            };
        }

        private static UIElement CreateDefaultCursor()
        {
            var gazeCursor = new Ellipse
            {
                Fill = new SolidColorBrush(Colors.IndianRed),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = 2 * DEFAULT_CURSOR_RADIUS,
                Height = 2 * DEFAULT_CURSOR_RADIUS,
                Margin = new Thickness(-DEFAULT_CURSOR_RADIUS, -DEFAULT_CURSOR_RADIUS, 0, 0),
                IsHitTestVisible = false
            };
            return gazeCursor;
        }

        private void SetVisibility()
        {
            var isOpen = _isCursorVisible && _isGazeEntered;
            if (_gazePopup.IsOpen != isOpen)
            {
                _gazePopup.IsOpen = isOpen;
            }
#if WINDOWS_UWP
            else if (isOpen)
            {
                Popup topmost;

                if (Windows.Foundation.Metadata.ApiInformation.IsPropertyPresent("Windows.UI.Xaml.UIElement", "XamlRoot") && _gazePopup.XamlRoot != null)
                {
                    topmost = VisualTreeHelper.GetOpenPopupsForXamlRoot(_gazePopup.XamlRoot).First();
                }
                else
                {
                    topmost = VisualTreeHelper.GetOpenPopups(Window.Current).First();
                }

                if (_gazePopup != topmost)
                {
                    _gazePopup.IsOpen = false;
                    _gazePopup.IsOpen = true;
                }
            }
#endif
        }

        private readonly Popup _gazePopup;
        private PointF _cursorPosition = default;
        private bool _isCursorVisible = DEFAULT_CURSOR_VISIBILITY;
        private bool _isGazeEntered;
    }
}