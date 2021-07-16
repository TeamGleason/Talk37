using System.Diagnostics;
using System.Windows.Controls.Primitives;
using TeamGleason.SpeakFaster.KeyboardLayouts;

namespace TeamGleason.SpeakFaster.SimpleKeyboard
{
    internal class FunctionCommandButtonManager : ButtonManager<CommandKey>
    {
        private FunctionCommandButtonManager(KeyboardControl parent, CommandKey key, ButtonBase button)
            : base(parent, key, button)
        {
        }

        internal static FunctionCommandButtonManager CreateInstance(KeyboardControl parent, CommandKey key)
        {
            Debug.Assert(!key.Toggles);
            ButtonBase button = new KeyboardButton();
            button.Content = key.Label ?? key.Icon;
            var manager = new FunctionCommandButtonManager(parent, key, button);
            return manager;
        }

        protected override void Execute()
        {
            base.Execute();
        }
    }
}