using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace San.Base
{
    public class CustomCommand<T> : ICommand
    {
        private bool _canExecute;
        private Action<T> _executeAction;
        private Predicate<T> _canExecutePredicate;

        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="action">Generic action that is called when the command is executed</param>
        /// <param name="canExecute">Init vlaue if command can be executed</param>
        public CustomCommand(Action<T> action, bool canExecute = true)
        {
            ExecuteAction = action;
            _canExecute = canExecute;
        }

        public CustomCommand(Action<T> action, Predicate<T> canExecute)
            : this(action)
        {
            _canExecutePredicate = canExecute;
        }

        /// <summary>
        ///    Determines whether the command can execute in its current state
        /// </summary>
        public bool CanExecute
        {
            get { return _canExecute; }
            set
            {
                if (_canExecute != value)
                {
                    _canExecute = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        ///     Current action that is called when the command is executed
        /// </summary>
        public Action<T> ExecuteAction
        {
            get { return _executeAction; }
            set
            {
                if (value != null && _executeAction == value) return;
                _executeAction = value;
            }
        }

        /// <summary>
        ///     Required ICommand implementation
        /// </summary>
        /// <param name="parameter">CommandParam is ignored</param>
        /// <returns>Value of the CanExecute property</returns>
        bool ICommand.CanExecute(object parameter)
        {
            if (_canExecutePredicate != null)
            {
                _canExecutePredicate.Invoke((T)parameter);
            }
            return CanExecute;
        }

        /// <summary>
        ///     Required ICommand implementation
        ///     Calls the submitted action
        /// </summary>
        /// <param name="parameter">CommandParameter from binding</param>
        public void Execute(object parameter)
        {
            try
            {
                //Check for type safety
                var param = (T)parameter;
                ExecuteAction.Invoke(param);
            }
            catch (InvalidCastException)
            { }
        }

        public event EventHandler CanExecuteChanged;
    }

    /// <summary>
    ///     ICommand impementation without generics
    ///     Always used, when a no command parameter is required.
    /// </summary>
    public class CustomCommand : ICommand
    {
        private bool _canExecute;
        private Action _executeAction;
        private Predicate<object> _canExecutePredicate;

        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="action">Generic action that is called when the command is executed</param>
        /// <param name="canExecute">Init vlaue if command can be executed</param>
        public CustomCommand(Action action, bool canExecute = true)
        {
            ExecuteAction = action;
            _canExecute = canExecute;
        }

        public CustomCommand(Action action, Predicate<object> canExecute)
            : this(action)
        {
            _canExecutePredicate = canExecute;
        }

        /// <summary>
        ///    Determines whether the command can execute in its current state
        /// </summary>
        public bool CanExecute
        {
            get { return _canExecute; }
            set
            {
                if (_canExecute != value)
                {
                    _canExecute = value;
                    var canExecuteChanged = CanExecuteChanged;
                    //refresh the can exscute state for the binding
                    canExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        ///     Current action that is called when the command is executed
        /// </summary>
        public Action ExecuteAction
        {
            private get { return _executeAction; }
            set
            {
                if (_executeAction != null && _executeAction == value) return;
                _executeAction = value;
            }
        }

        /// <summary>
        ///     Required ICommand implementation
        /// </summary>
        /// <param name="parameter">CommandParam is ignored</param>
        /// <returns>Value of the CanExecute property</returns>
        bool ICommand.CanExecute(object parameter)
        {
            if (_canExecutePredicate != null)
            {
                return _canExecutePredicate.Invoke(parameter);
            }

            return CanExecute;
        }

        /// <summary>
        ///     Required ICommand implementation
        ///     Calls the submitted action
        /// </summary>
        /// <param name="parameter">CommandParameter from binding</param>
        public void Execute(object parameter)
        {
            ExecuteAction.Invoke();
        }

        public event EventHandler CanExecuteChanged;
    }

}


