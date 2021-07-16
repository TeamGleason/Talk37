using System.Diagnostics;
using System.Windows.Controls.Primitives;
using TeamGleason.SpeakFaster.KeyboardLayouts;

namespace TeamGleason.SpeakFaster.SimpleKeyboard
{
    internal class NavigateCommandButtonManager : CommandButtonManager
    {
        private NavigateCommandButtonManager(KeyboardControl parent, CommandKey key, ButtonBase button)
            : base(parent, key, button)
        {
        }

        internal static NavigateCommandButtonManager CreateInstance(KeyboardControl parent, CommandKey key)
        {
            Debug.Assert(!key.Toggles);
            ButtonBase button = new KeyboardButton();
            var manager = new NavigateCommandButtonManager(parent, key, button);
            return manager;
        }

        protected override void Execute()
        {
            _parent.NavigateToView(_key.CommandParameter);
        }
    }
}