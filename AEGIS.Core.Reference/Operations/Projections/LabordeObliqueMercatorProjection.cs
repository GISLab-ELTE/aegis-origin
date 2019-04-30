/// <copyright file="LabordeObliqueMercatorProjection.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Management;
using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a Laborde Oblique Mercator projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::9813", "Laborde Oblique Mercator")]
    public class LabordeObliqueMercatorProjection : ObliqueMercatorProjection
    {
        #region Private fields

        private const Double _epsilon = 1E-11; 
        private readonly Double _falseEasting; // projection parameters
        private readonly Double _falseNorthing;
        private readonly Double _c; // projection constants
        private readonly Double _fiS;
        private readonly Double _r;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LabordeObliqueMercatorProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The method is null.
        /// or
        /// The defined operation method requires parameters.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The parameter is not an angular value as required by the method.
        /// or
        /// The parameter is not a length value as required by the method.
        /// or
        /// The parameter is not a double precision floating-point number as required by the method.
        /// or
        /// The parameter does not have the same measurement unit as the ellipsoid.
        /// </exception>
        public LabordeObliqueMercatorProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.LabordeObliqueMercatorProjection, parameters, ellipsoid, areaOfUse)
        {
            // source: EPSG Guidance Note number 7, part 2, page 59

            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).Value;
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).Value;

            _fiS = Math.Asin(Math.Sin(_latitudeOfProjectionCentre)/_b);
            _r = ellipsoid.SemiMajorAxis.BaseValue * _scaleFactorOnInitialLine * (Math.Sqrt(1 - ellipsoid.EccentricitySquare)/(1-ellipsoid.EccentricitySquare*Numerics.Calculator.Sin2(_latitudeOfProjectionCentre)));           
            _c = Math.Log(Math.Tan(Constants.PI / 4 + _fiS / 2)) -
                 _b * Math.Log(Math.Tan(Constants.PI / 4 + _latitudeOfProjectionCentre / 2) * Math.Pow((1 - ellipsoid.Eccentricity * Math.Sin(_latitudeOfProjectionCentre)) / (1 + ellipsoid.Eccentricity * Math.Sin(_latitudeOfProjectionCentre)), ellipsoid.Eccentricity/2));
        }


        #endregion

        #region Protected operation methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate location)
        {
            // source: EPSG Guidance Note number 7, part 2, page 61

            Double l = _b * (location.Longitude.BaseValue - _longitudeOfProjectionCentre);
            Double q = _c + _b * Math.Log(Math.Tan(Constants.PI / 4 + location.Latitude.BaseValue / 2) * Math.Pow((1 - Ellipsoid.Eccentricity * Math.Sin(location.Latitude.BaseValue)) / (1 + Ellipsoid.Eccentricity * Math.Sin(location.Latitude.BaseValue)), Ellipsoid.Eccentricity / 2));
            Double p = 2 * Math.Atan(Math.Pow(Math.E, q)) - Math.PI / 2;
            Double u = Math.Cos(p) * Math.Cos(l) * Math.Cos(_fiS) + Math.Sin(p) * Math.Sin(_fiS);
            Double v = Math.Cos(p) * Math.Cos(l) * Math.Sin(_fiS) - Math.Sin(p) * Math.Cos(_fiS);
            Double w = Math.Cos(p) * Math.Sin(l);
            Double d = Math.Sqrt(u * u + v * v);

            Double l2 = 0.0;
            Double p2 = 0.0;
            if (Math.Abs(d) > _epsilon)
            {
                l2 = 2 * Math.Atan(v / (u + d));
                p2 = Math.Atan(w / d);
            }
            else 
            {
                p2 = Math.Sign(w) * Constants.PI / 2;
            }

            Double hReal = -1 * l2;
            Double hImg  = Math.Log(Math.Tan(Constants.PI / 4 + p2/2));

            Double gReal = (1 - Math.Cos(2*_azimuthOfInitialLine))/12;
            Double gImg  = (Math.Sin(2*_azimuthOfInitialLine))/12;

            Complex h = new Complex(hReal, hImg);
            Complex g = new Complex(gReal, gImg);

            Complex temp = h + (g * (h * h * h));

            Double easting = _falseEasting + _r * temp.Imaginary;
            Double northing = _falseNorthing + _r * temp.Real;

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 61

            Complex g = new Complex((1 - Math.Cos(2 * _azimuthOfInitialLine)) / 12, Math.Sin(2 * _azimuthOfInitialLine) / 12);
            Complex h0 = new Complex((coordinate.Y - _falseNorthing)/_r, (coordinate.X - _falseEasting)/_r);
            Complex h1 = h0 / (h0 + g * h0*h0*h0);

            while (Math.Abs((h0 - h1 - g * (h1 * h1 * h1)).Real) > _epsilon)
            {
                h1 = (h0 + 2 * g * (h1 * h1 * h1)) / (3 * g * (h1 * h1) + 1);
            }
            
            Double l1 = -1 *h1.Real;
            Double p1 = 2 * Math.Atan(Math.Pow(Constants.E , h1.Imaginary)) - Constants.PI / 2;
            Double u1 = Math.Cos(p1) * Math.Cos(l1) * Math.Cos(_fiS) + Math.Cos(p1) * Math.Sin(l1) * Math.Sin(_fiS);

            Double v1 = Math.Sin(p1);
            Double w1 = Math.Cos(p1)*Math.Cos(l1)*Math.Sin(_fiS) - Math.Cos(p1)*Math.Sin(l1)*Math.Cos(_fiS);
            Double d = Math.Sqrt(u1 * u1 + v1 * v1);

            Double l = 0;
            Double p = 0;
            if (Math.Abs(d) > _epsilon)
            {
                l = 2 * Math.Atan(v1 / (u1 + d));
                p = Math.Atan(w1 / d);
            }
            else
            {
                l = 0;
                p = Math.Sign(w1) * Constants.PI / 2;
            }

            Double lambda = _longitudeOfProjectionCentre + (l / _b);
            Double q1 = (Math.Log(Math.Tan(Numerics.Constants.PI / 4 + p / 2)) - _c) / _b;

            Double fi = 2 * Math.Atan(Math.Pow(Constants.E, q1)) - Constants.PI / 2;
            Double fiPre = 0f;
            do
            {
                fiPre = fi;
                fi = 2 * Math.Atan(Math.Pow((1 + Ellipsoid.Eccentricity * Math.Sin(fi)) / (1 - Ellipsoid.Eccentricity * Math.Sin(fi)), (Ellipsoid.Eccentricity / 2)) * Math.Pow(Constants.E, q1)) - Constants.PI / 2;
            } while (Math.Abs(fiPre - fi) > _epsilon);

            return new GeoCoordinate(fi, lambda);
        }

        #endregion
    }
}
