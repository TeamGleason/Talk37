using System.Diagnostics;
using System.Windows.Controls.Primitives;
using TeamGleason.SpeakFaster.KeyboardLayouts;

namespace TeamGleason.SpeakFaster.SimpleKeyboard
{
    internal class CommandButtonManager : ButtonManager
    {
        private readonly CommandKey _key;

        public CommandButtonManager(KeyboardControl parent, CommandKey key, ButtonBase button)
            : base(parent, button)
        {
            _key = key;
        }

        internal static CommandButtonManager CreateInstance(KeyboardControl parent, CommandKey key)
        {
            ButtonBase button = key.Toggles ? new KeyboardToggleButton() : new KeyboardButton();
            button.Content = key.Label ?? key.Icon;
            var manager = new CommandButtonManager(parent, key, button);
            return manager;
        }

        protected override void Execute()
        {
            base.Execute();

            switch (_key.CommandType)
            {
                case "Navigate":
                    _parent.NavigateToView(_key.CommandParameter);
                    break;
                case "Function":
                    break;
                case "Modifier":
                    break;
                case "Custom":
                    break;
            }
        }
    }
}