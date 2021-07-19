using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TeamGleason.SpeakFaster.BasicKeyboard.Special;
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

        private readonly bool[] _states = new bool[4];

        private readonly List<ButtonManager> _managers = new List<ButtonManager>();

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
                SendUpDown(Key.CapsLock);
            }
            else
            {
                Key key;

                switch (modifier)
                {
                    case StateModifier.Ctrl:
                        key = Key.LeftCtrl;
                        break;

                    case StateModifier.Alt:
                        key = Key.LeftAlt;
                        break;

                    default:
                    case StateModifier.Shift:
                        key = Key.LeftShift;
                        break;
                }

                if (newState)
                {
                    SendDown(key);
                }
                else
                {
                    SendUp(key);
                }
            }
        }

        internal void NavigateToView(string viewName)
        {
            var view = _layout.Views[viewName];
            NavigateToView(view);
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
            Key key)
        {
            InteropHelper.SendKey(sendDown, sendUp, key);
        }

        internal void SendDown(Key code)
        {
            SendKey(true, false, code);
        }

        internal void SendUp(Key code)
        {
            SendKey(false, true, code);
        }

        internal void SendUpDown(Key code)
        {
            SendKey(true, true, code);
        }

        internal void SendText(string text)
        {
            var isShift = _states[(int)StateModifier.Shift];
            var isCtrl = _states[(int)StateModifier.Ctrl];
            var isAlt = _states[(int)StateModifier.Alt];
            var isWindows = false;
            InteropHelper.SendText(isShift: isShift, isCtrl: isCtrl, isAlt: isAlt, isWindows: isWindows, text: text);
        }

        private static void OnLayoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((KeyboardControl)d).OnLayoutChanged((KeyboardLayout)e.NewValue);
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
