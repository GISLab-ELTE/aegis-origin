/// <copyright file="PrecisionModel.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://opensource.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Numerics;
using System;
using System.Linq;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a type defining precision of <see cref="IGeometry"/> instances.
    /// </summary>
    public class PrecisionModel : IComparable<PrecisionModel>, IEquatable<PrecisionModel>
    {
        #region Private fields

        /// <summary>
        /// The base tolerance value.
        /// </summary>
        private Double _baseTolerance;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the type of the model.
        /// </summary>
        /// <value>The type of the precision model.</value>
        public PrecisionModelType ModelType { get; private set; }

        /// <summary>
        /// Gets the maximum number of significant digits in the model.
        /// </summary>
        /// <value>The maximum number of significant digits in the precision model.</value>
        public Int32 MaximumSignificantDigits
        {
            get
            {
                switch (ModelType)
                {
                    case PrecisionModelType.FloatingSingle:
                        return 6;
                    case PrecisionModelType.Floating:
                        return 16;
                    default: // PrecisionModelType.Fixed
                        return 1 + Math.Max(0, (Int32)Math.Ceiling(Math.Log(Scale) / Math.Log(10)));
                }
            }
        }

        /// <summary>
        /// Gets the maximum precise value in the model.
        /// </summary>
        /// <value>The greatest precise value in the precision model.</value>
        public Double MaximumPreciseValue
        { 
            get 
            {
                switch (ModelType)
                {
                    case PrecisionModelType.FloatingSingle:
                        return 8388607.0;
                    case PrecisionModelType.Floating:
                        return 9007199254740992.0;
                    default: // PrecisionModelType.Fixed
                        return Math.Floor((9007199254740992.5 * Scale) / Scale);
                }
            }
        }

        /// <summary>
        /// Gets the scale of the fixed precision model.
        /// </summary>
        /// <value>The scale of the precision model if the type is fixed, otherwise <c>0</c>.</value>
        public Double Scale { get; private set; }

        /// <summary>
        /// Gets the smallest positive value representable by the specified precision.
        /// </summary>
        /// <value>The smallest positive value greater than 0, which is representable by the specified precision.</value>
        public Double Epsilon
        {
            get
            {
                switch (ModelType)
                {
                    case PrecisionModelType.FloatingSingle:
                        return Single.Epsilon;
                    case PrecisionModelType.Floating:
                        return Double.Epsilon;
                    default:
                        return Scale;
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PrecisionModel"/> class.
        /// </summary>
        public PrecisionModel()
        {
            ModelType = PrecisionModelType.Floating;
            _baseTolerance = 1 / Math.Pow(10, MaximumSignificantDigits - 1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrecisionModel"/> class.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        public PrecisionModel(PrecisionModelType modelType)
        {
            ModelType = modelType;
            _baseTolerance = 1 / Math.Pow(10, MaximumSignificantDigits - 1);

            if (modelType == PrecisionModelType.Fixed)
            {
                Scale = 1.0;
                _baseTolerance = 0.5;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrecisionModel"/> class.
        /// </summary>
        /// <param name="scale">The scale of the model.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The scale is equal to or less than 0.</exception>
        public PrecisionModel(Double scale)
        {
            if (scale <= 0)
                throw new ArgumentOutOfRangeException("scale", "The scale is equal to or less than 0.");

            ModelType = PrecisionModelType.Fixed;
            Scale = scale;
            _baseTolerance = 0.5 / Scale;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Rounds the specified value to match the precision model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The precise value.</returns>
        public Double MakePrecise(Double value)
        {
            if (Double.IsNaN(value)) 
                return value;

            switch (ModelType)
            { 
                case PrecisionModelType.FloatingSingle:
                    return (Single)value;
                case PrecisionModelType.Fixed:
                    return Math.Floor((value * Scale) + 0.5) / Scale;
                default:
                    return value;
            }
        }

        /// <summary>
        /// Rounds the specified coordinate to match the precision model.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The precise coordinate.</returns>
        public Coordinate MakePrecise(Coordinate coordinate)
        {
            switch (ModelType)
            {
                case PrecisionModelType.FloatingSingle:
                    return new Coordinate((Single)coordinate.X, (Single)coordinate.Y, (Single)coordinate.Z);
                case PrecisionModelType.Fixed:
                    return new Coordinate(Math.Floor((coordinate.X * Scale) + 0.5) / Scale, 
                                          Math.Floor((coordinate.Y * Scale) + 0.5) / Scale, 
                                          Math.Floor((coordinate.Z * Scale) + 0.5) / Scale);
                default:
                    return coordinate;
            }
        }

        /// <summary>
        /// Rounds the specified coordinate vector to match the precision model.
        /// </summary>
        /// <param name="vector">The coordinate vector.</param>
        /// <returns>The precise coordinate vector.</returns>
        public CoordinateVector MakePrecise(CoordinateVector vector)
        {
            switch (ModelType)
            {
                case PrecisionModelType.FloatingSingle:
                    return new CoordinateVector((Single)vector.X, (Single)vector.Y, (Single)vector.Z);
                case PrecisionModelType.Fixed:
                    return new CoordinateVector(Math.Floor((vector.X * Scale) + 0.5) / Scale,
                                                Math.Floor((vector.Y * Scale) + 0.5) / Scale,
                                                Math.Floor((vector.Z * Scale) + 0.5) / Scale);
                default:
                    return vector;
            }
        }

        /// <summary>
        /// Determines whether the specified values are equal.
        /// </summary>
        /// <param name="first">The first value.</param>
        /// <param name="second">The second value.</param>
        /// <returns><c>true</c> if the two values are considered equal at the specified precision; otherwise, <c>false</c>.</returns>
        public Boolean AreEqual(Double first, Double second)
        {
            if (first == second)
                return true;

            return Math.Abs(first - second) < Math.Max(first, second) * _baseTolerance;
        }

        /// <summary>
        /// Determines whether the specified coordinates are equal.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <returns><c>true</c> if the two coordinates are considered equal at the specified precision; otherwise, <c>false</c>.</returns>
        public Boolean AreEqual(Coordinate first, Coordinate second)
        {
            return AreEqual(first.X, second.X) && AreEqual(first.Y, second.Y) && AreEqual(first.Z, second.Z);
        }

        /// <summary>
        /// Determines whether the specified coordinate vectors are equal.
        /// </summary>
        /// <param name="first">The first coordinate vector.</param>
        /// <param name="second">The second coordinate vector.</param>
        /// <returns><c>true</c> if the two coordinate vectors are considered equal at the specified precision; otherwise, <c>false</c>.</returns>
        public Boolean AreEqual(CoordinateVector first, CoordinateVector second)
        {
            return AreEqual(first.X, second.X) && AreEqual(first.Y, second.Y) && AreEqual(first.Z, second.Z);
        }

        /// <summary>
        /// Returns the tolerance for the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The tolerance for the values at the specified precision.</returns>
        public Double Tolerance(params Double[] values)
        {
            if (ModelType == PrecisionModelType.Fixed)
                return _baseTolerance;

            return values.Max(value => Math.Abs(value)) * _baseTolerance;
        }

        /// <summary>
        /// Returns the tolerance for the specified coordinates.
        /// </summary>
        /// <param name="values">The coordinates.</param>
        /// <returns>The tolerance for the coordinates at the specified precision.</returns>
        public Double Tolerance(params Coordinate[] values)
        {
            if (ModelType == PrecisionModelType.Fixed)
                return _baseTolerance;

            return values.Max(coordinate => Calculator.Max(Math.Abs(coordinate.X), Math.Abs(coordinate.Y), Math.Abs(coordinate.Z))) * _baseTolerance;
        }

        /// <summary>
        /// Returns the tolerance for the specified coordinate vectors.
        /// </summary>
        /// <param name="values">The coordinate vectors.</param>
        /// <returns>The tolerance for the coordinate vectors at the specified precision.</returns>
        public Double Tolerance(params CoordinateVector[] values)
        {
            if (ModelType == PrecisionModelType.Fixed)
                return _baseTolerance;

            return values.Max(vector => Calculator.Max(Math.Abs(vector.X), Math.Abs(vector.Y), Math.Abs(vector.Z))) * _baseTolerance;
        }

        #endregion

        #region IComparable methods

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public Int32 CompareTo(PrecisionModel other)
        {
            return MaximumSignificantDigits.CompareTo(other.MaximumSignificantDigits);
        }

        #endregion

        #region IEquatable methods

        /// <summary>
        /// Indicates whether the current object is equal to another precision model.
        /// </summary>
        /// <param name="other">A precision model to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.</returns>
        public Boolean Equals(PrecisionModel other)
        {
            return ModelType == other.ModelType && Scale == other.Scale;
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Determines whether the specified <see cref="Object" /> is equal to the current precision model.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            return (obj is PrecisionModel) && ModelType == (obj as PrecisionModel).ModelType && Scale == (obj as PrecisionModel).Scale;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current precision model.</returns>
        public override Int32 GetHashCode()
        {
            return ModelType.GetHashCode() ^ Scale.GetHashCode() ^ 75632723;
        }

        /// <summary>
        /// Returns a string that represents the current precision model.
        /// </summary>
        /// <returns>A string that represents the current precision model.</returns>
        public override String ToString()
        {
            switch (ModelType)
            {
                case PrecisionModelType.FloatingSingle:
                    return "Floating (single)";
                case PrecisionModelType.Fixed:
                    return "Fixed (" + Scale + ")";
                default:
                    return "Floating";
            }
        }

        #endregion

        #region Private static fields

        /// <summary>
        /// The lazily initialized default precision model.
        /// </summary>
        private static readonly Lazy<PrecisionModel> _default = new Lazy<PrecisionModel>(() => new PrecisionModel(PrecisionModelType.Floating));

        #endregion

        #region Public static properties

        /// <summary>
        /// Gets the default precision model.
        /// </summary>
        /// <value>The default floating precision model.</value>
        public static PrecisionModel Default { get { return _default.Value; } }

        #endregion

        #region Public static methods

        /// <summary>
        /// Returns the least precise precision model.
        /// </summary>
        /// <param name="models">The precision models.</param>
        /// <returns>The least precise model.</returns>
        /// <exception cref="System.ArgumentException">No models are specified.</exception>
        public static PrecisionModel LeastPrecise(params PrecisionModel[] models)
        {
            if (models.Length == 0)
                throw new ArgumentException("models", "No models are specified.");

            Int32 preciceIndex = 0;

            for (Int32 index = 1; index < models.Length; index++)
            {
                if (models[preciceIndex].CompareTo(models[index]) < 0)
                    preciceIndex = index;
            }

            return models[preciceIndex];
        }

        /// <summary>
        /// Returns the most precise precision model.
        /// </summary>
        /// <param name="models">The precision models.</param>
        /// <returns>The most precise model.</returns>
        /// <exception cref="System.ArgumentException">No models are specified.</exception>
        public static PrecisionModel MostPrecise(params PrecisionModel[] models)
        {
            if (models.Length == 0)
                throw new ArgumentException("models", "No models are specified.");

            Int32 preciceIndex = 0;

            for (Int32 index = 1; index < models.Length; index++)
            {
                if (models[preciceIndex].CompareTo(models[index]) > 0)
                    preciceIndex = index;
            }

            return models[preciceIndex];
        }

        #endregion
    }
}
