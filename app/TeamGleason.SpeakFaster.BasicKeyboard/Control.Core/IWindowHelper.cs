using System;
using System.Windows;

namespace TeamGleason.SpeakFaster.BasicKeyboard.Control.Core
{
    public interface IWindowHelper
    {
        Rect WindowRect { get; set; }

        event EventHandler Closing;

        bool TryParseRect(string altWindowRectString, out Rect altWindowRect);
    }
}