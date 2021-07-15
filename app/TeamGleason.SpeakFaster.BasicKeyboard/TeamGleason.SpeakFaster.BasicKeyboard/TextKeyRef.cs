using System;
using System.Diagnostics;
using System.Xml;
using TeamGleason.SpeakFaster.BasicKeyboard.Special;

namespace TeamGleason.SpeakFaster.BasicKeyboard
{
    internal class TextKeyRef : KeyRefBase
    {
        private readonly string _normalCaption;
        private readonly string _shiftedCaption;

        internal TextKeyRef(MainWindow window, XmlReader reader) : base(window, reader)
        {
            if(KeyRef.Length==1)
            {
                _normalCaption = KeyRef;
                _shiftedCaption = KeyRef.ToUpperInvariant();
            }
            else
            {
                switch(KeyRef)
                {
                    case "Punctuation.Comma":
                        _normalCaption = ",";
                        break;

                    case "Punctuation.Period":
                        _normalCaption = ".";
                        break;

                    case "Punctuation.Question":
                        _normalCaption = "?";
                        break;

                    case "Punctuation.Exclamation":
                        _normalCaption = "!";
                        break;

                    default:
                        _normalCaption = "Error";
                        break;
                }

                _shiftedCaption = _normalCaption;
            }
        }

        protected override void Execute()
        {
            base.Execute();

            Debug.Assert(_shiftedCaption.Length == 1);
            var ch = _shiftedCaption[0];
            InteropHelper.SendCharacter(_shiftedCaption[0]);
        }

        internal override void SetState(bool isShift, bool isControl, bool isCapsLock)
        {
            base.SetState(isShift, isControl, isCapsLock);

            var caption = isShift == isCapsLock ? _normalCaption : _shiftedCaption;
            _control.Content = caption;
        }
    }
}