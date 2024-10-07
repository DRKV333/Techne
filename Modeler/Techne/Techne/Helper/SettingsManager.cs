/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Deployment.Application;

namespace Techne
{
    public static class SettingsManager
    {
        internal static SettingsViewModel GetViewModel()
        {
            SettingsViewModel vm = new SettingsViewModel();

            //CheckPath();
            PopulateSettings(vm);

            return vm;
        }

        private static void PopulateSettings(SettingsViewModel vm)
        {
            var properties = typeof(SettingsViewModel).GetProperties();
            var settingsProperties = typeof(Properties.Settings).GetProperties();

            foreach (var item in properties)
            {
                var property = settingsProperties.Where(x => x.Name == item.Name).FirstOrDefault();

                if (property == null)
                    continue;

                item.GetSetMethod().Invoke(vm, new object[] { property.GetGetMethod().Invoke(Properties.Settings.Default, null)});
            }
        }

        private static void CheckPath()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string userFilePath = Path.Combine(localAppData, "MyCompany");

            if (!Directory.Exists(userFilePath))
                Directory.CreateDirectory(userFilePath);

            string sourceFilePath;

            try
            {
                sourceFilePath = Path.Combine(ApplicationDeployment.CurrentDeployment.DataDirectory, "MyFile.txt");
            }
            catch
            {
                sourceFilePath = "./myfile.txt";
            }

            string destFilePath = Path.Combine(userFilePath, "MyFile.txt");

            if (!File.Exists(destFilePath))
                File.Copy(sourceFilePath, destFilePath);
        }
    }
}

