using System;
using System.Windows.Input;

namespace Utilities
{
    public class SimpleCommand : ICommand
    {
        private Action<object> ExecuteDelagete { get; }
        private Func<object, bool> CanExecuteDelegate { get; }

        public event EventHandler CanExecuteChanged;

        public SimpleCommand(Action<object> executeDelegate, Func<object, bool> canExecuteDelegate = null)
        {
            ExecuteDelagete = executeDelegate;
            CanExecuteDelegate = canExecuteDelegate;
        }

        public bool CanExecute(object parameter) => CanExecuteDelegate?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => ExecuteDelagete?.Invoke(parameter);

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, new EventArgs());
    }
}
