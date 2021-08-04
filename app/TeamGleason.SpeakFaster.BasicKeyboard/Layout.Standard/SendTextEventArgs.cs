﻿using System;

namespace TeamGleason.SpeakFaster.BasicKeyboard.Layout.Standard
{
    public class SendTextEventArgs : EventArgs
    {
        public SendTextEventArgs(bool isShift, bool isCtrl, bool isAlt, bool isWindows, string text)
        {
            IsShift = isShift;
            IsCtrl = isAlt;
            IsAlt = isAlt;
            IsWindows = isWindows;
            Text = text;
        }

        public bool IsShift { get; }
        public bool IsCtrl { get; }
        public bool IsAlt { get; }
        public bool IsWindows { get; }
        public string Text { get; }
    }
}
