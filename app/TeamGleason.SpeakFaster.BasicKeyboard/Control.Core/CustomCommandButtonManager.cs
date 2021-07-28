using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using TeamGleason.SpeakFaster.BasicKeyboard.Control.Core.Properties;
using TeamGleason.SpeakFaster.BasicKeyboard.Layout.Standard;

namespace TeamGleason.SpeakFaster.BasicKeyboard.Control.Core
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
            var mainWindow = _parent._windowHelper;
            var currentWindowRectString = mainWindow.WindowRect.ToString();
            var windowRectString = Settings.Default.WindowRect;
            var altWindowRectString = Settings.Default.AltWindowRect;

            if (currentWindowRectString == windowRectString)
            {
                if (mainWindow.TryParseRect(altWindowRectString, out var altWindowRect))
                {
                    Settings.Default.WindowRect = altWindowRectString;
                    Settings.Default.AltWindowRect = windowRectString;
                    mainWindow.WindowRect = altWindowRect;
                }
            }
            else
            {
                Settings.Default.AltWindowRect = currentWindowRectString;
                if (mainWindow.TryParseRect(windowRectString, out var windowRect))
                {
                    mainWindow.WindowRect = windowRect;
                }
            }
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