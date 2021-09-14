using System;
using Windows.UI.Xaml;

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
