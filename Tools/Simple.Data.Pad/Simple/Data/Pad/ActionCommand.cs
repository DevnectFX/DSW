using System;
using System.Windows.Input;
namespace Simple.Data.Pad
{
    internal class ActionCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;
        public event EventHandler CanExecuteChanged;
        public ActionCommand(Action execute)
            : this(execute, () => true)
        {
        }
        public ActionCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }
            this._execute = execute;
            this._canExecute = canExecute;
        }
        public void Execute(object parameter)
        {
            this._execute();
        }
        public bool CanExecute(object parameter)
        {
            return this._canExecute();
        }
    }
}
