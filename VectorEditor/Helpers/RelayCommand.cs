using System;
using System.Windows.Input;

namespace VectorEditor.Helpers
{
    /// <summary>
    /// Команда от представления
    /// </summary>
    class RelayCommand<T> : ICommand
    {
        /// <summary>
        /// Функция, выполняемая командой
        /// </summary>
        private readonly Action<T> fAction;

        /// <summary>
        /// Функция проверки разрешения на выполнение команды
        /// </summary>
        private readonly Predicate<T> fCanExecute;

        public RelayCommand(Action<T> aExecuteFunc, Predicate<T> aCanExecuteFunc = null)
        {
            fAction = aExecuteFunc;
            fCanExecute = aCanExecuteFunc;
        }

        /// <summary>
        /// (ICommand) Событие изменения разрешения на выполнение команды
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// (ICommand) Метод проверки разрешения выполнения команды
        /// </summary>
        public bool CanExecute(object aParameter)
        {
            return fCanExecute == null || fCanExecute((T)aParameter);
        }

        /// <summary>
        /// (ICommand) Метод выполнения команды
        /// </summary>
        public void Execute(object aParameter)
        {
            fAction((T)aParameter);
        }
    }
}
