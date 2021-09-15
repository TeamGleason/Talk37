using System;
#if WINDOWS_UWP
using Windows.UI.Xaml;
#else
using System.Windows;
#endif

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction
{
    class InvokeGazeTargetItem : GazeTargetItem
    {
        private readonly Action<UIElement> _action;

        internal InvokeGazeTargetItem(UIElement element, Action<UIElement> action)
            : base(element)
        {
            _action = action;
        }

        internal override void Invoke(UIElement element) => _action(element);
    }
}
