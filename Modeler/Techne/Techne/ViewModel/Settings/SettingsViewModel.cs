/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using Cinch;
using Techne.Manager;
using Techne.Model.Settings;
using WPG;

namespace Techne
{
    internal class SettingsViewModel : ViewModelBase
    {
        private readonly SettingsManager settingsManager;
        //private DispatcherNotifiedObservableCollection<object> settingCollection;
        private List<PropertyTabContent> settingCollection;

        internal SettingsViewModel(SettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;

            //settingCollection = new DispatcherNotifiedObservableCollection<dynamic>();
            settingCollection = new List<PropertyTabContent>();

            foreach (var property in settingsManager.GetType().GetProperties())
            {
                try
                {
                    var attribute = Attribute.GetCustomAttribute(property, typeof(SettingCollectionAttribute)) as SettingCollectionAttribute;

                    if (attribute == null)
                        continue;

                    var propertyGrid = new PropertyGrid();
                    propertyGrid.Instance = property.GetValue(settingsManager, null);

                    settingCollection.Add(new PropertyTabContent
                                          {
                                              PropertyGrid = propertyGrid,
                                              Attribute = attribute
                                          });
                }
                catch (Exception)
                {
                }
            }

            SaveCommand = new SimpleCommand<object, object>(ExecuteSaveCommand);
            ResetCommand = new SimpleCommand<object, object>(ExecuteResetCommand);
        }

        //private PropertyGrid propertyGrid;

        //public PropertyGrid PropertyGrid
        //{
        //    get { return propertyGrid; }
        //    set { propertyGrid = value; }
        //}

        public List<PropertyTabContent> SettingCollection
        {
            get { return settingCollection; }
            set { settingCollection = value; }
        }

        public SimpleCommand<Object, Object> SaveCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> ResetCommand
        {
            get;
            private set;
        }

        //public List<KeyValuePair<object, SettingCollectionAttribute>> SettingCollection
        //{
        //    get { return settingCollection; }
        //    set { settingCollection = value; }
        //}

        private void ExecuteSaveCommand(Object obj)
        {
            settingsManager.Save();
            RaiseCloseRequest(true);
        }

        private void ExecuteResetCommand(Object obj)
        {
            settingsManager.Reload();
        }

        public override void RaiseCloseRequest(bool? dialogResult)
        {
            settingsManager.Reload();
            base.RaiseCloseRequest(dialogResult);
        }
    }
}

