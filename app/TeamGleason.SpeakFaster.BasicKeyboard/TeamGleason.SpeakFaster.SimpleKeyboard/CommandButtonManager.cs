using System.Windows.Controls.Primitives;
using TeamGleason.SpeakFaster.KeyboardLayouts;

namespace TeamGleason.SpeakFaster.SimpleKeyboard
{
    internal class CommandButtonManager : ButtonManager
    {
        public CommandButtonManager(KeyboardControl parent, CommandKey key, ButtonBase button)
            : base(parent, button)
        {
        }

        internal static CommandButtonManager CreateInstance(KeyboardControl parent, CommandKey key)
        {
            ButtonBase button = key.Toggles ? new KeyboardToggleButton() : new KeyboardButton();
            button.Content = key.Label ?? key.Icon;
            var manager = new CommandButtonManager(parent, key, button);
            return manager;
        }
    }
}