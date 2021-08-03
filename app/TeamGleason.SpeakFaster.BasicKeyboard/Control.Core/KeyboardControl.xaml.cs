using System;
using System.Collections.Generic;
#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#else
using System.Windows;
using System.Windows.Controls;
#endif
using System.Windows.Input;
using TeamGleason.SpeakFaster.BasicKeyboard.Control.Properties;
using TeamGleason.SpeakFaster.BasicKeyboard.Layout.Standard;

namespace TeamGleason.SpeakFaster.BasicKeyboard.Control
{
    /// <summary>
    /// Interaction logic for KeyboardControl.xaml
    /// </summary>
    public partial class KeyboardControl : UserControl, IKeyboardControl
    {
        public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register(nameof(Layout), typeof(KeyboardLayout), typeof(KeyboardControl),
            new PropertyMetadata(null, OnLayoutChanged));
        public static readonly DependencyProperty InteropHelperProperty = DependencyProperty.Register(nameof(InteropHelper), typeof(IInteropHelper), typeof(KeyboardControl),
            new PropertyMetadata(null, OnInteropHelperChanged));

        private KeyboardLayout _layout;

        private readonly bool[] _states = new bool[Enum.GetValues(typeof(StateModifier)).Length];

        private readonly List<ButtonManager> _managers = new List<ButtonManager>();

        private IInteropHelper _interopHelper = null;

        private IWindowHelper _windowHelper = null;

        public KeyboardControl()
        {
            InitializeComponent();
        }

        public KeyboardLayout Layout
        {
            get => (KeyboardLayout)GetValue(LayoutProperty);
            set => SetValue(LayoutProperty, value);
        }

        public IInteropHelper InteropHelper
        {
            get => (IInteropHelper)GetValue(InteropHelperProperty);
            set => SetValue(InteropHelperProperty, value);
        }

        private static IWindowHelper GetWindow(DependencyObject ob)
        {
            IWindowHelper value = null;

            for (var walker = ob as FrameworkElement;
                walker != null && value == null;
                walker = walker.Parent as FrameworkElement)
            {
                value = walker as IWindowHelper;
            }

            return value;
        }

        internal IWindowHelper GetWindow()
        {
            return _windowHelper;
        }

#if WINDOWS_UWP
#else

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            if (_windowHelper != null)
            {
                _windowHelper.Closing -= OnClosing;
            }

            _windowHelper = GetWindow(this);
            if (_windowHelper != null)
            {
                _windowHelper.Closing += OnClosing;

                var rectString = Settings.Default.WindowRect;
                var value = _windowHelper.TryParseRect(rectString, out var rect);
                if (value)
                {
                    _windowHelper.WindowRect = rect;
                }
            }

            base.OnVisualParentChanged(oldParent);
        }

#endif

        private void OnClosing(object sender, EventArgs e)
        {
            if (_windowHelper != null)
            {
                var rect = _windowHelper.WindowRect;
                Settings.Default.WindowRect = rect.ToString();
                Settings.Default.Save();
            }
        }

        private void NavigateToView(View view)
        {
            _managers.Clear();
            TheGrid.Children.Clear();

            foreach (var key in view.KeyRefs)
            {
                key.Create(this);
            }
        }

        internal bool GetState(StateModifier modifier)
        {
            var value = _states[(int)modifier];
            return value;
        }

        internal void SetState(StateModifier modifier, bool newState)
        {
            var currentState = _states[(int)modifier];
            if (currentState != newState)
            {
                _states[(int)modifier] = newState;

                foreach (var manager in _managers)
                {
                    manager.UpdateStateModifiers();
                }
            }

            if (modifier == StateModifier.CapsLock)
            {
                SendUpDown(KeyName.CapsLock);
            }
            else
            {
                KeyName keyName;

                switch (modifier)
                {
                    case StateModifier.Ctrl:
                        keyName = KeyName.Ctrl;
                        break;

                    case StateModifier.Alt:
                        keyName = KeyName.Alt;
                        break;

                    default:
                    case StateModifier.Shift:
                        keyName = KeyName.Shift;
                        break;
                }

                if (newState)
                {
                    SendDown(keyName);
                }
                else
                {
                    SendUp(keyName);
                }
            }
        }

        internal void NavigateToView(string viewName)
        {
            var view = _layout.Views[viewName];
            NavigateToView(view);
        }

        private void OnWindowHelperChanged(IWindowHelper helper)
        {

        }

        private void OnLayoutChanged(KeyboardLayout layout)
        {
            TheGrid.Children.Clear();
            _managers.Clear();
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

        internal void SendKey(bool sendDown,
            bool sendUp,
            KeyName keyName)
        {
            _interopHelper?.SendKey(sendDown, sendUp, keyName);
        }

        internal void SendDown(KeyName keyName)
        {
            SendKey(true, false, keyName);
        }

        internal void SendUp(KeyName keyName)
        {
            SendKey(false, true, keyName);
        }

        internal void SendUpDown(KeyName keyName)
        {
            SendKey(true, true, keyName);
        }

        internal void SendText(string text)
        {
            var isShift = _states[(int)StateModifier.Shift];
            var isCtrl = _states[(int)StateModifier.Ctrl];
            var isAlt = _states[(int)StateModifier.Alt];
            var isWindows = false;
            _interopHelper?.SendText(isShift: isShift, isCtrl: isCtrl, isAlt: isAlt, isWindows: isWindows, text: text);

            SetState(StateModifier.Shift, false);
            SetState(StateModifier.Ctrl, false);
            SetState(StateModifier.Alt, false);
        }

        private static void OnLayoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((KeyboardControl)d).OnLayoutChanged((KeyboardLayout)e.NewValue);
        }

        private static void OnInteropHelperChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((KeyboardControl)d)._interopHelper = (IInteropHelper)e.NewValue;
        }

        private void AddManager(KeyRefBase keyRef, ButtonManager manager)
        {
            var button = manager.Button;
            Grid.SetRow(button, keyRef.Row);
            Grid.SetRowSpan(button, keyRef.RowSpan);
            Grid.SetColumn(button, keyRef.Column);
            Grid.SetColumnSpan(button, keyRef.ColumnSpan);
            TheGrid.Children.Add(button);
            _managers.Add(manager);
        }

        void IKeyboardControl.Create(TextKeyRef keyRef, TextKey key)
        {
            var manager = TextButtonManager.CreateInstance(this, key);
            AddManager(keyRef, manager);
        }

        void IKeyboardControl.Create(CommandKeyRef keyRef, CommandKey key)
        {
            ButtonManager manager;

            switch (key.CommandType)
            {
                case "Navigate":
                    manager = NavigateCommandButtonManager.CreateInstance(this, key);
                    break;
                case "Function":
                    manager = FunctionCommandButtonManager.CreateInstance(this, key);
                    break;
                case "Modifier":
                    manager = ModifierCommandButtonManager.CreateInstance(this, key);
                    break;
                case "Custom":
                    manager = CustomCommandButtonManager.CreateInstance(this, key);
                    break;
                default:
                    throw new InvalidOperationException();
            }
            AddManager(keyRef, manager);
        }

        void IKeyboardControl.Create(PredictionKeyRef keyRef, PredictionKey key)
        {
            var manager = PredictionButtonManager.CreateInstance(this, key);
            AddManager(keyRef, manager);
        }
    }
}
