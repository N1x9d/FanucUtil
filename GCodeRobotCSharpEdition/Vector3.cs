//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Numerics;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Text.Json.Serialization;
//using System.Threading.Tasks;

//namespace GCodeRobotCSharpEdition
//{

//    public struct Vector3 : IEquatable<Vector3>
//    {
//        #region Fields

//        /// <summary>
//        /// The X component of the Vector3.
//        /// </summary>
//        public double X;

//        /// <summary>
//        /// The Y component of the Vector3.
//        /// </summary>
//        public double Y;

//        /// <summary>
//        /// The Z component of the Vector3.
//        /// </summary>
//        public double Z;

//        #endregion Fields

//        #region Constructors

//        /// <summary>
//        /// Constructs a new Vector3.
//        /// </summary>
//        /// <param name="x">The x component of the Vector3.</param>
//        /// <param name="y">The y component of the Vector3.</param>
//        /// <param name="z">The z component of the Vector3.</param>
//        public Vector3(double x, double y, double z)
//        {
//            this.X = x;
//            this.Y = y;
//            this.Z = z;
//        }

//        /// <summary>
//        /// Constructs a new instance from the given Vector2d.
//        /// </summary>
//        /// <param name="v">The Vector2d to copy components from.</param>
//        public Vector3(Vector2 v, double z = 0)
//        {
//            X = v.X;
//            Y = v.Y;
//            this.Z = z;
//        }

//        /// <summary>
//        /// Constructs a new instance from the given Vector3d.
//        /// </summary>
//        /// <param name="v">The Vector3d to copy components from.</param>
//        public Vector3(Vector3 v)
//        {
//            X = v.X;
//            Y = v.Y;
//            Z = v.Z;
//        }

//        //public Vector3(Vector3Float v)
//        //{
//        //    X = v.X;
//        //    Y = v.Y;
//        //    Z = v.Z;
//        //}

//        public Vector3(double[] doubleArray)
//        {
//            X = doubleArray[0];
//            Y = doubleArray[1];
//            Z = doubleArray[2];
//        }

//        /// <summary>
//        /// Constructs a new instance from the given Vector4d.
//        /// </summary>
//        /// <param name="v">The Vector4d to copy components from.</param>
//        public Vector3(Vector4 v)
//        {
//            X = v.X;
//            Y = v.Y;
//            Z = v.Z;
//        }



//        #endregion Constructors

//        #region Properties

//        public double this[int index]
//        {
//            get
//            {
//                switch (index)
//                {
//                    case 0:
//                        return X;

//                    case 1:
//                        return Y;

//                    case 2:
//                        return Z;

//                    default:
//                        return 0;
//                }
//            }

//            set
//            {
//                switch (index)
//                {
//                    case 0:
//                        X = value;
//                        break;

//                    case 1:
//                        Y = value;
//                        break;

//                    case 2:
//                        Z = value;
//                        break;

//                    default:
//                        throw new Exception();
//                }
//            }
//        }

//        #endregion Properties

//        #region Public Members

//        #region Instance

//        #region public double Length

//        /// <summary>
//        /// Gets the length (magnitude) of the vector.
//        /// </summary>
//        /// <see cref="LengthFast"/>
//        /// <seealso cref="LengthSquared"/>
//        [JsonIgnore]
//        public double Length
//        {
//            get { return System.Math.Sqrt(X * X + Y * Y + Z * Z); }
//        }

//        public double DistanceToSegment(Vector3 start, Vector3 end)
//        {
//            var segmentDelta = end - start;
//            var segmentLength = segmentDelta.Length;
//            var segmentNormal = segmentDelta.GetNormal();
//            var deltaToStart = this - start;
//            var distanceFromStart = segmentNormal.Dot(deltaToStart);
//            if (distanceFromStart >= 0 && distanceFromStart < segmentLength)
//            {
//                var perpendicular = segmentNormal.GetPerpendicular(new Vector3(0, 0, 1));
//                var distanceFromLine = Math.Abs(deltaToStart.Dot(perpendicular));
//                return distanceFromLine;
//            }

//            if (distanceFromStart < 0)
//            {
//                return deltaToStart.Length;
//            }

//            var deltaToEnd = this - end;
//            return deltaToEnd.Length;
//        }


//        #endregion public double Length

//        #region public double LengthSquared

//        /// <summary>
//        /// Gets the square of the vector length (magnitude).
//        /// </summary>
//        /// <remarks>
//        /// This property avoids the costly square root operation required by the Length property. This makes it more suitable
//        /// for comparisons.
//        /// </remarks>
//        /// <see cref="Length"/>
//        /// <seealso cref="LengthFast"/>
//        [JsonIgnore]
//        public double LengthSquared
//        {
//            get { return X * X + Y * Y + Z * Z; }
//        }

//        #endregion public double LengthSquared

//        #region public void Normalize()

//        /// <summary>
//        /// Returns a normalized Vector of this.
//        /// </summary>
//        /// <returns></returns>
//        public Vector3 GetNormal()
//        {
//            Vector3 temp = this;
//            temp.Normalize();
//            return temp;
//        }

//        /// <summary>
//        /// Scales the Vector3d to unit length.
//        /// </summary>
//        public void Normalize()
//        {
//            double length = this.Length;
//            if (length != 0)
//            {
//                double scale = 1.0 / this.Length;
//                X *= scale;
//                Y *= scale;
//                Z *= scale;
//            }
//        }

//        #endregion public void Normalize()

//        public bool IsValid()
//        {
//            if (double.IsNaN(X) || double.IsInfinity(X)
//                                || double.IsNaN(Y) || double.IsInfinity(Y)
//                                || double.IsNaN(Z) || double.IsInfinity(Z))
//            {
//                return false;
//            }

//            return true;
//        }

//        #region public double[] ToArray()

//        public double[] ToArray()
//        {
//            return new double[] {X, Y, Z};
//        }

//        #endregion public double[] ToArray()

//        #endregion Instance

//        #region Static

//        #region Fields

//        /// <summary>
//        /// Defines a unit-length Vector3d that points towards the X-axis.
//        /// </summary>
//        public static readonly Vector3 UnitX = new Vector3(1, 0, 0);

//        /// <summary>
//        /// Defines a unit-length Vector3d that points towards the Y-axis.
//        /// </summary>
//        public static readonly Vector3 UnitY = new Vector3(0, 1, 0);

//        /// <summary>
//        /// /// Defines a unit-length Vector3d that points towards the Z-axis.
//        /// </summary>
//        public static readonly Vector3 UnitZ = new Vector3(0, 0, 1);

//        /// <summary>
//        /// Defines a zero-length Vector3.
//        /// </summary>
//        public static readonly Vector3 Zero = new Vector3(0, 0, 0);

//        /// <summary>
//        /// Defines an instance with all components set to 1.
//        /// </summary>
//        public static readonly Vector3 One = new Vector3(1, 1, 1);

//        /// <summary>
//        /// Defines an instance with all components set to positive infinity.
//        /// </summary>
//        public static readonly Vector3 PositiveInfinity =
//            new Vector3(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);

//        /// <summary>
//        /// Defines an instance with all components set to negative infinity.
//        /// </summary>
//        public static readonly Vector3 NegativeInfinity =
//            new Vector3(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);

//        /// <summary>
//        /// Defines the size of the Vector3d struct in bytes.
//        /// </summary>
//        public static readonly int SizeInBytes = Marshal.SizeOf(new Vector3());

//        #endregion Fields

//        #region Add

//        /// <summary>
//        /// Adds two vectors.
//        /// </summary>
//        /// <param name="a">Left operand.</param>
//        /// <param name="b">Right operand.</param>
//        /// <returns>Result of operation.</returns>
//        public static Vector3 Add(Vector3 a, Vector3 b)
//        {
//            Add(ref a, ref b, out a);
//            return a;
//        }

//        /// <summary>
//        /// Adds two vectors.
//        /// </summary>
//        /// <param name="a">Left operand.</param>
//        /// <param name="b">Right operand.</param>
//        /// <param name="result">Result of operation.</param>
//        public static void Add(ref Vector3 a, ref Vector3 b, out Vector3 result)
//        {
//            result = new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
//        }

//        #endregion Add

//        #region Subtract

//        /// <summary>
//        /// Subtract one Vector from another
//        /// </summary>
//        /// <param name="a">First operand</param>
//        /// <param name="b">Second operand</param>
//        /// <returns>Result of subtraction</returns>
//        public static Vector3 Subtract(Vector3 a, Vector3 b)
//        {
//            Subtract(ref a, ref b, out a);
//            return a;
//        }

//        /// <summary>
//        /// Subtract one Vector from another
//        /// </summary>
//        /// <param name="a">First operand</param>
//        /// <param name="b">Second operand</param>
//        /// <param name="result">Result of subtraction</param>
//        public static void Subtract(ref Vector3 a, ref Vector3 b, out Vector3 result)
//        {
//            result = new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
//        }

//        #endregion Subtract

//        #region Multiply

//        /// <summary>
//        /// Multiplies a vector by a scalar.
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <returns>Result of the operation.</returns>
//        public static Vector3 Multiply(Vector3 vector, double scale)
//        {
//            Multiply(ref vector, scale, out vector);
//            return vector;
//        }

//        /// <summary>
//        /// Multiplies a vector by a scalar.
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <param name="result">Result of the operation.</param>
//        public static void Multiply(ref Vector3 vector, double scale, out Vector3 result)
//        {
//            result = new Vector3(vector.X * scale, vector.Y * scale, vector.Z * scale);
//        }

//        /// <summary>
//        /// Multiplies a vector by the components a vector (scale).
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <returns>Result of the operation.</returns>
//        public static Vector3 Multiply(Vector3 vector, Vector3 scale)
//        {
//            Multiply(ref vector, ref scale, out vector);
//            return vector;
//        }

//        /// <summary>
//        /// Multiplies a vector by the components of a vector (scale).
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <param name="result">Result of the operation.</param>
//        public static void Multiply(ref Vector3 vector, ref Vector3 scale, out Vector3 result)
//        {
//            result = new Vector3(vector.X * scale.X, vector.Y * scale.Y, vector.Z * scale.Z);
//        }

//        #endregion Multiply

//        #region Divide

//        /// <summary>
//        /// Divides a vector by a scalar.
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <returns>Result of the operation.</returns>
//        public static Vector3 Divide(Vector3 vector, double scale)
//        {
//            Divide(ref vector, scale, out vector);
//            return vector;
//        }

//        /// <summary>
//        /// Divides a vector by a scalar.
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <param name="result">Result of the operation.</param>
//        public static void Divide(ref Vector3 vector, double scale, out Vector3 result)
//        {
//            Multiply(ref vector, 1 / scale, out result);
//        }

//        /// <summary>
//        /// Divides a vector by the components of a vector (scale).
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <returns>Result of the operation.</returns>
//        public static Vector3 Divide(Vector3 vector, Vector3 scale)
//        {
//            Divide(ref vector, ref scale, out vector);
//            return vector;
//        }

//        /// <summary>
//        /// Divide a vector by the components of a vector (scale).
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <param name="result">Result of the operation.</param>
//        public static void Divide(ref Vector3 vector, ref Vector3 scale, out Vector3 result)
//        {
//            result = new Vector3(vector.X / scale.X, vector.Y / scale.Y, vector.Z / scale.Z);
//        }

//        #endregion Divide

//        #region ComponentMin

//        /// <summary>
//        /// Calculate the component-wise minimum of two vectors
//        /// </summary>
//        /// <param name="a">First operand</param>
//        /// <param name="b">Second operand</param>
//        /// <returns>The component-wise minimum</returns>
//        public static Vector3 ComponentMin(Vector3 a, Vector3 b)
//        {
//            a.X = a.X < b.X ? a.X : b.X;
//            a.Y = a.Y < b.Y ? a.Y : b.Y;
//            a.Z = a.Z < b.Z ? a.Z : b.Z;
//            return a;
//        }

//        public static Vector3 Parse(string s)
//        {
//            var result = Vector3.Zero;

//            var values = s.Split(',').Select(sValue =>
//            {
//                double.TryParse(sValue, out double number);
//                return number;
//            }).ToArray();

//            for (int i = 0; i < Math.Min(3, values.Length); i++)
//            {
//                result[i] = values[i];
//            }

//            return result;
//        }

//        /// <summary>
//        /// Calculate the component-wise minimum of two vectors
//        /// </summary>
//        /// <param name="a">First operand</param>
//        /// <param name="b">Second operand</param>
//        /// <param name="result">The component-wise minimum</param>
//        public static void ComponentMin(ref Vector3 a, ref Vector3 b, out Vector3 result)
//        {
//            result.X = a.X < b.X ? a.X : b.X;
//            result.Y = a.Y < b.Y ? a.Y : b.Y;
//            result.Z = a.Z < b.Z ? a.Z : b.Z;
//        }

//        #endregion ComponentMin

//        #region ComponentMax

//        /// <summary>
//        /// Calculate the component-wise maximum of two vectors
//        /// </summary>
//        /// <param name="a">First operand</param>
//        /// <param name="b">Second operand</param>
//        /// <returns>The component-wise maximum</returns>
//        public static Vector3 ComponentMax(Vector3 a, Vector3 b)
//        {
//            a.X = a.X > b.X ? a.X : b.X;
//            a.Y = a.Y > b.Y ? a.Y : b.Y;
//            a.Z = a.Z > b.Z ? a.Z : b.Z;
//            return a;
//        }

