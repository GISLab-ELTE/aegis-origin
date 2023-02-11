// <copyright file="Ellipsoid.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using System;
using System.Linq;
using ELTE.AEGIS.Numerics;

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Represents an ellipsoid used in planetary surface modeling.
    /// </summary>
    public sealed class Ellipsoid : IdentifiedObject
    {
        #region Private fields

        private readonly Length _semiMajorAxis;
        private readonly Length _semiMinorAxis;
        private readonly Double _inverseFattening;
        private readonly Double _eccentricity;
        private readonly Double _secondEccentricity;
        private readonly Double _radiusOfAuthalicSphere;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the semi-major axis.
        /// </summary>
        public Length SemiMajorAxis { get { return _semiMajorAxis; } }

        /// <summary>
        /// Gets the semi-minor axis.
        /// </summary>
        public Length SemiMinorAxis { get { return _semiMinorAxis; } }

        /// <summary>
        /// Gets the inverse fattening.
        /// </summary>
        public Double InverseFattening { get { return _inverseFattening; } }

        /// <summary>
        /// Gets a value indicating whether the ellipsoid is a sphere.
        /// </summary>
        public Boolean IsSphere { get { return InverseFattening == 1; } }

        /// <summary>
        /// Gets the flattening.
        /// </summary>
        public Double Flattening { get { return 1 / _inverseFattening; } }

        /// <summary>
        /// Gets the eccentricity.
        /// </summary>
        public Double Eccentricity { get { return _eccentricity; } }

        /// <summary>
        /// Gets the square of the eccentricity.
        /// </summary>
        public Double EccentricitySquare { get { return _eccentricity * _eccentricity; } }

        /// <summary>
        /// Gets the second eccentricity.
        /// </summary>
        public Double SecondEccentricity { get { return _secondEccentricity; } }

        /// <summary>
        /// Gets the radius of the authalic sphere.
        /// </summary>
        public Double RadiusOfAuthalicSphere { get { return _radiusOfAuthalicSphere; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="semiMajorAxis">The semi-major axis.</param>
        /// <param name="semiMinorAxis">The semi-minor axis.</param>
        /// <param name="inverseFlattening">The inverse flattening.</param>
        /// <param name="flattening">The flattening.</param>
        /// <param name="eccentricity">The eccentricity.</param>
        private Ellipsoid(String identifier, String name, Length semiMajorAxis, Length semiMinorAxis, Double inverseFlattening, Double flattening, Double eccentricity)
            : base(identifier, name)
        {
            _semiMajorAxis = semiMajorAxis;
            _semiMinorAxis = semiMinorAxis;
            _inverseFattening = inverseFlattening;

            _eccentricity = eccentricity;
            _secondEccentricity = Math.Sqrt(EccentricitySquare / (1 - EccentricitySquare));
            _radiusOfAuthalicSphere = _semiMajorAxis.BaseValue * Math.Sqrt((1 - (1 - EccentricitySquare) / (2 * _eccentricity) * Math.Log((1 - _eccentricity) / (1 + _eccentricity))) * 0.5);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="semiMajorAxis">The semi-major axis (defined in <see cref="UnitsOfMeasurement.Metre" />).</param>
        /// <param name="semiMinorAxis">The semi-minor axis (defined in <see cref="UnitsOfMeasurement.Metre" />).</param>
        /// <param name="inverseFlattening">The inverse flattening.</param>
        /// <param name="flattening">The flattening.</param>
        /// <param name="eccentricity">The eccentricity.</param>
        private Ellipsoid(String identifier, String name, Double semiMajorAxis, Double semiMinorAxis, Double inverseFlattening, Double flattening, Double eccentricity)
            : base(identifier, name)
        {
            _semiMajorAxis = new Length(semiMajorAxis, UnitsOfMeasurement.Metre);
            _semiMinorAxis = new Length(semiMinorAxis, UnitsOfMeasurement.Metre);
            _inverseFattening = inverseFlattening;

            _eccentricity = eccentricity;
            _secondEccentricity = Math.Sqrt(EccentricitySquare / (1 - EccentricitySquare));
            _radiusOfAuthalicSphere = _semiMajorAxis.BaseValue * Math.Sqrt((1 - (1 - EccentricitySquare) / (2 * _eccentricity) * Math.Log((1 - _eccentricity) / (1 + _eccentricity))) * 0.5);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines the radius of meridian curvature at a specified latitude.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <returns>The radius of meridian curvature at the specified latitude.</returns>
        /// <exception cref="System.ArgumentException">The latitude value is not valid.</exception>
        public Length RadiusOfMeridianCurvature(Angle latitude)
        {
            if (latitude.BaseValue > Constants.PI / 2 || latitude.BaseValue < -Constants.PI / 2)
                throw new ArgumentException("The latitude value is not valid.", "latitude");

            if (IsSphere)
                return _semiMajorAxis; 
            else
                return new Length(_semiMajorAxis.Value * (1 - EccentricitySquare) / Math.Pow(1 - EccentricitySquare * Calculator.Square(Math.Sin(latitude.BaseValue)), 1.5), _semiMajorAxis.Unit);
        }

        /// <summary>
        /// Determines the radius of meridian curvature at a specified latitude.
        /// </summary>
        /// <param name="latitude">The latitude (defined in <see cref="UnitsOfMeasurement.Radian" />).</param>
        /// <returns>The radius of meridian curvature at the specified latitude.</returns>
        /// <exception cref="System.ArgumentException">The latitude value is not valid.</exception>
        public Double RadiusOfMeridianCurvature(Double latitude)
        {
            if (latitude > Constants.PI / 2 || latitude < -Constants.PI / 2)
                throw new ArgumentException("The latitude value is not valid.", "latitude");

            if (IsSphere)
                return _semiMajorAxis.Value;
            else
                return _semiMajorAxis.Value * (1 - EccentricitySquare) / Math.Pow(1 - EccentricitySquare * Calculator.Square(Math.Sin(latitude)), 1.5);
        }

        /// <summary>
        /// Determines the radius of prime vertical curvature at a specified latitude.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <returns>The radius of prime vertical curvature at the specified latitude.</returns>
        /// <exception cref="System.ArgumentException">The latitude value is not valid.</exception>
        public Length RadiusOfPrimeVerticalCurvature(Angle latitude)
        {
            if (latitude.BaseValue > Constants.PI / 2 || latitude.BaseValue < -Constants.PI / 2)
                throw new ArgumentException("The latitude value is not valid.", "latitude");

            if (IsSphere)
                return _semiMajorAxis;
            else
                return new Length(_semiMajorAxis.Value / Math.Sqrt(1 - EccentricitySquare * Calculator.Square(Math.Sin(latitude.BaseValue))), _semiMajorAxis.Unit);
        }

        /// <summary>
        /// Determines the radius of prime vertical curvature at a specified latitude.
        /// </summary>
        /// <param name="latitude">The latitude (defined in <see cref="UnitsOfMeasurement.Radian" />).</param>
        /// <returns>The radius of prime vertical curvature at the specified latitude.</returns>
        /// <exception cref="System.ArgumentException">The latitude value is not valid.</exception>
        public Double RadiusOfPrimeVerticalCurvature(Double latitude)
        {
            if (latitude > Constants.PI / 2 || latitude < -Constants.PI / 2)
                throw new ArgumentException("The latitude value is not valid.", "latitude");

            if (IsSphere)
                return _semiMajorAxis.Value;
            else
                return _semiMajorAxis.Value / Math.Sqrt(1 - EccentricitySquare * Calculator.Square(Math.Sin(latitude)));
        }

        /// <summary>
        /// Determines the radius of parallel curvature at a specified latitude.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <returns>The radius of parallel curvature at the specified latitude.</returns>
        /// <exception cref="System.ArgumentException">The latitude value is not valid.</exception>
        public Length RadiusOfParalellCurvature(Angle latitude)
        {
            if (latitude.BaseValue > Constants.PI / 2 || latitude.BaseValue < -Constants.PI / 2)
                throw new ArgumentException("The latitude value is not valid.", "latitude");

            if (IsSphere)
                return _semiMajorAxis * Math.Cos(latitude.BaseValue);
            else
                return new Length(_semiMajorAxis.Value * Math.Cos(latitude.BaseValue) / Math.Sqrt(1 - EccentricitySquare * Calculator.Square(Math.Sin(latitude.BaseValue))), _semiMajorAxis.Unit);
        }

        /// <summary>
        /// Determines the radius of parallel curvature at a specified latitude.
        /// </summary>
        /// <param name="latitude">The latitude (defined in <see cref="UnitsOfMeasurement.Radian" />).</param>
        /// <returns>The radius of parallel curvature at the specified latitude.</returns>
        /// <exception cref="System.ArgumentException">The latitude value is not valid.</exception>
        public Double RadiusOfParalellCurvature(Double latitude)
        {
            if (latitude > Constants.PI / 2 || latitude < -Constants.PI / 2)
                throw new ArgumentException("The latitude value is not valid.", "latitude");

            if (IsSphere)
                return _semiMajorAxis.Value * Math.Cos(latitude);
            else
                return _semiMajorAxis.Value * Math.Cos(latitude) / Math.Sqrt(1 - EccentricitySquare * Calculator.Square(Math.Sin(latitude)));
        }

        /// <summary>
        /// Determines the radius of conformal sphere at a specified latitude.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <returns>The radius of conformal sphere at the specified latitude.</returns>
        /// <exception cref="System.ArgumentException">The latitude value is not valid.</exception>
        public Length RadiusOfConformalSphere(Angle latitude)
        {
            Length rho = RadiusOfMeridianCurvature(latitude);
            Length nu = RadiusOfPrimeVerticalCurvature(latitude);
            return new Length(Math.Sqrt(rho.Value * nu.Value), rho.Unit);
        }

        /// <summary>
        /// Determines the radius of conformal sphere at a specified latitude.
        /// </summary>
        /// <param name="latitude">The latitude (defined in <see cref="UnitsOfMeasurement.Radian" />).</param>
        /// <returns>The radius of conformal sphere at the specified latitude.</returns>
        /// <exception cref="System.ArgumentException">The latitude value is not valid.</exception>
        public Double RadiusOfConformalSphere(Double latitude)
        {
            return Math.Sqrt(RadiusOfMeridianCurvature(latitude) * RadiusOfPrimeVerticalCurvature(latitude));
        }

        #endregion

        #region Public static factory methods

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the semi-minor axis.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="semiMajorAxis">The semi-major axis.</param>
        /// <param name="semiMinorAxis">The semi-minor axis.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the semi-minor axis.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentException">The semi-minor axis must be measured in the same measurement unit as the semi-major axis.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-minor axis is less than the semi-major axis.
        /// or
        /// The semi-minor axis is less than or equal to 0.
        /// </exception>
        public static Ellipsoid FromSemiMinorAxis(String identifier, String name, Length semiMajorAxis, Length semiMinorAxis)
        {
            if (semiMajorAxis.Unit != semiMinorAxis.Unit)
                throw new ArgumentException("The semi-minor axis must be measured in the same measurement unit as the semi-major axis.", "semiMinorAxis");
            if (semiMajorAxis < semiMinorAxis)
                throw new ArgumentOutOfRangeException("semiMinorAxis", "The semi-minor axis is less than the semi-major axis.");
            if (semiMinorAxis.Value <= 0)
                throw new ArgumentOutOfRangeException("semiMinorAxis", "The semi-minor axis is less than or equal to 0.");

            Double inverseFlattening = (semiMajorAxis == semiMinorAxis) ? 1 : semiMajorAxis.Value / (semiMajorAxis.Value - semiMinorAxis.Value);

            return new Ellipsoid(identifier, name, semiMajorAxis, semiMinorAxis, inverseFlattening, inverseFlattening == 1 ? 0 : 1 / inverseFlattening, Math.Sqrt(2 * 1 / inverseFlattening - Calculator.Square(1 / inverseFlattening)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the semi-minor axis.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="semiMajorAxis">The semi-major axis (defined in <see cref="UnitsOfMeasurement.Metre" />).</param>
        /// <param name="semiMinorAxis">The semi-minor axis (defined in <see cref="UnitsOfMeasurement.Metre" />).</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the semi-minor axis.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-minor axis is less than the semi-major axis.
        /// or
        /// The semi-minor axis is less than or equal to 0.
        /// </exception>
        public static Ellipsoid FromSemiMinorAxis(String identifier, String name, Double semiMajorAxis, Double semiMinorAxis)
        {
            if (semiMajorAxis < semiMinorAxis)
                throw new ArgumentOutOfRangeException("semiMinorAxis", "The semi-minor axis is less than the semi-major axis.");
            if (semiMinorAxis <= 0)
                throw new ArgumentOutOfRangeException("semiMinorAxis", "The semi-minor axis is less than or equal to 0.");

            Double inverseFlattening = (semiMajorAxis == semiMinorAxis) ? 1 : semiMajorAxis / (semiMajorAxis - semiMinorAxis);

            return new Ellipsoid(identifier, name, semiMajorAxis, semiMinorAxis, inverseFlattening, inverseFlattening == 1 ? 0 : 1 / inverseFlattening, Math.Sqrt(2 * 1 / inverseFlattening - Calculator.Square(1 / inverseFlattening)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the inverse flattening.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="semiMajorAxis">The semi-major axis.</param>
        /// <param name="inverseFlattening">The inverse flattening.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the inverse flattening.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-major is less than or equal to 0.
        /// or
        /// inverseThe inverse flattening is less than 1.
        /// </exception>
        public static Ellipsoid FromInverseFlattening(String identifier, String name, Length semiMajorAxis, Double inverseFlattening)
        {
            if (semiMajorAxis.Value <= 0)
                throw new ArgumentOutOfRangeException("semiMajorAxis", "The semi-major is less than or equal to 0.");
            if (inverseFlattening < 1)
                throw new ArgumentOutOfRangeException("inverseFlattening", "The inverse flattening is less than 1.");

            Length semiMinorAxis = new Length((inverseFlattening != 0) ? semiMajorAxis.Value * (1 - 1 / inverseFlattening) : semiMajorAxis.Value, semiMajorAxis.Unit);

            return new Ellipsoid(identifier, name, semiMajorAxis, semiMinorAxis, inverseFlattening, inverseFlattening == 1 ? 0 : 1 / inverseFlattening, Math.Sqrt(2 * 1 / inverseFlattening - Calculator.Square(1 / inverseFlattening)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the inverse flattening.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="semiMajorAxis">The semi-major axis (defined in <see cref="UnitsOfMeasurement.Metre" />).</param>
        /// <param name="inverseFlattening">The inverse flattening.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the inverse flattening.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-major is less than or equal to 0.
        /// or
        /// inverseThe inverse flattening is less than 1.
        /// </exception>
        public static Ellipsoid FromInverseFlattening(String identifier, String name, Double semiMajorAxis, Double inverseFlattening)
        {
            if (semiMajorAxis <= 0)
                throw new ArgumentOutOfRangeException("semiMajorAxis", "The semi-major is less than or equal to 0.");
            if (inverseFlattening < 1)
                throw new ArgumentOutOfRangeException("inverseFlattening", "The inverse flattening is less than 1.");

            Double semiMinorAxis = (inverseFlattening != 0) ? semiMajorAxis * (1 - 1 / inverseFlattening) : semiMajorAxis;

            return new Ellipsoid(identifier, name, semiMajorAxis, semiMinorAxis, inverseFlattening, inverseFlattening == 1 ? 0 : 1 / inverseFlattening, Math.Sqrt(2 * 1 / inverseFlattening - Calculator.Square(1 / inverseFlattening)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the flattening.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="semiMajorAxis">The semi-major axis.</param>
        /// <param name="inverseFlattening">The inverse flattening.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the flattening.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-major is less than or equal to 0.
        /// or
        /// The flattening is less than or equal to 0.
        /// or
        /// The flattening greater than 1.
        /// </exception>
        public static Ellipsoid FromFlattening(String identifier, String name, Length semiMajorAxis, Double flattening)
        {
            if (semiMajorAxis.Value <= 0)
                throw new ArgumentOutOfRangeException("semiMajorAxis", "The semi-major is less than or equal to 0.");
            if (flattening <= 0)
                throw new ArgumentOutOfRangeException("flattening", "The flattening is less than or equal to 0.");
            if (flattening > 1)
                throw new ArgumentOutOfRangeException("flattening", "The flattening greater than 1.");

            Length semiMinorAxis = new Length((flattening != 1) ? semiMajorAxis.Value * (1 - flattening) : semiMajorAxis.Value, UnitsOfMeasurement.Metre);

            return new Ellipsoid(identifier, name, semiMajorAxis, semiMinorAxis, 1 / flattening, flattening, Math.Sqrt(2 * flattening - Calculator.Square(flattening)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the flattening.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="semiMajorAxis">The semi-major axis (defined in <see cref="UnitsOfMeasurement.Metre" />).</param>
        /// <param name="flattening">The flattening.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the flattening.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-major is less than or equal to 0.
        /// or
        /// The flattening is less than or equal to 0.
        /// or
        /// The flattening greater than 1.
        /// </exception>
        public static Ellipsoid FromFlattening(String identifier, String name, Double semiMajorAxis, Double flattening)
        {
            if (semiMajorAxis <= 0)
                throw new ArgumentOutOfRangeException("semiMajorAxis", "The semi-major is less than or equal to 0.");
            if (flattening <= 0)
                throw new ArgumentOutOfRangeException("flattening", "The flattening is less than or equal to 0.");
            if (flattening > 1)
                throw new ArgumentOutOfRangeException("flattening", "The flattening greater than 1.");

            Double semiMinorAxis = (flattening != 1) ? semiMajorAxis * (1 - flattening) : semiMajorAxis;

            return new Ellipsoid(identifier, name, semiMajorAxis, semiMinorAxis, 1 / flattening, flattening, Math.Sqrt(2 * flattening - Calculator.Square(flattening)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the eccentricity.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="semiMajorAxis">The semi-major axis.</param>
        /// <param name="inverseFlattening">The inverse flattening.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the eccentricity.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-major is less than or equal to 0.
        /// or
        /// The eccentricity is less than or equal to 0.
        /// or
        /// The eccentricity greater than 1.
        /// </exception>
        public static Ellipsoid FromEccentricity(String identifier, String name, Length semiMajorAxis, Double eccentricity)
        {
            if (semiMajorAxis.Value <= 0)
                throw new ArgumentOutOfRangeException("semiMajorAxis", "The semi-major is less than or equal to 0.");
            if (eccentricity <= 0)
                throw new ArgumentOutOfRangeException("eccentricity", "The eccentricity is less than or equal to 0.");
            if (eccentricity > 1)
                throw new ArgumentOutOfRangeException("eccentricity", "The eccentricity greater than 1.");

            Double flattening = 1 - Math.Sqrt(1 - Calculator.Square(eccentricity));
            Length semiMinorAxis = new Length((flattening != 1) ? semiMajorAxis.Value * (1 - flattening) : semiMajorAxis.Value, UnitsOfMeasurement.Metre);

            return new Ellipsoid(identifier, name, semiMajorAxis, semiMinorAxis, 1 - Math.Sqrt(1 - Calculator.Square(eccentricity)), flattening, eccentricity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the eccentricity.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="semiMajorAxis">The semi-major axis (defined in <see cref="UnitsOfMeasurement.Metre" />).</param>
        /// <param name="eccentricity">The eccentricity.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the eccentricity.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-major is less than or equal to 0.
        /// or
        /// The eccentricity is less than or equal to 0.
        /// or
        /// The eccentricity greater than 1.
        /// </exception>
        public static Ellipsoid FromEccentricity(String identifier, String name, Double semiMajorAxis, Double eccentricity)
        {
            if (semiMajorAxis <= 0)
                throw new ArgumentOutOfRangeException("semiMajorAxis", "The semi-major is less than or equal to 0.");
            if (eccentricity <= 0)
                throw new ArgumentOutOfRangeException("eccentricity", "The eccentricity is less than or equal to 0.");
            if (eccentricity > 1)
                throw new ArgumentOutOfRangeException("eccentricity", "The eccentricity greater than 1.");

            Double flattening = 1 - Math.Sqrt(1 - Calculator.Square(eccentricity));
            Double semiMinorAxis = (flattening != 1) ? semiMajorAxis * (1 - flattening) : semiMajorAxis;

            return new Ellipsoid(identifier, name, semiMajorAxis, semiMinorAxis, 1 - Math.Sqrt(1 - Calculator.Square(eccentricity)), flattening, eccentricity);
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class as a sphere.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="semiMajorAxis">The semi-major axis.</param>
        /// <param name="inverseFlattening">The inverse flattening.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class as a sphere.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The semi axis is less than or equal to 0.</exception>
        public static Ellipsoid FromSphere(String identifier, String name, Length semiAxis)
        {
            if (semiAxis.Value <= 0)
                throw new ArgumentOutOfRangeException("semiAxis", "The semi axis is less than or equal to 0.");

            return new Ellipsoid(identifier, name, semiAxis, semiAxis, 1, 0, 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class as a sphere.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="semiMajorAxis">The semi-major axis (defined in <see cref="UnitsOfMeasurement.Metre" />).</param>
        /// <param name="inverseFlattening">The inverse flattening.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class as a sphere.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The semi axis is less than or equal to 0.</exception>
        public static Ellipsoid FromSphere(String identifier, String name, Double semiAxis)
        {
            if (semiAxis <= 0)
                throw new ArgumentOutOfRangeException("semiAxis", "The semi axis is less than or equal to 0.");

            return new Ellipsoid(identifier, name, semiAxis, semiAxis, 1, 0, 0);
        }

        #endregion
    }
}
