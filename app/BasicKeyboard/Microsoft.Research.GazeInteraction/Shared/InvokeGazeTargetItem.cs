using System;
using System.Linq;
using PointF = System.Drawing.PointF;
#if WINDOWS_UWP
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Media;
#else
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Media;
#endif

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction
{
    class InvokeGazeTargetItem : GazeTargetItem
    {
        private readonly FrameworkElement _element;
        private readonly Action<UIElement> _action;

        private GazeFeedbackControl _feedbackPopup;

        internal InvokeGazeTargetItem(FrameworkElement element, Action<UIElement> action)
        {
            _element = element;
            _action = action;
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

        private TimeSpan GetElementStateDelay(DependencyProperty property, TimeSpan defaultValue)
        {
            UIElement walker = _element;
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

        protected override TimeSpan GetElementStateDelay(PointerState pointerState, TimeSpan defaultValue)
        {
            var property = GetProperty(pointerState);
            var value = GetElementStateDelay(property, defaultValue);
            return value;
        }

        protected override TimeSpan GetElementRepeatDelay(TimeSpan defaultValue)
        {
            var value = GetElementStateDelay(GazeInput.RepeatDelayDurationProperty, defaultValue);
            return value;
        }

        protected override int GetMaxDwellRepeatCount() => GazeInput.GetMaxDwellRepeatCount(_element);

        protected override void Invoke() => _action(_element);

        protected override void ShowFeedback(DwellProgressState state, double progress)
        {
            if (state != DwellProgressState.Idle)
            {
                if (_feedbackPopup == null)
                {
                    _feedbackPopup = GazeInput.GazeFeedbackPopupFactory.Get(_element);
                }

                _feedbackPopup.SetState(state, progress);
            }
            else
            {
                if (_feedbackPopup != null)
                {
                    GazeInput.GazeFeedbackPopupFactory.Return(_feedbackPopup);
                    _feedbackPopup = null;
                }
            }
        }

#if WINDOWS_UWP
        internal static GazeTargetItem GetHitTarget(PointF gazePoint)
        {
            GazeTargetItem invokable;

            switch (Window.Current.CoreWindow.ActivationMode)
            {
                case CoreWindowActivationMode.ActivatedInForeground:
                case CoreWindowActivationMode.ActivatedNotForeground:
                    var gazePointD = new Point(gazePoint.X, gazePoint.Y);
                    var elements = VisualTreeHelper.FindElementsInHostCoordinates(gazePointD, null, false);
                    var element = elements.FirstOrDefault();

                    invokable = GetInvokable(element);

                    break;

                default:
                    invokable = null;
                    break;
            }

            return invokable;
        }
#else
        internal static GazeTargetItem GetHitTarget(PointF gazePoint)
        {
            GazeTargetItem invokable;

            var gazePointD = new Point(gazePoint.X, gazePoint.Y);
            var window = Application.Current.MainWindow;
            if (window != null)
            {
                var pointFromScreen = window.PointFromScreen(new Point(gazePoint.X, gazePoint.Y));
                var hitTestParameters = new PointHitTestParameters(pointFromScreen);
                var element = (UIElement)null;
                var pointHit = new Point();
                VisualTreeHelper.HitTest(window, null, OnResultCallback, hitTestParameters);

                HitTestResultBehavior OnResultCallback(HitTestResult result)
                {
                    HitTestResultBehavior value;

                    if (result is PointHitTestResult hitTestResult &&
                        hitTestResult.VisualHit is UIElement elementHit &&
                        elementHit.IsHitTestVisible)
                    {
                        element = elementHit;
                        pointHit = hitTestResult.PointHit;
                        value = HitTestResultBehavior.Stop;
                    }
                    else
                    {
                        value = HitTestResultBehavior.Continue;
                    }

                    return value;
                }

                invokable = GetInvokable(element);
            }
            else
            {
                invokable = null;
            }

            return invokable;
        }
#endif
        private static GazeTargetItem GetInvokable(UIElement element)
        {
            GazeTargetItem invokable;

            var invokableElement = element;

            if (invokableElement == null)
            {
                invokable = null;
            }
            else
            {
                invokable = GazeTargetFactory.GetOrCreate(invokableElement);

                while (invokableElement != null && invokable == null)
                {
                    invokableElement = VisualTreeHelper.GetParent(invokableElement) as UIElement;

                    if (invokableElement != null)
                    {
                        invokable = GazeTargetFactory.GetOrCreate(invokableElement);
                    }
                }
            }

            if (invokable != null)
            {
                Interaction interaction;
                do
                {
                    interaction = GazeInput.GetInteraction(invokableElement);
                    if (interaction == Interaction.Inherited)
                    {
                        invokableElement = GetInheritenceParent(invokableElement);
                    }
                }
                while (interaction == Interaction.Inherited && invokableElement != null);

                if (interaction == Interaction.Inherited)
                {
                    interaction = GazeInput.Interaction;
                }

                if (interaction != Interaction.Enabled)
                {
                    invokable = null;
                }
            }

            return invokable;
        }
    }
}
