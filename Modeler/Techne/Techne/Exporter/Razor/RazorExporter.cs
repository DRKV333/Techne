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
using System.Xml.Linq;
using Techne.Models;
using Techne.Plugins.Interfaces;

namespace Techne.Exporter.Razor
{
    internal class RazorExporter : IExportPlugin
    {
        private readonly List<string> defaultExtension;
        private readonly string filter;
        private readonly string generalExtension;
        private readonly List<string> prefix;
        private readonly List<string> template;

        public RazorExporter(string file)
        {
            XDocument doc = XDocument.Load(file);

            var x = (from c in doc.Descendants("Template")
                     let firstOrDefault = c.Descendants("Prefix").FirstOrDefault()
                     let orDefault = c.Descendants("Content").FirstOrDefault()
                     let xElement = c.Descendants("DefaultExtension").FirstOrDefault()
                     select new
                            {
                                DefaultExtension = xElement != null ? xElement.Value : "",
                                Template = orDefault != null ? orDefault.Value : "",
                                Prefix = firstOrDefault != null ? firstOrDefault.Value : "",
                            }).ToList();

            var y = (from c in doc.Descendants("Info")
                     let @default = c.Descendants("Filter").FirstOrDefault()
                     let element = c.Descendants("DefaultExtension").FirstOrDefault()

                     select new
                            {
                                Filter = @default != null ? @default.Value : "",
                                DefaultExtension = element != null ? element.Value : "",
                            }).FirstOrDefault();

            if (y != null)
            {
                filter = y.Filter;
                generalExtension = y.DefaultExtension;
            }

            if (x != null)
            {
                defaultExtension = new List<string>(x.Count);
                template = new List<string>(x.Count);
                prefix = new List<string>(x.Count);

                foreach (var item in x)
                {
                    defaultExtension.Add(item.DefaultExtension);
                    template.Add(item.Template);
                    prefix.Add(item.Prefix);
                }
            }
        }

        #region IExportPlugin Members
        public void Export(string filename, IEnumerable<ITechneVisual> visuals, Dictionary<string, IShapePlugin> shapes, TechneModel techneModel)
        {
            var path = Path.GetDirectoryName(filename);
            var name = Path.GetFileName(filename);

            try
            {
                for (int i = 0; i < template.Count; i++)
                {
                    var result = RazorEngine.Razor.Parse(template[i], techneModel.Models.FirstOrDefault());
                    File.WriteAllText(Path.Combine(path, prefix[i] + name), result);
                }
            }
            catch (RazorEngine.Templating.TemplateCompilationException exception)
            {
                try
                {
                    ExceptionXElement elem = new ExceptionXElement(exception);

                    string s = elem.ToString();
                    s += "\r\n======= Errors ========\r\n" + string.Join("\r\n", exception.Errors.Select(c => c.ToString()));

                    if (MainWindowViewModel.settingsManager.TechneSettings.AutoReportErrors)
                    {
                        App.ReportError(s);
                    }
                    else
                    {
                        var result = MessageBox.Show("Now that isnt't good, the exporter threw an exception\r\nDo you want to help me fix the bug? Just click \"Yes\"\r\nand send us the crash-report\r\nYou can find the data that's being sent in\r\n%localappdata%\\Techne",
                                                     "Send Crash-Report?",
                                                     MessageBoxButton.YesNo,
                                                     MessageBoxImage.Question,
                                                     MessageBoxResult.Yes);

                        if (result == MessageBoxResult.Yes)
                            App.ReportError(s);
                    }
                }
                catch
                {
                }
            }
        }

        public Guid Guid
        {
            get { throw new NotImplementedException(); }
        }

        public void OnLoad()
        {
            throw new NotImplementedException();
        }

        public string DefaultExtension
        {
            get { return generalExtension; }
        }

        public string Filter
        {
            get { return filter; }
        }

        public string MenuHeader
        {
            get { return ""; }
        }
        #endregion
    }
}

