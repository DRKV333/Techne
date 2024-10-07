using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EditableBehaviorLibrary
{
    public static class EditableBehaviorManager
    {
        private static List<EditableBehavior> _behaviors;
        private static ICommand _editAll;
        private static ICommand _saveAll;
        private static ICommand _cancelAll;
        private static Action _canEditCommandExecuteChanged;
        private static Action _canSaveCommandExecuteChanged;
        private static Action _canCancelCommandExecuteChanged;

        static EditableBehaviorManager()
        {
            _behaviors = new List<EditableBehavior>();
            _editAll = CommandFactory.CreateCommand(EditCommand_CanExecute, EditCommand_Execute, out _canEditCommandExecuteChanged);
            _saveAll = CommandFactory.CreateCommand(SaveCommand_CanExecute, SaveCommand_Execute, out _canSaveCommandExecuteChanged);
            _cancelAll = CommandFactory.CreateCommand(CancelCommand_CanExecute, CancelCommand_Execute, out _canCancelCommandExecuteChanged);
        }

        public static ICommand EditAll
        {
            get { return _editAll; }
        }

        public static ICommand SaveAll
        {
            get { return _saveAll; }
        }

        public static ICommand CancelAll
        {
            get { return _cancelAll; }
        }

        private static void InvalidateCommands()
        {
            _canEditCommandExecuteChanged();
            _canSaveCommandExecuteChanged();
            _canCancelCommandExecuteChanged(); 
        }

        public static void RegisterBehavior(EditableBehavior behavior)
        {
            if (!_behaviors.Contains(behavior))
            {
                behavior.StateChanged += new EventHandler(behavior_StateChanged);
                _behaviors.Add(behavior);
            }
            InvalidateCommands();
        }

        public static void UnRegisterBehavior(EditableBehavior behavior)
        {
            if (_behaviors.Contains(behavior))
            {
                behavior.StateChanged -= new EventHandler(behavior_StateChanged);
                _behaviors.Remove(behavior);
            }
        }

        static void behavior_StateChanged(object sender, EventArgs e)
        {
            InvalidateCommands();
        }

        private static bool EditCommand_CanExecute()
        {
            bool canExecute = false;
            foreach (EditableBehavior be in _behaviors)
            {
                canExecute = canExecute | be.CanEdit;
            }
            return canExecute;
        }

        private static void EditCommand_Execute()
        {
            foreach (EditableBehavior be in _behaviors)
            {
                be.EnterEditMode();
            }
        }

        private static bool SaveCommand_CanExecute()
        {
            bool canExecute = false;
            foreach (EditableBehavior be in _behaviors)
            {
                canExecute = canExecute | be.CanSave;
            }
            return canExecute;
        }

        private static void SaveCommand_Execute()
        {
            foreach (EditableBehavior be in _behaviors)
            {
                be.ExitEditMode(true);
            }
        }

        private static bool CancelCommand_CanExecute()
        {
            bool canExecute = false;
            foreach (EditableBehavior be in _behaviors)
            {
                canExecute = canExecute | be.CanSave;
            }
            return canExecute;
        }

        private static void CancelCommand_Execute()
        {
            foreach (EditableBehavior be in _behaviors)
            {
                be.ExitEditMode(false);
            }
        }
    }
}
