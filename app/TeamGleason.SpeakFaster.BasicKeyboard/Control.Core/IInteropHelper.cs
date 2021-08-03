#if WINDOWS_UWP
#else
using System.Windows.Input;
#endif

using TeamGleason.SpeakFaster.BasicKeyboard.Layout.Standard;

namespace TeamGleason.SpeakFaster.BasicKeyboard.Control
{
    public interface IInteropHelper
    {
        void SendKey(bool sendDown, bool sendUp, KeyName keyName);
        void SendText(bool isShift, bool isCtrl, bool isAlt, bool isWindows, string text);
    }
}
