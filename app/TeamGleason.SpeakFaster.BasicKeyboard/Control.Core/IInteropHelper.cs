using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TeamGleason.SpeakFaster.BasicKeyboard.Control.Core
{
    public interface IInteropHelper
    {
        void SendKey(bool sendDown, bool sendUp, Key key);
        void SendText(bool isShift, bool isCtrl, bool isAlt, bool isWindows, string text);
    }
}
