using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

using SysMath = System.Math;

namespace MyoSharp.Math
{
    public class QuaternionF
    {
        #region Fields
        private readonly float _x;
        private readonly float _y;
        private readonly float _z;
        private readonly float _w;
        #endregion

        #region Constructors
        public QuaternionF()
            : this(0, 0, 0, 1)
        { 
        }

        public QuaternionF(float x, float y, float z, float w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }
        #endregion

        #region Properties
        public float X { get { return _x; } }

        public float Y { get { return _y; } }

        public float Z { get { return _z; } }

        public float W { get { return _w; } }
        #endregion

        #region Methods
        public static QuaternionF operator -(QuaternionF quat)
        {
            Contract.Requires<ArgumentNullException>(quat != null, "quat");
            Contract.Ensures(Contract.Result<QuaternionF>() != null);

            return new QuaternionF(-quat._x, -quat._y, -quat._z, -quat._w);
        }

        public static QuaternionF operator +(QuaternionF quat1, QuaternionF quat2)
        {
            Contract.Requires<ArgumentNullException>(quat1 != null, "quat1");
            Contract.Requires<ArgumentNullException>(quat2 != null, "quat2");
            Contract.Ensures(Contract.Result<QuaternionF>() != null);

            return new QuaternionF(
                quat1._x + quat2._x,
                quat1._y + quat2._y,
                quat1._z + quat2._z,
                quat1._w + quat2._w);
        }

        public static QuaternionF operator -(QuaternionF quat1, QuaternionF quat2)
        {
            Contract.Requires<ArgumentNullException>(quat1 != null, "quat1");
            Contract.Requires<ArgumentNullException>(quat2 != null, "quat2");
            Contract.Ensures(Contract.Result<QuaternionF>() != null);

            return quat1 + (-quat2);
        }

        public static QuaternionF operator *(QuaternionF quat, float scalar)
        {
            Contract.Requires<ArgumentNullException>(quat != null, "quat");
            Contract.Ensures(Contract.Result<QuaternionF>() != null);

            return new QuaternionF(
                quat._x * scalar,
                quat._y * scalar,
                quat._z * scalar,
                quat._w * scalar);
        }

        public static QuaternionF operator *(float scalar, QuaternionF quat)
        {
            Contract.Requires<ArgumentNullException>(quat != null, "quat");
            Contract.Ensures(Contract.Result<QuaternionF>() != null);

            return quat * scalar;
        }

        public static QuaternionF operator /(QuaternionF quat, float scalar)
        {
            Contract.Requires<ArgumentNullException>(quat != null, "quat");
            Contract.Ensures(Contract.Result<QuaternionF>() != null);

            return new QuaternionF(
                quat._x / scalar,
                quat._y / scalar,
                quat._z / scalar,
                quat._w / scalar);
        }

        public static QuaternionF operator *(QuaternionF quat1, QuaternionF quat2)
        {
            Contract.Requires<ArgumentNullException>(quat1 != null, "quat1");
            Contract.Requires<ArgumentNullException>(quat2 != null, "quat2");
            Contract.Ensures(Contract.Result<QuaternionF>() != null);

            return new QuaternionF(
                quat1._w * quat2._x + quat1._x * quat2._w + quat1._y * quat2._z - quat1._z * quat2._y,
                quat1._w * quat2._y - quat1._x * quat2._z + quat1._y * quat2._w + quat1._z * quat2._x,
                quat1._w * quat2._z + quat1._x * quat2._y - quat1._y * quat2._x + quat1._z * quat2._w,
                quat1._w * quat2._w - quat1._x * quat2._x - quat1._y * quat2._y - quat1._z * quat2._z);
        }

        public static Vector3F operator *(QuaternionF quat, Vector3F vec)
        {
            Contract.Requires<ArgumentNullException>(quat != null, "quat");
            Contract.Requires<ArgumentNullException>(vec != null, "vec");
            Contract.Ensures(Contract.Result<Vector3F>() != null);

            var qvec = new QuaternionF(vec.X, vec.Y, vec.Z, 0);
            var result = quat * qvec * quat.Conjugate();
            return new Vector3F(result.X, result.Y, result.Z);
        }

        public static QuaternionF Normalize(QuaternionF quat)
        {
            Contract.Requires<ArgumentNullException>(quat != null, "quat");
            Contract.Ensures(Contract.Result<QuaternionF>() != null);

            return (quat / quat.Magnitude());
        }

        public QuaternionF Conjugate()
        {
            Contract.Ensures(Contract.Result<QuaternionF>() != null);

            return new QuaternionF(-_x, -_y, -_z, _w);
        }

        public static float Roll(QuaternionF quat)
        {
            Contract.Requires<ArgumentNullException>(quat != null, "quat");

            return (float)SysMath.Atan2(
                2.0f * (quat._w * quat._x + quat._y * quat._z),
                1.0f - 2.0f * (quat._x * quat._x + quat._y * quat._y));
        }

        public static float Pitch(QuaternionF quat)
        {
            Contract.Requires<ArgumentNullException>(quat != null, "quat");

            return (float)SysMath.Asin(2.0f * (quat._w * quat._y - quat._z * quat._x));
        }

        public static float Yaw(QuaternionF quat)
        {
            Contract.Requires<ArgumentNullException>(quat != null, "quat");

            return (float)SysMath.Atan2(
                2.0f * (quat._w * quat._z + quat._x * quat._y),
                1.0f - 2.0f * (quat._y * quat._y + quat._z * quat._z));
        }

        public float Magnitude()
        {
            return (float)SysMath.Sqrt(_w * _w + _x * _x + _y * _y + _z * _z);
        }

        public override string ToString()
        {
            return String.Format("{0},{1},{2},{3}", X, Y, Z, W);
        }
        #endregion
    }
}
