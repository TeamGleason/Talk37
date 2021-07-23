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
        public MainWindow()
        {
            InitializeComponent();

            SetWindowPosition(Settings.Default.WindowRect);

            var layout = KeyboardLayout.ReadDefaultKeyboardLayout();
            TheKeyboard.Layout = layout;

            Closing += OnClosing;
        }

        internal Rect WindowRect
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

        internal static bool TryParseRect(string source, out Rect rect)
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

        internal bool SetWindowPosition(string rectString)
        {
            var value = TryParseRect(rectString, out var rect);
            if (value)
            {
                WindowRect = rect;
            }
            return value;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            var rect = WindowRect;
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
