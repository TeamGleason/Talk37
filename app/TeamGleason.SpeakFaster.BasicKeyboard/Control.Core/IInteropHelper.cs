using System.Windows.Input;

namespace TeamGleason.SpeakFaster.BasicKeyboard.Control
{
    public interface IInteropHelper
    {
        void SendKey(bool sendDown, bool sendUp, Key key);
        void SendText(bool isShift, bool isCtrl, bool isAlt, bool isWindows, string text);
    }
}
