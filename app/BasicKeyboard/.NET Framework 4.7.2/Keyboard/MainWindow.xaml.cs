using Microsoft.Toolkit.Uwp.Input.GazeInteraction;
using System;
using System.Diagnostics;
using System.Windows;
using TeamGleason.SpeakFaster.BasicKeyboard.Layout;

namespace TeamGleason.SpeakFaster.BasicKeyboard.Keyboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MouseMoveGazeTarget _mouseMoveGazeTarget = new MouseMoveGazeTarget();

        private bool _targetting;
        private DateTimeOffset _targettingExpiry;

        public MainWindow()
        {
            InitializeComponent();

            var layout = KeyboardLayout.ReadDefaultKeyboardLayout();
            TheKeyboard.Layout = layout;

            Loaded += PositionWindowHelper.OnOpening;
            TheKeyboard.PositionWindow += (s, e) => PositionWindowHelper.DoPositionWindow(this);
            Closing += PositionWindowHelper.OnClosing;

            TheKeyboard.MouseLeftClick += OnMouseLeftClick;
        }

        private void OnMouseLeftClick(object sender, EventArgs e)
        {
            if (!_targetting)
            {
                _targetting = true;
                _targettingExpiry = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(30);
                GazeInput.HitTest += OnHitTest;
            }
        }

        private void OnHitTest(object sender, GazeHitTestArgs e)
        {
            Debug.Assert(_targetting);

            if (!e.Handled)
            {
                if (_targettingExpiry <= DateTimeOffset.UtcNow)
                {
                    GazeInput.HitTest -= OnHitTest;
                }
                else
                {
                    e.SetTarget(_mouseMoveGazeTarget);
                }
            }
        }

        private class MouseMoveGazeTarget : GazeTargetItem
        {
            protected override TimeSpan GetElementRepeatDelay(TimeSpan defaultValue)
            {
                return defaultValue;
            }

            protected override TimeSpan GetElementStateDelay(PointerState pointerState, TimeSpan defaultValue)
            {
                return defaultValue;
            }

            protected override int GetMaxDwellRepeatCount()
            {
                return 0;
            }

            protected override void Invoke()
            {
            }

            protected override void ShowFeedback(DwellProgressState state, double progress)
            {
            }
        }
    }
}
