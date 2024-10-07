/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using Techne.Plugins.Attributes;
using Techne.Plugins.Interfaces;
using Techne.Plugins.ValueConverter;
using Techne.ValueConverters;

namespace Techne.Plugins.Shapes
{
    [PluginExport("Turbo-Model-Thingy Cube-Visual3D", "0.1", "ZeuX", "Adds Support for TMT-Cubes", "DE81AA14-BD60-4228-8D8D-5238BCD3CAAA")]
    public class TurboModelThingyCubeVisual3DPlugin : IShapePlugin
    {
        public PluginType PluginType
        {
            get { return PluginType.Shape; }
        }

        #region IShapePlugin Members
        /// <summary>
        /// Gets the icon.
        /// </summary>
        public BitmapSource Icon
        {
            get
            {
                Assembly myAssembly = Assembly.GetExecutingAssembly();

                using (Stream stream = myAssembly.GetManifestResourceStream("Techne.Plugins.Shapes.Resources.AddCube.png"))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();

                    return bitmap;
                }

                //return new BitmapImage();
            }
        }

        public Guid Guid
        {
            get { return new Guid("DE81AA14-BD60-4228-8D8D-5238BCD3CAAA"); }
        }

        /// <summary>
        /// Gets the alt text.
        /// </summary>
        public String AltText
        {
            get { return "TMT-Cube"; }
        }

        public ITechneVisual CreateVisual(string name = "New Shape",
                                          Vector3D size = new Vector3D(),
                                          float scale = 0,
                                          Vector3D position = new Vector3D(),
                                          Vector3D offset = new Vector3D(),
                                          Vector textureOffset = new Vector(),
                                          Vector3D rotation = new Vector3D(),
                                          bool isMirrored = false,
                                          bool isDecorative = false,
                                          Dictionary<string, MetadataBase> metadata = null)
        {
            if (size.X <= 0)
                size.X = 1;
            if (size.Y <= 0)
                size.Y = 1;
            if (size.Z <= 0)
                size.Z = 1;

            if (metadata == null)
                metadata = new Dictionary<string, MetadataBase>();

            return new TurboModelThingyCubeVisual3D
                   {
                       Rotation = rotation,
                       IsDecorative = isDecorative,
                       IsMirrored = isMirrored,
                       TextureOffset = textureOffset,
                       Size = size,
                       Scale = scale,
                       Position = position,
                       Offset = offset,
                       Name = name,
                       Metadata = metadata
                   };
        }

        /// <summary>
        /// Creates the visual.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public ITechneVisual CreateVisual(Rect3D size)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the visual.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        /// <param name="height">The height.</param>
        /// <param name="widht">The widht.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public ITechneVisual CreateVisual(double x, double y, double z, double height, double widht, double length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when plugin is loaded
        /// </summary>
        public void OnLoad()
        {
            //nothing to do here, maybe load the icon?
        }

