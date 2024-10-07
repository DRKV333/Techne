using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace EditableBehaviorLibrary
{
    public abstract class EditableBehavior : Behavior<Label>
    {
        private EditIconAdorner editIconAdorner;
        private EditableAdorner editableAdorner;

        public double MinimumEditWidth { get; set; }

        public bool CanEdit
        {
            get { return editableAdorner == null; }
        }

        public bool CanSave
        {
            get { return editableAdorner != null; }
        }

        public event EventHandler StateChanged;
        public event Action<object, OnSaveEventArgs> OnSaving;

        protected override void OnAttached()
        {
            AttachAssociatedObjectEvents();
            EditableBehaviorManager.RegisterBehavior(this);
        }

        protected override void OnDetaching()
        {
            DetachAssoicatedObjectEvents();
            EditableBehaviorManager.UnRegisterBehavior(this);
        }

        protected abstract EditableAdorner GetEditableAdorner();

        private void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
        {
            if (editIconAdorner != null && !editIconAdorner.IsMouseOver)
            {
                editIconAdorner.FadeOut();
            }
        }

        private void AssociatedObject_MouseEnter(object sender, MouseEventArgs e)
        {
            if (editIconAdorner == null)
            {
                editIconAdorner = new EditIconAdorner(AssociatedObject, AdornerLayer.GetAdornerLayer(AssociatedObject));
                editIconAdorner.Edit += _editIconAdorner_Edit;
            }
            editIconAdorner.FadeIn();
        }

        private void _editIconAdorner_Edit(object sender, EventArgs e)
        {
            EnterEditMode();
        }

        private void _editableAdorner_Save(object sender, EventArgs e)
        {
            ExitEditMode(true);
        }

        private void _editableAdorner_Cancel(object sender, EventArgs e)
        {
            ExitEditMode(false);
        }

        public void ExitEditMode(bool shouldSave)
        {
            if (editableAdorner != null)
            {
                if (shouldSave)
                {
                    var eventArgs = new OnSaveEventArgs(AssociatedObject.Content.ToString(),
                                                        editableAdorner.TextEntered);
                    if (OnSaving != null)
                    {
                        OnSaving(this, eventArgs);
                        if (eventArgs.Cancel)
                        {
                            editableAdorner.ValidationError(eventArgs.ErrorMessage);
                            return;
                        }
                    }
                    AssociatedObject.Content = editableAdorner.TextEntered.Replace("_", "");
                }
                AttachAssociatedObjectEvents();
                editableAdorner.Save -= _editableAdorner_Save;
                editableAdorner.Cancel -= _editableAdorner_Cancel;
                editableAdorner.Destroy();
                editableAdorner = null;
                RaiseStateChanged();
            }
        }

        public void EnterEditMode()
        {
            if (editableAdorner == null)
            {
                AssociatedObject.MouseEnter -= AssociatedObject_MouseEnter;
                AssociatedObject.MouseLeave -= AssociatedObject_MouseLeave;

                if (editIconAdorner != null)
                {
                    editIconAdorner.Destroy();
                    editIconAdorner = null;
                }
                editableAdorner = GetEditableAdorner();
                editableAdorner.Save += _editableAdorner_Save;
                editableAdorner.Cancel += _editableAdorner_Cancel;

                //this isn't working
                AssociatedObject.KeyDown += delegate(object o, KeyEventArgs e)
                {
                    if (e.Key == Key.F2)
                        EnterEditMode();
                };

                RaiseStateChanged();
            }
        }

        private void RaiseStateChanged()
        {
            if (StateChanged != null)
            {
                StateChanged(this, EventArgs.Empty);
            }
        }

        private void AttachAssociatedObjectEvents()
        {
            AssociatedObject.MouseEnter += AssociatedObject_MouseEnter;
            AssociatedObject.MouseLeave += AssociatedObject_MouseLeave;
            AssociatedObject.MouseDoubleClick += delegate {
                EnterEditMode();
            };
        }


        private void DetachAssoicatedObjectEvents()
        {
            if (editIconAdorner != null)
            {
                editIconAdorner.Destroy();
                editIconAdorner = null;
            }
            if (editableAdorner != null)
            {
                editableAdorner.Destroy();
                editableAdorner = null;
            }
            AssociatedObject.MouseEnter -= AssociatedObject_MouseEnter;
            AssociatedObject.MouseLeave -= AssociatedObject_MouseLeave;
        }
    }

    public class OnSaveEventArgs : EventArgs
    {
        public OnSaveEventArgs(string oldValue, string newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
            ErrorMessage = string.Empty;
            Cancel = false;
        }

        public string OldValue { get; private set; }
        public string NewValue { get; private set; }
        public string ErrorMessage { get; set; }
        public bool Cancel { get; set; }
    }
}