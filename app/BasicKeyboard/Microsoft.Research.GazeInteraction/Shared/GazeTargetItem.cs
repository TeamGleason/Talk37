// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using PointF = System.Drawing.PointF;
using System.Linq;
#if WINDOWS_UWP
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
#else
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
#endif

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction
{
    internal abstract class GazeTargetItem
    {
#if WINDOWS_UWP
#else
        private double _scalingX = 1;
        private double _scalingY = 1;
#endif

        internal TimeSpan DetailedTime { get; set; }

        internal TimeSpan OverflowTime { get; set; }

        internal TimeSpan ElapsedTime
        {
            get { return DetailedTime + OverflowTime; }
        }

        internal TimeSpan NextStateTime { get; set; }

        internal TimeSpan LastTimestamp { get; set; }

        internal PointerState ElementState { get; set; }

        private UIElement TargetElement { get; set; }

        internal int RepeatCount { get; set; }

        internal int MaxDwellRepeatCount { get; set; }

        internal GazeTargetItem(UIElement target)
        {
#if WINDOWS_UWP
#else
            var screenRect = System.Windows.Forms.Screen.PrimaryScreen;
            _scalingX = (float)(SystemParameters.PrimaryScreenWidth / screenRect.Bounds.Width);
            _scalingY = (float)(SystemParameters.PrimaryScreenHeight / screenRect.Bounds.Height);
#endif

            TargetElement = target;
        }

        internal void RaiseGazePointerEvent(PointerState state, TimeSpan elapsedTime)
        {
            if (state == PointerState.Dwell)
            {
                Invoke();
            }
        }

        internal static DependencyProperty GetProperty(PointerState state)
        {
            switch (state)
            {
                case PointerState.Fixation: return GazeInput.FixationDurationProperty;
                case PointerState.Dwell: return GazeInput.DwellDurationProperty;
                case PointerState.DwellRepeat: return GazeInput.DwellRepeatDurationProperty;
                case PointerState.Enter: return GazeInput.ThresholdDurationProperty;
                case PointerState.Exit: return GazeInput.ThresholdDurationProperty;
                default: return null;
            }
        }

        internal TimeSpan GetElementStateDelay(GazePointer gazePointer, PointerState pointerState)
        {
            var property = GetProperty(pointerState);
            var defaultValue = gazePointer.GetDefaultPropertyValue(pointerState);
            var ticks = GetElementStateDelay(gazePointer, property, defaultValue);

            switch (pointerState)
            {
                case PointerState.Dwell:
                case PointerState.DwellRepeat:
                    gazePointer._maxHistoryTime = new TimeSpan(Math.Max(gazePointer._maxHistoryTime.Ticks, 2 * ticks.Ticks));
                    break;
            }

            return ticks;
        }

        internal TimeSpan GetElementStateDelay(GazePointer gazePointer, DependencyProperty property, TimeSpan defaultValue)
        {
            UIElement walker = TargetElement;
            object valueAtWalker = walker.GetValue(property);

            while (GazeInput.UnsetTimeSpan.Equals(valueAtWalker) && walker != null)
            {
                walker = GetInheritenceParent(walker);

                if (walker != null)
                {
                    valueAtWalker = walker.GetValue(property);
                }
            }

            var ticks = GazeInput.UnsetTimeSpan.Equals(valueAtWalker) ? defaultValue : (TimeSpan)valueAtWalker;

            return ticks;
        }

        /// <summary>
        /// Find the parent to inherit properties from.
        /// </summary>
        private static UIElement GetInheritenceParent(UIElement child)
        {
            // The result value.
            object parent = null;

            // Get the automation peer...
            var peer = FrameworkElementAutomationPeer.FromElement(child);
            if (peer != null)
            {
                // ...if it exists, get the peer's parent...
#if WINDOWS_UWP
                var peerParent = peer.Navigate(AutomationNavigationDirection.Parent) as FrameworkElementAutomationPeer;
#else
                var peerParent = peer.GetParent() as FrameworkElementAutomationPeer;
#endif
                if (peerParent != null)
                {
                    // ...and if it has a parent, get the corresponding object.
                    parent = peerParent.Owner;
                }
            }

            // If the above failed to find a parent...
            if (parent == null)
            {
                // ...use the visual parent.
                parent = VisualTreeHelper.GetParent(child);
            }

            // Safely pun the value we found to a UIElement reference.
            return parent as UIElement;
        }

        internal static GazeTargetItem GetHitTarget(PointF gazePoint)
        {
            GazeTargetItem invokable = null;

#if WINDOWS_UWP
            switch (Window.Current.CoreWindow.ActivationMode)
            {
                case CoreWindowActivationMode.ActivatedInForeground:
                case CoreWindowActivationMode.ActivatedNotForeground:
#endif
            var gazePointD = new Point(gazePoint.X, gazePoint.Y);
#if WINDOWS_UWP
            var elements = VisualTreeHelper.FindElementsInHostCoordinates(gazePointD, null, false);
#else
            var window = Application.Current.MainWindow;
            var pointFromScreen = window.PointFromScreen(new Point(gazePoint.X, gazePoint.Y));
            var hitTestParameters = new PointHitTestParameters(pointFromScreen);
            var visualHit = (UIElement)null;
            var pointHit = new Point();
            VisualTreeHelper.HitTest(window, null, OnResultCallback, hitTestParameters);

            HitTestResultBehavior OnResultCallback(HitTestResult result)
            {
                HitTestResultBehavior value;

                if (result is PointHitTestResult hitTestResult &&
                    hitTestResult.VisualHit is UIElement elementHit &&
                    elementHit.IsHitTestVisible)
                {
                    visualHit = elementHit;
                    pointHit = hitTestResult.PointHit;
                    value = HitTestResultBehavior.Stop;
                }
                else
                {
                    value = HitTestResultBehavior.Continue;
                }

                return value;
            }

            var elements = visualHit != null ?
                new UIElement[] { visualHit } :
                new UIElement[0];
#endif
            var element = elements.FirstOrDefault();

            invokable = null;

            if (element != null)
            {
                invokable = GazeTargetFactory.GetOrCreate(element);

                while (element != null && invokable==null)
                {
                    element = VisualTreeHelper.GetParent(element) as UIElement;

                    if (element != null)
                    {
                        invokable = GazeTargetFactory.GetOrCreate(element);
                    }
                }
            }

            if (element != null && invokable!=null)
            {
                Interaction interaction;
                do
                {
                    interaction = GazeInput.GetInteraction(element);
                    if (interaction == Interaction.Inherited)
                    {
                        element = GetInheritenceParent(element);
                    }
                }
                while (interaction == Interaction.Inherited && element != null);

                if (interaction == Interaction.Inherited)
                {
                    interaction = GazeInput.Interaction;
                }

                if (interaction != Interaction.Enabled)
                {
                    invokable = null;
                }
            }
#if WINDOWS_UWP
                break;
            }
#endif

            return invokable;
        }

        internal void Invoke() => Invoke(TargetElement);

        internal abstract void Invoke(UIElement element);

        internal virtual bool IsInvokable
        {
            get { return true; }
        }

        internal void Reset(TimeSpan nextStateTime)
        {
            ElementState = PointerState.PreEnter;
            DetailedTime = TimeSpan.Zero;
            OverflowTime = TimeSpan.Zero;
            NextStateTime = nextStateTime;
            RepeatCount = 0;
            MaxDwellRepeatCount = GazeInput.GetMaxDwellRepeatCount(TargetElement);
        }

        internal void GiveFeedback()
        {
            if (_nextStateTime != NextStateTime)
            {
                _prevStateTime = _nextStateTime;
                _nextStateTime = NextStateTime;
            }

            if (ElementState != _notifiedPointerState)
            {
                switch (ElementState)
                {
                    case PointerState.Enter:
                        RaiseProgressEvent(DwellProgressState.Fixating);
                        break;

                    case PointerState.Dwell:
                    case PointerState.Fixation:
                        RaiseProgressEvent(DwellProgressState.Progressing);
                        break;

                    case PointerState.Exit:
                    case PointerState.PreEnter:
                        RaiseProgressEvent(DwellProgressState.Idle);
                        break;
                }

                _notifiedPointerState = ElementState;
            }
            else if (ElementState == PointerState.Dwell || ElementState == PointerState.Fixation)
            {
                if (RepeatCount <= MaxDwellRepeatCount)
                {
                    RaiseProgressEvent(DwellProgressState.Progressing);
                }
                else
                {
                    RaiseProgressEvent(DwellProgressState.Complete);
                }
            }
        }

        private void RaiseProgressEvent(DwellProgressState state)
        {
            // TODO: We should eliminate non-invokable controls before we arrive here!
            if (TargetElement is Page)
            {
                return;
            }

            if (_notifiedProgressState != state || state == DwellProgressState.Progressing)
            {
                if (state != DwellProgressState.Idle)
                {
                    if (_feedbackPopup == null)
                    {
                        _feedbackPopup = GazeInput.GazePointerInstance.GazeFeedbackPopupFactory.Get();
                    }

                    var control = TargetElement as FrameworkElement;

#if WINDOWS_UWP
                    var transform = control.TransformToVisual(_feedbackPopup);
                    var bounds = transform.TransformBounds(new Rect(
                        new Point(0, 0),
                        new Size((float)control.ActualWidth, (float)control.ActualHeight)));
#else
                    var controlLeftTop = control.PointToScreen(new Point(0, 0));
                    var controlRightBottom = control.PointToScreen(new Point(control.ActualWidth, control.ActualHeight));
                    controlLeftTop = new Point(controlLeftTop.X * _scalingX, controlLeftTop.Y * _scalingY);
                    controlRightBottom = new Point(controlRightBottom.X * _scalingX, controlRightBottom.Y * _scalingY);
                    var bounds = new Rect(controlLeftTop, controlRightBottom);
#endif
                    var feedback = (GazeFeedbackControl)_feedbackPopup.Child;
                    _feedbackPopup.HorizontalOffset = bounds.Left;
                    _feedbackPopup.VerticalOffset = bounds.Top;
                    feedback.Width = bounds.Width;
                    feedback.Height = bounds.Height;

                    var feedbackProgress = state == DwellProgressState.Progressing ?
                        ((double)(ElapsedTime - _prevStateTime).Ticks) / (_nextStateTime - _prevStateTime).Ticks : 
                        0.0;
                    feedback.SetState(state, feedbackProgress);

                    _feedbackPopup.IsOpen = true;
                }
                else
                {
                    if (_feedbackPopup != null)
                    {
                        GazeInput.GazePointerInstance.GazeFeedbackPopupFactory.Return(_feedbackPopup);
                        _feedbackPopup = null;
                    }
                }
            }

            _notifiedProgressState = state;
        }

        private PointerState _notifiedPointerState = PointerState.Exit;
        private TimeSpan _prevStateTime;
        private TimeSpan _nextStateTime;
        private DwellProgressState _notifiedProgressState = DwellProgressState.Idle;
        private Popup _feedbackPopup;
    }
}