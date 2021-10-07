using System;
#if WINDOWS_UWP
using System.Linq;
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
        internal static UIElement GetInheritenceParent(UIElement child)
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
    }
}
