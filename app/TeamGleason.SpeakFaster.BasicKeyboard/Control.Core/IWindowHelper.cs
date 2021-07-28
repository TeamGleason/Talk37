using System.Windows;

namespace TeamGleason.SpeakFaster.BasicKeyboard.Control.Core
{
    internal interface IWindowHelper
    {
        Rect WindowRect { get; set; }

        bool TryParseRect(string altWindowRectString, out Rect altWindowRect);
    }
}