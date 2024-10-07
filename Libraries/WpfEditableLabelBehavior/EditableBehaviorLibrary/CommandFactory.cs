using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EditableBehaviorLibrary
{
    public static class CommandFactory
    {
        public static ICommand CreateCommand(Func<bool> canExecuteAction, Action executeAction, out Action canExecuteChangedAction)
        {
            Command command = new Command(canExecuteAction, executeAction);
            canExecuteChangedAction = command.OnCanExecuteChanged;
            return command;
        }
        private class Command : ICommand
        {
            private Func<bool> _canExecuteAction;
            private Action _executeAction;

            public Command(Func<bool> canExecuteAction, Action executeAction)
            {
                _canExecuteAction = canExecuteAction;
                _executeAction = executeAction;
            }

            #region ICommand Members

            public bool CanExecute(object parameter)
            {
                return _canExecuteAction();
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                _executeAction();
            }

            #endregion

            public void OnCanExecuteChanged()
            {
                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, EventArgs.Empty);
                }
            }
        }
    }
}
