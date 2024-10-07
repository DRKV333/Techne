/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using Cinch;
using Techne.Model;
using Techne.Plugins.Interfaces;
using GongSolutions.Wpf.DragDrop;
using System.Collections.ObjectModel;
using Techne.ValueConverters;
using Techne.ViewModel;
using System.Windows;
using System.Collections;
using System.ComponentModel;
using System.Windows.Controls;
using System.Linq;
using System.Reflection;
using GongSolutions.Wpf.DragDrop.Utilities;
using System.Collections.Generic;
using System.Windows.Threading;
using Techne.Controls;
using System.Windows.Media;

namespace Techne
{
    public class ModelTreeViewModel : ViewModelBase, IDropTarget, IDragSource
    {
        private ObservableCollection<ITechneVisual> visuals;
        private readonly MainWindowViewModel mainWindowViewModel;

        public ModelTreeViewModel(ObservableCollection<ITechneVisual> visuals, MainWindowViewModel mainWindowViewModel)
        {
            this.visuals = visuals;
            AddPieceCommand = new SimpleCommand<object, object>(ExecuteAddPieceCommand);
            AddFolderCommand = new SimpleCommand<object, object>(ExecuteAddFolderCommand);
            this.mainWindowViewModel = mainWindowViewModel;

            ModelTreeTreeView = new TreeView
            {
                Name = "modelTree",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Margin = new Thickness(0),
                ItemTemplateSelector = new ITechneVisualDataSelector(),
                ItemsSource = Visuals
            };
            GongSolutions.Wpf.DragDrop.DragDrop.SetIsDragSource(ModelTreeTreeView, true);
            GongSolutions.Wpf.DragDrop.DragDrop.SetIsDropTarget(ModelTreeTreeView, true);
            GongSolutions.Wpf.DragDrop.DragDrop.SetDropHandler(ModelTreeTreeView, this);

            ModelTreeTreeView.SelectedItemChanged += ModelTreeTreeView_SelectedItemChanged;
        }

        private void ModelTreeTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (Dispatcher.CurrentDispatcher.CheckAccess())
            {
                if (e.NewValue is ITechneVisual)
                {
                    var visual = e.NewValue as ITechneVisual;
                    MainWindowViewModel.SelectedVisual.SelectVisual(visual);
                }
                else if (e.NewValue is IList<ITechneVisual>)
                {
                    MainWindowViewModel.SelectedVisual.SelectVisual(e.NewValue as IList<ITechneVisual>);
                }
                if (MainWindowViewModel.SelectedVisual.SelectedVisuals.Count > 0)
                    MainWindowViewModel.ChangeOpacity(0.5, mainWindowViewModel.TechneModel);
                mainWindowViewModel.UpdateTextureOverlay();
            }
            else
            {
                Dispatcher.CurrentDispatcher.Invoke(new Action(() => ModelTreeTreeView_SelectedItemChanged(sender, e)));
            }
        }

        public ObservableCollection<ITechneVisual> Visuals
        {
            get { return visuals; }
            set { visuals = value; }
        }

        public TreeView ModelTreeTreeView
        {
            get;
            set;
        }

        public SimpleCommand<Object, Object> AddPieceCommand { get; set; }
        public SimpleCommand<Object, Object> AddFolderCommand { get; set; }

        public MainWindowViewModel MainWindowViewModel
        {
            get { return mainWindowViewModel; }
        }

        private void ExecuteAddPieceCommand(Object o)
        {
            var visual = new TechneVisualCollection()
            {
                Name = MainWindowViewModel.GetUniqueName("Piece"),
                Texture = MainWindowViewModel.Texture,
                TextureSize = MainWindowViewModel.TextureViewModel.TextureSize
            };
            MainWindowViewModel.AddVisual((Visual3D)visual);

            MainWindowViewModel.ChangeOpacity(1, MainWindowViewModel.TechneModel);
            if (Keyboard.Modifiers != ModifierKeys.Shift && Keyboard.Modifiers == ModifierKeys.Control)
            {
                MainWindowViewModel.SelectedVisual.SelectVisual(visual);
            }
            else
            {
                MainWindowViewModel.SelectedVisual.AddVisual(visual);
            }
            MainWindowViewModel.ChangeOpacity(0.5, MainWindowViewModel.TechneModel);
        }

        private void ExecuteAddFolderCommand(Object o)
        {
            var visual = new TechneVisualFolder()
            {
                Name = MainWindowViewModel.GetUniqueName("Folder")
            };
            MainWindowViewModel.AddVisual((Visual3D)visual);

            MainWindowViewModel.ChangeOpacity(1, MainWindowViewModel.TechneModel);
            if (Keyboard.Modifiers != ModifierKeys.Shift && Keyboard.Modifiers == ModifierKeys.Control)
            {
                MainWindowViewModel.SelectedVisual.SelectVisual(visual);
            }
            else
            {
                MainWindowViewModel.SelectedVisual.AddVisual(visual);
            }
            MainWindowViewModel.ChangeOpacity(0.5, MainWindowViewModel.TechneModel);
        }

        public void StartDrag(IDragInfo dragInfo)
        {
            //throw new NotImplementedException();
        }

        public void Dropped(IDropInfo dropInfo)
        {
            //throw new NotImplementedException();
        }

