using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using TeamGleason.SpeakFaster.KeyboardLayouts;

namespace TeamGleason.SpeakFaster.SimpleKeyboard
{
    internal class FunctionCommandButtonManager : CommandButtonManager
    {
        private static readonly KeyValuePair<string, Key>[] _keyValuePairs = new KeyValuePair<string, Key>[]
        {
            new KeyValuePair<string, Key>("Function.Backspace", Key.Back),
            new KeyValuePair<string, Key>("Function.Tab", Key.Tab),
            new KeyValuePair<string, Key>("Function.Enter", Key.Enter),
            new KeyValuePair<string, Key>("Function.Space", Key.Space),
            new KeyValuePair<string, Key>("Function.Home", Key.Home),
            new KeyValuePair<string, Key>("Function.End", Key.End),
            new KeyValuePair<string, Key>("Function.Delete", Key.Delete),
            new KeyValuePair<string, Key>("Function.PageUp", Key.PageUp),
            new KeyValuePair<string, Key>("Function.PageDown", Key.PageDown),
            new KeyValuePair<string, Key>("Function.ArrowUp", Key.Up),
            new KeyValuePair<string, Key>("Function.ArrowDown", Key.Down),
            new KeyValuePair<string, Key>("Function.ArrowLeft", Key.Left),
            new KeyValuePair<string, Key>("Function.ArrowRight", Key.Right),
            new KeyValuePair<string, Key>("Function.F1", Key.F1),
            new KeyValuePair<string, Key>("Function.F2", Key.F2),
            new KeyValuePair<string, Key>("Function.F3", Key.F3),
            new KeyValuePair<string, Key>("Function.F4", Key.F4),
            new KeyValuePair<string, Key>("Function.F5", Key.F5),
            new KeyValuePair<string, Key>("Function.F6", Key.F6),
            new KeyValuePair<string, Key>("Function.F7", Key.F7),
            new KeyValuePair<string, Key>("Function.F8", Key.F8),
            new KeyValuePair<string, Key>("Function.F9", Key.F9),
            new KeyValuePair<string, Key>("Function.F10", Key.F10),
            new KeyValuePair<string, Key>("Function.F11", Key.F11),
            new KeyValuePair<string, Key>("Function.F12", Key.F12),
            new KeyValuePair<string, Key>("Function.Escape", Key.Escape)
        };

        private static readonly Dictionary<string, Key> _nameToKey = new Dictionary<string, Key>(_keyValuePairs);

        private FunctionCommandButtonManager(KeyboardControl parent, CommandKey key, ButtonBase button)
            : base(parent, key, button)
        {
        }

        internal static FunctionCommandButtonManager CreateInstance(KeyboardControl parent, CommandKey key)
        {
            Debug.Assert(!key.Toggles);
            ButtonBase button = new KeyboardButton();
            var manager = new FunctionCommandButtonManager(parent, key, button);
            return manager;
        }

        protected override void Execute()
        {
        }
    }
}