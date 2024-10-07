/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Cinch;
using HelixToolkit;
using Techne.Model;
using Techne.Plugins.Interfaces;
using Techne.Visuals;

namespace Techne.Manager
{
    internal class HitTestManager
    {
        #region Fields

        #region Hittest stuff
        /// <summary>
        /// The hit test exclude selection filter callback.
        /// </summary>
        private readonly HitTestFilterCallback hitTestExcludeSelectionFilter;

        private readonly MainWindowViewModel mainWindowViewModel;

        /// <summary>
        /// The mouse pressed hit test result callback.
        /// </summary>
        private readonly HitTestResultCallback mousePressedHitTestResultCallback;

        private RayHitTestResult viewableHitResult;
        #endregion

        //private double changeDelta;
        //private double combinedDelta;
        //private bool isManipulating;
        //private Point3D lastPoint;
        //private double originalPosition;
        //private double originalValue;
        //private Plane3D plane;
        //private ThreeAxisControlVisual3D threeAxisControlVisual;
        #endregion

        public HitTestManager(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            //threeAxisControlVisual = new ThreeAxisControlVisual3D();
            //threeAxisControlVisual.Type = EnumSelectionType.Position;

            hitTestExcludeSelectionFilter = ExcludeSelectionFilter;
            mousePressedHitTestResultCallback = ProcessMousePressedHitTestResult;
        }

        //public ThreeAxisControlVisual3D ThreeAxisControlVisual3D
        //{
        //    get { return threeAxisControlVisual; }
        //}


        /// <summary>
        /// Stores the first viewable hit test result and the dragging sphere hit.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private HitTestResultBehavior ProcessMousePressedHitTestResult(HitTestResult result)
        {
            viewableHitResult = result as RayHitTestResult;

            if (viewableHitResult != null)
            {
                var visual = viewableHitResult.VisualHit as ITechneVisual;

                if (visual != null && visual.IsFixed)
                    return HitTestResultBehavior.Continue;
            }

            return HitTestResultBehavior.Stop;
        }

        /// <summary>
        /// Exclude selections from the hit results.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private HitTestFilterBehavior ExcludeSelectionFilter(DependencyObject target)
        {
            //if (target is ITechneVisual || target is IHelixView3D)
            //    return HitTestFilterBehavior.Continue;
            //else
            //    return HitTestFilterBehavior.ContinueSkipSelf;

            if (target is GridLinesVisual3D || target is TubeVisual3D)
            {
                return HitTestFilterBehavior.ContinueSkipSelf;
            }
            return HitTestFilterBehavior.Continue;
        }


        internal void HitTest(MouseButtonEventArgs mouseArgs, HelixView3D helixView)
        {
            PointHitTestParameters hitParameters = new PointHitTestParameters(mouseArgs.GetPosition(helixView));
            viewableHitResult = null;
            VisualTreeHelper.HitTest(helixView, hitTestExcludeSelectionFilter, mousePressedHitTestResultCallback, hitParameters);

            if (viewableHitResult != null)
            {
                if (viewableHitResult.VisualHit is ITechneVisual)
                {
                    ITechneVisual visual = (ITechneVisual)viewableHitResult.VisualHit;

                    if (visual.IsFixed)
                        return;

                    mainWindowViewModel.ChangeOpacity(1, mainWindowViewModel.TechneModel);

                    if (Keyboard.Modifiers == ModifierKeys.Shift)
                    {
                        mainWindowViewModel.SelectedVisual.ToggleSelection(visual);
                    }
                    else
                    {
                        //threeAxisControlVisual = new ThreeAxisControlVisual3D();
                        mainWindowViewModel.SelectedVisual.SelectVisual(visual);
                    }

                    if (mainWindowViewModel.SelectedVisual.SelectedVisuals.Count > 0)
                        mainWindowViewModel.ChangeOpacity(0.5, mainWindowViewModel.TechneModel);

                    mainWindowViewModel.ShowWireframeAndAnchor();
                }
                else if (viewableHitResult.VisualHit is AxisControlVisual3D)
                {
                    //var visual = (viewableHitResult.VisualHit as AxisControlVisual3D);

                    //if (visual == null)
                    //    return;

                    //visual.Clicked();

                    //plane = new Plane3D(viewableHitResult.PointHit, GetAxisNormal(visual.Axis));
                    //lastPoint = viewableHitResult.PointHit;

                    //isManipulating = true;
                }
            }
            else
            {
                mainWindowViewModel.DeselectVisual();
                //threeAxisControlVisual = null;
            }
            mainWindowViewModel.UpdateTextureOverlay();
        }

/*
        private Vector3D GetAxisNormal(EnumAxis enumAxis)
        {
            switch (enumAxis)
            {
                case EnumAxis.X:
                    return new Vector3D(0, 0, 1);
                    break;
                case EnumAxis.Y:
                    return new Vector3D(1, 0, 0);
                    break;
                case EnumAxis.Z:
                    return new Vector3D(0, 1, 0);
                    break;
                default:
                    return new Vector3D(0, 0, 0);
                    break;
            }
        }
*/

