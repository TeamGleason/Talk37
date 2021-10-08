using CefSharp.Wpf;
using Microsoft.Toolkit.Uwp.Input.GazeInteraction;
using System.Diagnostics;

namespace WebWatcher
{
    class BrowserTargetItem : FrameworkGazeTargetItem
    {
        internal BrowserTargetItem(ChromiumWebBrowser element)
            : base(element)
        {

        }

        public override GazeTargetItem Specify(double x, double y)
        {
            Debug.WriteLine($"Spefify({x},{y})");
            return base.Specify(x, y);
        }

        protected override GazeFeedbackControl GetFeedbackControl()
        {
            var control = GetFeedbackControl(100, 200, 300, 50);
            return control;
        }

        protected override void Invoke()
        {
            Debug.WriteLine("Invoked");
        }
    }
}
