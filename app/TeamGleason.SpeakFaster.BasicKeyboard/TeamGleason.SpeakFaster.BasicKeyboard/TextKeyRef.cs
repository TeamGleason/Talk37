using System.Xml;
using TeamGleason.SpeakFaster.BasicKeyboard.Special;

namespace TeamGleason.SpeakFaster.BasicKeyboard
{
    internal class TextKeyRef : KeyRefBase
    {
        internal TextKeyRef(XmlReader reader) : base(reader) { }

        protected override void Execute()
        {
            base.Execute();

            InteropHelper.SendKeystroke();
        }
    }
}