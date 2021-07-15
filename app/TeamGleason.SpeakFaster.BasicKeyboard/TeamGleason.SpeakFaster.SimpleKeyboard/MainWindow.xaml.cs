using System.Windows;
using TeamGleason.SpeakFaster.KeyboardLayouts;

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

            var layout = KeyboardLayout.ReadDefaultKeyboardLayout();
            TheKeyboard.Layout = layout;
        }
    }
}
