/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using Techne.Model;
using Techne.Model.Settings;
using Techne.Properties;

namespace Techne.Manager
{
    public class SettingsManager
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private readonly Settings settings = new Settings();

        internal SettingsManager(MainWindowViewModel mainWindowViewModel)
        {
            foreach (var property in GetType().GetProperties())
            {
                var attribute = Attribute.GetCustomAttribute(property, typeof(SettingCollectionAttribute)) as SettingCollectionAttribute;

                if (attribute == null)
                    continue;

                var propertyType = property.PropertyType;
                var constructor = propertyType.GetConstructors();

                if (constructor == null || constructor.Length == 0)
                    continue;

                var obj = constructor[0].Invoke(new object[] { settings });

                property.SetValue(this, obj, null);
            }
            //HelixViewSettings = new HelixViewSettings(settings);

            this.mainWindowViewModel = mainWindowViewModel;
        }

        public static SettingsManager Manager
        {
            get;
            set;
        }

        public Settings Settings
        {
            get { return settings; }
        }

        [SettingCollectionAttribute("Techne Settings", "")]
        public TechneSettings TechneSettings
        {
            get;
            private set;
        }

        [SettingCollectionAttribute("Viewport 3D settings", "")]
        public HelixViewSettings HelixViewSettings
        {
            get;
            private set;
        }

        public Version Version
        {
            get { return new Version(settings.VersionNumber); }
            set { settings.VersionNumber = value.ToString(); }
        }

        public void Save()
        {
            settings.Save();
            mainWindowViewModel.ReloadConfiguration();
        }

        public void Reset()
        {
            settings.Reset();
        }

        internal void Reload()
        {
            settings.Reload();
        }
    }
}

