using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using TeamGleason.SpeakFaster.BasicKeyboard.Control.Core;
using TeamGleason.SpeakFaster.BasicKeyboard.Layout.Standard;
using TeamGleason.SpeakFaster.BasicKeyboard.Special;

namespace TeamGleason.SpeakFaster.SimpleKeyboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IWindowHelper, IInteropHelper
    {
        public MainWindow()
        {
            InitializeComponent();

            var layout = KeyboardLayout.ReadDefaultKeyboardLayout();
            TheKeyboard.Layout = layout;

            Closing += (s, e) => _closing?.Invoke(s, e);
        }

        Rect IWindowHelper.WindowRect
        {
            get => new Rect(x: Left, y: Top, width: Width, height: Height);
            set
            {
                Left = value.Left;
                Top = value.Top;
                Width = value.Width;
                Height = value.Height;
            }
        }

        bool IWindowHelper.TryParseRect(string source, out Rect rect)
        {
            bool value;

            try
            {
                rect = Rect.Parse(source);
                value = true;
            }
            catch (InvalidOperationException)
            {
                value = false;
            }

            return value;
        }

        event EventHandler IWindowHelper.Closing
        {
            add => _closing += value;
            remove => _closing -= value;
        }
        private event EventHandler _closing;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            //Set the window style to noactivate.
            var helper = new WindowInteropHelper(this);
            InteropHelper.SetMainWindowStyle(helper.Handle);
        }

        void IInteropHelper.SendKey(bool sendDown, bool sendUp, Key key)
        {
            InteropHelper.SendKey(sendDown, sendUp, key);
            throw new NotImplementedException();
        }

        void IInteropHelper.SendText(bool isShift, bool isCtrl, bool isAlt, bool isWindows, string text)
        {
            InteropHelper.SendText(isShift: isShift, isCtrl: isCtrl, isAlt: isAlt, isWindows: isWindows, text: text);
        }
    }
}
