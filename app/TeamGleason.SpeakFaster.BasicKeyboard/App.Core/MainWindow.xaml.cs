using System;
using System.Windows;
using TeamGleason.SpeakFaster.BasicKeyboard.Control;
using TeamGleason.SpeakFaster.BasicKeyboard.Layout.Standard;

namespace TeamGleason.SpeakFaster.BasicKeyboard.App.Core
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IWindowHelper
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
                rect = new Rect();
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
    }
}
