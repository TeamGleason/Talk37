using CefSharp;
using Microsoft.Toolkit.Uwp.Input.GazeInteraction;
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WebWatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ArrayList gazeButtons = new ArrayList();

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

        private static string ReadFileContents(string textFile)
        {
            StreamReader file = new StreamReader(textFile);
            return file.ReadToEnd();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string htmlContents = ReadFileContents("assets/index.html");
            TheBrowser.LoadHtml(htmlContents);
            TheBrowser.ExecuteScriptAsyncWhenPageLoaded("document.addEventListener('DOMContentLoaded', function(){ alert('DomLoaded'); });");

            //Wait for the page to finish loading (all resources will have been loaded, rendering is likely still happening)
            TheBrowser.LoadingStateChanged += async (s, args) =>
            {
                //Wait for the Page to finish loading
                if (args.IsLoading == false)
                {
                    RemoveGazeButtons();

                    await AddGazeButtons();
                }
            };
        }

        private async Task AddGazeButtons()
        {
            var devToolsClient = TheBrowser.GetDevToolsClient();

            var document = await devToolsClient.DOM.GetDocumentAsync();
            var responseQuerySelectorAllAsync = await devToolsClient.DOM.QuerySelectorAllAsync(document.Root.NodeId, "button, a");

            foreach (int nodeId in responseQuerySelectorAllAsync.NodeIds)
            {
                var responseGetBoxModelAsync = await devToolsClient.DOM.GetBoxModelAsync(nodeId);
                var border = responseGetBoxModelAsync.Model.Border;
                var left = border[0];
                var top = border[1];
                var right = border[4];
                var bottom = border[5];

                await AddGazeButton(left, top, right, bottom);
            }
        }

        private void RemoveGazeButtons()
        {
            foreach (UIElement uIElement in gazeButtons)
            {
                overlayCanvas.Children.Remove(uIElement);
            }
            gazeButtons.Clear();
        }

        private async Task AddGazeButton(double left, double top, double right, double bottom)
        {
            await Application.Current.Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    var gazeButton = new Button
                    {
                        Width = right - left,
                        Height = bottom - top,
                        Opacity = 0.01
                    };
                    gazeButton.Click += GazeButton_Click;

                    gazeButtons.Add(gazeButton);

                    overlayCanvas.Children.Add(gazeButton);
                    Canvas.SetLeft(gazeButton, left);
                    Canvas.SetTop(gazeButton, top);
                }));
        }

        private async void GazeButton_Click(object sender, RoutedEventArgs e)
        {
            var gazeButton = sender as Button;

            var centerX = Canvas.GetLeft(gazeButton) + gazeButton.ActualWidth / 2;
            var centerY = Canvas.GetTop(gazeButton) + gazeButton.ActualHeight / 2;

            var devToolsClient = TheBrowser.GetDevToolsClient();

            await devToolsClient.Input.DispatchMouseEventAsync(
                type: CefSharp.DevTools.Input.DispatchMouseEventType.MousePressed,
                x: centerX,
                y: centerY,
                button: CefSharp.DevTools.Input.MouseButton.Left,
                clickCount: 1);
            await devToolsClient.Input.DispatchMouseEventAsync(
                type: CefSharp.DevTools.Input.DispatchMouseEventType.MouseReleased,
                x: centerX,
                y: centerY,
                button: CefSharp.DevTools.Input.MouseButton.Left,
                clickCount: 1);
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
