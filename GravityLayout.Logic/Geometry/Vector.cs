﻿using System;
using System.Collections.Generic;

namespace GravityLayout.Logic.Geometry
{
    /// <summary>
    /// Implements a vector that can be set and get in either rectangular or polar coordinates,
    /// where the conversion is delayed until needed.
    /// Positive X axis is to the right.
    /// Positive Y axis is to the top.
    /// FiRad is in the range [0, 2*Pi).
    /// FiDeg is in the range [0, 360)
    /// </summary>
    public class Vector
    {
        private double? _x;
        private double? _y;
        private double? _length;
        private double? _fiRad;

        /// <summary>Positive X axis is to the right.</summary>
        public double X
        {
            get
            {
                if (_x is null)
                    CalculateXY();
                return (double)_x;
            }
            private set
            {
                if (_x != value)
                {
                    _x = value;
                    ResetPolar();
                }
            }
        }

        /// <summary>Positive Y axis is to the top.</summary>
        public double Y
        {
            get
            {
                if (_y is null)
                    CalculateXY();
                return (double)_y;
            }
            private set
            {
                if (_y != value)
                {
                    _y = value;
                    ResetPolar();
                }
            }
        }

        public double Length
        {
            get
            {
                if (_length is null)
                    CalculatePolar();
                return (double)_length;
            }
            set
            {
                if (_length != value)
                {
                    _length = value;
                    ResetXY();
                }
            }
        }

        /// <summary>
        /// Angle of the vector to the positive X axis, expressed in radians.
        /// Range is [0, 2*Pi).
        /// </summary>
        public double FiRad
        {
            get
            {
                if (_fiRad is null)
                    CalculatePolar();
                return (double)_fiRad;
            }
            set
            {
                if (_fiRad != value)
                {
                    _fiRad = value % (2 * Math.PI);
                    ResetXY();
                }
            }
        }

        /// <summary>
        /// Angle of the vector to the positive X axis, expressed in degrees.
        /// Range is [0, 360).
        /// </summary>
        public double FiDeg
        {
            get { return FiRad / Math.PI * 180.0; }
            set { FiRad = value * Math.PI / 180.0; }
        }

        /// <summary>
        /// The default constructor is hidden, to enforce construction either via coordinates or direction/length.
        /// </summary>
        private Vector()
        {
        }

        /// <summary>Creates a vector from its rectangular (X, Y) coordinates.</summary>
        public static Vector FromXY(double x, double y)
        {
            return new Vector { _x = x, _y = y, _length = null, _fiRad = null };
        }

        /// <summary>Creates a vector from its polar coordinates.</summary>
        public static Vector FromPolar(double length, double fiRad)
        {
            if (fiRad >= 2 * Math.PI)
                fiRad = fiRad % (2 * Math.PI);
            else if (fiRad < 0.0)
                fiRad = fiRad + (1 + (int)(fiRad / 2 / Math.PI)) * 2 * Math.PI;

            var v = new Vector { _x = null, _y = null, _length = length, _fiRad = fiRad };
            if (v.Length < 0)
                return v.Reverse();
            return v;
        }

        private void CalculateXY()
        {
            if (_length is null || _fiRad is null)
                throw new Exception("To calculate the X and Y coordinates of a vector, its direction and length must be known.");

            double length = (double)_length;
            double fiRad = (double)_fiRad;
            _x = length * Math.Cos((double)fiRad);
            _y = length * Math.Sin((double)fiRad);
        }

        private void ResetXY()
        {
            _x = null;
            _y = null;
        }

        private void CalculatePolar()
        {
            if (_x is null || _y is null)
                throw new Exception("To calculate the polar coordinates of a vector, its X and Y coordinates must be known.");

            double x = (double)_x;
            double y = (double)_y;
            _length = Math.Sqrt(x * x + y * y);
            _fiRad = Math.Atan2(y, x);
            if(_fiRad < 0.0)
                _fiRad = _fiRad + 2 * Math.PI;
        }

        private void ResetPolar()
        {
            _length = null;
            _fiRad = null;
        }

        public static Vector operator +(Vector v1, Vector v2) =>
            FromXY(v1.X + v2.X, v1.Y + v2.Y);

        public static Vector operator -(Vector v1, Vector v2) =>
            FromXY(v1.X - v2.X, v1.Y - v2.Y);

        public static Vector operator *(Vector v, double m) =>
            FromPolar(v.Length * m, v.FiRad);

        public Vector Reverse()
        {
            if (Length < 0)
                return FromPolar(-Length, FiRad);
            else
                return FromPolar(Length, FiRad + Math.PI);
        }

        public static Vector Sum(IEnumerable<Vector> vectors)
        {
            if (vectors is null)
                throw new ArgumentNullException(nameof(vectors));

            double sumX = 0;
            double sumY = 0;
            foreach (Vector vector in vectors)
            {
                sumX += vector.X;
                sumY += vector.Y;
            }

            return FromXY(sumX, sumY);
        }
    }
}