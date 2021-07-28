using System.Windows;

namespace TeamGleason.SpeakFaster.BasicKeyboard.Control.Core
{
    public interface IWindowHelper
    {
        Rect WindowRect { get; set; }

        bool TryParseRect(string altWindowRectString, out Rect altWindowRect);
    }
}