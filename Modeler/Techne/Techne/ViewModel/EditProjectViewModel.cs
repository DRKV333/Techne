/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Windows;
using System.Windows.Controls;
using Cinch;
using Techne.Models;
using Techne.Plugins;

namespace Techne.ViewModel
{
    internal class EditProjectViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private readonly TechneModel techneModel;
        private int textureHeight;
        private string textureHeightValue;
        private ComboBoxItem textureSizeXItem;
        private ComboBoxItem textureSizeYItem;
        private int textureWidth;
        private string textureWidthValue;

        public EditProjectViewModel(MainWindowViewModel mainWindowViewModel, TechneModel techneModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            this.techneModel = techneModel;


            CloseViewCommand = new SimpleCommand<object, object>(CloseViewCommandCommand);

            TextureSizes = new DispatcherNotifiedObservableCollection<ComboBoxItem>();

            var sizes = new int[5];

            for (int i = 5; i < 10; i++)
            {
                sizes[i - 5] = (int)Math.Pow(2, i);
                TextureSizes.Add(new ComboBoxItem
                                 {
                                     Content = sizes[i - 5].ToString()
                                 });
            }

            textureWidthValue = mainWindowViewModel.ActiveModel.TextureSize.X.ToString();
            textureHeightValue = mainWindowViewModel.ActiveModel.TextureSize.Y.ToString();
            //if (sizes.Contains((int)mainWindowViewModel.ActiveModel.TextureSize.X))
            //{
            //    var index = (int)(Math.Log(mainWindowViewModel.ActiveModel.TextureSize.X, 2));
            //    textureSizeXItem = TextureSizes[index - 5];
            //}
            //else
            //{
            //    textureWidthValue = mainWindowViewModel.ActiveModel.TextureSize.X.ToString();
            //}
            //if (sizes.Contains((int)mainWindowViewModel.ActiveModel.TextureSize.Y))
            //{
            //    var index = (int)(Math.Log(mainWindowViewModel.ActiveModel.TextureSize.Y, 2));
            //    textureSizeYItem = TextureSizes[index - 5];
            //}
            //else
            //{
            //    textureHeightValue = mainWindowViewModel.ActiveModel.TextureSize.Y.ToString();
            //}
        }

        public SimpleCommand<Object, Object> CloseViewCommand
        {
            get;
            private set;
        }

        public int ProjectType
        {
            get { return (int)techneModel.ProjectType; }
            set
            {
                techneModel.ProjectType = (ProjectType)value;
                mainWindowViewModel.OnTechneModelChanged();
            }
        }

        public DispatcherNotifiedObservableCollection<ComboBoxItem> TextureSizes
        {
            get;
            set;
        }

        public ComboBoxItem TextureSizeXItem
        {
            get { return textureSizeXItem; }
            set
            {
                textureSizeXItem = value;
                UpdateTextureSize();
            }
        }

        public ComboBoxItem TextureSizeYItem
        {
            get { return textureSizeYItem; }
            set
            {
                textureSizeYItem = value;
                UpdateTextureSize();
            }
        }

        public string TextureWidthValue
        {
            get { return textureWidthValue; }
            set
            {
                textureWidthValue = value;
                UpdateTextureSize();
            }
        }

        public string TextureHeightValue
        {
            get { return textureHeightValue; }
            set
            {
                textureHeightValue = value;
                UpdateTextureSize();
            }
        }

        public string ModelName
        {
            get { return techneModel.Name; }
            set
            {
                techneModel.Name = value;
                techneModel.ProjectName = value;
                techneModel.Models[0].Name = value;
            }
        }

        public string BaseClass
        {
            get { return techneModel.Models[0].BaseClass; }
            set { techneModel.Models[0].BaseClass = value; }
        }

        private void UpdateTextureSize()
        {
            if (TextureWidthValue != null)
            {
                if (!Int32.TryParse(TextureWidthValue, out textureWidth))
                    textureWidth = 64;
            }
            //else if (TextureSizeXItem != null)
            //{
            //    if (!Int32.TryParse(TextureSizeXItem.Content.ToString(), out textureWidth))
            //        textureWidth = 64;
            //}

            if (TextureHeightValue != null)
            {
                if (!Int32.TryParse(TextureHeightValue, out textureHeight))
                    textureHeight = 32;
            }
            //else if (TextureSizeYItem != null)
            //{
            //    if (!Int32.TryParse(TextureSizeYItem.Content.ToString(), out textureHeight))
            //        textureHeight = 32;
            //}

            mainWindowViewModel.ActiveModel.TextureSize = new Vector(textureWidth, textureHeight);
            mainWindowViewModel.TextureViewModel.TextureSize = new Vector(textureWidth, textureHeight);
        }

        internal void CloseViewCommandCommand(Object o)
        {
            RaiseCloseRequest(true);
        }
    }
}

