using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Xml;
using TeamGleason.SpeakFaster.BasicKeyboard.Special;

namespace TeamGleason.SpeakFaster.BasicKeyboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly KeyValuePair<string, Func<XmlReader, KeyRefBase>>[] _types = new KeyValuePair<string, Func<XmlReader, KeyRefBase>>[]
        {
            new KeyValuePair<string, Func<XmlReader, KeyRefBase>>(nameof(PredictionKeyRef), r => new PredictionKeyRef(r)),
            new KeyValuePair<string, Func<XmlReader, KeyRefBase>>(nameof(CommandKeyRef), r => new CommandKeyRef(r)),
            new KeyValuePair<string, Func<XmlReader, KeyRefBase>>(nameof(TextKeyRef), r => new TextKeyRef(r))
        };
        private static readonly Dictionary<string, Func<XmlReader, KeyRefBase>> _constructors = new Dictionary<string, Func<XmlReader, KeyRefBase>>(_types);

        private readonly SemaphoreSlim _stateSemaphore = new SemaphoreSlim(1);

        private readonly KeyRefBase[] _keyRefs;

        public MainWindow()
        {
            InitializeComponent();

            var keyRefs = ReadLayout();
            _keyRefs = new List<KeyRefBase>(keyRefs).ToArray();

            var rows = 0;
            var columns = 0;

            foreach (var keyRef in keyRefs)
            {
                if (rows < keyRef.Row + keyRef.RowSpan)
                {
                    rows = keyRef.Row + keyRef.RowSpan;
                }
                if (columns < keyRef.Column + keyRef.ColumnSpan)
                {
                    columns = keyRef.Column + keyRef.ColumnSpan;
                }

                var control = keyRef.CreateControl();
                TheGrid.Children.Add(control);
            }

            for (var i = 0; i < rows; i++)
            {
                TheGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (var i = 0; i < columns; i++)
            {
                TheGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            InteropHelper.StateChange += OnStateChanged;
            _ = WatchStateAsync();
        }

        private void OnStateChanged(object sender, EventArgs e)
        {
            _stateSemaphore.Release();
        }

        private async Task WatchStateAsync()
        {
            for (; ; )
            {
                await _stateSemaphore.WaitAsync();

                while (_stateSemaphore.Wait(0))
                {
                    // Spin.
                }

                Debug.WriteLine($"xCapsLock = {InteropHelper.IsCapsLock}");
                Debug.WriteLine($"xShift = {InteropHelper.IsShift}");
                Debug.WriteLine($"xControl = {InteropHelper.IsControl}");

                await Task.Delay(50);

                Debug.WriteLine($"CapsLock = {InteropHelper.IsCapsLock}");
                Debug.WriteLine($"Shift = {InteropHelper.IsShift}");
                Debug.WriteLine($"Control = {InteropHelper.IsControl}");
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            //Set the window style to noactivate.
            var helper = new WindowInteropHelper(this);
            InteropHelper.SetMainWindowStyle(helper.Handle);
        }

        private IEnumerable<KeyRefBase> ReadLayout()
        {
            var keyRefs = new List<KeyRefBase>();

            var xmlLayout = Properties.Resources.Layout;
            using (var input = new StringReader(xmlLayout))
            {
                var settings = new XmlReaderSettings
                {
                    ConformanceLevel = ConformanceLevel.Fragment
                };

                using (var reader = XmlReader.Create(input, settings))
                {
                    while (reader.Read() && reader.NodeType == XmlNodeType.Whitespace) { }

                    while (reader.NodeType == XmlNodeType.Element)
                    {
                        if (!_constructors.TryGetValue(reader.Name, out var constructor))
                        {
                            throw new InvalidDataException();
                        }
                        var item = constructor(reader);
                        keyRefs.Add(item);

                        while (reader.Read() && reader.NodeType == XmlNodeType.Whitespace) { }
                    }

                    if (reader.NodeType != XmlNodeType.None)
                    {
                        throw new InvalidDataException();
                    }
                }
            }

            return keyRefs;
        }
    }
}
