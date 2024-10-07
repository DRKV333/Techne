/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System.ComponentModel;
using HelixToolkit;
using Techne.Model.Settings;

namespace Techne.Model
{
    public class HelixViewSettings : SettingsModelBase
    {
        public HelixViewSettings(Properties.Settings settings) : base(settings)
        {
        }

        [Category("Advanced")]
        [DisplayName("Camera Inertia  Factor")]
        [DescriptionAttribute("")]
        public double CameraInertiaFactor
        {
            get { return settings.CameraInertiaFactor; }
            set { settings.CameraInertiaFactor = value; }
        }

        [CategoryAttribute("Advanced")]
        [DisplayName("Infinite Spin")]
        [DescriptionAttribute("")]
        public bool InfiniteSpin
        {
            get { return settings.InfiniteSpin; }
            set { settings.InfiniteSpin = value; }
        }

        [CategoryAttribute("Camera controls")]
        [DisplayName("Camera Rotation Mode")]
        [DescriptionAttribute("")]
        public CameraRotationMode CameraRotationMode
        {
            get { return settings.CameraRotationMode; }
            set { settings.CameraRotationMode = value; }
        }

        [CategoryAttribute("Camera controls")]
        [DisplayName("Camera Mode")]
        [DescriptionAttribute("")]
        public CameraMode CameraMode
        {
            get { return settings.CameraMode; }
            set { settings.CameraMode = value; }
        }

        [CategoryAttribute("Camera controls")]
        [DisplayName("Show Camera Target")]
        [DescriptionAttribute("")]
        public bool ShowCameraTarget
        {
            get { return settings.ShowCameraTarget; }
            set { settings.ShowCameraTarget = value; }
        }

        //[CategoryAttribute("Advanced")]
        //[DisplayName("Model Up Direction")]
        //[DescriptionAttribute("")]
        //public global::System.Windows.Media.Media3D.Vector3D ModelUpDirection
        //{
        //    get
        //    {
        //        return settings.ModelUpDirection;
        //    }
        //    set
        //    {
        //        settings.ModelUpDirection = value;
        //    }
        //}

        [CategoryAttribute("Camera controls")]
        [DisplayName("Use Orthographic Camera")]
        [DescriptionAttribute("")]
        public bool Orthographic
        {
            get { return settings.Orthographic; }
            set { settings.Orthographic = value; }
        }

        [CategoryAttribute("Camera controls")]
        [DisplayName("Rotation Sensitivity")]
        [DescriptionAttribute("")]
        public double RotationSensitivity
        {
            get { return settings.RotationSensitivity; }
            set { settings.RotationSensitivity = value; }
        }

        [CategoryAttribute("Camera controls")]
        [DisplayName("Show Coordinate System")]
        [DescriptionAttribute("")]
        public bool ShowCoordinateSystem
        {
            get { return settings.ShowCoordinateSystem; }
            set { settings.ShowCoordinateSystem = value; }
        }

        [CategoryAttribute("Display")]
        [DisplayName("Show FOV")]
        [DescriptionAttribute("")]
        public bool ShowFieldOfView
        {
            get { return settings.ShowFieldOfView; }
            set { settings.ShowFieldOfView = value; }
        }

        [CategoryAttribute("Display")]
        [DisplayName("Show FrameRate")]
        [DescriptionAttribute("")]
        public bool ShowFrameRate
        {
            get { return settings.ShowFrameRate; }
            set { settings.ShowFrameRate = value; }
        }

        [CategoryAttribute("Display")]
        [DisplayName("Show View-Cube")]
        [DescriptionAttribute("")]
        public bool ShowViewCube
        {
            get { return settings.ShowViewCube; }
            set { settings.ShowViewCube = value; }
        }
    }
}

