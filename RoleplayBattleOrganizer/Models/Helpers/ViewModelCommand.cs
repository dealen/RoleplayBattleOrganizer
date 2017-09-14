using System;
using System.Windows.Input;

namespace RoleplayBattleOrganizer.Models.Helpers
{

    public class ViewModelCommand : ICommand
    {
        #region Private Fields

        private readonly Action<object> _prExecuteAction;
        private readonly Predicate<object> _fnCanExecute;

        #endregion Private Fields


        #region Constructor

        public ViewModelCommand(Action<object> executeAction, Predicate<object> canExecute = null)
        {
            if (executeAction == null)
                throw new ArgumentNullException("executeAction");

            _prExecuteAction = executeAction;
            _fnCanExecute = canExecute;
        }

        #endregion Constructor


        #region Public Properties

        public bool CanExecute(object parameter)
        {
            if (_fnCanExecute == null)
                return true;

            return _fnCanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (_prExecuteAction != null)
                _prExecuteAction(parameter);
        }

        public event EventHandler CanExecuteChanged;

        #endregion Public Properties
    }
}
