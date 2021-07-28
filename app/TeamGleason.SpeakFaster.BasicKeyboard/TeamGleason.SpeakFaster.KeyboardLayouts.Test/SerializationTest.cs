using NUnit.Framework;
using TeamGleason.SpeakFaster.BasicKeyboard.Layout.Standard;

namespace TeamGleason.SpeakFaster.KeyboardLayouts.Test
{
    public class Tests
    {
        [Test]
        public void ReadDefaultKeyboardLayoutTest()
        {
            var layout = KeyboardLayout.ReadDefaultKeyboardLayout();
            Assert.IsNotNull(layout);
        }
    }
}