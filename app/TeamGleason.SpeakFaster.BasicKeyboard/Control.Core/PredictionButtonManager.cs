using System.Windows.Controls.Primitives;
using TeamGleason.SpeakFaster.BasicKeyboard.Layout.Standard;

namespace TeamGleason.SpeakFaster.BasicKeyboard.Control.Core
{
    internal class PredictionButtonManager : ButtonManager<PredictionKey>
    {
        public PredictionButtonManager(KeyboardControl parent, PredictionKey key, ButtonBase button)
            : base(parent, key, button)
        {
        }

        internal static PredictionButtonManager CreateInstance(KeyboardControl parent, PredictionKey key)
        {
            var button = new KeyboardButton();
            var manager = new PredictionButtonManager(parent, key, button);
            return manager;
        }

        protected override void Execute()
        {
        }
    }
}