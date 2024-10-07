/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;
using Cinch;

namespace Techne
{
    public class TextureViewerViewModel : EditableValidatingViewModelBase
    {
        private static readonly PropertyChangedEventArgs textureSizeChangeArgs = ObservableHelper.CreateArgs<TextureViewerViewModel>(x => x.TextureSize);

        private Vector textureSize;

        public FrameworkElement Overlay
        {
            get;
            set;
        }

        public BitmapSource Texture
        {
            get;
            set;
        }

        public Vector TextureSize
        {
            get
            {
                //return Texture == null ? new Vector(64, 32) : new Vector(Texture.PixelWidth, Texture.PixelHeight);
                if (textureSize == null)
                    textureSize = new Vector(64, 32);
                return textureSize;
            }
            set
            {
                textureSize = value;
                NotifyPropertyChanged(textureSizeChangeArgs);
            }
        }
    }
}

