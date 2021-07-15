using System.Windows.Controls.Primitives;
using TeamGleason.SpeakFaster.KeyboardLayouts;

namespace TeamGleason.SpeakFaster.SimpleKeyboard
{
    internal abstract class ButtonManager
    {
        internal readonly KeyboardControl _parent;

        internal ButtonManager(KeyboardControl parent, ButtonBase button)
        {
            _parent = parent;
            Button = button;
        }

        internal ButtonBase Button { get; }

        internal static TextButtonManager Create(KeyboardControl parent, TextKey key)
        {
            var manager = TextButtonManager.Create(parent, key);
            return manager;
        }
        internal static CommandButtonManager Create(KeyboardControl parent, CommandKey key)
        {
            var manager = CommandButtonManager.Create(parent, key);
            return manager;
        }
        internal static PredictionButtonManager Create(KeyboardControl parent, PredictionKey key)
        {
            var manager = PredictionButtonManager.Create(parent, key);
            return manager;
        }
    }
}