        public void DragOver(IDropInfo dropInfo)
        {
            if (CanAcceptData(dropInfo))
            {
                dropInfo.Effects = DragDropEffects.Move;

                if (dropInfo.TargetItem is ITechneVisualCollection)
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                else
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            int insertIndex = dropInfo.InsertIndex;
            IList destinationList = GetList(dropInfo.TargetCollection);
            IEnumerable data = ExtractData(dropInfo.Data);

            if (dropInfo.DragInfo.VisualSource == dropInfo.VisualTarget)
            {
                IList sourceList = GetList(dropInfo.DragInfo.SourceCollection);

                foreach (object o in data)
                {
                    if (dropInfo.TargetItem != null && dropInfo.TargetItem.Equals(o))
                        continue;

                    var techneVisual = dropInfo.TargetItem as ITechneVisual;
                    if (techneVisual != null)
                    {
                        var parent = techneVisual.Parent;
                        while (parent != null)
                        {
                            if (parent == o)
                            {
                                return;
                            }
                            parent = parent.Parent;
                        }
                    }

                    int index = sourceList.IndexOf(o);

                    if (index != -1)
                    {
                        try
                        {
                            sourceList.RemoveAt(index);
                        }
                        catch (NullReferenceException ex)
                        {
                            if (!ex.TargetSite.Name.Equals("EvaluateOldNewStates", StringComparison.InvariantCultureIgnoreCase))
                                throw ex;
                        }

                        if (dropInfo.TargetItem == null || !(dropInfo.TargetItem is ITechneVisualCollection))
                        {
                            if (dropInfo.Data != null && dropInfo.Data is ITechneVisual)
                            {
                                var visual = (ITechneVisual)dropInfo.Data;

                                if (visual.Parent != null)
                                {
                                    //visual.Parent.Parent
                                    visual.Parent = null;
                                }
                            }
                        }

                        if (dropInfo.TargetItem is ITechneVisualCollection && dropInfo.Data is ITechneVisual)
                        {
                            var visual = (ITechneVisual)o;

                            if (visual.Parent != null)
                                visual.Parent.RemoveChild(visual);
                        }

                        if (sourceList == destinationList && index < insertIndex)
                        {
                            --insertIndex;
                        }
                    }
                }

                if (destinationList != sourceList && dropInfo.TargetItem is ITechneVisual && !(dropInfo.TargetItem is ITechneVisualCollection) && ((ITechneVisual)dropInfo.TargetItem).Parent != null)
                {
                    var parent = ((ITechneVisual)dropInfo.TargetItem).Parent;

                    foreach (object o in data)
                    {
                        if (dropInfo.TargetItem != null && dropInfo.TargetItem.Equals(o))
                            continue;

                        if (insertIndex > destinationList.Count)
                            insertIndex = destinationList.Count;

                        parent.InsertChild(insertIndex++, (ITechneVisual)o);
                    }
                }
                else if (dropInfo.TargetItem is ITechneVisualCollection && dropInfo.Data is ITechneVisual)
                {
                    var target = (ITechneVisualCollection)dropInfo.TargetItem;

                    foreach (object o in data)
                    {
                        if (dropInfo.TargetItem != null && dropInfo.TargetItem.Equals(o))
                            continue;

                        if (insertIndex > destinationList.Count)
                            insertIndex = destinationList.Count;

                        target.InsertChild(insertIndex++, (ITechneVisual)o);
                    }
                }
                else if (dropInfo.TargetItem == null || !(dropInfo.TargetItem is ITechneVisualCollection))
                {
                    foreach (object o in data)
                    {
                        if (dropInfo.TargetItem != null && dropInfo.TargetItem.Equals(o))
                            continue;

                        if (insertIndex > destinationList.Count)
                            insertIndex = destinationList.Count;

                        destinationList.Insert(insertIndex++, o);
                    }
                }
            }
        }

        static bool CanAcceptData(IDropInfo dropInfo)
        {
            if (dropInfo.DragInfo.SourceCollection == dropInfo.TargetCollection)
            {
                return GetList(dropInfo.TargetCollection) != null;
            }
            else if (TestCompatibleTypes(dropInfo.TargetCollection, dropInfo.Data))
            {
                return true;
            }
            return false;
        }

        static IList GetList(IEnumerable enumerable)
        {
            if (enumerable is ICollectionView)
            {
                return ((ICollectionView)enumerable).SourceCollection as IList;
            }
            else
            {
                return enumerable as IList;
            }
        }

        static IEnumerable ExtractData(object data)
        {
            if (data is IEnumerable && !(data is string))
            {
                return (IEnumerable)data;
            }
            else
            {
                return Enumerable.Repeat(data, 1);
            }
        }

        static bool TestCompatibleTypes(IEnumerable target, object data)
        {
            TypeFilter filter = (t, o) => (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            var enumerableInterfaces = target.GetType().FindInterfaces(filter, null);
            var enumerableTypes = from i in enumerableInterfaces select i.GetGenericArguments().Single();

            if (enumerableTypes.Count() > 0)
            {
                Type dataType = TypeUtilities.GetCommonBaseClass(ExtractData(data));
                return enumerableTypes.Any(t => t.IsAssignableFrom(dataType));
            }
            else
            {
                return target is IList;
            }
        }
    }
}

