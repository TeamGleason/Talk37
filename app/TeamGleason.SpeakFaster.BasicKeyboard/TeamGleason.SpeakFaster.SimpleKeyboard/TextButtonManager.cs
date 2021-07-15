using System.Windows.Controls;
using TeamGleason.SpeakFaster.KeyboardLayouts;

namespace TeamGleason.SpeakFaster.SimpleKeyboard
{
    internal class TextButtonManager : ButtonManager
    {
        public TextButtonManager(KeyboardControl parent, TextKey key, Button button)
            : base(parent, button)
        {
        }

        internal static TextButtonManager CreateInstance(KeyboardControl parent, TextKey key)
        {
            var button = new KeyboardButton { Content = key.Label };
            var manager = new TextButtonManager(parent, key, button);
            return manager;
        }
    }
}