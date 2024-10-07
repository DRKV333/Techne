/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Net;
using Cinch;
using Microsoft.Windows.Controls;

namespace Techne.ViewModel
{
    public class FeedbackViewModel : ViewModelBase
    {
        public FeedbackViewModel()
        {
            SendFeedbackCommand = new SimpleCommand<object, object>(ExecuteSendFeedbackCommand);
        }

        public String Name
        {
            get;
            set;
        }

        public String Contact
        {
            get;
            set;
        }

        public String FeedbackText
        {
            get;
            set;
        }

        public SimpleCommand<object, object> SendFeedbackCommand
        {
            get;
            set;
        }

        private void ExecuteSendFeedbackCommand(Object obj)
        {
            if (FeedbackText != null && FeedbackText.Length != 0)
            {
                try
                {
                    WebClient wc = new WebClient();
                    wc.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                    wc.Headers.Add(HttpRequestHeader.Accept, "*/*");
                    wc.UploadString("http://techne.apphb.com/feedback/new", "name=" + Name + "&contact=" + Contact + "&text=" + FeedbackText);

                    RaiseCloseRequest(true);
                }
                catch
                {
                    MessageBox.Show("Could not send feedback");
                }
            }
        }
    }
}

