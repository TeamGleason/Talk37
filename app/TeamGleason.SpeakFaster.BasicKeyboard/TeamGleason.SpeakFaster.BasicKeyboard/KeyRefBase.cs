using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace TeamGleason.SpeakFaster.BasicKeyboard
{
    public abstract class KeyRefBase : ICommand
    {
        internal KeyRefBase(XmlReader reader)
        {
            for (var attribute = reader.MoveToFirstAttribute(); attribute; attribute = reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case nameof(KeyRef): KeyRef = reader.Value; break;
                    case nameof(ColumnSpan): ColumnSpan = int.Parse(reader.Value); break;
                    case nameof(Column): Column = int.Parse(reader.Value); break;
                    case nameof(RowSpan): RowSpan = int.Parse(reader.Value); break;
                    case nameof(Row): Row = int.Parse(reader.Value); break;
                    default: throw new InvalidDataException();
                }
            }
        }

        public string KeyRef { get; }
        public int ColumnSpan { get; } = 1;
        public int Column { get; }
        public int RowSpan { get; } = 1;
        public int Row { get; }

        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
                _canExecuteChanged += value;
            }

            remove
            {
                _canExecuteChanged -= value;
            }
        }
        private EventHandler _canExecuteChanged;

        internal Control CreateControl()
        {
            var control = new Button { Content = KeyRef, Command = this };
            Grid.SetRow(control, Row);
            Grid.SetRowSpan(control, RowSpan);
            Grid.SetColumn(control, Column);
            Grid.SetColumnSpan(control, ColumnSpan);
            return control;
        }

        bool ICommand.CanExecute(object parameter)
        {
            return true;
        }

        void ICommand.Execute(object parameter)
        {
            Execute();
        }

        protected virtual void Execute()
        {

        }
    }
}
