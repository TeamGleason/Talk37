using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;
using TeamGleason.SpeakFaster.BasicKeyboard.Special;
using TeamGleason.SpeakFaster.KeyboardLayouts;
using TeamGleason.SpeakFaster.SimpleKeyboard.Properties;

namespace TeamGleason.SpeakFaster.SimpleKeyboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static bool TryParseRect(string source, out Rect rect)
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

        private bool SetWindowPosition(string rectString)
        {
            var value = TryParseRect(rectString, out var rect);
            if (value)
            {
                Left = rect.Left;
                Top = rect.Top;
                Width = rect.Width;
                Height = rect.Height;
            }
            return value;
        }

        public MainWindow()
        {
            InitializeComponent();

            SetWindowPosition(Settings.Default.WindowRect);

            var layout = KeyboardLayout.ReadDefaultKeyboardLayout();
            TheKeyboard.Layout = layout;

            Closing += OnClosing;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            var rect = new Rect(x: Left, y: Top, width: Width, height: Height);
            Settings.Default.WindowRect = rect.ToString();
            Settings.Default.Save();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            //Set the window style to noactivate.
            var helper = new WindowInteropHelper(this);
            InteropHelper.SetMainWindowStyle(helper.Handle);
        }
    }
}