//        /// <summary>
//        /// Calculate the component-wise maximum of two vectors
//        /// </summary>
//        /// <param name="a">First operand</param>
//        /// <param name="b">Second operand</param>
//        /// <param name="result">The component-wise maximum</param>
//        public static void ComponentMax(ref Vector3 a, ref Vector3 b, out Vector3 result)
//        {
//            result.X = a.X > b.X ? a.X : b.X;
//            result.Y = a.Y > b.Y ? a.Y : b.Y;
//            result.Z = a.Z > b.Z ? a.Z : b.Z;
//        }

//        #endregion ComponentMax

//        #region Min

//        /// <summary>
//        /// Returns the Vector3d with the minimum magnitude
//        /// </summary>
//        /// <param name="left">Left operand</param>
//        /// <param name="right">Right operand</param>
//        /// <returns>The minimum Vector3</returns>
//        public static Vector3 Min(Vector3 left, Vector3 right)
//        {
//            return left.LengthSquared < right.LengthSquared ? left : right;
//        }

//        #endregion Min

//        #region Max

//        /// <summary>
//        /// Returns the Vector3d with the minimum magnitude
//        /// </summary>
//        /// <param name="left">Left operand</param>
//        /// <param name="right">Right operand</param>
//        /// <returns>The minimum Vector3</returns>
//        public static Vector3 Max(Vector3 left, Vector3 right)
//        {
//            return left.LengthSquared >= right.LengthSquared ? left : right;
//        }

//        #endregion Max

//        #region Clamp

//        /// <summary>
//        /// Clamp a vector to the given minimum and maximum vectors
//        /// </summary>
//        /// <param name="vec">Input vector</param>
//        /// <param name="min">Minimum vector</param>
//        /// <param name="max">Maximum vector</param>
//        /// <returns>The clamped vector</returns>
//        public static Vector3 Clamp(Vector3 vec, Vector3 min, Vector3 max)
//        {
//            vec.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
//            vec.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
//            vec.Z = vec.Z < min.Z ? min.Z : vec.Z > max.Z ? max.Z : vec.Z;
//            return vec;
//        }

//        /// <summary>
//        /// Clamp a vector to the given minimum and maximum vectors
//        /// </summary>
//        /// <param name="vec">Input vector</param>
//        /// <param name="min">Minimum vector</param>
//        /// <param name="max">Maximum vector</param>
//        /// <param name="result">The clamped vector</param>
//        public static void Clamp(ref Vector3 vec, ref Vector3 min, ref Vector3 max, out Vector3 result)
//        {
//            result.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
//            result.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
//            result.Z = vec.Z < min.Z ? min.Z : vec.Z > max.Z ? max.Z : vec.Z;
//        }

//        #endregion Clamp

//        #region Normalize

//        /// <summary>
//        /// Scale a vector to unit length
//        /// </summary>
//        /// <param name="vec">The input vector</param>
//        /// <returns>The normalized vector</returns>
//        public static Vector3 Normalize(Vector3 vec)
//        {
//            double scale = 1.0 / vec.Length;
//            vec.X *= scale;
//            vec.Y *= scale;
//            vec.Z *= scale;
//            return vec;
//        }

//        /// <summary>
//        /// Scale a vector to unit length
//        /// </summary>
//        /// <param name="vec">The input vector</param>
//        /// <param name="result">The normalized vector</param>
//        public static void Normalize(ref Vector3 vec, out Vector3 result)
//        {
//            double scale = 1.0 / vec.Length;
//            result.X = vec.X * scale;
//            result.Y = vec.Y * scale;
//            result.Z = vec.Z * scale;
//        }

//        #endregion Normalize

//        #region Utility

//        /// <summary>
//        /// Checks if 3 points are collinear (all lie on the same line).
//        /// </summary>
//        /// <param name="a"></param>
//        /// <param name="b"></param>
//        /// <param name="c"></param>
//        /// <param name="epsilon"></param>
//        /// <returns></returns>
//        public static bool Collinear(Vector3 a, Vector3 b, Vector3 c, double epsilon = .000001)
//        {
//            // Return true if a, b, and c all lie on the same line.
//            return Math.Abs((b - a).Cross(c - a).Length) < epsilon;
//        }

//        /// <summary>
//        /// Given an arbitrary vector find a perpendicular from the infinite perpendiculars that are available
//        /// </summary>
//        /// <param name="a">The vector to find a perpendicular for</param>
//        /// <returns>A perpendicular vector to a</returns>
//        public Vector3 GetPerpendicular()
//        {
//            if (this.X != 0)
//            {
//                return new Vector3(-(this.Y + this.Z) / this.X, 1, 1);
//            }
//            else if (this.Y != 0)
//            {
//                return new Vector3(1, -(this.X + this.Z) / this.Y, 1);
//            }
//            else if (this.Z != 0)
//            {
//                return new Vector3(1, 1, -(this.X + this.Y) / this.Z);
//            }

//            // the input vector has no length (no vector is perpendicular to it)
//            return default(Vector3);
//        }

//        public Vector3 GetPerpendicular(Vector3 b)
//        {
//            return GetPerpendicular(this, b);
//        }

//        public static Vector3 GetPerpendicular(Vector3 a, Vector3 b)
//        {
//            if (!Collinear(a, b, Zero))
//            {
//                return a.Cross(b);
//            }
//            else
//            {
//                Vector3 zOne = new Vector3(0, 0, 100000);
//                if (!Collinear(a, b, zOne))
//                {
//                    return Vector3Ex.Cross(a - zOne, b - zOne);
//                }
//                else
//                {
//                    Vector3 xOne = new Vector3(1000000, 0, 0);
//                    return Vector3Ex.Cross(a - xOne, b - xOne);
//                }
//            }
//        }

//        #endregion Utility

//        #region Lerp

//        /// <summary>
//        /// Returns a new Vector that is the linear blend of the 2 given Vectors
//        /// </summary>
//        /// <param name="a">First input vector</param>
//        /// <param name="b">Second input vector</param>
//        /// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
//        /// <returns>a when blend=0, b when blend=1, and a linear combination otherwise</returns>
//        public static Vector3 Lerp(Vector3 a, Vector3 b, double blend)
//        {
//            if (blend == 0)
//            {
//                return a;
//            }

//            if (blend == 1)
//            {
//                return b;
//            }

//            a.X = blend * (b.X - a.X) + a.X;
//            a.Y = blend * (b.Y - a.Y) + a.Y;
//            a.Z = blend * (b.Z - a.Z) + a.Z;
//            return a;
//        }

//        /// <summary>
//        /// Returns a new Vector that is the linear blend of the 2 given Vectors
//        /// </summary>
//        /// <param name="a">First input vector</param>
//        /// <param name="b">Second input vector</param>
//        /// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
//        /// <param name="result">a when blend=0, b when blend=1, and a linear combination otherwise</param>
//        public static void Lerp(ref Vector3 a, ref Vector3 b, double blend, out Vector3 result)
//        {
//            result.X = blend * (b.X - a.X) + a.X;
//            result.Y = blend * (b.Y - a.Y) + a.Y;
//            result.Z = blend * (b.Z - a.Z) + a.Z;
//        }

//        #endregion Lerp

//        #region Barycentric

//        /// <summary>
//        /// Interpolate 3 Vectors using Barycentric coordinates
//        /// </summary>
//        /// <param name="a">First input Vector</param>
//        /// <param name="b">Second input Vector</param>
//        /// <param name="c">Third input Vector</param>
//        /// <param name="u">First Barycentric Coordinate</param>
//        /// <param name="v">Second Barycentric Coordinate</param>
//        /// <returns>a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise</returns>
//        public static Vector3 BaryCentric(Vector3 a, Vector3 b, Vector3 c, double u, double v)
//        {
//            return a + u * (b - a) + v * (c - a);
//        }

//        /// <summary>Interpolate 3 Vectors using Barycentric coordinates</summary>
//        /// <param name="a">First input Vector.</param>
//        /// <param name="b">Second input Vector.</param>
//        /// <param name="c">Third input Vector.</param>
//        /// <param name="u">First Barycentric Coordinate.</param>
//        /// <param name="v">Second Barycentric Coordinate.</param>
//        /// <param name="result">Output Vector. a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise</param>
//        public static void BaryCentric(ref Vector3 a, ref Vector3 b, ref Vector3 c, double u, double v,
//            out Vector3 result)
//        {
//            result = a; // copy

//            Vector3 temp = b; // copy
//            Subtract(ref temp, ref a, out temp);
//            Multiply(ref temp, u, out temp);
//            Add(ref result, ref temp, out result);

//            temp = c; // copy
//            Subtract(ref temp, ref a, out temp);
//            Multiply(ref temp, v, out temp);
//            Add(ref result, ref temp, out result);
//        }

//        #endregion Barycentric

//        #region CalculateAngle

//        /// <summary>
//        /// Calculates the angle (in radians) between two vectors.
//        /// </summary>
//        /// <param name="first">The first vector.</param>
//        /// <param name="second">The second vector.</param>
//        /// <returns>Angle (in radians) between the vectors.</returns>
//        /// <remarks>Note that the returned angle is never bigger than the constant Pi.</remarks>
//        public static double CalculateAngle(Vector3 first, Vector3 second)
//        {
//            return System.Math.Acos((first.Dot(second)) / (first.Length * second.Length));
//        }

//        /// <summary>Calculates the angle (in radians) between two vectors.</summary>
//        /// <param name="first">The first vector.</param>
//        /// <param name="second">The second vector.</param>
//        /// <param name="result">Angle (in radians) between the vectors.</param>
//        /// <remarks>Note that the returned angle is never bigger than the constant Pi.</remarks>
//        public static void CalculateAngle(ref Vector3 first, ref Vector3 second, out double result)
//        {
//            double temp;
//            first.Dot(ref second, out temp);
//            result = System.Math.Acos(temp / (first.Length * second.Length));
//        }

//        #endregion CalculateAngle

//        #endregion Static

//        #region Swizzle

//        /// <summary>
//        /// Gets or sets an OpenTK.Vector2d with the X and Y components of this instance.
//        /// </summary>
//        [JsonIgnore]
//        public Vector2 Xy
//        {
//            get { return new Vector2((float) X, (float) Y); }
//            set
//            {
//                X = value.X;
//                Y = value.Y;
//            }
//        }

//        #endregion Swizzle

//        #region Operators

//        /// <summary>
//        /// Adds two instances.
//        /// </summary>
//        /// <param name="left">The first instance.</param>
//        /// <param name="right">The second instance.</param>
//        /// <returns>The result of the calculation.</returns>
//        public static Vector3 operator +(Vector3 left, Vector3 right)
//        {
//            left.X += right.X;
//            left.Y += right.Y;
//            left.Z += right.Z;
//            return left;
//        }

//        /// <summary>
//        /// Subtracts two instances.
//        /// </summary>
//        /// <param name="left">The first instance.</param>
//        /// <param name="right">The second instance.</param>
//        /// <returns>The result of the calculation.</returns>
//        public static Vector3 operator -(Vector3 left, Vector3 right)
//        {
//            left.X -= right.X;
//            left.Y -= right.Y;
//            left.Z -= right.Z;
//            return left;
//        }

//        /// <summary>
//        /// Negates an instance.
//        /// </summary>
//        /// <param name="vec">The instance.</param>
//        /// <returns>The result of the calculation.</returns>
//        public static Vector3 operator -(Vector3 vec)
//        {
//            vec.X = -vec.X;
//            vec.Y = -vec.Y;
//            vec.Z = -vec.Z;
//            return vec;
//        }

//        /// <summary>
//        /// Component wise multiply two vectors together, x*x, y*y, z*z.
//        /// </summary>
//        /// <param name="vecA"></param>
//        /// <param name="vecB"></param>
//        /// <returns></returns>
//        public static Vector3 operator *(Vector3 vecA, Vector3 vecB)
//        {
//            vecA.X *= vecB.X;
//            vecA.Y *= vecB.Y;
//            vecA.Z *= vecB.Z;
//            return vecA;
//        }

//        /// <summary>
//        /// Multiplies an instance by a scalar.
//        /// </summary>
//        /// <param name="vec">The instance.</param>
//        /// <param name="scale">The scalar.</param>
//        /// <returns>The result of the calculation.</returns>
//        public static Vector3 operator *(Vector3 vec, double scale)
//        {
//            vec.X *= scale;
//            vec.Y *= scale;
//            vec.Z *= scale;
//            return vec;
//        }

//        /// <summary>
//        /// Multiplies an instance by a scalar.
//        /// </summary>
//        /// <param name="scale">The scalar.</param>
//        /// <param name="vec">The instance.</param>
//        /// <returns>The result of the calculation.</returns>
//        public static Vector3 operator *(double scale, Vector3 vec)
//        {
//            vec.X *= scale;
//            vec.Y *= scale;
//            vec.Z *= scale;
//            return vec;
//        }