        //internal void MouseMoved()
        //{
        //    if (!isManipulating || mainWindowViewModel.SelectedVisual == null || threeAxisControlVisual == null)
        //        return;

        //    var ray = HelixToolkit.Viewport3DHelper.Point2DtoRay3D((mainWindowViewModel.HelixView as HelixView3D).Viewport, Mouse.GetPosition(mainWindowViewModel.HelixView as HelixView3D));

        //    var point3D = ray.PlaneIntersection(plane.Position, plane.Normal);

        //    if (!point3D.HasValue)
        //        return;

        //    switch (threeAxisControlVisual.Type)
        //    {
        //        case EnumSelectionType.Nothing:
        //            break;
        //        case EnumSelectionType.Position:
        //            ChangePosition(point3D.Value);
        //            break;
        //        case EnumSelectionType.Offset:
        //            break;
        //        case EnumSelectionType.Size:
        //            ChangeSize(point3D.Value);
        //            break;
        //        case EnumSelectionType.TextureOffset:
        //            break;
        //        case EnumSelectionType.Rotation:
        //            break;
        //        default:
        //            break;
        //    }

        //    lastPoint = point3D.Value;
        //}

        //private void ChangeSize(Point3D point3D)
        //{
        //    var size = mainWindowViewModel.SelectedVisual.Size;

        //    switch (threeAxisControlVisual.SelectedAxis)
        //    {
        //        case Techne.Models.Enums.EnumAxis.X:
        //            changeDelta += point3D.X - lastPoint.X;

        //            double sizeX = 0;

        //            if (changeDelta < 1)
        //            {
        //                combinedDelta += changeDelta;
        //            }
        //            else
        //            {
        //                sizeX = Math.Round(combinedDelta, MidpointRounding.AwayFromZero);
        //                combinedDelta -= sizeX;
        //            }

        //            if (threeAxisControlVisual.SelectedAxisDirection == 1)
        //            {
        //                mainWindowViewModel.PositionX += sizeX;
        //                sizeX = sizeX * -1;
        //            }

        //            mainWindowViewModel.Width += sizeX;
        //            break;
        //        case Techne.Models.Enums.EnumAxis.Y:
        //            var sizeY = Math.Round(point3D.Y - plane.Position.Y, MidpointRounding.AwayFromZero);
        //            mainWindowViewModel.Length += sizeY;
        //            size = new Vector3D(size.X, sizeY, size.Z);
        //            break;
        //        case Techne.Models.Enums.EnumAxis.Z:
        //            var sizeZ = Math.Round(point3D.Z - plane.Position.Z, MidpointRounding.AwayFromZero);
        //            mainWindowViewModel.Height += sizeZ;
        //            size = new Vector3D(size.X, size.Y, sizeZ);
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //private void ChangePosition(Point3D point3D)
        //{
        //    var position = mainWindowViewModel.SelectedVisual.Position;

        //    switch (threeAxisControlVisual.SelectedAxis)
        //    {
        //        case Techne.Models.Enums.EnumAxis.X:
        //            var posX = Math.Round(point3D.X - plane.Position.X, MidpointRounding.AwayFromZero);
        //            mainWindowViewModel.PositionX = posX;
        //            position = new Vector3D(posX, position.Y, position.Z);
        //            break;
        //        case Techne.Models.Enums.EnumAxis.Y:
        //            var posY = Math.Round(point3D.Y - plane.Position.Y, MidpointRounding.AwayFromZero);
        //            mainWindowViewModel.PositionY = posY;
        //            position = new Vector3D(position.X, posY, position.Z);
        //            break;
        //        case Techne.Models.Enums.EnumAxis.Z:
        //            var posZ = Math.Round(point3D.Z - plane.Position.Z, MidpointRounding.AwayFromZero);
        //            mainWindowViewModel.PositionZ = posZ;
        //            position = new Vector3D(position.X, position.Y, posZ);
        //            break;
        //        default:
        //            break;
        //    }

        //    position.X += mainWindowViewModel.SelectedVisual.Width / 2;
        //    position.Y += mainWindowViewModel.SelectedVisual.Length / 2;
        //    position.Z += mainWindowViewModel.SelectedVisual.Height / 2;
        //    threeAxisControlVisual.Position = position;
        //}

        internal void MouseButtonUp(MouseButtonEventArgs mouseArgs)
        {
            //if (isManipulating)
            //{
            //    //threeAxisControlVisual = null;
            //    threeAxisControlVisual.Activated = false;
            //    isManipulating = false;
            //    changeDelta = 0;
            //    combinedDelta = 0;
            //}
        }
    }
}

