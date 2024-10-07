/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
using System;
using System.ComponentModel;
using Cinch;
using Techne.Models;
using System.Collections.Generic;
using Techne.Plugins.Interfaces;

namespace Techne.ViewModel
{
    public class SelectedVisualViewModel : ViewModelBase
    {
        private static readonly PropertyChangedEventArgs widthChangeArgs = ObservableHelper.CreateArgs<SelectedVisualViewModel>(x => x.Width);
        private static readonly PropertyChangedEventArgs heightChangeArgs = ObservableHelper.CreateArgs<SelectedVisualViewModel>(x => x.Height);
        private static readonly PropertyChangedEventArgs lengthChangeArgs = ObservableHelper.CreateArgs<SelectedVisualViewModel>(x => x.Length);

        private static readonly PropertyChangedEventArgs positionXChangeArgs = ObservableHelper.CreateArgs<SelectedVisualViewModel>(x => x.PositionX);
        private static readonly PropertyChangedEventArgs positionYChangeArgs = ObservableHelper.CreateArgs<SelectedVisualViewModel>(x => x.PositionY);
        private static readonly PropertyChangedEventArgs positionZChangeArgs = ObservableHelper.CreateArgs<SelectedVisualViewModel>(x => x.PositionZ);

        private static readonly PropertyChangedEventArgs offsetXChangeArgs = ObservableHelper.CreateArgs<SelectedVisualViewModel>(x => x.OffsetX);
        private static readonly PropertyChangedEventArgs offsetYChangeArgs = ObservableHelper.CreateArgs<SelectedVisualViewModel>(x => x.OffsetY);
        private static readonly PropertyChangedEventArgs offsetZChangeArgs = ObservableHelper.CreateArgs<SelectedVisualViewModel>(x => x.OffsetZ);

        private static readonly PropertyChangedEventArgs textureOffsetXChangeArgs = ObservableHelper.CreateArgs<SelectedVisualViewModel>(x => x.TextureOffsetX);
        private static readonly PropertyChangedEventArgs textureOffsetYChangeArgs = ObservableHelper.CreateArgs<SelectedVisualViewModel>(x => x.TextureOffsetY);

        private static readonly PropertyChangedEventArgs elementNameChangeArgs = ObservableHelper.CreateArgs<SelectedVisualViewModel>(x => x.ElementName);
        private static readonly PropertyChangedEventArgs rotationXChangeArgs = ObservableHelper.CreateArgs<SelectedVisualViewModel>(x => x.RotationX);
        private static readonly PropertyChangedEventArgs rotationYChangeArgs = ObservableHelper.CreateArgs<SelectedVisualViewModel>(x => x.RotationY);
        private static readonly PropertyChangedEventArgs rotationZChangeArgs = ObservableHelper.CreateArgs<SelectedVisualViewModel>(x => x.RotationZ);
        private static readonly PropertyChangedEventArgs isMirroredChangeArgs = ObservableHelper.CreateArgs<SelectedVisualViewModel>(x => x.IsMirrored);
        private static readonly PropertyChangedEventArgs alternativeRotationXChangeArgs = ObservableHelper.CreateArgs<SelectedVisualViewModel>(x => x.AlternativeRotationX);
        private static readonly PropertyChangedEventArgs alternativeRotationYChangeArgs = ObservableHelper.CreateArgs<SelectedVisualViewModel>(x => x.AlternativeRotationY);
        private static readonly PropertyChangedEventArgs alternativeRotationZChangeArgs = ObservableHelper.CreateArgs<SelectedVisualViewModel>(x => x.AlternativeRotationZ);
        private static readonly PropertyChangedEventArgs visualSelectedChangeArgs = ObservableHelper.CreateArgs<SelectedVisualViewModel>(x => x.VisualSelected);

        private readonly MainWindowViewModel mainWindowViewModel;
        private readonly SelectedVisualModel model;

