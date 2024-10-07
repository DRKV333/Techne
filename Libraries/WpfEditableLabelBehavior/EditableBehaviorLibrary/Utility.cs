using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace EditableBehaviorLibrary
{
    public static class Utility
    {
        private const string EDIT_ICON_URI = @"/EditableBehaviorLibrary;component/Images/pen.png";
        private const string SAVE_ICON_URI = @"/EditableBehaviorLibrary;component/Images/check.png";
        private const string CANCEL_ICON_URI = @"/EditableBehaviorLibrary;component/Images/cancel.png";

        public static string EditIconUri
        {
            get { return EDIT_ICON_URI; }
        }

        public static string SaveIconUri
        {
            get { return SAVE_ICON_URI; }
        }

        public static string CancelIconUri
        {
            get { return CANCEL_ICON_URI; }
        }

        public static BitmapImage GetBitmapImage(string uri)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }
}

