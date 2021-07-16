using System.Diagnostics;
using System.Windows.Controls.Primitives;
using TeamGleason.SpeakFaster.KeyboardLayouts;

namespace TeamGleason.SpeakFaster.SimpleKeyboard
{
    internal class CustomCommandButtonManager : CommandButtonManager
    {
        private CustomCommandButtonManager(KeyboardControl parent, CommandKey key, ButtonBase button)
            : base(parent, key, button)
        {
        }

        internal static CustomCommandButtonManager CreateInstance(KeyboardControl parent, CommandKey key)
        {
            Debug.Assert(!key.Toggles);
            ButtonBase button = new KeyboardButton();
            var manager = new CustomCommandButtonManager(parent, key, button);
            return manager;
        }

        protected override void Execute()
        {
            base.Execute();
        }
    }
}