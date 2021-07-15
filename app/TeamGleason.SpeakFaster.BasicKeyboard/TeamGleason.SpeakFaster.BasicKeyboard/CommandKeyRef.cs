using System.Windows.Media;
using System.Xml;

namespace TeamGleason.SpeakFaster.BasicKeyboard
{
    internal class CommandKeyRef : KeyRefBase
    {
        private Brush _normalBackground;

        internal CommandKeyRef(MainWindow window, XmlReader reader) : base(window, reader)
        {
            switch (KeyRef)
            {
                case "Modifier.Shift":
                    Caption = "Shift";
                    break;
                case "Modifier.Ctrl":
                    Caption = "Ctrl";
                    break;
            }
        }

        internal override void SetState(bool isShift, bool isControl, bool isCapsLock)
        {
            base.SetState(isShift, isControl, isCapsLock);

            switch (KeyRef)
            {
                case "Modifier.Shift":
                    if (isShift)
                    {
                        _normalBackground = _control.Background;
                        _control.Background = Brushes.Blue;
                    }
                    else if (_normalBackground != null)
                    {
                        _control.Background = _normalBackground;
                    }
                    break;

                case "Modifier.Ctrl":
                    if (isControl)
                    {
                        _normalBackground = _control.Background;
                        _control.Background = Brushes.Blue;
                    }
                    else if (_normalBackground != null)
                    {
                        _control.Background = _normalBackground;
                    }
                    break;
            }
        }
    }
}