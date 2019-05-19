using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VectorEditor.Helpers
{
    /// <summary>
    /// Класс самой простой комманды
    /// </summary>
    class Command : ICommand
    {
        /// <summary>
        /// Делегат на функцию, выполняемую командой
        /// </summary>
        private Action fAction;

        public Command(Action aAction)
        {
            fAction = aAction;
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
            return true;
        }

        /// <summary>
        /// (ICommand) Метод выполнения команды
        /// </summary>
        public void Execute(object aParameter)
        {
            fAction();
        }
    }
}
