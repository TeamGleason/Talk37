using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using TeamGleason.SpeakFaster.KeyboardLayouts;
using TeamGleason.SpeakFaster.SimpleKeyboard.Properties;

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

        private void DoWindowPosition()
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.SetWindowPosition(Settings.Default.WindowRect);
        }

        protected override void Execute()
        {
            switch (_key.Command)
            {
                case "Custom.WindowPosition":
                    DoWindowPosition();
                    break;
            }
        }
    }
}