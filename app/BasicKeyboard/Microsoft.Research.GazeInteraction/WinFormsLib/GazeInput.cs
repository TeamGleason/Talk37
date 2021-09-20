using Microsoft.Toolkit.Uwp.Input.GazeInteraction;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsLib
{
    public class GazeInput
    {
        private static GazePointer _gazePointer;

        public static void Start(Form form)
        {
            var device = new MouseDevice(form);
            _gazePointer = new GazePointer(device, new NonCursor(form), p => null);
            _gazePointer.AddRoot(0);
            _gazePointer.IsCursorVisible = true;
        }

        private class MouseDevice : IGazeDevice
        {
            private readonly Form _form;
            private readonly int _epoch = Environment.TickCount;

            internal MouseDevice(Form form)
            {
                _form = form;
                _form.MouseMove += OnMouseMove;
            }

            private void OnMouseMove(object sender, MouseEventArgs e)
            {
                var timestamp = TimeSpan.FromMilliseconds(Environment.TickCount - _epoch);

                if (!_form.Capture)
                {
                    _form.Capture = true;
                }

                var point = _form.PointToScreen(new Point(e.X, e.Y));
                var args = new GazeMovedEventArgs(timestamp, point.X, point.Y, false);
                _gazeMoved?.Invoke(this, args);
            }

            bool IGazeDevice.IsAvailable => true;

            TimeSpan IGazeDevice.EyesOffDelay { get; set; }

            event EventHandler IGazeDevice.IsAvailableChanged
            {
                add { }
                remove { }
            }

            event EventHandler IGazeSource.GazeEntered
            {
                add { }
                remove { }
            }

            event EventHandler<GazeMovedEventArgs> IGazeSource.GazeMoved
            {
                add => _gazeMoved += value;
                remove => _gazeMoved -= value;
            }
            private EventHandler<GazeMovedEventArgs> _gazeMoved;

            event EventHandler IGazeSource.GazeExited
            {
                add { }
                remove { }
            }

            event EventHandler IGazeSource.EyesOff
            {
                add { }
                remove { }
            }

            Task<bool> IGazeDevice.RequestCalibrationAsync()
            {
                return Task.FromResult(false);
            }
        }

        private class NonCursor : IGazeCursor
        {
            private readonly Form _form;
            private PictureBox _pictureBox;

            internal NonCursor(Form form)
            {
                _form = form;
            }

            public bool IsCursorVisible
            {
                get => _isCursorVisible;
                set
                {
                    if (_isCursorVisible != value)
                    {
                        _isCursorVisible = value;

                        if (value)
                        {
                            var diameter = 20;
                            var bitmap = new Bitmap(diameter, diameter);
                            using (var graphics = Graphics.FromImage(bitmap))
                            {
                                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                graphics.FillEllipse(Brushes.Green, 0, 0, diameter, diameter);
                            }

                            _pictureBox = new PictureBox();
                            _pictureBox.Location = new Point(10, 10);
                            _pictureBox.Size = new Size(diameter, diameter);

                            _pictureBox.Image = bitmap;
                            _form.Controls.Add(_pictureBox);
                            _form.Controls.SetChildIndex(_pictureBox, 0);
                        }
                        else
                        {
                            _form.Controls.Remove(_pictureBox);
                            _pictureBox = null;
                        }
                    }
                }
            }
            bool _isCursorVisible;

            public bool IsGazeEntered { get; set; }
            public PointF Position
            {
                get => _position;
                set
                {
                    var point = _form.PointToClient(new Point((int)value.X, (int)value.Y));
                    var left = point.X - _pictureBox.Size.Width / 2;
                    var top = point.Y - _pictureBox.Size.Height / 2;
                    _pictureBox.Location = new Point(left, top);
                }
            }
            PointF _position;

            public void LoadSettings(IDictionary<string, object> settings)
            {
            }
        }
    }
}
