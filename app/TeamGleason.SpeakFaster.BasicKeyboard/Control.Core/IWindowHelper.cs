using System;
#if WINDOWS_UWP
using Windows.Foundation;
#else
using System.Windows;
#endif

namespace TeamGleason.SpeakFaster.BasicKeyboard.Control
{
    public interface IWindowHelper
    {
        Rect WindowRect { get; set; }

        event EventHandler Closing;

        bool TryParseRect(string altWindowRectString, out Rect altWindowRect);
    }
}