        public Panel GetTextureViewerOverlay(ITechneVisual visual, bool isSelectedVisual)
        {
            Grid result = new Grid();
            result.Background = new SolidColorBrush(Color.FromRgb(50, 50, 50));
            result.VerticalAlignment = VerticalAlignment.Top;
            result.HorizontalAlignment = HorizontalAlignment.Left;

            Binding heightBinding = new Binding("Height");
            Binding lengthBinding = new Binding("Length");
            Binding widthBinding = new Binding("Width");
            Binding visualSizeBinding = new Binding("Size");
            Binding textureSizeBinding = new Binding("TextureSize");
            Binding textureCoordinateBinding = new Binding("TextureCoordinates");

            heightBinding.Source = visual;
            lengthBinding.Source = visual;
            visualSizeBinding.Source = visual;
            widthBinding.Source = visual;
            textureSizeBinding.Source = visual;
            textureCoordinateBinding.Source = visual;

            //MultiBinding multibinding = new MultiBinding();

            //multibinding.Bindings.Add(heightBinding);
            //multibinding.Bindings.Add(lengthBinding);
            //multibinding.Bindings.Add(widthBinding);

            //these two bindings -> nirvana
            MultiBinding gridWidthBinding = new MultiBinding();
            gridWidthBinding.Bindings.Add(heightBinding);
            gridWidthBinding.Bindings.Add(widthBinding);
            gridWidthBinding.ConverterParameter = 2;
            gridWidthBinding.Converter = new DoubleAdderConverter();

            MultiBinding gridHeightBinding = new MultiBinding();
            gridHeightBinding.Bindings.Add(lengthBinding);
            gridHeightBinding.Bindings.Add(heightBinding);
            gridHeightBinding.Converter = new DoubleAdderConverter();

            result.SetBinding(FrameworkElement.HeightProperty, gridHeightBinding);
            result.SetBinding(FrameworkElement.WidthProperty, gridWidthBinding);
            result.Opacity = 0.5;


            Rectangle leftSide = GetBoundRectangle(lengthBinding, heightBinding, textureSizeBinding, visualSizeBinding, CubeSide.Left, widthBinding);
            leftSide.Fill = new SolidColorBrush(Colors.Red);
            Rectangle rightSide = GetBoundRectangle(lengthBinding, heightBinding, textureSizeBinding, visualSizeBinding, CubeSide.Right, widthBinding);
            rightSide.Fill = new SolidColorBrush(Colors.Red);

            Rectangle topSide = GetBoundRectangle(heightBinding, widthBinding, textureSizeBinding, visualSizeBinding, CubeSide.Top, lengthBinding);
            topSide.Fill = new SolidColorBrush(Colors.Green);
            Rectangle bottomSide = GetBoundRectangle(heightBinding, widthBinding, textureSizeBinding, visualSizeBinding, CubeSide.Bottom, lengthBinding);
            bottomSide.Fill = new SolidColorBrush(Colors.Green);

            Rectangle frontSide = GetBoundRectangle(lengthBinding, widthBinding, textureSizeBinding, visualSizeBinding, CubeSide.Front, heightBinding);
            frontSide.Fill = new SolidColorBrush(Colors.Blue);
            Rectangle backSide = GetBoundRectangle(lengthBinding, widthBinding, textureSizeBinding, visualSizeBinding, CubeSide.Back, heightBinding);
            backSide.Fill = new SolidColorBrush(Colors.Blue);


            Binding marginBinding = new Binding("TextureOffset");
            marginBinding.Source = visual;
            marginBinding.Converter = new VectorThicknessConverter();

            result.SetBinding(FrameworkElement.MarginProperty, marginBinding);

            result.Children.Add(leftSide);
            result.Children.Add(rightSide);
            result.Children.Add(topSide);
            result.Children.Add(bottomSide);
            result.Children.Add(frontSide);
            result.Children.Add(backSide);

            return result;
        }
        #endregion

        private Rectangle GetBoundRectangle(Binding overlayHeightBinding, Binding overlayWidthBinding, Binding textureSizeBinding, Binding textureCoordinateBinding, CubeSide cubeSide, Binding updaterBinding)
        {
            MultiBinding marginBinding = new MultiBinding();
            marginBinding.Bindings.Add(textureCoordinateBinding);
            marginBinding.Bindings.Add(textureSizeBinding);
            marginBinding.Bindings.Add(updaterBinding);
            marginBinding.ConverterParameter = cubeSide;
            marginBinding.Converter = new TexturePartMarginConverter();

            Rectangle rectangle = new Rectangle();
            rectangle.HorizontalAlignment = HorizontalAlignment.Left;
            rectangle.VerticalAlignment = VerticalAlignment.Top;
            rectangle.SetBinding(FrameworkElement.HeightProperty, overlayHeightBinding);
            rectangle.SetBinding(FrameworkElement.WidthProperty, overlayWidthBinding);
            rectangle.SetBinding(FrameworkElement.MarginProperty, marginBinding);
            //rectangle.Margin = new System.Windows.Thickness(0, 0, 0, 0);

            return rectangle;
        }
    }
}