//        /// <summary>
//        /// Creates a new vector which is the numerator divide by each component of the vector.
//        /// </summary>
//        /// <param name="numerator"></param>
//        /// <param name="vec"></param>
//        /// <returns>The result of the calculation.</returns>
//        public static Vector3 operator /(double numerator, Vector3 vec)
//        {
//            return new Vector3((numerator / vec.X), (numerator / vec.Y), (numerator / vec.Z));
//        }

//        /// <summary>
//        /// Divides an instance by a scalar.
//        /// </summary>
//        /// <param name="vec">The instance.</param>
//        /// <param name="scale">The scalar.</param>
//        /// <returns>The result of the calculation.</returns>
//        public static Vector3 operator /(Vector3 vec, double scale)
//        {
//            double mult = 1 / scale;
//            vec.X *= mult;
//            vec.Y *= mult;
//            vec.Z *= mult;
//            return vec;
//        }

//        /// <summary>
//        /// Compares two instances for equality.
//        /// </summary>
//        /// <param name="left">The first instance.</param>
//        /// <param name="right">The second instance.</param>
//        /// <returns>True, if left equals right; false otherwise.</returns>
//        public static bool operator ==(Vector3 left, Vector3 right)
//        {
//            return left.Equals(right);
//        }

//        /// <summary>
//        /// Compares two instances for inequality.
//        /// </summary>
//        /// <param name="left">The first instance.</param>
//        /// <param name="right">The second instance.</param>
//        /// <returns>True, if left does not equa lright; false otherwise.</returns>
//        public static bool operator !=(Vector3 left, Vector3 right)
//        {
//            return !left.Equals(right);
//        }

//        #endregion Operators

//        #region Overrides

//        #region public override string ToString()

//        /// <summary>
//        /// Returns a System.String that represents the current Vector3.
//        /// </summary>
//        /// <returns></returns>
//        public override string ToString()
//        {
//            return String.Format($"[{X:0.####}, {Y:0.####}, {Z:0.####}]");
//        }

//        #endregion public override string ToString()

//        #region public override int GetHashCode()

//        /// <summary>
//        /// Returns the hashcode for this instance.
//        /// </summary>
//        /// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
//        public override int GetHashCode()
//        {
//            return new {X, Y, Z}.GetHashCode();
//        }

//        ///// <summary>
//        ///// return a 64 bit hash code proposed by Jon Skeet
//        //// http://stackoverflow.com/questions/8094867/good-gethashcode-override-for-list-of-foo-objects-respecting-the-order
//        ///// </summary>
//        ///// <returns></returns>
//        //public ulong GetLongHashCode(ulong hash = 14695981039346656037)
//        //{
//        //	hash = Vector4.GetLongHashCode(X, hash);
//        //	hash = Vector4.GetLongHashCode(Y, hash);
//        //	hash = Vector4.GetLongHashCode(Z, hash);

//        //	return hash;
//        //}

//        #endregion public override int GetHashCode()

//        #region public override bool Equals(object obj)

//        /// <summary>
//        /// Indicates whether this instance and a specified object are equal.
//        /// </summary>
//        /// <param name="obj">The object to compare to.</param>
//        /// <returns>True if the instances are equal; false otherwise.</returns>
//        public override bool Equals(object obj)
//        {
//            if (!(obj is Vector3))
//                return false;

//            return this.Equals((Vector3) obj);
//        }

//        /// <summary>
//        /// Indicates whether this instance and a specified object are equal within an error range.
//        /// </summary>
//        /// <param name="OtherVector"></param>
//        /// <param name="ErrorValue"></param>
//        /// <returns>True if the instances are equal; false otherwise.</returns>
//        public bool Equals(Vector3 OtherVector, double ErrorValue)
//        {
//            if ((X < OtherVector.X + ErrorValue && X > OtherVector.X - ErrorValue) &&
//                (Y < OtherVector.Y + ErrorValue && Y > OtherVector.Y - ErrorValue) &&
//                (Z < OtherVector.Z + ErrorValue && Z > OtherVector.Z - ErrorValue))
//            {
//                return true;
//            }

//            return false;
//        }

//        #endregion public override bool Equals(object obj)

//        #endregion Overrides

//        #endregion Public Members

//        #region IEquatable<Vector3> Members

//        /// <summary>Indicates whether the current vector is equal to another vector.</summary>
//        /// <param name="other">A vector to compare with this vector.</param>
//        /// <returns>true if the current vector is equal to the vector parameter; otherwise, false.</returns>
//        public bool Equals(Vector3 other)
//        {
//            return
//                X == other.X &&
//                Y == other.Y &&
//                Z == other.Z;
//        }

//        #endregion IEquatable<Vector3> Members

//        #region IConvertable

//        public TypeCode GetTypeCode()
//        {
//            throw new NotImplementedException();
//        }

//        public bool ToBoolean(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public char ToChar(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public sbyte ToSByte(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public byte ToByte(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public short ToInt16(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public ushort ToUInt16(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public int ToInt32(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public uint ToUInt32(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public long ToInt64(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public ulong ToUInt64(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public float ToSingle(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public double ToDouble(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public decimal ToDecimal(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public DateTime ToDateTime(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public string ToString(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public object ToType(Type conversionType, IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        #endregion IConvertable

//        public static double ComponentMax(Vector3 vector3)
//        {
//            return Math.Max(vector3.X, Math.Max(vector3.Y, vector3.Z));
//        }

//        public static double ComponentMin(Vector3 vector3)
//        {
//            return Math.Min(vector3.X, Math.Min(vector3.Y, vector3.Z));
//        }
//    }

//    public static class Vector3Ex
//    {
//        #region Dot

//        /// <summary>
//        /// Calculate the dot (scalar) product of two vectors
//        /// </summary>
//        /// <param name="left">First operand</param>
//        /// <param name="right">Second operand</param>
//        /// <returns>The dot product of the two inputs</returns>
//        public static double Dot(this Vector3 left, Vector3 right)
//        {
//            return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
//        }

//        /// <summary>
//        /// Calculate the dot (scalar) product of two vectors
//        /// </summary>
//        /// <param name="left">First operand</param>
//        /// <param name="right">Second operand</param>
//        /// <param name="result">The dot product of the two inputs</param>
//        public static void Dot(this Vector3 left, ref Vector3 right, out double result)
//        {
//            result = left.X * right.X + left.Y * right.Y + left.Z * right.Z;
//        }

//        #endregion Dot

//        #region Cross

//        /// <summary>
//        /// Calculate the cross (vector) product of two vectors
//        /// </summary>
//        /// <param name="left">First operand</param>
//        /// <param name="right">Second operand</param>
//        /// <returns>The cross product of the two inputs</returns>
//        public static Vector3 Cross(this Vector3 left, Vector3 right)
//        {
//            Vector3 result;
//            left.Cross(ref right, out result);
//            return result;
//        }

//        /// <summary>
//        /// Calculate the cross (vector) product of two vectors
//        /// </summary>
//        /// <param name="left">First operand</param>
//        /// <param name="right">Second operand</param>
//        /// <returns>The cross product of the two inputs</returns>
//        /// <param name="result">The cross product of the two inputs</param>
//        public static void Cross(this Vector3 left, ref Vector3 right, out Vector3 result)
//        {
//            result = new Vector3(left.Y * right.Z - left.Z * right.Y,
//                left.Z * right.X - left.X * right.Z,
//                left.X * right.Y - left.Y * right.X);
//        }

//        #endregion Cross

//        #region Transform

//        /// <summary>Transform a direction vector by the given Matrix
//        /// Assumes the matrix has a bottom row of (0,0,0,1), that is the translation part is ignored.
//        /// </summary>
//        /// <param name="vec">The vector to transform</param>
//        /// <param name="mat">The desired transformation</param>
//        /// <returns>The transformed vector</returns>
//        public static Vector3 TransformVector(this Vector3 vec, Matrix4X4 mat)
//        {
//            return new Vector3(
//                vec.Dot(new Vector3(mat.Column0)),
//                vec.Dot(new Vector3(mat.Column1)),
//                vec.Dot(new Vector3(mat.Column2)));
//        }

//        /// <summary>Transform a direction vector by the given Matrix
//        /// Assumes the matrix has a bottom row of (0,0,0,1), that is the translation part is ignored.
//        /// </summary>
//        /// <param name="vec">The vector to transform</param>
//        /// <param name="mat">The desired transformation</param>
//        /// <param name="result">The transformed vector</param>
//        public static void TransformVector(this Vector3 vec, ref Matrix4X4 mat, out Vector3 result)
//        {
//            result.X = vec.X * mat.Row0.X +
//                       vec.Y * mat.Row1.X +
//                       vec.Z * mat.Row2.X;

//            result.Y = vec.X * mat.Row0.Y +
//                       vec.Y * mat.Row1.Y +
//                       vec.Z * mat.Row2.Y;

//            result.Z = vec.X * mat.Row0.Z +
//                       vec.Y * mat.Row1.Z +
//                       vec.Z * mat.Row2.Z;
//        }

//        /// This calculates the inverse of the given matrix, use TransformNormalInverse if you
//        /// already have the inverse to avoid this extra calculation
//        /// <param name="normal">The normal to transform</param>
//        /// <param name="mat">The desired transformation</param>
//        /// <returns>The transformed normal</returns>
//        public static Vector3 TransformNormal(this Vector3 normal, Matrix4X4 mat)
//        {
//            Vector3 result;
//            TransformNormal(normal, ref mat, out result);
//            return result;
//        }

//        /// <summary>Transform a Normal by the given Matrix</summary>
//        /// <remarks>
//        /// This calculates the inverse of the given matrix, use TransformNormal if you have
//        /// a point on the plane (fastest) or TransformNormalInverse if you
//        /// have the inverse but not a point on the plane - to avoid this extra calculation
//        /// </remarks>
//        /// <param name="normal">The normal to transform</param>
//        /// <param name="mat">The desired transformation</param>
//        /// <param name="result">The transformed normal</param>
//        public static void TransformNormal(this Vector3 normal, ref Matrix4X4 mat, out Vector3 result)
//        {
//            Matrix4X4 Inverse = Matrix4X4.Invert(mat);
//            TransformNormalInverse(normal, ref Inverse, out result);
//        }

//        /// <summary>Transform a Normal by the (transpose of the) given Matrix</summary>
//        /// <remarks>
//        /// This version doesn't calculate the inverse matrix.
//        /// Use this version if you already have the inverse of the desired transform to hand
//        /// </remarks>
//        /// <param name="normal">The normal to transform</param>
//        /// <param name="invMat">The inverse of the desired transformation</param>
//        /// <returns>The transformed normal</returns>
//        public static Vector3 TransformNormalInverse(this Vector3 normal, Matrix4X4 invMat)
//        {
//            return new Vector3(
//                normal.Dot(new Vector3(invMat.Row0)),
//                normal.Dot(new Vector3(invMat.Row1)),
//                normal.Dot(new Vector3(invMat.Row2)));
//        }

//        /// <summary>Transform a Normal by the (transpose of the) given Matrix</summary>
//        /// <remarks>
//        /// This version doesn't calculate the inverse matrix.
//        /// Use this version if you already have the inverse of the desired transform to hand
//        /// </remarks>
//        /// <param name="normal">The normal to transform</param>
//        /// <param name="invMat">The inverse of the desired transformation</param>
//        /// <param name="result">The transformed normal</param>
//        public static void TransformNormalInverse(this Vector3 normal, ref Matrix4X4 invMat, out Vector3 result)
//        {
//            result.X = normal.X * invMat.Row0.X +
//                       normal.Y * invMat.Row0.Y +
//                       normal.Z * invMat.Row0.Z;

//            result.Y = normal.X * invMat.Row1.X +
//                       normal.Y * invMat.Row1.Y +
//                       normal.Z * invMat.Row1.Z;

//            result.Z = normal.X * invMat.Row2.X +
//                       normal.Y * invMat.Row2.Y +
//                       normal.Z * invMat.Row2.Z;
//        }

//        /// <summary>Transform a Position by the given Matrix</summary>
//        /// <param name="pos">The position to transform</param>
//        /// <param name="mat">The desired transformation</param>
//        /// <returns>The transformed position</returns>
//        public static Vector3 TransformPosition(this Vector3 pos, Matrix4X4 mat)
//        {
//            return new Vector3(
//                pos.Dot(new Vector3(mat.Column0)) + mat.Row3.X,
//                pos.Dot(new Vector3(mat.Column1)) + mat.Row3.Y,
//                pos.Dot(new Vector3(mat.Column2)) + mat.Row3.Z);
//        }

//        /// <summary>Transform a Position by the given Matrix</summary>
//        /// <param name="pos">The position to transform</param>
//        /// <param name="mat">The desired transformation</param>
//        /// <param name="result">The transformed position</param>
//        public static void TransformPosition(this Vector3 pos, ref Matrix4X4 mat, out Vector3 result)
//        {
//            result.X = pos.X * mat.Row0.X +
//                       pos.Y * mat.Row1.X +
//                       pos.Z * mat.Row2.X +
//                       mat.Row3.X;

//            result.Y = pos.X * mat.Row0.Y +
//                       pos.Y * mat.Row1.Y +
//                       pos.Z * mat.Row2.Y +
//                       mat.Row3.Y;

