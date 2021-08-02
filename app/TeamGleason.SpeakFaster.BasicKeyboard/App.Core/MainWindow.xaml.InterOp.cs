using System;
using System.Windows.Input;
using System.Windows.Interop;
using TeamGleason.SpeakFaster.BasicKeyboard.Control;
using TeamGleason.SpeakFaster.BasicKeyboard.Special;

namespace TeamGleason.SpeakFaster.BasicKeyboard.App.Core
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IInteropHelper
    {
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            //Set the window style to noactivate.
            var helper = new WindowInteropHelper(this);
            InteropHelper.SetMainWindowStyle(helper.Handle);
            TheKeyboard.InteropHelper = this;
        }

        void IInteropHelper.SendKey(bool sendDown, bool sendUp, Key key)
        {
            InteropHelper.SendKey(sendDown, sendUp, key);
        }

        void IInteropHelper.SendText(bool isShift, bool isCtrl, bool isAlt, bool isWindows, string text)
        {
            InteropHelper.SendText(isShift: isShift, isCtrl: isCtrl, isAlt: isAlt, isWindows: isWindows, text: text);
        }
    }
}
