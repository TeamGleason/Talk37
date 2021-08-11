using System.Diagnostics;
#if WINDOWS_UWP
using Windows.UI.Xaml.Controls.Primitives;
#else
using System.Windows.Controls.Primitives;
#endif
using TeamGleason.SpeakFaster.BasicKeyboard.Control.Properties;
using TeamGleason.SpeakFaster.BasicKeyboard.Layout.Standard;

namespace TeamGleason.SpeakFaster.BasicKeyboard.Control
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
            var helper = _parent.GetWindow();
            if (helper != null)
            {
                var currentWindowRectString = helper.WindowRect.ToString();
                var windowRectString = Settings.Default.WindowRect;
                var altWindowRectString = Settings.Default.AltWindowRect;

                if (currentWindowRectString == windowRectString)
                {
                    if (helper.TryParseRect(altWindowRectString, out var altWindowRect))
                    {
                        Settings.Default.WindowRect = altWindowRectString;
                        Settings.Default.AltWindowRect = windowRectString;
                        helper.WindowRect = altWindowRect;
                    }
                }
                else
                {
                    Settings.Default.AltWindowRect = currentWindowRectString;
                    if (helper.TryParseRect(windowRectString, out var windowRect))
                    {
                        helper.WindowRect = windowRect;
                    }
                }
            }
        }

        protected override void Execute()
        {
            switch (_key.Command)
            {
                case "Custom.ExpandHint":
                    _parent.RaiseExpandHint();
                    break;

                case "Custom.MouseLeftClick":
                    _parent.RaiseMouseLeftClick();
                    break;

                case "Custom.WindowPosition":
                    DoWindowPosition();
                    break;
            }
        }
    }
}