using System.Windows.Controls.Primitives;
using TeamGleason.SpeakFaster.KeyboardLayouts;

namespace TeamGleason.SpeakFaster.SimpleKeyboard
{
    internal class PredictionButtonManager : ButtonManager
    {
        public PredictionButtonManager(KeyboardControl parent, PredictionKey key, ButtonBase button)
            : base(parent, button)
        {
        }

        internal static PredictionButtonManager CreateInstance(KeyboardControl parent, PredictionKey key)
        {
            var button = new KeyboardButton();
            var manager = new PredictionButtonManager(parent, key, button);
            return manager;
        }
    }
}