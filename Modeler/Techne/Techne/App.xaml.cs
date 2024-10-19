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
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Cinch;
using Techne.Compat;

namespace Techne
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly string crashFile;
        private readonly string path;

        #region Initialisation
        /// <summary>
        /// Initiliase Cinch using the CinchBootStrapper. 
        /// </summary>
        public App()
        {
            try
            {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Techne");
                crashFile = Path.Combine(path, "TechneCrashFile");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch
            {
                MessageBox.Show("Couldn't create workspace folder, that might lead to unexpected behavior.\r\nDe- and reinstalling Tecchne might fix this problem.");
            }

            CinchBootStrapper.Initialise(new List<Assembly>
                                         {
                                             typeof(App).Assembly
                                         });
            UISynchronizationContext.Instance.Initialize();
            InitializeComponent();
        }
        #endregion

        private void DispatcherUnhandledExceptionEventHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception.TargetSite.Name.Equals("EvaluateOldNewStates", StringComparison.InvariantCultureIgnoreCase))
            {
                e.Handled = true;
                return;
            }

            var s = SendErrorReport(e.Exception);

            try
            {
                string filename = "Error-" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH''mm'-'ss.fffffff");
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Techne");
                string log = Path.Combine(path, filename + ".log");
                string model = Path.Combine(path, filename + ".tcn");

                if (!Directory.Exists(Path.GetPathRoot(path)))
                {
                    Directory.CreateDirectory(Path.GetPathRoot(path));
                }

                try
                {
                    if (MainWindow != null)
                    {
                        var vm = (MainWindow.DataContext as MainWindowViewModel);

                        if (vm != null)
                        {
                            vm.SaveModel(model);
                        }
                        MessageBox.Show("Whoops, Techne is about to crash\r\nBut don't worry, we saved your model just in time\r\nHave a look at %localappdata%\\Techne");
                    }
                }
                catch
                {
                }

                try
                {
                    File.WriteAllText(log, s);
                }
                catch
                {
                }
            }
            catch
            {
            }
        }

        public static string SendErrorReport(Exception e, string comment = null)
        {
            ExceptionXElement elem = new ExceptionXElement(e);

            string s = elem.ToString();

            // TODO: Also move this to plugin maybe?
            /*
            if (e is RazorEngine.Templating.TemplateCompilationException)
                s += "\r\n======= Errors ========\r\n" +
                     string.Join("\r\n",
                                 ((RazorEngine.Templating.TemplateCompilationException) e).Errors.Select(
                                     c => c.ToString()));
            */

            if (!String.IsNullOrEmpty(comment))
                s += "\r\n" + comment;

            try
            {
                if (MainWindowViewModel.settingsManager.TechneSettings.AutoReportErrors)
                {
                    ReportError(s);
                }
                else
                {
                    var result =
                        MessageBox.Show(
                            "Do you want to submit the crash-report to us so we can fix the bugs?\r\nThe only data that is going to be sent is the crash-report which you can find in \r\n%localappdata%\\Techne",
                            "Send Crash-Report?",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question,
                            MessageBoxResult.Yes);

                    if (result == MessageBoxResult.Yes)
                        ReportError(s);
                }
            }
            catch
            {
            }
            return s;
        }

        public static void ReportError(string s)
        {
            s = s.Replace("&", "%26");
            WebClient wc = new WebClient();
            wc.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
            wc.Headers.Add(HttpRequestHeader.Accept, "*/*");
            //wc.Headers.Remove(HttpRequestHeader.Expect);
            wc.UploadString("http://techne.apphb.com/Error/Report", "x=" + s);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Check if this was launched by double-clicking a doc. If so, use that as the
            // startup file name.
            try
            {
                if (ActivationArgumentReader.Current.StartupFileName != null)
                {
                    try
                    {
                        string fname = ActivationArgumentReader.Current.StartupFileName;

                        // It comes in as a URI; this helps to convert it to a path.
                        Uri uri = new Uri(fname);
                        fname = uri.LocalPath;

                        Properties["FileName"] = fname;
                    }
                    catch (Exception ex)
                    {
                        // For some reason, this couldn't be read as a URI.
                        // Do what you must...
                    }
                }
            }
            catch
            {
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (File.Exists(crashFile))
            {
                File.Delete(crashFile);
            }

            CleanUpBackups();
            CleanReports();

            base.OnExit(e);
        }

        private void CleanReports()
        {
            foreach (var file in Directory.GetFiles(path, "Error-*.log", SearchOption.TopDirectoryOnly).Where(x => (DateTime.Now - File.GetCreationTime(x)).Days > 7))
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }

        private void CleanUpBackups()
        {
            foreach (var file in Directory.GetFiles(path, "Backup-*.tcn", SearchOption.TopDirectoryOnly).Where(x => (DateTime.Now - File.GetCreationTime(x)).Days > 1))
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }

            foreach (var file in Directory.GetFiles(path, "Error-*.tcn", SearchOption.TopDirectoryOnly).Where(x => (DateTime.Now - File.GetCreationTime(x)).Days > 7))
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }
    }
}