//            result.Z = pos.X * mat.Row0.Z +
//                       pos.Y * mat.Row1.Z +
//                       pos.Z * mat.Row2.Z +
//                       mat.Row3.Z;
//        }

//        /// <summary>
//        /// Transform all the vectors in the array by the given Matrix.
//        /// </summary>
//        /// <param name="boundsVerts"></param>
//        /// <param name="rotationQuaternion"></param>
//        public static void Transform(this Vector3[] vecArray, Matrix4X4 mat)
//        {
//            for (int i = 0; i < vecArray.Length; i++)
//            {
//                vecArray[i] = Transform(vecArray[i], mat);
//            }
//        }

//        /// <summary>Transform a Vector by the given Matrix</summary>
//        /// <param name="vec">The vector to transform</param>
//        /// <param name="mat">The desired transformation</param>
//        /// <returns>The transformed vector</returns>
//        public static Vector3 Transform(this Vector3 vec, Matrix4X4 mat)
//        {
//            return new Vector3(
//                vec.X * mat.Row0.X + vec.Y * mat.Row1.X + vec.Z * mat.Row2.X + mat.Row3.X,
//                vec.X * mat.Row0.Y + vec.Y * mat.Row1.Y + vec.Z * mat.Row2.Y + mat.Row3.Y,
//                vec.X * mat.Row0.Z + vec.Y * mat.Row1.Z + vec.Z * mat.Row2.Z + mat.Row3.Z);
//        }

//        /// <summary>Transform a Vector by the given Matrix</summary>
//        /// <param name="vec">The vector to transform</param>
//        /// <param name="mat">The desired transformation</param>
//        /// <param name="result">The transformed vector</param>
//        public static void Transform(this Vector3 vec, ref Matrix4X4 mat, out Vector3 result)
//        {
//            result = new Vector3(
//                vec.X * mat.Row0.X + vec.Y * mat.Row1.X + vec.Z * mat.Row2.X + mat.Row3.X,
//                vec.X * mat.Row0.Y + vec.Y * mat.Row1.Y + vec.Z * mat.Row2.Y + mat.Row3.Y,
//                vec.X * mat.Row0.Z + vec.Y * mat.Row1.Z + vec.Z * mat.Row2.Z + mat.Row3.Z);
//        }

//        /// <summary>
//        /// Transforms a vector by a quaternion rotation.
//        /// </summary>
//        /// <param name="vec">The vector to transform.</param>
//        /// <param name="quat">The quaternion to rotate the vector by.</param>
//        /// <returns>The result of the operation.</returns>
//        public static Vector3 Transform(this Vector3 vec, Quaternion quat)
//        {
//            Vector3 result;
//            Transform(vec, ref quat, out result);
//            return result;
//        }

//        /// <summary>
//        /// Transforms a vector by a quaternion rotation.
//        /// </summary>
//        /// <param name="vec">The vector to transform.</param>
//        /// <param name="quat">The quaternion to rotate the vector by.</param>
//        /// <param name="result">The result of the operation.</param>
//        //public static void Transform(this Vector3 vec, ref Quaternion quat, out Vector3 result)
//        //{
//        //    // Since vec.W == 0, we can optimize quat * vec * quat^-1 as follows:
//        //    // vec + 2.0 * cross(quat.xyz, cross(quat.xyz, vec) + quat.w * vec)
//        //    Vector3 xyz = quat.Xyz, temp, temp2;
//        //    xyz.Cross(ref vec, out temp);
//        //    Vector3.Multiply(ref vec, quat.W, out temp2);
//        //    Vector3.Add(ref temp, ref temp2, out temp);
//        //    xyz.Cross(ref temp, out temp);
//        //    Vector3.Multiply(ref temp, 2, out temp);
//        //    Vector3.Add(ref vec, ref temp, out result);
//        //}

//        /// <summary>
//        /// Transform all the vectors in the array by the quaternion rotation.
//        /// </summary>
//        /// <param name="boundsVerts"></param>
//        /// <param name="rotationQuaternion"></param>
//        public static void Transform(this Vector3[] vecArray, Quaternion rotationQuaternion)
//        {
//            for (int i = 0; i < vecArray.Length; i++)
//            {
//                vecArray[i] = Transform(vecArray[i], rotationQuaternion);
//            }
//        }

//        /// <summary>
//        /// Transform a Vector3d by the given Matrix, and project the resulting Vector4 back to a Vector3
//        /// </summary>
//        /// <param name="vec">The vector to transform</param>
//        /// <param name="mat">The desired transformation</param>
//        /// <returns>The transformed vector</returns>
//        public static Vector3 TransformPerspective(this Vector3 vec, Matrix4X4 mat)
//        {
//            Vector3 result;
//            TransformPerspective(vec, ref mat, out result);
//            return result;
//        }

//        /// <summary>Transform a Vector3d by the given Matrix, and project the resulting Vector4d back to a Vector3d</summary>
//        /// <param name="vec">The vector to transform</param>
//        /// <param name="mat">The desired transformation</param>
//        /// <param name="result">The transformed vector</param>
//        public static void TransformPerspective(this Vector3 vec, ref Matrix4X4 mat, out Vector3 result)
//        {
//            Vector4 v = new Vector4(vec);
//            Vector4.Transform(v, ref mat, out v);
//            result.X = v.X / v.W;
//            result.Y = v.Y / v.W;
//            result.Z = v.Z / v.W;
//        }

//        #endregion Transform
//    }

//    /// <summary>Represents a 2D vector using two double-precision floating-point numbers.</summary>

//    public struct Vector2 : IEquatable<Vector2>
//    {
//        /// <summary>
//        /// Defines an instance with all components set to positive infinity.
//        /// </summary>
//        public static readonly Vector2 PositiveInfinity = new Vector2(double.PositiveInfinity, double.PositiveInfinity);

//        /// <summary>
//        /// Defines an instance with all components set to negative infinity.
//        /// </summary>
//        public static readonly Vector2 NegativeInfinity = new Vector2(double.NegativeInfinity, double.NegativeInfinity);

//        #region Fields

//        /// <summary>The X coordinate of this instance.</summary>
//        public double X;

//        /// <summary>The Y coordinate of this instance.</summary>
//        public double Y;

//        /// <summary>
//        /// Defines a unit-length Vector2d that points towards the X-axis.
//        /// </summary>
//        public static Vector2 UnitX = new Vector2(1, 0);

//        /// <summary>
//        /// Defines a unit-length Vector2d that points towards the Y-axis.
//        /// </summary>
//        public static Vector2 UnitY = new Vector2(0, 1);

//        /// <summary>
//        /// Defines a zero-length Vector2d.
//        /// </summary>
//        public static Vector2 Zero = new Vector2(0, 0);

//        /// <summary>
//        /// Defines an instance with all components set to 1.
//        /// </summary>
//        public static readonly Vector2 One = new Vector2(1, 1);

//        /// <summary>
//        /// Defines the size of the Vector2d struct in bytes.
//        /// </summary>
//        public static readonly int SizeInBytes = Marshal.SizeOf(new Vector2());

//        #endregion Fields

//        #region Constructors

//        /// <summary>Constructs left vector with the given coordinates.</summary>
//        /// <param name="x">The X coordinate.</param>
//        /// <param name="y">The Y coordinate.</param>
//        public Vector2(double x, double y)
//        {
//            this.X = x;
//            this.Y = y;
//        }

//        public Vector2(Vector3 vector)
//        {
//            this.X = vector.X;
//            this.Y = vector.Y;
//        }

//        //public Vector2(Vector3Float vector)
//        //{
//        //	this.X = vector.X;
//        //	this.Y = vector.Y;
//        //}

//        #endregion Constructors

//        #region Properties

//        public double this[int index]
//        {
//            get
//            {
//                switch (index)
//                {
//                    case 0:
//                        return X;

//                    case 1:
//                        return Y;

//                    default:
//                        return 0;
//                }
//            }

//            set
//            {
//                switch (index)
//                {
//                    case 0:
//                        X = value;
//                        break;

//                    case 1:
//                        Y = value;
//                        break;

//                    default:
//                        throw new Exception();
//                }
//            }
//        }



//        //public double GetDeltaAngle(Vector2 startPosition, Vector2 endPosition)
//        //{
//        //	startPosition -= this;
//        //	var startAngle = Math.Atan2(startPosition.Y, startPosition.X);
//        //	startAngle = startAngle < 0 ? startAngle + MathHelper.Tau : startAngle;

//        //	endPosition -= this;
//        //	var endAngle = Math.Atan2(endPosition.Y, endPosition.X);
//        //	endAngle = endAngle < 0 ? endAngle + MathHelper.Tau : endAngle;

//        //	return endAngle - startAngle;
//        //}

//        #endregion Properties

//        #region Public Members

//        #region Instance

//        #region public double Length

//        /// <summary>
//        /// Gets the length (magnitude) of the vector.
//        /// </summary>
//        /// <seealso cref="LengthSquared"/>
//        [JsonIgnore]
//        public double Length
//        {
//            get { return System.Math.Sqrt(X * X + Y * Y); }
//        }

//        public double Distance(Vector2 p)
//        {
//            return (this - p).Length;
//        }

//        #endregion public double Length

//        #region public double LengthSquared

//        /// <summary>
//        /// Gets the square of the vector length (magnitude).
//        /// </summary>
//        /// <remarks>
//        /// This property avoids the costly square root operation required by the Length property. This makes it more suitable
//        /// for comparisons.
//        /// </remarks>
//        /// <see cref="Length"/>
//        [JsonIgnore]
//        public double LengthSquared
//        {
//            get { return X * X + Y * Y; }
//        }

//        #endregion public double LengthSquared

//        public void Rotate(double radians)
//        {
//            this = Vector2.Rotate(this, radians);
//        }

//        public Vector2 GetRotated(double radians)
//        {
//            return Vector2.Rotate(this, radians);
//        }

//        public double GetAngle()
//        {
//            return System.Math.Atan2(Y, X);
//        }



//        #region public Vector2d PerpendicularRight

//        /// <summary>
//        /// Gets the perpendicular vector on the right side of this vector.
//        /// </summary>
//        public Vector2 GetPerpendicularRight()
//        {
//            return new Vector2(Y, -X);
//        }

//        #endregion public Vector2d PerpendicularRight

//        #region public Vector2d PerpendicularLeft

//        /// <summary>
//        /// Gets the perpendicular vector on the left side of this vector.
//        /// </summary>
//        public Vector2 GetPerpendicularLeft()
//        {
//            return new Vector2(-Y, X);
//        }

//        #endregion public Vector2d PerpendicularLeft

//        #region public void Normalize()

//        /// <summary>
//        /// Returns a normalized Vector of this.
//        /// </summary>
//        /// <returns></returns>
//        public Vector2 GetNormal()
//        {
//            Vector2 temp = this;
//            temp.Normalize();
//            return temp;
//        }

//        /// <summary>
//        /// Scales the Vector2 to unit length.
//        /// </summary>
//        public void Normalize()
//        {
//            double scale = 1.0 / Length;
//            X *= scale;
//            Y *= scale;
//        }

//        #endregion public void Normalize()

//        public bool IsValid()
//        {
//            if (double.IsNaN(X) || double.IsInfinity(X)
//                                || double.IsNaN(Y) || double.IsInfinity(Y))
//            {
//                return false;
//            }

//            return true;
//        }

//        #endregion Instance

//        #region Static

//        #region Add

//        /// <summary>
//        /// Adds two vectors.
//        /// </summary>
//        /// <param name="a">Left operand.</param>
//        /// <param name="b">Right operand.</param>
//        /// <returns>Result of operation.</returns>
//        public static Vector2 Add(Vector2 a, Vector2 b)
//        {
//            Add(ref a, ref b, out a);
//            return a;
//        }

//        /// <summary>
//        /// Adds two vectors.
//        /// </summary>
//        /// <param name="a">Left operand.</param>
//        /// <param name="b">Right operand.</param>
//        /// <param name="result">Result of operation.</param>
//        public static void Add(ref Vector2 a, ref Vector2 b, out Vector2 result)
//        {
//            result = new Vector2(a.X + b.X, a.Y + b.Y);
//        }

//        #endregion Add

//        #region Subtract

//        /// <summary>
//        /// Subtract one Vector from another
//        /// </summary>
//        /// <param name="a">First operand</param>
//        /// <param name="b">Second operand</param>
//        /// <returns>Result of subtraction</returns>
//        public static Vector2 Subtract(Vector2 a, Vector2 b)
//        {
//            Subtract(ref a, ref b, out a);
//            return a;
//        }

//        /// <summary>
//        /// Subtract one Vector from another
//        /// </summary>
//        /// <param name="a">First operand</param>
//        /// <param name="b">Second operand</param>
//        /// <param name="result">Result of subtraction</param>
//        public static void Subtract(ref Vector2 a, ref Vector2 b, out Vector2 result)
//        {
//            result = new Vector2(a.X - b.X, a.Y - b.Y);
//        }

//        #endregion Subtract

//        #region Multiply

//        /// <summary>
//        /// Multiplies a vector by a scalar.
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <returns>Result of the operation.</returns>
//        public static Vector2 Multiply(Vector2 vector, double scale)
//        {
//            Multiply(ref vector, scale, out vector);
//            return vector;
//        }

