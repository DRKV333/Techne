/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Cinch;
using Techne.Models;
using Techne.Plugins;

namespace Techne.ViewModel
{
    internal class NewProjectViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel mainWindow;
        private string modelName = "";
        private ComboBoxItem selectedBaseClass;
        private ProjectType type;

        public NewProjectViewModel(MainWindowViewModel mainwindow)
        {
            var modelBase = new ComboBoxItem
                            {
                                Content = "ModelBase",
                                IsSelected = true
                            };

            BaseClasses = new DispatcherNotifiedObservableCollection<ComboBoxItem>
                          {
                              modelBase,
                              new ComboBoxItem
                              {
                                  Content = "ModelBiped"
                              },
                              new ComboBoxItem
                              {
                                  Content = "ModelCow"
                              },
                              new ComboBoxItem
                              {
                                  Content = "ModelCreeper"
                              },
                              new ComboBoxItem
                              {
                                  Content = "ModelEnderman"
                              },
                              new ComboBoxItem
                              {
                                  Content = "ModelPig"
                              },
                              new ComboBoxItem
                              {
                                  Content = "ModelSpider"
                              },
                              new ComboBoxItem
                              {
                                  Content = "ModelWolf"
                              },
                              new ComboBoxItem
                              {
                                  Content = ""
                              }
                          };

            TextureSizes = new DispatcherNotifiedObservableCollection<ComboBoxItem>();

            for (int i = 5; i < 10; i++)
            {
                TextureSizes.Add(new ComboBoxItem
                                 {
                                     Content = Math.Pow(2, i).ToString()
                                 });
            }

            //Binding binding = new Binding("SelectBaseClassCommand")
            //{
            //    Source = this
            //};

            CreateModelCommand = new SimpleCommand<object, object>(ExecuteCreateModelCommand);
            SelectBaseClassCommand = new SimpleCommand<object, object>(ExecuteSelectBaseClassCommand);

            mainWindow = mainwindow;

            BaseClasses.Add(new ComboBoxItem
                            {
                                Content = new Button
                                          {
                                              Command = SelectBaseClassCommand,
                                              Content = "Select"
                                          }
                            });

            SelectedBaseClass = modelBase;

            TextureSizeXItem = TextureSizes[1];
            TextureSizeYItem = TextureSizes[0];
        }

        public DispatcherNotifiedObservableCollection<ComboBoxItem> BaseClasses
        {
            get;
            set;
        }

        public DispatcherNotifiedObservableCollection<ComboBoxItem> TextureSizes
        {
            get;
            set;
        }

        public int ProjectType
        {
            get { return (int)type; }
            set { type = (ProjectType)value; }
        }

        public string ModelName
        {
            get { return modelName; }
            set { modelName = value; }
        }

        public ComboBoxItem TextureSizeXItem
        {
            get;
            set;
        }

        public ComboBoxItem TextureSizeYItem
        {
            get;
            set;
        }

        public string TextureWidthValue
        {
            get;
            set;
        }

        public string TextureHeightValue
        {
            get;
            set;
        }

        public ComboBoxItem SelectedBaseClass
        {
            get { return selectedBaseClass; }
            set { selectedBaseClass = value; }
        }

        public Boolean ShowOnStartup
        {
            get { return MainWindowViewModel.settingsManager.TechneSettings.ShowNewProjectDialogOnStartup; }
            set
            {
                MainWindowViewModel.settingsManager.TechneSettings.ShowNewProjectDialogOnStartup = value;
                MainWindowViewModel.settingsManager.Save();
            }
        }

        public SimpleCommand<Object, Object> CreateModelCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> SelectBaseClassCommand
        {
            get;
            private set;
        }

        public void ExecuteCreateModelCommand(Object o)
        {
            string baseClass = "ModelBase";

            if (selectedBaseClass != null && selectedBaseClass.Content != null && !String.IsNullOrEmpty(selectedBaseClass.Content.ToString()))
            {
                baseClass = selectedBaseClass.Content.ToString();
            }

            int textureSizeX = 64;
            int textureSizeY = 32;

            if (TextureWidthValue != null)
            {
                if (!Int32.TryParse(TextureWidthValue, out textureSizeX))
                    textureSizeX = 64;
            }
            else if (TextureSizeXItem != null)
            {
                if (!Int32.TryParse(TextureSizeXItem.Content.ToString(), out textureSizeX))
                    textureSizeX = 64;
            }

            if (TextureHeightValue != null)
            {
                if (!Int32.TryParse(TextureHeightValue, out textureSizeY))
                    textureSizeY = 32;
            }
            else if (TextureSizeYItem != null)
            {
                if (!Int32.TryParse(TextureSizeYItem.Content.ToString(), out textureSizeY))
                    textureSizeY = 32;
            }

            TechneModel techneModel = new TechneModel
                                      {
                                          Author = "ZeuX",
                                          HasChanged = false,
                                          ProjectName = modelName == "" ? "New" : modelName,
                                          Name = modelName == "" ? "New" : modelName,
                                          ProjectType = (ProjectType)ProjectType,
                                          Models = new List<SaveModel>
                                                   {
                                                       new SaveModel
                                                       {
                                                           BaseClass = baseClass,
                                                           TextureSize = new Vector(textureSizeX, textureSizeY),
                                                       }
                                                   }
                                      };

            mainWindow.ClearModel();
            mainWindow.OpenModel(techneModel);

            RaiseCloseRequest(true);
        }

        public void ExecuteSelectBaseClassCommand(Object o)
        {
            var service = mainWindow.openFileService;

            var contains = false;

            if (!String.IsNullOrEmpty(service.InitialDirectory) && !String.IsNullOrWhiteSpace(service.InitialDirectory))
            {
                var pathChars = Path.GetInvalidPathChars();

                foreach (var item in pathChars)
                {
                    if (service.InitialDirectory.Contains(item))
                    {
                        contains = true;
                        break;
                    }
                }
            }

            if (!contains && !String.IsNullOrEmpty(service.InitialDirectory))
            {
                var path = Path.GetFullPath(service.InitialDirectory);

                if (Directory.Exists(path))
                    service.InitialDirectory = Path.GetFullPath(service.InitialDirectory);
                else
                    service.InitialDirectory = "";
            }
            else
            {
                //service.InitialDirectory = System.IO.Path.GetFullPath("");
            }

            //service.FileName = "Techne Model | *tcn; *.zip";
            service.Filter = "Techne Model | *tcn; *.zip";

            var result = service.ShowDialog(null);

            if (result.HasValue && result.Value)
            {
                BaseClasses.Insert(4,
                                   new ComboBoxItem
                                   {
                                       Content = service.FileName
                                   });
                SelectedBaseClass = BaseClasses[4];
                NotifyPropertyChanged("SelectedBaseClass");
            }
        }
    }
}

