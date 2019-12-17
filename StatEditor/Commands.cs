using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StatEditor
{
    public class CommandAsync : ICommand
    {
        private readonly Func<Task> _executeFunc;
        private readonly Func<bool> _canExecuteFunc;

        public CommandAsync(Func<Task> executeFunc, Func<bool> canExecuteFunc = null)
        {
            _executeFunc = executeFunc;
            _canExecuteFunc = canExecuteFunc;
        }

        public bool CanExecute()
        {
            return _canExecuteFunc == null || _canExecuteFunc();
        }

        public Task Execute()
        {
            return _executeFunc();
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }

        async void ICommand.Execute(object parameter)
        {
            await Execute();
        }

        public event EventHandler CanExecuteChanged;

        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class CommandAsync<T> : ICommand
    {
        private readonly Func<T, Task> _executeFunc;
        private readonly Func<T, bool> _canExecuteFunc;

        public CommandAsync(Func<T, Task> executeFunc, Func<T, bool> canExecuteFunc = null)
        {
            _executeFunc = executeFunc;
            _canExecuteFunc = canExecuteFunc;
        }

        public bool CanExecute(T parameter)
        {
            return _canExecuteFunc == null || _canExecuteFunc(parameter);
        }

        public Task Execute(T parameter)
        {
            return _executeFunc(parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        async void ICommand.Execute(object parameter)
        {
            await Execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
