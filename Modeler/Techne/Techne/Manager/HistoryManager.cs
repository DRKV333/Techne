/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
//using System.Collections.Generic;
//using System.Reflection;
//using System.Windows.Media.Media3D;
//using Techne.Model;
//using Techne.Plugins.Interfaces;

//namespace Techne.Manager
//{
//    public class HistoryManager
//    {
//        #region Fields (4) 
//        private readonly MainWindowViewModel mainWindowViewModel;
//        private readonly Stack<HistoryUnit> redoStack = new Stack<HistoryUnit>();
//        private readonly Stack<HistoryUnit> undoStack = new Stack<HistoryUnit>();
//        private bool redoAction;
//        #endregion Fields 

//        #region Properties (1) 
//        public bool RecordAction
//        {
//            get { return redoAction; }
//            set { redoAction = value; }
//        }
//        #endregion Properties 

//        #region Constructors (1) 
//        public HistoryManager(MainWindowViewModel mainWindowViewModel)
//        {
//            this.mainWindowViewModel = mainWindowViewModel;
//        }
//        #endregion Constructors 

//        #region Methods (3) 
//        // Internal Methods (3) 

//        internal void AddEntry(HistoryUnit historyUnit)
//        {
//            if (redoStack.Count > 0)
//                redoStack.Clear();

//            undoStack.Push(historyUnit);
//        }

//        internal void RedoAction()
//        {
//            try
//            {
//                if (redoStack.Count > 0)
//                {
//                    RecordAction = true;

//                    var lastAction = redoStack.Pop();

//                    switch (lastAction.Action)
//                    {
//                        case HistoryAction.RemoveVisual:
//                            mainWindowViewModel.RemoveVisual(lastAction.VisualHit);
//                            break;
//                        case HistoryAction.AddVisual:
//                            mainWindowViewModel.AddVisual(lastAction.VisualHit);
//                            break;
//                        case HistoryAction.AddToSelection:
//                            mainWindowViewModel.AddVisualToSelection((Visual3D)lastAction.VisualHit);
//                            break;
//                        case HistoryAction.RemoveSelection:
//                            mainWindowViewModel.RemoveVisualFromCollection(mainWindowViewModel.SelectedVisual as TechneVisualCollection, (Visual3D)lastAction.VisualHit);
//                            break;
//                        case HistoryAction.ChangeProperty:
//                            lastAction.Property.GetSetMethod().Invoke(mainWindowViewModel, new[] {lastAction.NewValue});
//                            break;
//                        default:
//                            break;
//                    }

//                    RecordAction = false;
//                    AddEntry(lastAction);
//                }
//            }
//            catch
//            {
//                //todo: noooooo
//            }
//        }

//        //-                        AddVisualToSelection((Visual3D)lastAction.VisualHit);
//        //+                        
//        //+                        if (SelectedVisual == null)
//        //+                            SelectedVisual = lastAction.VisualHit;
//        //+                        else
//        //+                            AddVisualToSelection((Visual3D)lastAction.VisualHit);
//        //+                        break;

//        internal void UndoAction()
//        {
//            try
//            {
//                if (undoStack.Count > 0)
//                {
//                    redoAction = true;

//                    var lastAction = undoStack.Pop();

//                    switch (lastAction.Action)
//                    {
//                        case HistoryAction.RemoveVisual:
//                            mainWindowViewModel.AddVisual(lastAction.VisualHit);
//                            break;
//                        case HistoryAction.AddVisual:
//                            mainWindowViewModel.RemoveVisual(lastAction.VisualHit);
//                            break;
//                        case HistoryAction.AddToSelection:
//                            mainWindowViewModel.RemoveVisualFromCollection(mainWindowViewModel.SelectedVisual as TechneVisualCollection, (Visual3D)lastAction.VisualHit);
//                            break;
//                        case HistoryAction.RemoveSelection:
//                            if (mainWindowViewModel.SelectedVisual == null)
//                                mainWindowViewModel.SelectedVisual = lastAction.VisualHit;
//                            else
//                                mainWindowViewModel.AddVisualToSelection((Visual3D)lastAction.VisualHit);
//                            break;
//                        case HistoryAction.ChangeProperty:
//                            lastAction.Property.GetSetMethod().Invoke(mainWindowViewModel, new[] {lastAction.OldValue});
//                            break;
//                        default:
//                            break;
//                    }
//                    redoStack.Push(lastAction);
//                }
//            }
//            catch
//            {
//                //todo: noooooo
//            }
//            finally
//            {
//                RecordAction = false;
//            }
//        }

//        public void PropertyChanged(ITechneVisual visualHit, HistoryAction historyAction)
//        {
//            if (!mainWindowViewModel.historyManager.RecordAction)
//            {
//                AddEntry(new HistoryUnit(visualHit, historyAction));
//            }
//        }

//        public void PropertyChanged(object oldValue, object newValue, PropertyInfo property)
//        {
//            if (!RecordAction)
//            {
//                if ((oldValue != null && !oldValue.Equals(newValue)) || (newValue != null && !newValue.Equals(oldValue)))
//                {
//                    AddEntry(new HistoryUnit(oldValue, newValue, property, HistoryAction.ChangeProperty));
//                }
//            }
//        }
//        #endregion Methods 
//    }
//}

