using System;

namespace TeamGleason.SpeakFaster.BasicKeyboard.Layout.Standard
{
    public class SendKeyEventArgs : EventArgs
    {
        public SendKeyEventArgs(bool sendDown, bool sendUp, KeyName keyName)
        {
            SendDown = sendDown;
            SendUp = sendUp;
            KeyName = keyName;
        }

        public bool SendDown { get; }
        public bool SendUp { get; }
        public KeyName KeyName { get; }
    }
}
