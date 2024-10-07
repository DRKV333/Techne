/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System.ComponentModel;
using Techne.Models.Enums;

namespace Techne.Model.Settings
{
    public class TechneSettings : SettingsModelBase
    {
        public TechneSettings(Properties.Settings settings) : base(settings)
        {
        }

        [Category("Selected Shape")]
        [DisplayName("Focus Shape")]
        [DescriptionAttribute("")]
        public bool FocusSelectedShape
        {
            get { return settings.FocusSelectedShape; }
            set { settings.FocusSelectedShape = value; }
        }

        [CategoryAttribute("Selected Shape")]
        [DisplayName("Follow Shape")]
        [DescriptionAttribute("")]
        public bool FollowSelectedShape
        {
            get { return settings.FollowSelectedShape; }
            set { settings.FollowSelectedShape = value; }
        }

        [CategoryAttribute("Selected Shape")]
        [DisplayName("Anchorpoint Radius")]
        [DescriptionAttribute("")]
        public double OffsetSphereRadius
        {
            get { return settings.OffsetSphereRadius; }
            set { settings.OffsetSphereRadius = value; }
        }

        [CategoryAttribute("Techne")]
        [DisplayName("Open New Project Window on startup")]
        [DescriptionAttribute("")]
        public bool ShowNewProjectDialogOnStartup
        {
            get { return settings.ShowNewProjectDialogOnStartup; }
            set { settings.ShowNewProjectDialogOnStartup = value; }
        }

        [CategoryAttribute("Techne")]
        [DisplayName("Use Radians")]
        [DescriptionAttribute("")]
        public bool UseRadians
        {
            get { return settings.UseRadians; }
            set { settings.UseRadians = value; }
        }

        //[CategoryAttribute("Techne")]
        //[DisplayName("Show console as overlay")]
        //[DescriptionAttribute("")]
        public bool ShowConsoleQuakeStyle
        {
            get { return settings.ShowConsoleQuakeStyle; }
            set { settings.ShowConsoleQuakeStyle = value; }
        }

        [CategoryAttribute("Techne")]
        [DisplayName("Auto report crash-reports")]
        [DescriptionAttribute("")]
        public bool AutoReportErrors
        {
            get { return settings.AutoReportErrors; }
            set { settings.AutoReportErrors = value; }
        }

        [CategoryAttribute("Techne")]
        [DisplayName("Decoration Opacity")]
        [DescriptionAttribute("")]
        public double DecorationOpacity
        {
            get { return settings.DecorationOpacity; }
            set { settings.DecorationOpacity = value; }
        }

        [CategoryAttribute("Techne")]
        [DisplayName("Autosave Intervall")]
        [DescriptionAttribute("")]
        public int AutoSaveIntervall
        {
            get { return settings.AutoSaveIntervall; }
            set { settings.AutoSaveIntervall = value; }
        }

        [CategoryAttribute("Texture Viewer")]
        [DisplayName("Hide texture-overlay for non-selected shapes")]
        [DescriptionAttribute("When true, shows only overlay for selected box")]
        public bool DisplaySingleOverlay
        {
            get { return settings.DisplaySingleOverlay; }
            set { settings.DisplaySingleOverlay = value; }
        }

        [CategoryAttribute("Texture Viewer")]
        [DisplayName("Show Texture Captions")]
        [DescriptionAttribute("Texture Captions are the letters in the texture viewer")]
        public TextureCaptionType ShowTextureCaptions
        {
            get { return (TextureCaptionType)settings.TextureCaptionModus; }
            set { settings.TextureCaptionModus = (int)value; }
        }

        [CategoryAttribute("Texture Viewer")]
        [DisplayName("Export Texture Captions")]
        [DescriptionAttribute("When true captions will be exported with the texture map")]
        public bool ExportTextureCaptions
        {
            get { return settings.ExportTextureCaptions; }
            set { settings.ExportTextureCaptions = value; }
        }

        [CategoryAttribute("Techne")]
        [DisplayName("Show texture-resolution mismatch warning")]
        [DescriptionAttribute("Shows a warning when you are trying to load an image that has a different resolution than your project's.\r\nIf false: wont change your texture-settings")]
        public bool AskTextureResolution
        {
            get { return settings.AskTextureResolution; }
            set { settings.AskTextureResolution = value; }
        }

        [CategoryAttribute("Selected Shape")]
        [DisplayName("Show Anchor Point")]
        [DescriptionAttribute("If false the anchor point will not be shown (the blue sphere)")]
        public bool ShowAnchorPoint
        {
            get
            {
                return settings.ShowAnchorPoint;
            }
            set
            {
                settings.ShowAnchorPoint = value;
            }
        }
        [CategoryAttribute("Selected Shape")]
        [DisplayName("Show wireframe")]
        [DescriptionAttribute("Will hide wireframe if false")]
        public bool ShowWireFrame
        {
            get
            {
                return settings.ShowWireFrame;
            }
            set
            {
                settings.ShowWireFrame = value;
            }
        }
    }
}

