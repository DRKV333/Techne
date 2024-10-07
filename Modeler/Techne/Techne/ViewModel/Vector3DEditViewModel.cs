/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows.Media.Media3D;
using Cinch;
using MEFedMVVM.ViewModelLocator;

namespace Techne
{
    [ExportViewModel("VectorEditViewModel")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class Vector3DEditViewModel : ViewModelBase
    {
        private static readonly PropertyChangedEventArgs vectorXChangeArgs = ObservableHelper.CreateArgs<Vector3DEditViewModel>(x => x.X);
        private static readonly PropertyChangedEventArgs vectorYChangeArgs = ObservableHelper.CreateArgs<Vector3DEditViewModel>(x => x.Y);
        private static readonly PropertyChangedEventArgs vectorZChangeArgs = ObservableHelper.CreateArgs<Vector3DEditViewModel>(x => x.Z);
        private static readonly PropertyChangedEventArgs vectorChangeArgs = ObservableHelper.CreateArgs<Vector3DEditViewModel>(x => x.Vector);

        #region Constructor
        [ImportingConstructor]
        public Vector3DEditViewModel()
        {
        }
        #endregion

        private Vector3D vector;
        //Object
        public Double X
        {
            get { return vector.X; }
            set { Vector = new Vector3D(value, vector.Y, vector.Z); }
        }

        public Double Y
        {
            get { return vector.Y; }
            set { Vector = new Vector3D(vector.X, value, vector.Z); }
        }

        public Double Z
        {
            get { return vector.Z; }
            set { Vector = new Vector3D(vector.X, vector.Y, value); }
        }

        public Vector3D Vector
        {
            get { return vector; }
            set
            {
                vector = value;
                NotifyPropertyChanged(vectorChangeArgs);
                NotifyPropertyChanged(vectorXChangeArgs);
                NotifyPropertyChanged(vectorYChangeArgs);
                NotifyPropertyChanged(vectorZChangeArgs);
            }
        }

        //public DispatcherNotifiedObservableCollection<VectorModel> Vector { get; set; }
    }
}

