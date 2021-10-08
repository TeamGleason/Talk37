using Microsoft.Toolkit.Uwp.Input.GazeInteraction;
using System.Windows;
using System.Windows.Controls;

namespace WebWatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BrowserTargetItem _browserTargetItem;

        public MainWindow()
        {
            InitializeComponent();

            _browserTargetItem = new BrowserTargetItem(TheBrowser);
            GazeInput.GazeTarget.AddElementToTargetItemFactory(WebBrowserTargetFactory);
        }

        private GazeTargetItem WebBrowserTargetFactory(UIElement arg)
        {
            var item = arg == TheBrowser ? _browserTargetItem : null;
            return item;
        }

        private void OnNavigate(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var address = button?.Tag?.ToString();
            TheBrowser.Address = address;
        }
    }
}
