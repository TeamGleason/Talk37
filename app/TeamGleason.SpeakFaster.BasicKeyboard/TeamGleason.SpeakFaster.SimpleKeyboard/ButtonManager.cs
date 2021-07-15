using System;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using TeamGleason.SpeakFaster.KeyboardLayouts;

namespace TeamGleason.SpeakFaster.SimpleKeyboard
{
    internal abstract class ButtonManager : ICommand
    {
        internal readonly KeyboardControl _parent;

        internal ButtonManager(KeyboardControl parent, ButtonBase button)
        {
            _parent = parent;
            Button = button;

            button.Command = this;
        }

        internal ButtonBase Button { get; }

        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
            }

            remove
            {
            }
        }

        internal static TextButtonManager Create(KeyboardControl parent, TextKey key)
        {
            var manager = TextButtonManager.Create(parent, key);
            return manager;
        }

        internal static CommandButtonManager Create(KeyboardControl parent, CommandKey key)
        {
            var manager = CommandButtonManager.Create(parent, key);
            return manager;
        }

        internal static PredictionButtonManager Create(KeyboardControl parent, PredictionKey key)
        {
            var manager = PredictionButtonManager.Create(parent, key);
            return manager;
        }

        protected virtual void Execute()
        {

        }

        bool ICommand.CanExecute(object parameter)
        {
            return true;
        }

        void ICommand.Execute(object parameter)
        {
            Execute();
        }
    }
}
