using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Nginx_Core_Helper.Models
{
    public class Executor : ICommand
    {
        public Executor(Action<object> executeAction, Predicate<object> isExecutable = null)
        {
            ExecuteAction = executeAction;
            IsExecutable = isExecutable;
        }

        private Action<object> ExecuteAction { get; set; }

        private Predicate<object> IsExecutable { get; set; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (IsExecutable == null)
            {
                return true;
            }
            return IsExecutable.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            ExecuteAction.Invoke(parameter);
        }
    }
}
