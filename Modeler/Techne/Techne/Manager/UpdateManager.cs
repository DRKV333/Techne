/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Net;

namespace Techne.Manager
{
    public class UpdateManager
    {
        private readonly MainWindowViewModel mainWindowViewModel;

        public UpdateManager(MainWindowViewModel mainWindowViewModel)
        {
            // TODO: Complete member initialization
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public Version LatestVersion
        {
            get;
            set;
        }

        public event EventHandler NewVersionAvailableEvent;

        internal void CheckForUpdates(Version version, bool isDeployed)
        {
            try
            {
                //HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create("http://bit.ly/ktecsn");
                //request.BeginGetResponse(new AsyncCallback(GotResponse), this);

                WebClient wc = new WebClient();
                wc.Headers = CreateHeaders();
                wc.DownloadStringCompleted += wc_DownloadStringCompleted;

                try
                {
                    if (isDeployed)
                    {
                        wc.DownloadStringAsync(new Uri("http://bit.ly/TechneVersion"));
                    }
                    else
                    {
                        //wc.DownloadStringAsync(new Uri("http://bit.ly/TechneVersion"));
                        wc.DownloadStringAsync(new Uri("http://dl.dropbox.com/u/15368593/Minecraft/Techne/Version/CurrentVersion.txt"));
                    }
                }
                catch
                {
                }
            }
            catch
            {
            }
        }

        private WebHeaderCollection CreateHeaders()
        {
            var collection = new WebHeaderCollection();
            collection.Add(HttpRequestHeader.Host, "bit.ly");
            collection.Add(HttpRequestHeader.Referer, "http://www.minecraftforum.net/viewtopic.php?f=1022&t=217348");
            collection.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.65 Safari/534.24");
            collection.Add(HttpRequestHeader.Accept, "application/xml,application/xhtml+xml,text/html;q=0.9,text/plain");
            collection.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8");
            collection.Add(HttpRequestHeader.AcceptCharset, "ISO-8859-1,utf-8;q=0.7,*;q=0.3");

            return collection;
        }

        //void GotResponse(Object o)
        //{
        //    //var oa = (o as System.Net.ContextAwareResult);
        //}

        private void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(e.Result))
                {
                    Version remoteVersion = new Version(e.Result);

                    LatestVersion = remoteVersion;

                    if (remoteVersion.CompareTo(MainWindowViewModel.settingsManager.Version) > 0)
                    {
                        mainWindowViewModel.NewVersionAvailable(GetChangelog(MainWindowViewModel.IsDeployed));
                    }
                }
            }
            catch
            {
            }
        }

        internal ChangelogViewModel GetChangelog(bool isDeployed)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Headers = CreateHeaders();
                string result;
                //http://bit.ly/lY3UiU
                if (isDeployed)
                    result = wc.DownloadString("http://bit.ly/TechneChangelog");
                else
                    result = wc.DownloadString("http://dl.dropbox.com/u/15368593/Minecraft/Techne/Version/changelog.xml");

                return new ChangelogViewModel(result);
            }
            catch
            {
                return null;
            }
        }
    }
}