        public SelectedVisualViewModel(SelectedVisualModel model, MainWindowViewModel mainWindowViewModel)
        {
            this.model = model;
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public IList<ITechneVisual> SelectedVisuals
        {
            get
            {
                return model.SelectedVisual;
            }
        }

        public bool VisualSelected
        {
            get { return model.HasSelected(); }
        }

        public Double Width
        {
            get { return model.Width; }
            set
            {
                if (value < 0)
                    return;

                model.Width = (int)value;

                NotifyPropertyChanged(widthChangeArgs);
                
            }
        }

        public Double Height
        {
            get { return model.Height; }
            set
            {
                if (value < 0)
                    return;

                model.Height = (int)value;

                NotifyPropertyChanged(widthChangeArgs);
                NotifyPropertyChanged(heightChangeArgs);
                NotifyPropertyChanged(lengthChangeArgs);
                
            }
        }

        public Boolean IsMirrored
        {
            get { return model.IsMirrored; }
            set
            {
                model.IsMirrored = value;
                NotifyPropertyChanged(isMirroredChangeArgs);
            }
        }

        public Double Length
        {
            get { return model.Length; }
            set
            {
                if (value < 0)
                    return;

                model.Length = (int)value;

                NotifyPropertyChanged(widthChangeArgs);
                NotifyPropertyChanged(heightChangeArgs);
                NotifyPropertyChanged(lengthChangeArgs);
                
            }
        }

        public Double OffsetX
        {
            get { return model.OffsetX; }
            set
            {
                model.OffsetX = value;
                NotifyPropertyChanged(offsetXChangeArgs);


                if (MainWindowViewModel.settingsManager.TechneSettings.FollowSelectedShape)
                    mainWindowViewModel.FocusSelectedVisual();
            }
        }

        public Double OffsetY
        {
            get { return model.OffsetY; }
            set
            {
                model.OffsetY = value;
                NotifyPropertyChanged(offsetYChangeArgs);


                if (MainWindowViewModel.settingsManager.TechneSettings.FollowSelectedShape)
                    mainWindowViewModel.FocusSelectedVisual();
            }
        }

        public Double OffsetZ
        {
            get { return model.OffsetZ; }
            set
            {
                model.OffsetZ = value;
                NotifyPropertyChanged(offsetZChangeArgs);


                if (MainWindowViewModel.settingsManager.TechneSettings.FollowSelectedShape)
                    mainWindowViewModel.FocusSelectedVisual();
            }
        }

        public Double PositionX
        {
            get { return model.PositionX; }
            set
            {
                model.PositionX = value;
                NotifyPropertyChanged(positionXChangeArgs);


                if (MainWindowViewModel.settingsManager.TechneSettings.FollowSelectedShape)
                    mainWindowViewModel.FocusSelectedVisual();
            }
        }

        public Double PositionY
        {
            get { return model.PositionY; }
            set
            {
                model.PositionY = value;
                NotifyPropertyChanged(positionYChangeArgs);


                if (MainWindowViewModel.settingsManager.TechneSettings.FollowSelectedShape)
                    mainWindowViewModel.FocusSelectedVisual();
            }
        }

        public Double PositionZ
        {
            get { return model.PositionZ; }
            set
            {
                model.PositionZ = value;
                NotifyPropertyChanged(positionZChangeArgs);


                if (MainWindowViewModel.settingsManager.TechneSettings.FollowSelectedShape)
                    mainWindowViewModel.FocusSelectedVisual();
            }
        }

        public Double TextureOffsetX
        {
            get { return model.TextureOffsetX; }
            set
            {
                value = (int)value;
                model.TextureOffsetX = value;
                NotifyPropertyChanged(textureOffsetXChangeArgs);
                
            }
        }

        public Double TextureOffsetY
        {
            get { return model.TextureOffsetY; }
            set
            {
                value = (int)value;
                model.TextureOffsetY = value;
                NotifyPropertyChanged(textureOffsetYChangeArgs);
                
            }
        }

        public String ElementName
        {
            get { return model.ElementName; }
            set
            {
                model.ElementName = value.Replace("_", "");
                NotifyPropertyChanged(elementNameChangeArgs);
            }
        }

        public Double RotationX
        {
            get
            {
                var rotation = model.RotationX;
                if (MainWindowViewModel.settingsManager.TechneSettings.UseRadians)
                    rotation *= Math.PI / 180;

                return rotation;
            }
            set
            {
                var rotation = value;
                if (MainWindowViewModel.settingsManager.TechneSettings.UseRadians)
                    rotation *= 180 / Math.PI;

                model.RotationX = rotation;
                NotifyPropertyChanged(rotationXChangeArgs);
                NotifyPropertyChanged(alternativeRotationXChangeArgs);
                
            }
        }

        public Double RotationY
        {
            get
            {
                var rotation = model.RotationY;
                if (MainWindowViewModel.settingsManager.TechneSettings.UseRadians)
                    rotation *= Math.PI / 180;

                return rotation;
            }
            set
            {
                var rotation = value;
                if (MainWindowViewModel.settingsManager.TechneSettings.UseRadians)
                    rotation *= 180 / Math.PI;

                model.RotationY = rotation;
                NotifyPropertyChanged(rotationYChangeArgs);
                NotifyPropertyChanged(alternativeRotationYChangeArgs);
                
            }
        }

        public Double RotationZ
        {
            get
            {
                var rotation = model.RotationZ;
                if (MainWindowViewModel.settingsManager.TechneSettings.UseRadians)
                    rotation *= Math.PI / 180;

                return rotation;
            }
            set
            {
                var rotation = value;
                if (MainWindowViewModel.settingsManager.TechneSettings.UseRadians)
                    rotation *= 180 / Math.PI;

                model.RotationZ = rotation;
                NotifyPropertyChanged(rotationZChangeArgs);
                NotifyPropertyChanged(alternativeRotationZChangeArgs);
                
            }
        }

        public Double AlternativeRotationX
        {
            get
            {
                var rotation = model.RotationX;
                if (!MainWindowViewModel.settingsManager.TechneSettings.UseRadians)
                    rotation *= Math.PI / 180;

                return rotation;
            }
        }

        public Double AlternativeRotationY
        {
            get
            {
                var rotation = model.RotationY;
                if (!MainWindowViewModel.settingsManager.TechneSettings.UseRadians)
                    rotation *= Math.PI / 180;

                return rotation;
            }
        }

        public Double AlternativeRotationZ
        {
            get
            {
                var rotation = model.RotationZ;
                if (!MainWindowViewModel.settingsManager.TechneSettings.UseRadians)
                    rotation *= Math.PI / 180;

                return rotation;
            }
        }

        #region Model-Stuff
        internal bool IsSelected(Plugins.Interfaces.ITechneVisual techneVisual)
        {
            return model.IsSelected(techneVisual);
        }

        internal void RemoveVisual(Plugins.Interfaces.ITechneVisual techneVisual)
        {
            model.RemoveVisual(techneVisual);
            mainWindowViewModel.DeselectVisual(techneVisual);
            NotifyAboutUpdate();
        }

        internal void AddVisual(ITechneVisual techneVisual)
        {
            model.AddVisual(techneVisual);
            NotifyAboutUpdate();
            mainWindowViewModel.ShowWireframeAndAnchor();

            if (MainWindowViewModel.settingsManager.TechneSettings.FocusSelectedShape)
                mainWindowViewModel.FocusSelectedVisual();
        }

        internal void SelectVisual(IList<ITechneVisual> iList)
        {
            mainWindowViewModel.DeselectVisual();

            foreach (var child in iList)
            {
                model.AddVisual(child);
            }

            NotifyAboutUpdate();
        }

        internal void ClearSelection()
        {
            model.ClearSelection();
            NotifyAboutUpdate();
        }

        internal void SelectVisual(Plugins.Interfaces.ITechneVisual techneVisual)
        {
            mainWindowViewModel.DeselectVisual();
            model.SelectVisual(techneVisual);
            NotifyAboutUpdate();
            mainWindowViewModel.ShowWireframeAndAnchor();

            if (MainWindowViewModel.settingsManager.TechneSettings.FocusSelectedShape)
                mainWindowViewModel.FocusSelectedVisual();
        }

        internal void ToggleSelection(Plugins.Interfaces.ITechneVisual techneVisual)
        {
            if (model.IsSelected(techneVisual))
                RemoveVisual(techneVisual);
            else
                AddVisual(techneVisual);

            NotifyAboutUpdate();
        }

        internal bool HasSelected()
        {
            return model.HasSelected();
        }

        private void NotifyAboutUpdate()
        {
            NotifyPropertyChanged(widthChangeArgs);
            NotifyPropertyChanged(heightChangeArgs);
            NotifyPropertyChanged(lengthChangeArgs);
            NotifyPropertyChanged(positionXChangeArgs);
            NotifyPropertyChanged(positionYChangeArgs);
            NotifyPropertyChanged(positionZChangeArgs);
            NotifyPropertyChanged(offsetXChangeArgs);
            NotifyPropertyChanged(offsetYChangeArgs);
            NotifyPropertyChanged(offsetZChangeArgs);
            NotifyPropertyChanged(textureOffsetXChangeArgs);
            NotifyPropertyChanged(textureOffsetYChangeArgs);
            NotifyPropertyChanged(elementNameChangeArgs);
            NotifyPropertyChanged(rotationXChangeArgs);
            NotifyPropertyChanged(rotationYChangeArgs);
            NotifyPropertyChanged(rotationZChangeArgs);
            NotifyPropertyChanged(isMirroredChangeArgs);
            NotifyPropertyChanged(alternativeRotationXChangeArgs);
            NotifyPropertyChanged(alternativeRotationYChangeArgs);
            NotifyPropertyChanged(alternativeRotationZChangeArgs);
            NotifyPropertyChanged(visualSelectedChangeArgs);
        }
        #endregion
    }
}
// ReSharper restore MemberCanBePrivate.Global
// ReSharper restore UnusedMember.Global

