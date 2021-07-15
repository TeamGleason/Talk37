using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TeamGleason.SpeakFaster.KeyboardLayouts;

namespace TeamGleason.SpeakFaster.SimpleKeyboard
{
    /// <summary>
    /// Interaction logic for KeyboardControl.xaml
    /// </summary>
    public partial class KeyboardControl : UserControl, IKeyboardControl
    {
        public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register(nameof(Layout), typeof(KeyboardLayout), typeof(KeyboardControl),
            new PropertyMetadata(null, OnLayoutChanged));

        private KeyboardLayout _layout;

        public KeyboardControl()
        {
            InitializeComponent();
        }

        public KeyboardLayout Layout
        {
            get => (KeyboardLayout)GetValue(LayoutProperty);
            set => SetValue(LayoutProperty, value);
        }

        private void NavigateToView(View view)
        {
            foreach (var key in view.KeyRefs)
            {
                key.Create(this);
            }
        }

        private void OnLayoutChanged(KeyboardLayout layout)
        {
            TheGrid.Children.Clear();
            _layout = layout;

            if (layout != null)
            {
                while (TheGrid.RowDefinitions.Count < layout.Rows)
                {
                    TheGrid.RowDefinitions.Add(new RowDefinition());
                }
                while (layout.Rows < TheGrid.RowDefinitions.Count)
                {
                    TheGrid.RowDefinitions.RemoveAt(layout.Rows);
                }

                while (TheGrid.ColumnDefinitions.Count < layout.Columns)
                {
                    TheGrid.ColumnDefinitions.Add(new ColumnDefinition());
                }
                while (layout.Columns < TheGrid.ColumnDefinitions.Count)
                {
                    TheGrid.ColumnDefinitions.RemoveAt(layout.Columns);
                }

                if (_layout.Views.Count != 0)
                {
                    NavigateToView(_layout.Views[0]);
                }
            }
        }

        private static void OnLayoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((KeyboardControl)d).OnLayoutChanged((KeyboardLayout)e.NewValue);
        }

        private void CreateButton(KeyRefBase keyRef, bool isToggle, string label, string icon)
        {
            ButtonBase button = isToggle ? new KeyboardToggleButton() : new KeyboardButton();
            button.Content = label ?? icon;
            Grid.SetRow(button, keyRef.Row);
            Grid.SetRowSpan(button, keyRef.RowSpan);
            Grid.SetColumn(button, keyRef.Column);
            Grid.SetColumnSpan(button, keyRef.ColumnSpan);
            TheGrid.Children.Add(button);
        }

        void IKeyboardControl.Create(TextKeyRef keyRef, TextKey key)
        {
            CreateButton(keyRef, false, key.Label, null);
        }

        void IKeyboardControl.Create(CommandKeyRef keyRef, CommandKey key)
        {
            CreateButton(keyRef, key.Toggles, key.Label, key.Icon);
        }

        void IKeyboardControl.Create(PredictionKeyRef keyRef, PredictionKey key)
        {
            CreateButton(keyRef, false, string.Empty, null);
        }
    }
}
