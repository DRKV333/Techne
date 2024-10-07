/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows.Markup;
//using System.Windows.Data;
//using System.Globalization;
//using System.Windows;
//using System.Windows.Media.Media3D;
//using System.Windows.Media.Imaging;

//namespace Techne.ValueConverter
//{
//    ////[ValueConversion(typeof(Visual3D), typeof(Rect))]
//    //public class ViewBoxConverter : IValueConverter
//    //{
//    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//    //    {
//    //        var visual = (value as IMinecraftVisual);

//    //        if (visual == null)
//    //            return new Rect();

//    //        return McModeler.Helper.TextureHelper.GetViewboxRect(visual);
//    //    }

//    //    public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
//    //    {
//    //        throw new NotImplementedException();
//    //    }
//    //}

//    public class ViewBoxConverter : MarkupExtension, IMultiValueConverter
//    {
//        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
//        {
//            if (values == null || values.Length != 4 || double.IsNaN((double)values[2]) || double.IsNaN((double)values[3]))
//                return new Rect();

//            var param = (string)parameter;
//            var visual = (values[0] as MinecraftCubeVisual3D);
//            var texture = (values[1] as BitmapImage);
//            var imageHeight = (double)values[2];
//            var imageWidth = (double)values[3];

//            if (visual == null || texture == null)
//                return new Rect();

//            //var rect = McModeler.Helper.TextureHelper.GetViewboxRect(visual);

//            var rect = TextureHelper.GetViewboxRect(visual, (CubeSide)Enum.Parse(typeof(CubeSide), param));

//            //switch (param.ToUpper())
//            //{
//            //    case "LEFT":
//            //        rect.Offset(TextureHelper.GetOffset(visual, new Vector(imageWidth, imageHeight), CubeSide.Left));
//            //        break;
//            //    case "RIGHT":
//            //        rect.Offset(TextureHelper.GetOffset(visual, new Vector(imageWidth, imageHeight), CubeSide.Right));
//            //        break;
//            //    case "TOP":
//            //        rect.Offset(TextureHelper.GetOffset(visual, new Vector(imageWidth, imageHeight), CubeSide.Top));
//            //        break;
//            //    case "BOTTOM":
//            //        rect.Offset(TextureHelper.GetOffset(visual, new Vector(imageWidth, imageHeight), CubeSide.Bottom));
//            //        break;
//            //    case "FRONT":
//            //        rect.Offset(TextureHelper.GetOffset(visual, new Vector(imageWidth, imageHeight), CubeSide.Front));
//            //        break;
//            //    case "BACK":
//            //        rect.Offset(TextureHelper.GetOffset(visual, new Vector(imageWidth, imageHeight), CubeSide.Back));
//            //        break;
//            //    default:
//            //        break;
//            //}

//            rect.Scale(imageWidth / texture.PixelWidth, imageHeight / texture.PixelHeight);

//            return rect;
//        }

//        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }

//        public override object ProvideValue(IServiceProvider serviceProvider)
//        {
//            return new ViewBoxConverter();
//        }
//    }
//}


