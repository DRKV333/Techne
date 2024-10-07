/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cinch;

namespace Techne.Manager
{
    public class BackupManager
    {
        private static Queue<string> lastSave;
        private static readonly string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Techne");
        private Timer timer;

        public string BackupPath
        {
            get { return path; }
        }


        internal void Start(MainWindowViewModel mainWindowViewModel)
        {
            timer = new Timer(Backup, mainWindowViewModel, MainWindowViewModel.settingsManager.TechneSettings.AutoSaveIntervall * 1000, MainWindowViewModel.settingsManager.TechneSettings.AutoSaveIntervall * 1000);
            lastSave = new Queue<string>();
        }

        private static void Backup(Object state)
        {
            try
            {
                if (!UISynchronizationContext.Instance.InvokeRequired)
                {
                    DoBackup(state);
                }
                else
                {
                    UISynchronizationContext.Instance.InvokeAndBlockUntilCompletion(() => { DoBackup(state); });
                }
            }
            catch
            {
            }
        }

        private static void DoBackup(object state)
        {
            var mainWindowViewModel = (MainWindowViewModel)state;

            string filename = "Backup-" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH''mm'-'ss.fffffff");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string model = Path.Combine(path, filename + ".tcn");

            mainWindowViewModel.SaveModel(model, true);
            lastSave.Enqueue(model);

            if (lastSave.Count > 2)
            {
                File.Delete(lastSave.Dequeue());
            }
        }
    }
}