//        /// <summary>
//        /// Multiplies a vector by a scalar.
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <param name="result">Result of the operation.</param>
//        public static void Multiply(ref Vector2 vector, double scale, out Vector2 result)
//        {
//            result = new Vector2(vector.X * scale, vector.Y * scale);
//        }

//        /// <summary>
//        /// Multiplies a vector by the components a vector (scale).
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <returns>Result of the operation.</returns>
//        public static Vector2 Multiply(Vector2 vector, Vector2 scale)
//        {
//            Multiply(ref vector, ref scale, out vector);
//            return vector;
//        }

//        /// <summary>
//        /// Multiplies a vector by the components of a vector (scale).
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <param name="result">Result of the operation.</param>
//        public static void Multiply(ref Vector2 vector, ref Vector2 scale, out Vector2 result)
//        {
//            result = new Vector2(vector.X * scale.X, vector.Y * scale.Y);
//        }

//        #endregion Multiply

//        #region Divide

//        /// <summary>
//        /// Divides a vector by a scalar.
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <returns>Result of the operation.</returns>
//        public static Vector2 Divide(Vector2 vector, double scale)
//        {
//            Divide(ref vector, scale, out vector);
//            return vector;
//        }

//        /// <summary>
//        /// Divides a vector by a scalar.
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <param name="result">Result of the operation.</param>
//        public static void Divide(ref Vector2 vector, double scale, out Vector2 result)
//        {
//            Multiply(ref vector, 1 / scale, out result);
//        }

//        /// <summary>
//        /// Divides a vector by the components of a vector (scale).
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <returns>Result of the operation.</returns>
//        public static Vector2 Divide(Vector2 vector, Vector2 scale)
//        {
//            Divide(ref vector, ref scale, out vector);
//            return vector;
//        }

//        /// <summary>
//        /// Divide a vector by the components of a vector (scale).
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <param name="result">Result of the operation.</param>
//        public static void Divide(ref Vector2 vector, ref Vector2 scale, out Vector2 result)
//        {
//            result = new Vector2(vector.X / scale.X, vector.Y / scale.Y);
//        }

//        #endregion Divide

//        #region Min

//        /// <summary>
//        /// Calculate the component-wise minimum of two vectors
//        /// </summary>
//        /// <param name="a">First operand</param>
//        /// <param name="b">Second operand</param>
//        /// <returns>The component-wise minimum</returns>
//        public static Vector2 Min(Vector2 a, Vector2 b)
//        {
//            a.X = a.X < b.X ? a.X : b.X;
//            a.Y = a.Y < b.Y ? a.Y : b.Y;
//            return a;
//        }

//        public static Vector2 Parse(string s)
//        {
//            var result = Vector2.Zero;
//            var values = s.Split(',').Select(sValue =>
//            {
//                double number = 0;
//                if (double.TryParse(sValue, out number))
//                {
//                    return double.Parse(sValue);
//                }

//                return 0;
//            }).ToArray();

//            for (int i = 0; i < Math.Min(2, values.Length); i++)
//            {
//                result[i] = values[i];
//            }

//            return result;
//        }

//        /// <summary>
//        /// Calculate the component-wise minimum of two vectors
//        /// </summary>
//        /// <param name="a">First operand</param>
//        /// <param name="b">Second operand</param>
//        /// <param name="result">The component-wise minimum</param>
//        public static void Min(ref Vector2 a, ref Vector2 b, out Vector2 result)
//        {
//            result.X = a.X < b.X ? a.X : b.X;
//            result.Y = a.Y < b.Y ? a.Y : b.Y;
//        }

//        #endregion Min

//        #region Max

//        /// <summary>
//        /// Calculate the component-wise maximum of two vectors
//        /// </summary>
//        /// <param name="a">First operand</param>
//        /// <param name="b">Second operand</param>
//        /// <returns>The component-wise maximum</returns>
//        public static Vector2 Max(Vector2 a, Vector2 b)
//        {
//            a.X = a.X > b.X ? a.X : b.X;
//            a.Y = a.Y > b.Y ? a.Y : b.Y;
//            return a;
//        }

//        /// <summary>
//        /// Calculate the component-wise maximum of two vectors
//        /// </summary>
//        /// <param name="a">First operand</param>
//        /// <param name="b">Second operand</param>
//        /// <param name="result">The component-wise maximum</param>
//        public static void Max(ref Vector2 a, ref Vector2 b, out Vector2 result)
//        {
//            result.X = a.X > b.X ? a.X : b.X;
//            result.Y = a.Y > b.Y ? a.Y : b.Y;
//        }

//        #endregion Max

//        #region Clamp

//        /// <summary>
//        /// Clamp a vector to the given minimum and maximum vectors
//        /// </summary>
//        /// <param name="vec">Input vector</param>
//        /// <param name="min">Minimum vector</param>
//        /// <param name="max">Maximum vector</param>
//        /// <returns>The clamped vector</returns>
//        public static Vector2 Clamp(Vector2 vec, Vector2 min, Vector2 max)
//        {
//            vec.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
//            vec.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
//            return vec;
//        }

//        /// <summary>
//        /// Clamp a vector to the given minimum and maximum vectors
//        /// </summary>
//        /// <param name="vec">Input vector</param>
//        /// <param name="min">Minimum vector</param>
//        /// <param name="max">Maximum vector</param>
//        /// <param name="result">The clamped vector</param>
//        public static void Clamp(ref Vector2 vec, ref Vector2 min, ref Vector2 max, out Vector2 result)
//        {
//            result.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
//            result.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
//        }

//        #endregion Clamp

//        #region Normalize

//        /// <summary>
//        /// Scale a vector to unit length
//        /// </summary>
//        /// <param name="vec">The input vector</param>
//        /// <returns>The normalized vector</returns>
//        public static Vector2 Normalize(Vector2 vec)
//        {
//            double scale = 1.0 / vec.Length;
//            vec.X *= scale;
//            vec.Y *= scale;
//            return vec;
//        }

//        /// <summary>
//        /// Scale a vector to unit length
//        /// </summary>
//        /// <param name="vec">The input vector</param>
//        /// <param name="result">The normalized vector</param>
//        public static void Normalize(ref Vector2 vec, out Vector2 result)
//        {
//            double scale = 1.0 / vec.Length;
//            result.X = vec.X * scale;
//            result.Y = vec.Y * scale;
//        }

//        #endregion Normalize

//        #region Dot

//        /// <summary>
//        /// Calculate the dot (scalar) product of two vectors
//        /// </summary>
//        /// <param name="left">First operand</param>
//        /// <param name="right">Second operand</param>
//        /// <returns>The dot product of the two inputs</returns>
//        public static double Dot(Vector2 left, Vector2 right)
//        {
//            return left.X * right.X + left.Y * right.Y;
//        }

//        /// <summary>
//        /// Calculate the dot (scalar) product of two vectors
//        /// </summary>
//        /// <param name="left">First operand</param>
//        /// <param name="right">Second operand</param>
//        /// <param name="result">The dot product of the two inputs</param>
//        public static void Dot(ref Vector2 left, ref Vector2 right, out double result)
//        {
//            result = left.X * right.X + left.Y * right.Y;
//        }

//        #endregion Dot

//        #region Cross

//        /// <summary>
//        /// Calculate the cross product of two vectors
//        /// </summary>
//        /// <param name="left">First operand</param>
//        /// <param name="right">Second operand</param>
//        /// <returns>The cross product of the two inputs</returns>
//        public static double Cross(Vector2 left, Vector2 right)
//        {
//            return left.X * right.Y - left.Y * right.X;
//        }

//        public double Cross(Vector2 right)
//        {
//            return this.X * right.Y - this.Y * right.X;
//        }

//        #endregion Cross

//        #region Rotate

//        public static Vector2 Rotate(Vector2 toRotate, double radians)
//        {
//            Vector2 temp;
//            Rotate(ref toRotate, radians, out temp);
//            return temp;
//        }

//        public static void Rotate(ref Vector2 input, double radians, out Vector2 output)
//        {
//            double Cos, Sin;

//            Cos = (double) System.Math.Cos(radians);
//            Sin = (double) System.Math.Sin(radians);

//            output.X = input.X * Cos - input.Y * Sin;
//            output.Y = input.Y * Cos + input.X * Sin;
//        }

//        #endregion Rotate

//        #region Lerp

//        /// <summary>
//        /// Returns a new Vector that is the linear blend of the 2 given Vectors
//        /// </summary>
//        /// <param name="a">First input vector</param>
//        /// <param name="b">Second input vector</param>
//        /// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
//        /// <returns>a when blend=0, b when blend=1, and a linear combination otherwise</returns>
//        public static Vector2 Lerp(Vector2 a, Vector2 b, double blend)
//        {
//            a.X = blend * (b.X - a.X) + a.X;
//            a.Y = blend * (b.Y - a.Y) + a.Y;
//            return a;
//        }

//        /// <summary>
//        /// Returns a new Vector that is the linear blend of the 2 given Vectors
//        /// </summary>
//        /// <param name="a">First input vector</param>
//        /// <param name="b">Second input vector</param>
//        /// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
//        /// <param name="result">a when blend=0, b when blend=1, and a linear combination otherwise</param>
//        public static void Lerp(ref Vector2 a, ref Vector2 b, double blend, out Vector2 result)
//        {
//            result.X = blend * (b.X - a.X) + a.X;
//            result.Y = blend * (b.Y - a.Y) + a.Y;
//        }

//        #endregion Lerp

//        #region Barycentric

//        /// <summary>
//        /// Interpolate 3 Vectors using Barycentric coordinates
//        /// </summary>
//        /// <param name="a">First input Vector</param>
//        /// <param name="b">Second input Vector</param>
//        /// <param name="c">Third input Vector</param>
//        /// <param name="u">First Barycentric Coordinate</param>
//        /// <param name="v">Second Barycentric Coordinate</param>
//        /// <returns>a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise</returns>
//        public static Vector2 BaryCentric(Vector2 a, Vector2 b, Vector2 c, double u, double v)
//        {
//            return a + u * (b - a) + v * (c - a);
//        }

//        /// <summary>Interpolate 3 Vectors using Barycentric coordinates</summary>
//        /// <param name="a">First input Vector.</param>
//        /// <param name="b">Second input Vector.</param>
//        /// <param name="c">Third input Vector.</param>
//        /// <param name="u">First Barycentric Coordinate.</param>
//        /// <param name="v">Second Barycentric Coordinate.</param>
//        /// <param name="result">Output Vector. a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise</param>
//        public static void BaryCentric(ref Vector2 a, ref Vector2 b, ref Vector2 c, double u, double v,
//            out Vector2 result)
//        {
//            result = a; // copy

//            Vector2 temp = b; // copy
//            Subtract(ref temp, ref a, out temp);
//            Multiply(ref temp, u, out temp);
//            Add(ref result, ref temp, out result);

//            temp = c; // copy
//            Subtract(ref temp, ref a, out temp);
//            Multiply(ref temp, v, out temp);
//            Add(ref result, ref temp, out result);
//        }

//        #endregion Barycentric

//        #region Transform

//        /// <summary>
//        /// Transforms a vector by a quaternion rotation.
//        /// </summary>
//        /// <param name="vec">The vector to transform.</param>
//        /// <param name="quat">The quaternion to rotate the vector by.</param>
//        /// <returns>The result of the operation.</returns>


//        /// <summary>
//        /// Transforms a vector by a quaternion rotation.
//        /// </summary>
//        /// <param name="vec">The vector to transform.</param>
//        /// <param name="quat">The quaternion to rotate the vector by.</param>
//        /// <param name="result">The result of the operation.</param>


//        #endregion Transform

//        #region ComponentMin

//        /// <summary>
//        /// Calculate the component-wise minimum of two vectors
//        /// </summary>
//        /// <param name="a">First operand</param>
//        /// <param name="b">Second operand</param>
//        /// <returns>The component-wise minimum</returns>
//        public static Vector2 ComponentMin(Vector2 a, Vector2 b)
//        {
//            a.X = a.X < b.X ? a.X : b.X;
//            a.Y = a.Y < b.Y ? a.Y : b.Y;
//            return a;
//        }

//        /// <summary>
//        /// Calculate the component-wise minimum of two vectors
//        /// </summary>
//        /// <param name="a">First operand</param>
//        /// <param name="b">Second operand</param>
//        /// <param name="result">The component-wise minimum</param>
//        public static void ComponentMin(ref Vector2 a, ref Vector2 b, out Vector2 result)
//        {
//            result.X = a.X < b.X ? a.X : b.X;
//            result.Y = a.Y < b.Y ? a.Y : b.Y;
//        }

//        #endregion ComponentMin

//        #region ComponentMax

//        /// <summary>
//        /// Calculate the component-wise maximum of two vectors
//        /// </summary>
//        /// <param name="a">First operand</param>
//        /// <param name="b">Second operand</param>
//        /// <returns>The component-wise maximum</returns>
//        public static Vector2 ComponentMax(Vector2 a, Vector2 b)
//        {
//            a.X = a.X > b.X ? a.X : b.X;
//            a.Y = a.Y > b.Y ? a.Y : b.Y;
//            return a;
//        }

