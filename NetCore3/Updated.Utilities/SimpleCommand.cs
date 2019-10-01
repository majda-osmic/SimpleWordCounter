using System;
using System.Windows.Input;

namespace Utilities
{
    public class SimpleCommand : ICommand
    {
        private Action<object> ExecuteDelagete { get; }
        private Func<object, bool> CanExecuteDelegate { get; }

        public event EventHandler CanExecuteChanged = delegate { }; //C#8 delegate init magic

        public SimpleCommand(Action<object> executeDelegate) : this(executeDelegate, obj => true)
        {

        }

        public SimpleCommand(Action<object> executeDelegate, Func<object, bool> canExecuteDelegate)
        {
            ExecuteDelagete = executeDelegate;
            CanExecuteDelegate = canExecuteDelegate;
        }

        public bool CanExecute(object parameter) => CanExecuteDelegate.Invoke(parameter);

        public void Execute(object parameter) => ExecuteDelagete.Invoke(parameter);

        public void RaiseCanExecuteChanged() => CanExecuteChanged.Invoke(this, new EventArgs());
    }
}
