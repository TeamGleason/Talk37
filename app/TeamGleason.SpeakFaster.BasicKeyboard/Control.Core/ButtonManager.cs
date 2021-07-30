﻿using System;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace TeamGleason.SpeakFaster.BasicKeyboard.Control
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

        internal virtual void UpdateStateModifiers()
        {
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
            }

            remove
            {
            }
        }

        protected abstract void Execute();

        bool ICommand.CanExecute(object parameter)
        {
            return true;
        }

        void ICommand.Execute(object parameter)
        {
            Execute();
        }
    }

    internal abstract class ButtonManager<T> : ButtonManager
    {
        protected readonly T _key;

        internal ButtonManager(KeyboardControl parent, T key, ButtonBase button)
            : base(parent, button)
        {
            _key = key;
        }
    }
}