//        /// <summary>
//        /// Calculate the component-wise maximum of two vectors
//        /// </summary>
//        /// <param name="a">First operand</param>
//        /// <param name="b">Second operand</param>
//        /// <param name="result">The component-wise maximum</param>
//        public static void ComponentMax(ref Vector2 a, ref Vector2 b, out Vector2 result)
//        {
//            result.X = a.X > b.X ? a.X : b.X;
//            result.Y = a.Y > b.Y ? a.Y : b.Y;
//        }

//        #endregion ComponentMax

//        #endregion Static

//        #region Operators

//        /// <summary>
//        /// Adds two instances.
//        /// </summary>
//        /// <param name="left">The left instance.</param>
//        /// <param name="right">The right instance.</param>
//        /// <returns>The result of the operation.</returns>
//        public static Vector2 operator +(Vector2 left, Vector2 right)
//        {
//            left.X += right.X;
//            left.Y += right.Y;
//            return left;
//        }

//        /// <summary>
//        /// Subtracts two instances.
//        /// </summary>
//        /// <param name="left">The left instance.</param>
//        /// <param name="right">The right instance.</param>
//        /// <returns>The result of the operation.</returns>
//        public static Vector2 operator -(Vector2 left, Vector2 right)
//        {
//            left.X -= right.X;
//            left.Y -= right.Y;
//            return left;
//        }

//        /// <summary>
//        /// Negates an instance.
//        /// </summary>
//        /// <param name="vec">The instance.</param>
//        /// <returns>The result of the operation.</returns>
//        public static Vector2 operator -(Vector2 vec)
//        {
//            vec.X = -vec.X;
//            vec.Y = -vec.Y;
//            return vec;
//        }

//        /// <summary>
//        /// Multiplies an instance by a scalar.
//        /// </summary>
//        /// <param name="vec">The instance.</param>
//        /// <param name="f">The scalar.</param>
//        /// <returns>The result of the operation.</returns>
//        public static Vector2 operator *(Vector2 vec, double f)
//        {
//            vec.X *= f;
//            vec.Y *= f;
//            return vec;
//        }

//        /// <summary>
//        /// Multiply an instance by a scalar.
//        /// </summary>
//        /// <param name="f">The scalar.</param>
//        /// <param name="vec">The instance.</param>
//        /// <returns>The result of the operation.</returns>
//        public static Vector2 operator *(double f, Vector2 vec)
//        {
//            vec.X *= f;
//            vec.Y *= f;
//            return vec;
//        }

//        /// <summary>
//        /// Divides an instance by a scalar.
//        /// </summary>
//        /// <param name="vec">The instance.</param>
//        /// <param name="f">The scalar.</param>
//        /// <returns>The result of the operation.</returns>
//        public static Vector2 operator /(Vector2 vec, double f)
//        {
//            double mult = 1.0 / f;
//            vec.X *= mult;
//            vec.Y *= mult;
//            return vec;
//        }

//        /// <summary>
//        /// Divides a scaler by an instance components wise.
//        /// </summary>
//        /// <param name="vec">The scalar.</param>
//        /// <param name="f">The instance.</param>
//        /// <returns>The result of the operation.</returns>
//        public static Vector2 operator /(double f, Vector2 vec)
//        {
//            vec.X = f / vec.X;
//            vec.Y = f / vec.Y;
//            return vec;
//        }

//        /// <summary>
//        /// Compares two instances for equality.
//        /// </summary>
//        /// <param name="left">The left instance.</param>
//        /// <param name="right">The right instance.</param>
//        /// <returns>True, if both instances are equal; false otherwise.</returns>
//        public static bool operator ==(Vector2 left, Vector2 right)
//        {
//            return left.Equals(right);
//        }

//        /// <summary>
//        /// Compares two instances for ienquality.
//        /// </summary>
//        /// <param name="left">The left instance.</param>
//        /// <param name="right">The right instance.</param>
//        /// <returns>True, if the instances are not equal; false otherwise.</returns>
//        public static bool operator !=(Vector2 left, Vector2 right)
//        {
//            return !left.Equals(right);
//        }

//        #endregion Operators

//        #region Overrides

//        #region public override string ToString()

//        /// <summary>
//        /// Returns a System.String that represents the current instance.
//        /// </summary>
//        /// <returns></returns>
//        public override string ToString()
//        {
//            return String.Format("({0}, {1})", X, Y);
//        }

//        #endregion public override string ToString()

//        #region public override int GetHashCode()

//        /// <summary>
//        /// Returns the hashcode for this instance.
//        /// </summary>
//        /// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
//        public override int GetHashCode()
//        {
//            return new {X, Y}.GetHashCode();
//        }

//        #endregion public override int GetHashCode()

//        #region public override bool Equals(object obj)

//        /// <summary>
//        /// Indicates whether this instance and a specified object are equal.
//        /// </summary>
//        /// <param name="obj">The object to compare to.</param>
//        /// <returns>True if the instances are equal; false otherwise.</returns>
//        public override bool Equals(object obj)
//        {
//            if (!(obj is Vector2))
//                return false;

//            return this.Equals((Vector2) obj);
//        }

//        #endregion public override bool Equals(object obj)

//        #endregion Overrides

//        #endregion Public Members

//        #region IEquatable<Vector2d> Members

//        /// <summary>Indicates whether the current vector is equal to another vector.</summary>
//        /// <param name="other">A vector to compare with this vector.</param>
//        /// <returns>true if the current vector is equal to the vector parameter; otherwise, false.</returns>
//        public bool Equals(Vector2 other)
//        {
//            return
//                X == other.X &&
//                Y == other.Y;
//        }

//        /// <summary>Indicates whether the current vector is equal to another vector.</summary>
//        /// <param name="other">A vector to compare with this vector.</param>
//        /// <returns>true if the current vector is equal to the vector parameter; otherwise, false.</returns>
//        public bool Equals(Vector2 other, double errorRange)
//        {
//            if ((X < other.X + errorRange && X > other.X - errorRange) &&
//                (Y < other.Y + errorRange && Y > other.Y - errorRange))
//            {
//                return true;
//            }

//            return false;
//        }

//        #endregion IEquatable<Vector2d> Members
//    }

//    public static class Vector2Ex
//    {
//        public static double PolygonLength(this List<Vector2> polygon, bool isClosed = true)
//        {
//            var length = 0.0;
//            if (polygon.Count > 1)
//            {
//                var previousPoint = polygon[0];
//                if (isClosed)
//                {
//                    previousPoint = polygon[polygon.Count - 1];
//                }

//                for (int i = isClosed ? 0 : 1; i < polygon.Count; i++)
//                {
//                    var currentPoint = polygon[i];
//                    length += (previousPoint - currentPoint).Length;
//                    previousPoint = currentPoint;
//                }
//            }

//            return length;
//        }

//        public static double LengthTo(this List<Vector2> polygon, int index, bool isClosed = true)
//        {
//            var length = 0.0;
//            index = Math.Max(0, Math.Min(polygon.Count - 1, index));
//            for (int i = 1; i <= index; i++)
//            {
//                length += (polygon[i] - polygon[i - 1]).Length;
//            }

//            return length;
//        }

//        /// <summary>
//        /// Get the position of a point that is lengthFromStart distance around the perimeter.
//        /// </summary>
//        /// <param name="polygon">The polygon to find the position on</param>
//        /// <param name="lengthFromStart">The distance around the perimeter form the start</param>
//        /// <param name="closed">The polygon loops back on itself. There is a segment between the
//        /// last and the first point, if they are not the same</param>
//        /// <returns>The position on the perimeter</returns>
//        public static Vector2 GetPositionAt(this List<Vector2> polygon, double lengthFromStart, bool closed = true)
//        {
//            var totalLength = polygon.PolygonLength();
//            if (lengthFromStart > totalLength)
//            {
//                if (closed)
//                {
//                    var ratio = lengthFromStart / totalLength;
//                    var times = (int) ratio;
//                    var remainder = ratio - times;
//                    lengthFromStart = remainder * totalLength;
//                }
//                else
//                {
//                    return polygon[polygon.Count - 1];
//                }
//            }
//            else if (lengthFromStart <= 0)
//            {
//                if (closed)
//                {
//                    var ratio = lengthFromStart / totalLength;
//                    var times = (int) ratio;
//                    var remainder = ratio - times;
//                    lengthFromStart = (1 + remainder) * totalLength;
//                }
//                else
//                {
//                    return polygon[0];
//                }
//            }

//            var position = new Vector2();
//            var length = 0.0;
//            if (polygon.Count > 1)
//            {
//                position = polygon[0];
//                var currentPoint = polygon[0];

//                int polygonCount = polygon.Count;
//                for (int i = 1; i < (closed ? polygonCount + 1 : polygonCount); i++)
//                {
//                    var nextPoint = polygon[(polygonCount + i) % polygonCount];
//                    var segmentLength = (nextPoint - currentPoint).Length;
//                    if (length + segmentLength > lengthFromStart)
//                    {
//                        // return the distance along this segment
//                        var distanceAlongThisSegment = lengthFromStart - length;
//                        var delteFromCurrent = (nextPoint - currentPoint) * distanceAlongThisSegment / segmentLength;
//                        return currentPoint + delteFromCurrent;
//                    }

//                    position = nextPoint;

//                    length += segmentLength;
//                    currentPoint = nextPoint;
//                }
//            }

//            return position;
//        }

//        public static double GetTurnAmount(this Vector2 currentPoint, Vector2 prevPoint, Vector2 nextPoint)
//        {
//            if (prevPoint != currentPoint
//                && currentPoint != nextPoint
//                && nextPoint != prevPoint)
//            {
//                prevPoint = currentPoint - prevPoint;
//                nextPoint -= currentPoint;

//                double prevAngle = Math.Atan2(prevPoint.Y, prevPoint.X);
//                Vector2 rotatedPrev = prevPoint.GetRotated(-prevAngle);

//                // undo the rotation
//                nextPoint = nextPoint.GetRotated(-prevAngle);
//                double angle = Math.Atan2(nextPoint.Y, nextPoint.X);

//                return angle;
//            }

//            return 0;
//        }

//    }

//    public struct Vector4 : IEquatable<Vector4>
//    {
//        #region Fields

//        /// <summary>
//        /// The X component of the Vector4d.
//        /// </summary>
//        public double X;

//        /// <summary>
//        /// The Y component of the Vector4d.
//        /// </summary>
//        public double Y;

//        /// <summary>
//        /// The Z component of the Vector4d.
//        /// </summary>
//        public double Z;

//        /// <summary>
//        /// The W component of the Vector4d.
//        /// </summary>
//        public double W;

//        /// <summary>
//        /// Defines a unit-length Vector4d that points towards the X-axis.
//        /// </summary>
//        public static Vector4 UnitX = new Vector4(1, 0, 0, 0);

//        /// <summary>
//        /// Defines a unit-length Vector4d that points towards the Y-axis.
//        /// </summary>
//        public static Vector4 UnitY = new Vector4(0, 1, 0, 0);

//        /// <summary>
//        /// Defines a unit-length Vector4d that points towards the Z-axis.
//        /// </summary>
//        public static Vector4 UnitZ = new Vector4(0, 0, 1, 0);

//        /// <summary>
//        /// Defines a unit-length Vector4d that points towards the W-axis.
//        /// </summary>
//        public static Vector4 UnitW = new Vector4(0, 0, 0, 1);

//        /// <summary>
//        /// Defines a zero-length Vector4d.
//        /// </summary>
//        public static Vector4 Zero = new Vector4(0, 0, 0, 0);

//        /// <summary>
//        /// Defines an instance with all components set to 1.
//        /// </summary>
//        public static readonly Vector4 One = new Vector4(1, 1, 1, 1);

//        /// <summary>
//        /// Defines the size of the Vector4d struct in bytes.
//        /// </summary>
//        public static readonly int SizeInBytes = Marshal.SizeOf(new Vector4());

//        #endregion Fields

//        #region Constructors

//        /// <summary>
//        /// Constructs a new Vector4d.
//        /// </summary>
//        /// <param name="x">The x component of the Vector4d.</param>
//        /// <param name="y">The y component of the Vector4d.</param>
//        /// <param name="z">The z component of the Vector4d.</param>
//        /// <param name="w">The w component of the Vector4d.</param>
//        public Vector4(double x, double y, double z, double w)
//        {
//            this.X = x;
//            this.Y = y;
//            this.Z = z;
//            this.W = w;
//        }

//        /// <summary>
//        /// Constructs a new Vector4d from the given Vector2d.
//        /// </summary>
//        /// <param name="v">The Vector2d to copy components from.</param>
//        public Vector4(Vector2 v)
//        {
//            X = v.X;
//            Y = v.Y;
//            Z = 0.0f;
//            W = 0.0f;
//        }

//        /// <summary>
//        /// Constructs a new Vector4d from the given Vector3d.
//        /// </summary>
//        /// <param name="v">The Vector3d to copy components from.</param>
//        public Vector4(Vector3 v)
//        {
//            X = v.X;
//            Y = v.Y;
//            Z = v.Z;
//            W = 0.0f;
//        }

