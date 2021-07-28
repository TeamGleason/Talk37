using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using TeamGleason.SpeakFaster.BasicKeyboard.Layout.Standard;

namespace TeamGleason.SpeakFaster.SimpleKeyboard
{
    internal class FunctionCommandButtonManager : CommandButtonManager
    {
        private static readonly KeyValuePair<string, Key>[] _keyValuePairs = new KeyValuePair<string, Key>[]
        {
            new KeyValuePair<string, Key>("Backspace", Key.Back),
            new KeyValuePair<string, Key>("Tab", Key.Tab),
            new KeyValuePair<string, Key>("Enter", Key.Enter),
            new KeyValuePair<string, Key>("Space", Key.Space),
            new KeyValuePair<string, Key>("Home", Key.Home),
            new KeyValuePair<string, Key>("End", Key.End),
            new KeyValuePair<string, Key>("Delete", Key.Delete),
            new KeyValuePair<string, Key>("PageUp", Key.PageUp),
            new KeyValuePair<string, Key>("PageDown", Key.PageDown),
            new KeyValuePair<string, Key>("ArrowUp", Key.Up),
            new KeyValuePair<string, Key>("ArrowDown", Key.Down),
            new KeyValuePair<string, Key>("ArrowLeft", Key.Left),
            new KeyValuePair<string, Key>("ArrowRight", Key.Right),
            new KeyValuePair<string, Key>("F1", Key.F1),
            new KeyValuePair<string, Key>("F2", Key.F2),
            new KeyValuePair<string, Key>("F3", Key.F3),
            new KeyValuePair<string, Key>("F4", Key.F4),
            new KeyValuePair<string, Key>("F5", Key.F5),
            new KeyValuePair<string, Key>("F6", Key.F6),
            new KeyValuePair<string, Key>("F7", Key.F7),
            new KeyValuePair<string, Key>("F8", Key.F8),
            new KeyValuePair<string, Key>("F9", Key.F9),
            new KeyValuePair<string, Key>("F10", Key.F10),
            new KeyValuePair<string, Key>("F11", Key.F11),
            new KeyValuePair<string, Key>("F12", Key.F12),
            new KeyValuePair<string, Key>("Escape", Key.Escape)
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
            var code = _nameToKey[_key.CommandParameter];
            _parent.SendUpDown(code);
        }
    }
}