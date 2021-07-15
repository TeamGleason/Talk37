using NUnit.Framework;

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