//        /// <summary>
//        /// Constructs a new Vector4d from the specified Vector3d and w component.
//        /// </summary>
//        /// <param name="v">The Vector3d to copy components from.</param>
//        /// <param name="w">The w component of the new Vector4.</param>
//        public Vector4(Vector3 v, double w)
//        {
//            X = v.X;
//            Y = v.Y;
//            Z = v.Z;
//            this.W = w;
//        }

//        /// <summary>
//        /// Constructs a new Vector4d from the given Vector4d.
//        /// </summary>
//        /// <param name="v">The Vector4d to copy components from.</param>
//        public Vector4(Vector4 v)
//        {
//            X = v.X;
//            Y = v.Y;
//            Z = v.Z;
//            W = v.W;
//        }

//        #endregion Constructors

//        public static Vector4 Parse(string s)
//        {
//            var result = Vector4.Zero;

//            var values = s.Split(',').Select(sValue =>
//            {
//                double.TryParse(sValue, out double number);
//                return number;
//            }).ToArray();

//            for (int i = 0; i < Math.Min(4, values.Length); i++)
//            {
//                result[i] = values[i];
//            }

//            return result;
//        }


//        #region Public Members

//        #region Properties

//        public double this[int index]
//        {
//            get
//            {
//                switch (index)
//                {
//                    case 0:
//                        return X;

//                    case 1:
//                        return Y;

//                    case 2:
//                        return Z;

//                    case 3:
//                        return W;

//                    default:
//                        return 0;
//                }
//            }

//            set
//            {
//                switch (index)
//                {
//                    case 0:
//                        X = value;
//                        break;

//                    case 1:
//                        Y = value;
//                        break;

//                    case 2:
//                        Z = value;
//                        break;

//                    case 3:
//                        W = value;
//                        break;

//                    default:
//                        throw new Exception();
//                }
//            }
//        }

//        #endregion Properties

//        #region Instance

//        #region public double Length

//        /// <summary>
//        /// Gets the length (magnitude) of the vector.
//        /// </summary>
//        /// <see cref="LengthFast"/>
//        /// <seealso cref="LengthSquared"/>
//        public double Length
//        {
//            get { return System.Math.Sqrt(X * X + Y * Y + Z * Z + W * W); }
//        }

//        #endregion public double Length

//        #region public double LengthSquared

//        /// <summary>
//        /// Gets the square of the vector length (magnitude).
//        /// </summary>
//        /// <remarks>
//        /// This property avoids the costly square root operation required by the Length property. This makes it more suitable
//        /// for comparisons.
//        /// </remarks>
//        /// <see cref="Length"/>
//        public double LengthSquared
//        {
//            get { return X * X + Y * Y + Z * Z + W * W; }
//        }

//        #endregion public double LengthSquared

//        #region public void Normalize()

//        /// <summary>
//        /// Scales the Vector4d to unit length.
//        /// </summary>
//        public void Normalize()
//        {
//            double scale = 1.0 / this.Length;
//            X *= scale;
//            Y *= scale;
//            Z *= scale;
//            W *= scale;
//        }

//        #endregion public void Normalize()

//        public bool IsValid()
//        {
//            if (double.IsNaN(X) || double.IsInfinity(X)
//                                || double.IsNaN(Y) || double.IsInfinity(Y)
//                                || double.IsNaN(Z) || double.IsInfinity(Z)
//                                || double.IsNaN(W) || double.IsInfinity(W))
//            {
//                return false;
//            }

//            return true;
//        }

//        #endregion Instance

//        #region Static

//        #region Add

//        /// <summary>
//        /// Adds two vectors.
//        /// </summary>
//        /// <param name="a">Left operand.</param>
//        /// <param name="b">Right operand.</param>
//        /// <returns>Result of operation.</returns>
//        public static Vector4 Add(Vector4 a, Vector4 b)
//        {
//            Add(ref a, ref b, out a);
//            return a;
//        }

//        /// <summary>
//        /// Adds two vectors.
//        /// </summary>
//        /// <param name="a">Left operand.</param>
//        /// <param name="b">Right operand.</param>
//        /// <param name="result">Result of operation.</param>
//        public static void Add(ref Vector4 a, ref Vector4 b, out Vector4 result)
//        {
//            result = new Vector4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
//        }

//        #endregion Add

//        #region Subtract

//        /// <summary>
//        /// Subtract one Vector from another
//        /// </summary>
//        /// <param name="a">First operand</param>
//        /// <param name="b">Second operand</param>
//        /// <returns>Result of subtraction</returns>
//        public static Vector4 Subtract(Vector4 a, Vector4 b)
//        {
//            Subtract(ref a, ref b, out a);
//            return a;
//        }

//        /// <summary>
//        /// Subtract one Vector from another
//        /// </summary>
//        /// <param name="a">First operand</param>
//        /// <param name="b">Second operand</param>
//        /// <param name="result">Result of subtraction</param>
//        public static void Subtract(ref Vector4 a, ref Vector4 b, out Vector4 result)
//        {
//            result = new Vector4(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
//        }

//        #endregion Subtract

//        #region Multiply

//        /// <summary>
//        /// Multiplies a vector by a scalar.
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <returns>Result of the operation.</returns>
//        public static Vector4 Multiply(Vector4 vector, double scale)
//        {
//            Multiply(ref vector, scale, out vector);
//            return vector;
//        }

//        /// <summary>
//        /// Multiplies a vector by a scalar.
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <param name="result">Result of the operation.</param>
//        public static void Multiply(ref Vector4 vector, double scale, out Vector4 result)
//        {
//            result = new Vector4(vector.X * scale, vector.Y * scale, vector.Z * scale, vector.W * scale);
//        }

//        /// <summary>
//        /// Multiplies a vector by the components a vector (scale).
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <returns>Result of the operation.</returns>
//        public static Vector4 Multiply(Vector4 vector, Vector4 scale)
//        {
//            Multiply(ref vector, ref scale, out vector);
//            return vector;
//        }

//        /// <summary>
//        /// Multiplies a vector by the components of a vector (scale).
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <param name="result">Result of the operation.</param>
//        public static void Multiply(ref Vector4 vector, ref Vector4 scale, out Vector4 result)
//        {
//            result = new Vector4(vector.X * scale.X, vector.Y * scale.Y, vector.Z * scale.Z, vector.W * scale.W);
//        }

//        #endregion Multiply

//        #region Divide

//        /// <summary>
//        /// Divides a vector by a scalar.
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <returns>Result of the operation.</returns>
//        public static Vector4 Divide(Vector4 vector, double scale)
//        {
//            Divide(ref vector, scale, out vector);
//            return vector;
//        }

//        /// <summary>
//        /// Divides a vector by a scalar.
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <param name="result">Result of the operation.</param>
//        public static void Divide(ref Vector4 vector, double scale, out Vector4 result)
//        {
//            Multiply(ref vector, 1 / scale, out result);
//        }

//        /// <summary>
//        /// Divides a vector by the components of a vector (scale).
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <returns>Result of the operation.</returns>
//        public static Vector4 Divide(Vector4 vector, Vector4 scale)
//        {
//            Divide(ref vector, ref scale, out vector);
//            return vector;
//        }

//        /// <summary>
//        /// Divide a vector by the components of a vector (scale).
//        /// </summary>
//        /// <param name="vector">Left operand.</param>
//        /// <param name="scale">Right operand.</param>
//        /// <param name="result">Result of the operation.</param>
//        public static void Divide(ref Vector4 vector, ref Vector4 scale, out Vector4 result)
//        {
//            result = new Vector4(vector.X / scale.X, vector.Y / scale.Y, vector.Z / scale.Z, vector.W / scale.W);
//        }

//        #endregion Divide

//        #region Min

//        /// <summary>
//        /// Calculate the component-wise minimum of two vectors
//        /// </summary>
//        /// <param name="a">First operand</param>
//        /// <param name="b">Second operand</param>
//        /// <returns>The component-wise minimum</returns>
//        public static Vector4 Min(Vector4 a, Vector4 b)
//        {
//            a.X = a.X < b.X ? a.X : b.X;
//            a.Y = a.Y < b.Y ? a.Y : b.Y;
//            a.Z = a.Z < b.Z ? a.Z : b.Z;
//            a.W = a.W < b.W ? a.W : b.W;
//            return a;
//        }

//        /// <summary>
//        /// Calculate the component-wise minimum of two vectors
//        /// </summary>
//        /// <param name="a">First operand</param>
//        /// <param name="b">Second operand</param>
//        /// <param name="result">The component-wise minimum</param>
//        public static void Min(ref Vector4 a, ref Vector4 b, out Vector4 result)
//        {
//            result.X = a.X < b.X ? a.X : b.X;
//            result.Y = a.Y < b.Y ? a.Y : b.Y;
//            result.Z = a.Z < b.Z ? a.Z : b.Z;
//            result.W = a.W < b.W ? a.W : b.W;
//        }

//        #endregion Min

//        #region Max

//        /// <summary>
//        /// Calculate the component-wise maximum of two vectors
//        /// </summary>
//        /// <param name="a">First operand</param>
//        /// <param name="b">Second operand</param>
//        /// <returns>The component-wise maximum</returns>
//        public static Vector4 Max(Vector4 a, Vector4 b)
//        {
//            a.X = a.X > b.X ? a.X : b.X;
//            a.Y = a.Y > b.Y ? a.Y : b.Y;
//            a.Z = a.Z > b.Z ? a.Z : b.Z;
//            a.W = a.W > b.W ? a.W : b.W;
//            return a;
//        }

//        /// <summary>
//        /// Calculate the component-wise maximum of two vectors
//        /// </summary>
//        /// <param name="a">First operand</param>
//        /// <param name="b">Second operand</param>
//        /// <param name="result">The component-wise maximum</param>
//        public static void Max(ref Vector4 a, ref Vector4 b, out Vector4 result)
//        {
//            result.X = a.X > b.X ? a.X : b.X;
//            result.Y = a.Y > b.Y ? a.Y : b.Y;
//            result.Z = a.Z > b.Z ? a.Z : b.Z;
//            result.W = a.W > b.W ? a.W : b.W;
//        }

//        #endregion Max

//        #region Clamp

//        /// <summary>
//        /// Clamp a vector to the given minimum and maximum vectors
//        /// </summary>
//        /// <param name="vec">Input vector</param>
//        /// <param name="min">Minimum vector</param>
//        /// <param name="max">Maximum vector</param>
//        /// <returns>The clamped vector</returns>
//        public static Vector4 Clamp(Vector4 vec, Vector4 min, Vector4 max)
//        {
//            vec.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
//            vec.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
//            vec.Z = vec.X < min.Z ? min.Z : vec.Z > max.Z ? max.Z : vec.Z;
//            vec.W = vec.Y < min.W ? min.W : vec.W > max.W ? max.W : vec.W;
//            return vec;
//        }

//        /// <summary>
//        /// Clamp a vector to the given minimum and maximum vectors
//        /// </summary>
//        /// <param name="vec">Input vector</param>
//        /// <param name="min">Minimum vector</param>
//        /// <param name="max">Maximum vector</param>
//        /// <param name="result">The clamped vector</param>
//        public static void Clamp(ref Vector4 vec, ref Vector4 min, ref Vector4 max, out Vector4 result)
//        {
//            result.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
//            result.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
//            result.Z = vec.X < min.Z ? min.Z : vec.Z > max.Z ? max.Z : vec.Z;
//            result.W = vec.Y < min.W ? min.W : vec.W > max.W ? max.W : vec.W;
//        }

//        #endregion Clamp

//        #region Normalize

//        /// <summary>
//        /// Scale a vector to unit length
//        /// </summary>
//        /// <param name="vec">The input vector</param>
//        /// <returns>The normalized vector</returns>
//        public static Vector4 Normalize(Vector4 vec)
//        {
//            double scale = 1.0 / vec.Length;
//            vec.X *= scale;
//            vec.Y *= scale;
//            vec.Z *= scale;
//            vec.W *= scale;
//            return vec;
//        }

//        /// <summary>
//        /// Scale a vector to unit length
//        /// </summary>
//        /// <param name="vec">The input vector</param>
//        /// <param name="result">The normalized vector</param>
//        public static void Normalize(ref Vector4 vec, out Vector4 result)
//        {
//            double scale = 1.0 / vec.Length;
//            result.X = vec.X * scale;
//            result.Y = vec.Y * scale;
//            result.Z = vec.Z * scale;
//            result.W = vec.W * scale;
//        }

//        #endregion Normalize

//        #region Dot

//        /// <summary>
//        /// Calculate the dot product of two vectors
//        /// </summary>
//        /// <param name="left">First operand</param>
//        /// <param name="right">Second operand</param>
//        /// <returns>The dot product of the two inputs</returns>
//        public static double Dot(Vector4 left, Vector4 right)
//        {
//            return left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
//        }

//        /// <summary>
//        /// Calculate the dot product of two vectors
//        /// </summary>
//        /// <param name="left">First operand</param>
//        /// <param name="right">Second operand</param>
//        /// <param name="result">The dot product of the two inputs</param>
//        public static void Dot(ref Vector4 left, ref Vector4 right, out double result)
//        {
//            result = left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
//        }

//        #endregion Dot

//        #region Lerp

