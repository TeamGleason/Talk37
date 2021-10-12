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
            _browserTargetItem.AddTarget(50, 50, 200, 100, () => TheBrowser.Address = (string)Resources["BingUrl"]);
            _browserTargetItem.AddTarget(300, 50, 100, 200, () => TheBrowser.Address = (string)Resources["GoogleUrl"]);
            _browserTargetItem.AddTarget(50, 200, 200, 100, () => TheBrowser.Address = (string)Resources["BbcUrl"]);

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
