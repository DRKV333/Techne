/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace Techne
{
    public class TechneRotation3DAnimation : AnimationTimeline
    {
        private Vector3D axisDuration;
        //RotateTransform3D rotation;

        public override Type TargetPropertyType
        {
            get { return typeof(QuaternionRotation3D); }
        }

        public Vector3D From
        {
            get;
            set;
        }

        public Vector3D To
        {
            get;
            set;
        }

        public Vector3D Default
        {
            get;
            set;
        }

        public Vector3D AxisDuration
        {
            get { return axisDuration; }
            set
            {
                //axisDuration = value;
                var seconds = Math.Max(Math.Max(Double.IsInfinity(value.X) || Double.IsNaN(value.X) ? 0 : value.X, Double.IsInfinity(value.Y) || Double.IsNaN(value.Y) ? 0 : value.Y), Double.IsInfinity(value.Z) || Double.IsNaN(value.Z) ? 0 : value.Z);
                Duration = new Duration(new TimeSpan(0, 0, (int)seconds));
                axisDuration = new Vector3D(seconds / (Double.IsInfinity(value.X) ? 0 : value.X), seconds / (Double.IsInfinity(value.Y) ? 0 : value.Y), seconds / (Double.IsInfinity(value.Z) ? 0 : value.Z));
            }
        }

        public Vector3D Position
        {
            get;
            set;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new TechneRotation3DAnimation
                   {
                       From = From,
                       To = To,
                       AxisDuration = AxisDuration,
                       Default = Default
                   };
        }

        public override Object GetCurrentValue(Object defaultOriginValue, Object defaultDestinationValue, AnimationClock animationClock)
        {
            var rotationX = CalculateRotation(animationClock.CurrentProgress.Value, From.X, To.X, Default.X, AxisDuration.X);
            var rotationY = CalculateRotation(animationClock.CurrentProgress.Value, From.Y, To.Y, Default.Y, AxisDuration.Y);
            var rotationZ = CalculateRotation(animationClock.CurrentProgress.Value, From.Z, To.Z, Default.Z, AxisDuration.Z);

            rotationX = Double.IsNaN(rotationX) ? 0 : rotationX;
            rotationY = Double.IsNaN(rotationY) ? 0 : rotationY;
            rotationZ = Double.IsNaN(rotationZ) ? 0 : rotationZ;

            //System.Diagnostics.Debug.Write(animationClock.CurrentProgress.Value);
            //System.Diagnostics.Debug.Write(":\t");
            //System.Diagnostics.Debug.Write(rotationX);
            //System.Diagnostics.Debug.Write("\t");
            //System.Diagnostics.Debug.Write(rotationY);
            //System.Diagnostics.Debug.Write("\t");
            //System.Diagnostics.Debug.WriteLine(rotationZ);


            var result = CalculateQuaternion(rotationY, rotationZ, rotationX);
            return new QuaternionRotation3D(result);
        }

        private double CalculateRotation(double time, double from, double to, double defaultAngle, double duration)
        {
            time *= Math.PI * 2 * duration;

            var result = Math.Sin(time) * (from / 2 - to / 2) + defaultAngle;

            //if (!Double.IsNaN(result) && defaultAngle != 0)
            //{
            //    System.Diagnostics.Debug.Write(result);
            //    System.Diagnostics.Debug.Write("\t= Math.Sin(");
            //    System.Diagnostics.Debug.Write(time);
            //    System.Diagnostics.Debug.Write(" / ");
            //    System.Diagnostics.Debug.Write(duration);
            //    System.Diagnostics.Debug.Write(") * ");
            //    System.Diagnostics.Debug.Write(from);
            //    System.Diagnostics.Debug.Write(" - ");
            //    System.Diagnostics.Debug.Write(to);
            //    System.Diagnostics.Debug.Write(" + ");
            //    System.Diagnostics.Debug.Write(defaultAngle);

            //    System.Diagnostics.Debug.Write("\t\t");
            //    System.Diagnostics.Debug.Write(time / duration);
            //    System.Diagnostics.Debug.Write("\t");
            //    System.Diagnostics.Debug.WriteLine(from - to + defaultAngle);
            //}

            if (Double.IsNaN(result) || double.IsInfinity(result))
            {
                result = defaultAngle;
            }

            return result;
        }

        protected virtual Quaternion CalculateQuaternion(double heading, double attitude, double bank)
        {
            heading *= Math.PI / 181;
            attitude *= Math.PI / 181;
            bank *= Math.PI / 181;
            // Assuming the angles are in radians.
            double c1 = Math.Cos(heading);
            double s1 = Math.Sin(heading);
            double c2 = Math.Cos(attitude);
            double s2 = Math.Sin(attitude);
            double c3 = Math.Cos(bank);
            double s3 = Math.Sin(bank);
            var w = Math.Sqrt(1.0 + c1 * c2 + c1 * c3 - s1 * s2 * s3 + c2 * c3) / 2.0;
            double w4 = (4.0 * w);
            var x = (c2 * s3 + c1 * s3 + s1 * s2 * c3) / w4;
            var y = (s1 * c2 + s1 * c3 + c1 * s2 * s3) / w4;
            var z = (-s1 * s3 + c1 * s2 * c3 + s2) / w4;

            return new Quaternion(x, y, z, w);
        }

        private static double easeInOut(double timeFraction, double start, double delta, double bounciness, int bounces)
        {
            double returnValue = 0.0;

            // we cut each effect in half by multiplying the time fraction by two and halving the distance.
            if (timeFraction <= 0.5)
            {
                returnValue = easeIn(timeFraction * 2, start, delta / 2, bounciness, bounces);
            }
            else
            {
                returnValue = easeOut((timeFraction - 0.5) * 2, start, delta / 2, bounciness, bounces);
                returnValue += delta / 2;
            }
            return returnValue;
        }

        private static double easeOut(double timeFraction, double start, double delta, double bounciness, int bounces)
        {
            double returnValue = 0.0;

            // math magic: The cosine gives us the right wave, the timeFraction is the frequency of the wave, 
            // the absolute value keeps every value positive (so it "bounces" off the midpoint of the cosine 
            // wave, and the amplitude (the exponent) makes the sine wave get smaller and smaller at the end.
            returnValue = Math.Abs(Math.Pow((1 - timeFraction), bounciness)
                                   * Math.Cos(2 * Math.PI * timeFraction * bounces));
            returnValue = delta - (returnValue * delta);
            returnValue += start;
            return returnValue;
        }

        private static double easeIn(double timeFraction, double start, double delta, double bounciness, int bounces)
        {
            double returnValue = 0.0;
            // math magic: The cosine gives us the right wave, the timeFraction is the amplitude of the wave, 
            // the absolute value keeps every value positive (so it "bounces" off the midpoint of the cosine 
            // wave, and the amplitude (the exponent) makes the sine wave get bigger and bigger towards the end.
            returnValue = Math.Abs(Math.Pow((timeFraction), bounciness)
                                   * Math.Cos(2 * Math.PI * timeFraction * bounces));
            returnValue = returnValue * delta;
            returnValue += start;
            return returnValue;
        }
    }
}