//        /// <summary>
//        /// Returns a new Vector that is the linear blend of the 2 given Vectors
//        /// </summary>
//        /// <param name="a">First input vector</param>
//        /// <param name="b">Second input vector</param>
//        /// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
//        /// <returns>a when blend=0, b when blend=1, and a linear combination otherwise</returns>
//        public static Vector4 Lerp(Vector4 a, Vector4 b, double blend)
//        {
//            a.X = blend * (b.X - a.X) + a.X;
//            a.Y = blend * (b.Y - a.Y) + a.Y;
//            a.Z = blend * (b.Z - a.Z) + a.Z;
//            a.W = blend * (b.W - a.W) + a.W;
//            return a;
//        }

//        /// <summary>
//        /// Returns a new Vector that is the linear blend of the 2 given Vectors
//        /// </summary>
//        /// <param name="a">First input vector</param>
//        /// <param name="b">Second input vector</param>
//        /// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
//        /// <param name="result">a when blend=0, b when blend=1, and a linear combination otherwise</param>
//        public static void Lerp(ref Vector4 a, ref Vector4 b, double blend, out Vector4 result)
//        {
//            result.X = blend * (b.X - a.X) + a.X;
//            result.Y = blend * (b.Y - a.Y) + a.Y;
//            result.Z = blend * (b.Z - a.Z) + a.Z;
//            result.W = blend * (b.W - a.W) + a.W;
//        }

//        #endregion Lerp

//        #region Barycentric

//        /// <summary>
//        /// Interpolate 3 Vectors using Barycentric coordinates
//        /// </summary>
//        /// <param name="a">First input Vector</param>
//        /// <param name="b">Second input Vector</param>
//        /// <param name="c">Third input Vector</param>
//        /// <param name="u">First Barycentric Coordinate</param>
//        /// <param name="v">Second Barycentric Coordinate</param>
//        /// <returns>a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise</returns>
//        public static Vector4 BaryCentric(Vector4 a, Vector4 b, Vector4 c, double u, double v)
//        {
//            return a + u * (b - a) + v * (c - a);
//        }

//        /// <summary>Interpolate 3 Vectors using Barycentric coordinates</summary>
//        /// <param name="a">First input Vector.</param>
//        /// <param name="b">Second input Vector.</param>
//        /// <param name="c">Third input Vector.</param>
//        /// <param name="u">First Barycentric Coordinate.</param>
//        /// <param name="v">Second Barycentric Coordinate.</param>
//        /// <param name="result">Output Vector. a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise</param>
//        public static void BaryCentric(ref Vector4 a, ref Vector4 b, ref Vector4 c, double u, double v,
//            out Vector4 result)
//        {
//            result = a; // copy

//            Vector4 temp = b; // copy
//            Subtract(ref temp, ref a, out temp);
//            Multiply(ref temp, u, out temp);
//            Add(ref result, ref temp, out result);

//            temp = c; // copy
//            Subtract(ref temp, ref a, out temp);
//            Multiply(ref temp, v, out temp);
//            Add(ref result, ref temp, out result);
//        }

//        #endregion Barycentric

//        #region Transform

//        /// <summary>Transform a Vector by the given Matrix</summary>
//        /// <param name="vec">The vector to transform</param>
//        /// <param name="mat">The desired transformation</param>
//        /// <returns>The transformed vector</returns>
//        public static Vector4 Transform(Vector4 vec, Matrix4X4 mat)
//        {
//            Vector4 result;
//            Transform(vec, ref mat, out result);
//            return result;
//        }

//        /// <summary>Transform a Vector by the given Matrix</summary>
//        /// <param name="vec">The vector to transform</param>
//        /// <param name="mat">The desired transformation</param>
//        /// <param name="result">The transformed vector</param>
//        public static void Transform(Vector4 vec, ref Matrix4X4 mat, out Vector4 result)
//        {
//            result = new Vector4(
//                vec.X * mat.Row0.X + vec.Y * mat.Row1.X + vec.Z * mat.Row2.X + vec.W * mat.Row3.X,
//                vec.X * mat.Row0.Y + vec.Y * mat.Row1.Y + vec.Z * mat.Row2.Y + vec.W * mat.Row3.Y,
//                vec.X * mat.Row0.Z + vec.Y * mat.Row1.Z + vec.Z * mat.Row2.Z + vec.W * mat.Row3.Z,
//                vec.X * mat.Row0.W + vec.Y * mat.Row1.W + vec.Z * mat.Row2.W + vec.W * mat.Row3.W);
//        }

//        /// <summary>
//        /// Transforms a vector by a quaternion rotation.
//        /// </summary>
//        /// <param name="vec">The vector to transform.</param>
//        /// <param name="quat">The quaternion to rotate the vector by.</param>
//        /// <returns>The result of the operation.</returns>
       

//        /// <summary>
//        /// Transforms a vector by a quaternion rotation.
//        /// </summary>
//        /// <param name="vec">The vector to transform.</param>
//        /// <param name="quat">The quaternion to rotate the vector by.</param>
//        /// <param name="result">The result of the operation.</param>
       

//        #endregion Transform

//        #endregion Static

//        #region Swizzle

//        /// <summary>
//        /// Gets or sets an OpenTK.Vector2d with the X and Y components of this instance.
//        /// </summary>
//        public Vector2 Xy
//        {
//            get { return new Vector2(X, Y); }
//            set
//            {
//                X = value.X;
//                Y = value.Y;
//            }
//        }

//        /// <summary>
//        /// Gets or sets an OpenTK.Vector3d with the X, Y and Z components of this instance.
//        /// </summary>
//        public Vector3 Xyz
//        {
//            get { return new Vector3(X, Y, Z); }
//            set
//            {
//                X = value.X;
//                Y = value.Y;
//                Z = value.Z;
//            }
//        }

//        #endregion Swizzle

//        #region Operators

//        /// <summary>
//        /// Adds two instances.
//        /// </summary>
//        /// <param name="left">The first instance.</param>
//        /// <param name="right">The second instance.</param>
//        /// <returns>The result of the calculation.</returns>
//        public static Vector4 operator +(Vector4 left, Vector4 right)
//        {
//            left.X += right.X;
//            left.Y += right.Y;
//            left.Z += right.Z;
//            left.W += right.W;
//            return left;
//        }

//        /// <summary>
//        /// Subtracts two instances.
//        /// </summary>
//        /// <param name="left">The first instance.</param>
//        /// <param name="right">The second instance.</param>
//        /// <returns>The result of the calculation.</returns>
//        public static Vector4 operator -(Vector4 left, Vector4 right)
//        {
//            left.X -= right.X;
//            left.Y -= right.Y;
//            left.Z -= right.Z;
//            left.W -= right.W;
//            return left;
//        }

//        /// <summary>
//        /// Negates an instance.
//        /// </summary>
//        /// <param name="vec">The instance.</param>
//        /// <returns>The result of the calculation.</returns>
//        public static Vector4 operator -(Vector4 vec)
//        {
//            vec.X = -vec.X;
//            vec.Y = -vec.Y;
//            vec.Z = -vec.Z;
//            vec.W = -vec.W;
//            return vec;
//        }

//        /// <summary>
//        /// Multiplies an instance by a scalar.
//        /// </summary>
//        /// <param name="vec">The instance.</param>
//        /// <param name="scale">The scalar.</param>
//        /// <returns>The result of the calculation.</returns>
//        public static Vector4 operator *(Vector4 vec, double scale)
//        {
//            vec.X *= scale;
//            vec.Y *= scale;
//            vec.Z *= scale;
//            vec.W *= scale;
//            return vec;
//        }

//        /// <summary>
//        /// Multiplies an instance by a scalar.
//        /// </summary>
//        /// <param name="scale">The scalar.</param>
//        /// <param name="vec">The instance.</param>
//        /// <returns>The result of the calculation.</returns>
//        public static Vector4 operator *(double scale, Vector4 vec)
//        {
//            vec.X *= scale;
//            vec.Y *= scale;
//            vec.Z *= scale;
//            vec.W *= scale;
//            return vec;
//        }

//        /// <summary>
//        /// Divides an instance by a scalar.
//        /// </summary>
//        /// <param name="vec">The instance.</param>
//        /// <param name="scale">The scalar.</param>
//        /// <returns>The result of the calculation.</returns>
//        public static Vector4 operator /(Vector4 vec, double scale)
//        {
//            double mult = 1 / scale;
//            vec.X *= mult;
//            vec.Y *= mult;
//            vec.Z *= mult;
//            vec.W *= mult;
//            return vec;
//        }

//        /// <summary>
//        /// Compares two instances for equality.
//        /// </summary>
//        /// <param name="left">The first instance.</param>
//        /// <param name="right">The second instance.</param>
//        /// <returns>True, if left equals right; false otherwise.</returns>
//        public static bool operator ==(Vector4 left, Vector4 right)
//        {
//            return left.Equals(right);
//        }

//        /// <summary>
//        /// Compares two instances for inequality.
//        /// </summary>
//        /// <param name="left">The first instance.</param>
//        /// <param name="right">The second instance.</param>
//        /// <returns>True, if left does not equa lright; false otherwise.</returns>
//        public static bool operator !=(Vector4 left, Vector4 right)
//        {
//            return !left.Equals(right);
//        }

//        #endregion Operators

//        #region Overrides

//        #region public override string ToString()

//        /// <summary>
//        /// Returns a System.String that represents the current Vector4d.
//        /// </summary>
//        /// <returns></returns>
//        public override string ToString()
//        {
//            return String.Format("{0}, {1}, {2}, {3}", X, Y, Z, W);
//        }

//        /// <summary>
//        /// Returns a System.String that represents the current Vector4d, formatting each element with format.
//        /// </summary>
//        /// <param name="format"></param>
//        /// <returns></returns>
//        public string ToString(string format = "")
//        {
//            return X.ToString(format) + ", " + Y.ToString(format) + ", " + Z.ToString(format) + ", " +
//                   W.ToString(format);
//        }

//        #endregion public override string ToString()

//        #region public override int GetHashCode()

//        /// <summary>
//        /// Returns the hashcode for this instance.
//        /// </summary>
//        /// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
//        public override int GetHashCode()
//        {
//            return new {X, Y, Z, W}.GetHashCode();
//        }

//        public static ulong GetLongHashCode(double data, ulong hash = 14695981039346656037)
//        {
//            return ComputeHash(BitConverter.GetBytes(data), hash);
//        }

//        // FNV-1a (64-bit) non-cryptographic hash function.
//        // Adapted from: http://github.com/jakedouglas/fnv-java
//        public static ulong ComputeHash(byte[] bytes, ulong hash = 14695981039346656037)
//        {
//            const ulong fnv64Prime = 0x100000001b3;

//            for (var i = 0; i < bytes.Length; i++)
//            {
//                hash = hash ^ bytes[i];
//                hash *= fnv64Prime;
//            }

//            return hash;
//        }

//        /// <summary>
//        /// return a 64 bit hash code proposed by Jon Skeet
//        // http://stackoverflow.com/questions/8094867/good-gethashcode-override-for-list-of-foo-objects-respecting-the-order
//        /// </summary>
//        /// <returns></returns>
//        public ulong GetLongHashCode(ulong hash = 14695981039346656037)
//        {
//            hash = GetLongHashCode(X, hash);
//            hash = GetLongHashCode(Y, hash);
//            hash = GetLongHashCode(Z, hash);
//            hash = GetLongHashCode(W, hash);

//            return hash;
//        }

//        #endregion public override int GetHashCode()

//        #region public override bool Equals(object obj)

//        /// <summary>
//        /// Indicates whether this instance and a specified object are equal.
//        /// </summary>
//        /// <param name="obj">The object to compare to.</param>
//        /// <returns>True if the instances are equal; false otherwise.</returns>
//        public override bool Equals(object obj)
//        {
//            if (!(obj is Vector4))
//                return false;

//            return this.Equals((Vector4) obj);
//        }

//        /// <summary>
//        /// Indicates whether this instance and a specified object are equal within an error range.
//        /// </summary>
//        /// <param name="OtherVector"></param>
//        /// <param name="ErrorValue"></param>
//        /// <returns>True if the instances are equal; false otherwise.</returns>
//        public bool Equals(Vector4 OtherVector, double ErrorValue)
//        {
//            if ((X < OtherVector.X + ErrorValue && X > OtherVector.X - ErrorValue) &&
//                (Y < OtherVector.Y + ErrorValue && Y > OtherVector.Y - ErrorValue) &&
//                (Z < OtherVector.Z + ErrorValue && Z > OtherVector.Z - ErrorValue) &&
//                (W < OtherVector.W + ErrorValue && W > OtherVector.W - ErrorValue))
//            {
//                return true;
//            }

//            return false;
//        }

//        #endregion public override bool Equals(object obj)

//        #endregion Overrides

//        #endregion Public Members



//        /// <summary>Indicates whether the current vector is equal to another vector.</summary>
//        /// <param name="other">A vector to compare with this vector.</param>
//        /// <returns>true if the current vector is equal to the vector parameter; otherwise, false.</returns>
//        public bool Equals(Vector4 other)
//        {
//            return
//                X == other.X &&
//                Y == other.Y &&
//                Z == other.Z &&
//                W == other.W;
//        }

//    }
//}
