using System;
using System.Windows.Input;

namespace VK_R
{
    internal class Command : ICommand
    {
        /// <summary>
        /// Пользовательское действие при выполнении команды.
        /// </summary>
        private Action targetExecuteAction;
        /// <summary>
        /// Пользовательское условие проверки активности команды.
        /// </summary>
        private Func<bool> targetCanExecuteMethod;

        public Command(Action executeAction)
        {
            targetExecuteAction = executeAction;
        }

        public Command(Action executeAction, Func<bool> canExecuteMethod)
        {
            targetExecuteAction = executeAction;
            targetCanExecuteMethod = canExecuteMethod;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (targetCanExecuteMethod != null)
            {
                return targetCanExecuteMethod();
            }

            return targetExecuteAction != null;
        }

        public void Execute(object parameter)
        {
            targetExecuteAction?.Invoke();
        }
    }
    public class RelayCommand : ICommand
    {
        private readonly Action _action;
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// Creates instance of the command handler
        /// </summary>
        /// <param name="action">Action to be executed by the command</param>
        /// <param name="canExecute">A bolean property to containing current permissions to execute the command</param>
        public RelayCommand(Action action, Func<bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Wires CanExecuteChanged event 
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Forcess checking if execute is allowed
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }
}