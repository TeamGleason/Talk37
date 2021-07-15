using System;
using System.Diagnostics;
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

            Debug.Assert(KeyRef.Length == 1);
            var ch = KeyRef[0];
            var upperCh = char.ToUpper(ch);
            InteropHelper.SendCharacter(upperCh);

            switch (upperCh)
            {
                case 'A':
                    InteropHelper.StateChange += InteropHelper_StateChange;
                    break;

                case 'B':
                    InteropHelper.StateChange -= InteropHelper_StateChange;
                    break;

                case 'C':
                    InteropHelper.raise_StateChange(this, EventArgs.Empty);
                    break;
            }
        }

        private void InteropHelper_StateChange(object sender, System.EventArgs e)
        {
            Debug.WriteLine("Wibble");
        }
    }
}