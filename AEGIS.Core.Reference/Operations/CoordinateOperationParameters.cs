/// <copyright file="CoordinateOperationParameters.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a collection of known <see cref="CoordinateOperationParameter" /> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(CoordinateOperationParameter))]
    public static class CoordinateOperationParameters
    {
        #region Query fields

        private static CoordinateOperationParameter[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="CoordinateOperationParameter" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="CoordinateOperationParameter" /> instances within the collection.</value>
        public static IList<CoordinateOperationParameter> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(CoordinateOperationParameters).GetProperties().
                                                                 Where(property => property.Name != "All").
                                                                 Select(property => property.GetValue(null, null) as CoordinateOperationParameter).
                                                                 ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="CoordinateOperationParameter" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="CoordinateOperationParameter" /> instances that match the specified identifier.</returns>
        public static IList<CoordinateOperationParameter> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            // identifier correction
            identifier = Regex.Escape(identifier);

            return All.Where(obj => Regex.IsMatch(obj.Identifier, identifier, RegexOptions.IgnoreCase)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="CoordinateOperationParameter" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="CoordinateOperationParameter" /> instances that match the specified name.</returns>
        public static IList<CoordinateOperationParameter> FromName(String name)
        {
            if (name == null)
                return null;

            // name correction
            name = Regex.Escape(name);

            return All.Where(obj => Regex.IsMatch(obj.Name, name, RegexOptions.IgnoreCase) ||
                                    obj.Aliases != null && obj.Aliases.Any(alias => Regex.IsMatch(alias, name, RegexOptions.IgnoreCase))).ToList().AsReadOnly();
        }

        #endregion

        #region Private static fields

        private static CoordinateOperationParameter _A0;
        private static CoordinateOperationParameter _A1;
        private static CoordinateOperationParameter _A2;
        private static CoordinateOperationParameter _A3;
        private static CoordinateOperationParameter _A4;
        private static CoordinateOperationParameter _A5;
        private static CoordinateOperationParameter _A6;
        private static CoordinateOperationParameter _A7;
        private static CoordinateOperationParameter _A8;
        private static CoordinateOperationParameter _angleFromRectifiedToSkewGrid;
        private static CoordinateOperationParameter _Au0v1;
        private static CoordinateOperationParameter _Au0v2;
        private static CoordinateOperationParameter _Au0v3;
        private static CoordinateOperationParameter _Au0v4;
        private static CoordinateOperationParameter _Au0v5;
        private static CoordinateOperationParameter _Au0v6;
        private static CoordinateOperationParameter _Au0v8;
        private static CoordinateOperationParameter _Au1v0;
        private static CoordinateOperationParameter _Au1v1;
        private static CoordinateOperationParameter _Au1v2;
        private static CoordinateOperationParameter _Au1v3;
        private static CoordinateOperationParameter _Au1v4;
        private static CoordinateOperationParameter _Au1v5;
        private static CoordinateOperationParameter _Au1v9;
        private static CoordinateOperationParameter _Au2v0;
        private static CoordinateOperationParameter _Au2v1;
        private static CoordinateOperationParameter _Au2v2;
        private static CoordinateOperationParameter _Au2v3;
        private static CoordinateOperationParameter _Au2v4;
        private static CoordinateOperationParameter _Au2v7;
        private static CoordinateOperationParameter _Au3v0;
        private static CoordinateOperationParameter _Au3v1;
        private static CoordinateOperationParameter _Au3v2;
        private static CoordinateOperationParameter _Au3v3;
        private static CoordinateOperationParameter _Au3v9;
        private static CoordinateOperationParameter _Au4v0;
        private static CoordinateOperationParameter _Au4v1;
        private static CoordinateOperationParameter _Au4v2;
        private static CoordinateOperationParameter _Au5v0;
        private static CoordinateOperationParameter _Au5v1;
        private static CoordinateOperationParameter _Au5v2;
        private static CoordinateOperationParameter _Au6v0;
        private static CoordinateOperationParameter _Au9v0;
        private static CoordinateOperationParameter _azimuthOfInitialLine;
        private static CoordinateOperationParameter _B0;
        private static CoordinateOperationParameter _B00;
        private static CoordinateOperationParameter _B1;
        private static CoordinateOperationParameter _B2;
        private static CoordinateOperationParameter _B3;
        private static CoordinateOperationParameter _binGridOriginEasting;
        private static CoordinateOperationParameter _binGridOriginI;
        private static CoordinateOperationParameter _binGridOriginJ;
        private static CoordinateOperationParameter _binGridOriginNorthing;
        private static CoordinateOperationParameter _binNodeIncrementOnIAxis;
        private static CoordinateOperationParameter _binNodeIncrementOnJAxis;
        private static CoordinateOperationParameter _binWidthOnIAxis;
        private static CoordinateOperationParameter _binWidthOnJAxis;
        private static CoordinateOperationParameter _Bu0v1;
        private static CoordinateOperationParameter _Bu0v2;
        private static CoordinateOperationParameter _Bu0v3;
        private static CoordinateOperationParameter _Bu0v4;
        private static CoordinateOperationParameter _Bu0v5;
        private static CoordinateOperationParameter _Bu0v6;
        private static CoordinateOperationParameter _Bu0v9;
        private static CoordinateOperationParameter _Bu1v0;
        private static CoordinateOperationParameter _Bu1v1;
        private static CoordinateOperationParameter _Bu1v2;
        private static CoordinateOperationParameter _Bu1v3;
        private static CoordinateOperationParameter _Bu1v4;
        private static CoordinateOperationParameter _Bu1v5;
        private static CoordinateOperationParameter _Bu2v0;
        private static CoordinateOperationParameter _Bu2v1;
        private static CoordinateOperationParameter _Bu2v2;
        private static CoordinateOperationParameter _Bu2v3;
        private static CoordinateOperationParameter _Bu2v4;
        private static CoordinateOperationParameter _Bu2v7;
        private static CoordinateOperationParameter _Bu3v0;
        private static CoordinateOperationParameter _Bu3v1;
        private static CoordinateOperationParameter _Bu3v2;
        private static CoordinateOperationParameter _Bu3v3;
        private static CoordinateOperationParameter _Bu4v0;
        private static CoordinateOperationParameter _Bu4v1;
        private static CoordinateOperationParameter _Bu4v2;
        private static CoordinateOperationParameter _Bu4v4;
        private static CoordinateOperationParameter _Bu4v6;
        private static CoordinateOperationParameter _Bu4v9;
        private static CoordinateOperationParameter _Bu5v0;
        private static CoordinateOperationParameter _Bu5v1;
        private static CoordinateOperationParameter _Bu5v7;
        private static CoordinateOperationParameter _Bu6v0;
        private static CoordinateOperationParameter _Bu6v1;
        private static CoordinateOperationParameter _Bu7v0;
        private static CoordinateOperationParameter _Bu7v2;
        private static CoordinateOperationParameter _Bu8v1;
        private static CoordinateOperationParameter _Bu8v3;
        private static CoordinateOperationParameter _Bu9v2;
        private static CoordinateOperationParameter _Bu9v4;
        private static CoordinateOperationParameter _C1;
        private static CoordinateOperationParameter _C10;
        private static CoordinateOperationParameter _C2;
        private static CoordinateOperationParameter _C3;
        private static CoordinateOperationParameter _C4;
        private static CoordinateOperationParameter _C5;
        private static CoordinateOperationParameter _C6;
        private static CoordinateOperationParameter _C7;
        private static CoordinateOperationParameter _C8;
        private static CoordinateOperationParameter _C9;
        private static CoordinateOperationParameter _coLatitudeOfConeAxis;
        private static CoordinateOperationParameter _eastingAtFalseOrigin;
        private static CoordinateOperationParameter _eastingAtProjectionCentre;
        private static CoordinateOperationParameter _eastingOffset;
        private static CoordinateOperationParameter _ellipsoidalHeightOfTopocentricOrigin;
        private static CoordinateOperationParameter _ellipsoidalHeightOfTopogrphicOrigin;
        private static CoordinateOperationParameter _falseEasting;
        private static CoordinateOperationParameter _falseNorthing;
        private static CoordinateOperationParameter _flatteningDifference;
        private static CoordinateOperationParameter _geocenticXOfTopocentricOrigin;
        private static CoordinateOperationParameter _geocenticYOfTopocentricOrigin;
        private static CoordinateOperationParameter _geocenticZOfTopocentricOrigin;
        private static CoordinateOperationParameter _initialLongitude;
        private static CoordinateOperationParameter _latitudeOf1stStandardParallel;
        private static CoordinateOperationParameter _latitudeOf2ndStandardParallel;
        private static CoordinateOperationParameter _latitudeOfFalseOrigin;
        private static CoordinateOperationParameter _latitudeOffset;
        private static CoordinateOperationParameter _latitudeOfNaturalOrigin;
        private static CoordinateOperationParameter _latitudeOfProjectionCentre;
        private static CoordinateOperationParameter _latitudeOfPseudoStandardParallel;
        private static CoordinateOperationParameter _latitudeOfStandardParallel;
        private static CoordinateOperationParameter _latitudeOfTopocentricOrigin;
        private static CoordinateOperationParameter _latitudeOfTopographicOrigin;
        private static CoordinateOperationParameter _longitudeOfFalseOrigin;
        private static CoordinateOperationParameter _longitudeOffset;
        private static CoordinateOperationParameter _longitudeOfNaturalOrigin;
        private static CoordinateOperationParameter _longitudeOfOrigin;
        private static CoordinateOperationParameter _longitudeOfProjectionCentre;
        private static CoordinateOperationParameter _longitudeOfTopocentricOrigin;
        private static CoordinateOperationParameter _longitudeOfTopographicOrigin;
        private static CoordinateOperationParameter _mapGridBearingOfBinGridJAxis;
        private static CoordinateOperationParameter _northingAtFalseOrigin;
        private static CoordinateOperationParameter _northingAtProjectionCentre;
        private static CoordinateOperationParameter _northingOffset;
        private static CoordinateOperationParameter _ordinate1OfEvaluationPoint;
        private static CoordinateOperationParameter _ordinate1OfEvaluationPointInSource;
        private static CoordinateOperationParameter _ordinate1OfEvaluationPointInTarget;
        private static CoordinateOperationParameter _ordinate2OfEvaluationPoint;
        private static CoordinateOperationParameter _ordinate2OfEvaluationPointInSource;
        private static CoordinateOperationParameter _ordinate2OfEvaluationPointInTarget;
        private static CoordinateOperationParameter _ordinate3OfEvaluationPoint;
        private static CoordinateOperationParameter _pointScaleFactor;
        private static CoordinateOperationParameter _rotationAngleOfSourceCoordReferenceSystemAxes;
        private static CoordinateOperationParameter _scaleDifference;
        private static CoordinateOperationParameter _scaleFactorAtNaturalOrigin;
        private static CoordinateOperationParameter _scaleFactorForSourceCoordReferenceSystem1stAxis;
        private static CoordinateOperationParameter _scaleFactorForSourceCoordReferenceSystem2ndAxis;
        private static CoordinateOperationParameter _scaleFactorOfBinGrid;
        private static CoordinateOperationParameter _scaleFactorOnInitialLine;
        private static CoordinateOperationParameter _scaleFactorOnPseudoStandardParallel;
        private static CoordinateOperationParameter _scalingFactorForCoordinateDifferences;
        private static CoordinateOperationParameter _scalingFactorForSourceCoordinateDifferences;
        private static CoordinateOperationParameter _scalingFactorForTargetCoordinateDifferences;
        private static CoordinateOperationParameter _semiMajorAxisLengthDifference;
        private static CoordinateOperationParameter _verticalOffset;
        private static CoordinateOperationParameter _viewPointHeight;
        private static CoordinateOperationParameter _xAxisRotation;
        private static CoordinateOperationParameter _xAxisTranslation;
        private static CoordinateOperationParameter _yAxisRotation;
        private static CoordinateOperationParameter _yAxisTranslation;
        private static CoordinateOperationParameter _zAxisRotation;
        private static CoordinateOperationParameter _zAxisTranslation;
        private static CoordinateOperationParameter _zoneWidth;


        #endregion

        #region Public static properties

        /// <summary>
        /// Co-latitude of cone axis.
        /// </summary>
        public static CoordinateOperationParameter CoLatitudeOfConeAxis 
        { 
            get 
            { 
                return _coLatitudeOfConeAxis ?? (_coLatitudeOfConeAxis = 
                    new CoordinateOperationParameter("EPSG::1036", "Co-latitude of cone axis.", UnitQuantityType.Angle, 
                                                     "The rotation applied to spherical coordinates for the oblique projection, measured on the conformal sphere in the plane of the meridian of origin.")); 
            } 
        }

        /// <summary>
        /// Latitude offset.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOffset
        {
            get
            {
                return _latitudeOffset ?? (_latitudeOffset =
                    new CoordinateOperationParameter("EPSG::8601", "Latitude offset", UnitQuantityType.Angle,
                                                     "The difference between the latitude values of a point in the target and source coordinate reference systems."));
            }
        }

        /// <summary>
        /// Longitude offset.
        /// </summary>
        public static CoordinateOperationParameter LongitudeOffset
        {
            get
            {
                return _longitudeOffset ?? (_longitudeOffset =
                    new CoordinateOperationParameter("EPSG::8602", "Longitude offset", UnitQuantityType.Angle,
                                                     "The difference between the longitude values of a point in the target and source coordinate reference systems."));
            }
        }

        /// <summary>
        /// Vertical offset.
        /// </summary>
        public static CoordinateOperationParameter VerticalOffset
        {
            get
            {
                return _verticalOffset ?? (_verticalOffset =
                    new CoordinateOperationParameter("EPSG::8603", "Vertical offset", UnitQuantityType.Length,
                                                     "The difference between the height or depth values of a point in the target and source coordinate reference systems."));
            }
        }

        /// <summary>
        /// X-axis translation.
        /// </summary>
        public static CoordinateOperationParameter XAxisTranslation
        {
            get
            {
                return _xAxisTranslation ?? (_xAxisTranslation =
                    new CoordinateOperationParameter("EPSG::8605", "X-axis translation", UnitQuantityType.Length,
                                                     "The difference between the X values of a point in the target and source coordinate reference systems."));
            }
        }

        /// <summary>
        /// Y-axis translation.
        /// </summary>
        public static CoordinateOperationParameter YAxisTranslation
        {
            get
            {
                return _yAxisTranslation ?? (_yAxisTranslation =
                    new CoordinateOperationParameter("EPSG::8606", "Y-axis translation", UnitQuantityType.Length,
                                                     "The difference between the Y values of a point in the target and source coordinate reference systems."));
            }
        }

        /// <summary>
        /// Z-axis translation.
        /// </summary>
        public static CoordinateOperationParameter ZAxisTranslation
        {
            get
            {
                return _zAxisTranslation ?? (_zAxisTranslation =
                    new CoordinateOperationParameter("EPSG::8607", "Z-axis translation", UnitQuantityType.Length,
                                                     "The difference between the Z values of a point in the target and source coordinate reference systems."));
            }
        }

        /// <summary>
        /// X-axis rotation.
        /// </summary>
        public static CoordinateOperationParameter XAxisRotation
        {
            get
            {
                return _xAxisRotation ?? (_xAxisRotation =
                    new CoordinateOperationParameter("EPSG::8608", "X-axis rotation", UnitQuantityType.Angle,
                                                     "The angular difference between the Y and Z axes directions of target and source coordinate reference systems. This is a rotation about the X axis as viewed from the origin looking along that axis. The particular method defines which direction is positive, and what is being rotated (point or axis)."));
            }
        }

        /// <summary>
        /// Y-axis rotation.
        /// </summary>
        public static CoordinateOperationParameter YAxisRotation
        {
            get
            {
                return _yAxisRotation ?? (_yAxisRotation =
                    new CoordinateOperationParameter("EPSG::8609", "Y-axis rotation", UnitQuantityType.Angle,
                                                     "The angular difference between the X and Z axes directions of target and source coordinate reference systems. This is a rotation about the Y axis as viewed from the origin looking along that axis. The particular method defines which direction is positive, and what is being rotated (point or axis)."));
            }
        }

        /// <summary>
        /// Z-axis rotation.
        /// </summary>
        public static CoordinateOperationParameter ZAxisRotation
        {
            get
            {
                return _zAxisRotation ?? (_zAxisRotation =
                    new CoordinateOperationParameter("EPSG::8610", "Z-axis rotation", UnitQuantityType.Angle,
                                                     "The angular difference between the X and Y axes directions of target and source coordinate reference systems. This is a rotation about the Z axis as viewed from the origin looking along that axis. The particular method defines which direction is positive, and what is being rotated (point or axis)."));
            }
        }

        /// <summary>
        /// Scale difference.
        /// </summary>
        public static CoordinateOperationParameter ScaleDifference
        {
            get
            {
                return _scaleDifference ?? (_scaleDifference =
                    new CoordinateOperationParameter("EPSG::8611", "Scale difference", UnitQuantityType.Scale,
                                                     "The scale difference increased by unity equals the ratio of an the length of an arbitrary distance between two points in target and source coordinate reference systems. This is usually averaged for the intersection area of the two coordinate reference systems. If a distance of 100 km in the source coordinate reference system translates into a distance of 100.001 km in the target coordinate reference system, the scale difference is 1 ppm (the ratio being 1.000001)."));
            }
        }

        /// <summary>
        /// Ordinate 1 of evaluation point.
        /// </summary>
        public static CoordinateOperationParameter Ordinate1OfEvaluationPoint
        {
            get
            {
                return _ordinate1OfEvaluationPoint ?? (_ordinate1OfEvaluationPoint =
                    new CoordinateOperationParameter("EPSG::8617", "Ordinate 1 of evaluation point", UnitQuantityType.Length,
                                                    "The value of the first ordinate of the evaluation point."));
            }
        }

        /// <summary>
        /// Ordinate 2 of evaluation point.
        /// </summary>
        public static CoordinateOperationParameter Ordinate2OfEvaluationPoint
        {
            get
            {
                return _ordinate2OfEvaluationPoint ?? (_ordinate2OfEvaluationPoint =
                    new CoordinateOperationParameter("EPSG::8618", "Ordinate 2 of evaluation point", UnitQuantityType.Length,
                                           "The value of the second ordinate of the evaluation point."));
            }
        }

        /// <summary>
        /// Ordinate 1 of evaluation point in source CRS.
        /// </summary>
        public static CoordinateOperationParameter Ordinate1OfEvaluationPointInSource
        {
            get
            {
                return _ordinate1OfEvaluationPointInSource ?? (_ordinate1OfEvaluationPointInSource =
                    new CoordinateOperationParameter("EPSG::8619", "Ordinate 1 of evaluation point in source CRS", UnitQuantityType.Length,
                                                     "The value of the first ordinate of the evaluation point expressed in the source coordinate reference system."));
            }
        }

        /// <summary>
        /// Ordinate 2 of evaluation point in source CRS.
        /// </summary>
        public static CoordinateOperationParameter Ordinate2OfEvaluationPointInSource
        {
            get
            {
                return _ordinate2OfEvaluationPointInSource ?? (_ordinate2OfEvaluationPointInSource =
                    new CoordinateOperationParameter("EPSG::8620", "Ordinate 2 of evaluation point in source CRS", UnitQuantityType.Length,
                                                     "The value of the second ordinate of the evaluation point expressed in the source coordinate reference system."));
            }
        }

        /// <summary>
        /// Ordinate 1 of evaluation point in target CRS.
        /// </summary>
        public static CoordinateOperationParameter Ordinate1OfEvaluationPointInTarget
        {
            get
            {
                return _ordinate1OfEvaluationPointInTarget ?? (_ordinate1OfEvaluationPointInTarget =
                    new CoordinateOperationParameter("EPSG::8621", "Ordinate 1 of evaluation point in target CRS", UnitQuantityType.Length,
                                                     "The value of the first ordinate of the evaluation point expressed in the target coordinate reference system. In the case of an affine transformation the evaluation point is the origin of the source coordinate reference system."));
            }
        }

        /// <summary>
        /// Ordinate 2 of evaluation point in target CRS.
        /// </summary>
        public static CoordinateOperationParameter Ordinate2OfEvaluationPointInTarget
        {
            get
            {
                return _ordinate2OfEvaluationPointInTarget ?? (_ordinate2OfEvaluationPointInTarget =
                    new CoordinateOperationParameter("EPSG::8622", "Ordinate 2 of evaluation point in target CRS", UnitQuantityType.Length,
                                                     "The value of the second ordinate of the evaluation point expressed in the target coordinate reference system. In the case of an affine transformation the evaluation point is the origin of the source coordinate reference system."));
            }
        }

        /// <summary>
        /// A0.
        /// </summary>
        public static CoordinateOperationParameter A0
        {
            get
            {
                return _A0 ?? (_A0 =
                    new CoordinateOperationParameter("EPSG::8623", "A0", UnitQuantityType.Scale,
                                                     "Coefficient used in affine (general parametric) and polynomial transformations."));
            }
        }

        /// <summary>
        /// A1.
        /// </summary>
        public static CoordinateOperationParameter A1
        {
            get
            {
                return _A1 ?? (_A1 =
                    new CoordinateOperationParameter("EPSG::8624", "A1", UnitQuantityType.Scale,
                                                     "Coefficient used in affine (general parametric) and polynomial transformations."));
            }
        }

        /// <summary>
        /// A2.
        /// </summary>
        public static CoordinateOperationParameter A2
        {
            get
            {
                return _A2 ?? (_A2 =
                    new CoordinateOperationParameter("EPSG::8625", "A2", UnitQuantityType.Scale,
                                                     "Coefficient used in affine (general parametric) and polynomial transformations."));
            }
        }

        /// <summary>
        /// A3.
        /// </summary>
        public static CoordinateOperationParameter A3
        {
            get
            {
                return _A3 ?? (_A3 =
                    new CoordinateOperationParameter("EPSG::8626", "A3", UnitQuantityType.Scale,
                                                     "Coefficient used in affine (general parametric) and polynomial transformations."));
            }
        }

        /// <summary>
        /// A4.
        /// </summary>
        public static CoordinateOperationParameter A4
        {
            get
            {
                return _A4 ?? (_A4 =
                    new CoordinateOperationParameter("EPSG::8627", "A4", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// A5.
        /// </summary>
        public static CoordinateOperationParameter A5
        {
            get
            {
                return _A5 ?? (_A5 =
                    new CoordinateOperationParameter("EPSG::8628", "A5", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// A6.
        /// </summary>
        public static CoordinateOperationParameter A6
        {
            get
            {
                return _A6 ?? (_A6 =
                    new CoordinateOperationParameter("EPSG::8629", "A6", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// A7.
        /// </summary>
        public static CoordinateOperationParameter A7
        {
            get
            {
                return _A7 ?? (_A7 =
                    new CoordinateOperationParameter("EPSG::8630", "A7", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// A8.
        /// </summary>
        public static CoordinateOperationParameter A8
        {
            get
            {
                return _A8 ?? (_A8 =
                    new CoordinateOperationParameter("EPSG::8631", "A8", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au0v3.
        /// </summary>
        public static CoordinateOperationParameter Au0v3
        {
            get
            {
                return _Au0v3 ?? (_Au0v3 =
                    new CoordinateOperationParameter("EPSG::8632", "Au0v3", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au4v0.
        /// </summary>
        public static CoordinateOperationParameter Au4v0
        {
            get
            {
                return _Au4v0 ?? (_Au4v0 =
                    new CoordinateOperationParameter("EPSG::8633", "Au4v0", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au3v1.
        /// </summary>
        public static CoordinateOperationParameter Au3v1
        {
            get
            {
                return _Au3v1 ?? (_Au3v1 =
                    new CoordinateOperationParameter("EPSG::8634", "Au3v1", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au2v2.
        /// </summary>
        public static CoordinateOperationParameter Au2v2
        {
            get
            {
                return _Au2v2 ?? (_Au2v2 =
                    new CoordinateOperationParameter("EPSG::8635", "Au2v2", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au1v3.
        /// </summary>
        public static CoordinateOperationParameter Au1v3
        {
            get
            {
                return _Au1v3 ?? (_Au1v3 =
                    new CoordinateOperationParameter("EPSG::8636", "Au1v3", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au0v4.
        /// </summary>
        public static CoordinateOperationParameter Au0v4
        {
            get
            {
                return _Au0v4 ?? (_Au0v4 =
                    new CoordinateOperationParameter("EPSG::8637", "Au0v4", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// B00.
        /// </summary>
        public static CoordinateOperationParameter B00
        {
            get
            {
                return _B00 ?? (_B00 =
                    new CoordinateOperationParameter("EPSG::8638", "B00", UnitQuantityType.Scale,
                                                     "Coefficient used only in the Madrid to ED50 polynomial transformation method."));
            }
        }

        /// <summary>
        /// B0.
        /// </summary>
        public static CoordinateOperationParameter B0
        {
            get
            {
                return _B0 ?? (_B0 =
                    new CoordinateOperationParameter("EPSG::8639", "B0", UnitQuantityType.Scale,
                                                     "Coefficient used in affine (general parametric) and polynomial transformations."));
            }
        }

        /// <summary>
        /// B1.
        /// </summary>
        public static CoordinateOperationParameter B1
        {
            get
            {
                return _B1 ?? (_B1 =
                    new CoordinateOperationParameter("EPSG::8640", "B1", UnitQuantityType.Scale,
                                                     "Coefficient used in affine (general parametric) and polynomial transformations."));
            }
        }

        /// <summary>
        /// B2.
        /// </summary>
        public static CoordinateOperationParameter B2
        {
            get
            {
                return _B2 ?? (_B2 =
                    new CoordinateOperationParameter("EPSG::8641", "B2", UnitQuantityType.Scale,
                                                     "Coefficient used in affine (general parametric) and polynomial transformations."));
            }
        }

        /// <summary>
        /// B3.
        /// </summary>
        public static CoordinateOperationParameter B3
        {
            get
            {
                return _B3 ?? (_B3 =
                    new CoordinateOperationParameter("EPSG::8642", "B3", UnitQuantityType.Scale,
                                                     "Coefficient used in affine (general parametric) and polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu1v1.
        /// </summary>
        public static CoordinateOperationParameter Bu1v1
        {
            get
            {
                return _Bu1v1 ?? (_Bu1v1 =
                    new CoordinateOperationParameter("EPSG::8643", "Bu1v1", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu0v2.
        /// </summary>
        public static CoordinateOperationParameter Bu0v2
        {
            get
            {
                return _Bu0v2 ?? (_Bu0v2 =
                    new CoordinateOperationParameter("EPSG::8644", "Bu0v2", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu3v0.
        /// </summary>
        public static CoordinateOperationParameter Bu3v0
        {
            get
            {
                return _Bu3v0 ?? (_Bu3v0 =
                    new CoordinateOperationParameter("EPSG::8645", "Bu3v0", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu2v1.
        /// </summary>
        public static CoordinateOperationParameter Bu2v1
        {
            get
            {
                return _Bu2v1 ?? (_Bu2v1 =
                    new CoordinateOperationParameter("EPSG::8646", "Bu2v1", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu1v2.
        /// </summary>
        public static CoordinateOperationParameter Bu1v2
        {
            get
            {
                return _Bu1v2 ?? (_Bu1v2 =
                    new CoordinateOperationParameter("EPSG::8647", "Bu1v2", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu0v3.
        /// </summary>
        public static CoordinateOperationParameter Bu0v3
        {
            get
            {
                return _Bu0v3 ?? (_Bu0v3 =
                    new CoordinateOperationParameter("EPSG::8648", "Bu0v3", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu4v0.
        /// </summary>
        public static CoordinateOperationParameter Bu4v0
        {
            get
            {
                return _Bu4v0 ?? (_Bu4v0 =
                    new CoordinateOperationParameter("EPSG::8649", "Bu4v0", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu3v1.
        /// </summary>
        public static CoordinateOperationParameter Bu3v1
        {
            get
            {
                return _Bu3v1 ?? (_Bu3v1 =
                    new CoordinateOperationParameter("EPSG::8650", "Bu3v1", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu2v2.
        /// </summary>
        public static CoordinateOperationParameter Bu2v2
        {
            get
            {
                return _Bu2v2 ?? (_Bu2v2 =
                    new CoordinateOperationParameter("EPSG::8651", "Bu2v2", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu1v3.
        /// </summary>
        public static CoordinateOperationParameter Bu1v3
        {
            get
            {
                return _Bu1v3 ?? (_Bu1v3 =
                    new CoordinateOperationParameter("EPSG::8652", "Bu1v3", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu0v4.
        /// </summary>
        public static CoordinateOperationParameter Bu0v4
        {
            get
            {
                return _Bu0v4 ?? (_Bu0v4 =
                    new CoordinateOperationParameter("EPSG::8653", "Bu0v4", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Semi-major axis length difference.
        /// </summary>
        public static CoordinateOperationParameter SemiMajorAxisLengthDifference
        {
            get
            {
                return _semiMajorAxisLengthDifference ?? (_semiMajorAxisLengthDifference =
                    new CoordinateOperationParameter("EPSG::8654", "Semi-major axis length difference", UnitQuantityType.Length,
                                                     "The difference between the semi-major axis values of the ellipsoids used in the target and source coordinate reference systems."));
            }
        }

        /// <summary>
        /// Flattening difference.
        /// </summary>
        public static CoordinateOperationParameter FlatteningDifference
        {
            get
            {
                return _flatteningDifference ?? (_flatteningDifference =
                    new CoordinateOperationParameter("EPSG::8655", "Flattening difference", UnitQuantityType.Scale,
                                                     "The difference between the flattening values of the ellipsoids used in the target and source coordinate reference systems."));
            }
        }

        /// <summary>
        /// Point scale factor.
        /// </summary>
        public static CoordinateOperationParameter PointScaleFactor
        {
            get
            {
                return _pointScaleFactor ?? (_pointScaleFactor =
                    new CoordinateOperationParameter("EPSG::8663", "Point scale factor", UnitQuantityType.Scale,
                                                     "The point scale factor in a selected point of the target coordinate reference system. to be used as representative figure of the scale of the target coordinate reference system in a the area to which a coordinate transformation is defined."));
            }
        }

        /// <summary>
        /// Ordinate 3 of evaluation point.
        /// </summary>
        public static CoordinateOperationParameter Ordinate3OfEvaluationPoint
        {
            get
            {
                return _ordinate3OfEvaluationPoint ?? (_ordinate3OfEvaluationPoint =
                    new CoordinateOperationParameter("EPSG::8667", "Ordinate 3 of evaluation point", UnitQuantityType.Length,
                                                     "The value of the third ordinate of the evaluation point."));
            }
        }

        /// <summary>
        /// Au5v0.
        /// </summary>
        public static CoordinateOperationParameter Au5v0
        {
            get
            {
                return _Au5v0 ?? (_Au5v0 =
                    new CoordinateOperationParameter("EPSG::8668", "Au5v0", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au4v1.
        /// </summary>
        public static CoordinateOperationParameter Au4v1
        {
            get
            {
                return _Au4v1 ?? (_Au4v1 =
                    new CoordinateOperationParameter("EPSG::8669", "Au4v1", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au3v2.
        /// </summary>
        public static CoordinateOperationParameter Au3v2
        {
            get
            {
                return _Au3v2 ?? (_Au3v2 =
                    new CoordinateOperationParameter("EPSG::8670", "Au3v2", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au2v3.
        /// </summary>
        public static CoordinateOperationParameter Au2v3
        {
            get
            {
                return _Au2v3 ?? (_Au2v3 =
                    new CoordinateOperationParameter("EPSG::8671", "Au2v3", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au1v4.
        /// </summary>
        public static CoordinateOperationParameter Au1v4
        {
            get
            {
                return _Au1v4 ?? (_Au1v4 =
                    new CoordinateOperationParameter("EPSG::8672", "Au1v4", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au0v5.
        /// </summary>
        public static CoordinateOperationParameter Au0v5
        {
            get
            {
                return _Au0v5 ?? (_Au0v5 =
                    new CoordinateOperationParameter("EPSG::8673", "Au0v5", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au6v0.
        /// </summary>
        public static CoordinateOperationParameter Au6v0
        {
            get
            {
                return _Au6v0 ?? (_Au6v0 =
                    new CoordinateOperationParameter("EPSG::8674", "Au6v0", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au5v1.
        /// </summary>
        public static CoordinateOperationParameter Au5v1
        {
            get
            {
                return _Au5v1 ?? (_Au5v1 =
                    new CoordinateOperationParameter("EPSG::8675", "Au5v1", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au4v2.
        /// </summary>
        public static CoordinateOperationParameter Au4v2
        {
            get
            {
                return _Au4v2 ?? (_Au4v2 =
                    new CoordinateOperationParameter("EPSG::8676", "Au4v2", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au3v3.
        /// </summary>
        public static CoordinateOperationParameter Au3v3
        {
            get
            {
                return _Au3v3 ?? (_Au3v3 =
                    new CoordinateOperationParameter("EPSG::8677", "Au3v3", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au2v4.
        /// </summary>
        public static CoordinateOperationParameter Au2v4
        {
            get
            {
                return _Au2v4 ?? (_Au2v4 =
                    new CoordinateOperationParameter("EPSG::8678", "Au2v4", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au1v5.
        /// </summary>
        public static CoordinateOperationParameter Au1v5
        {
            get
            {
                return _Au1v5 ?? (_Au1v5 =
                    new CoordinateOperationParameter("EPSG::8679", "Au1v5", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au0v6.
        /// </summary>
        public static CoordinateOperationParameter Au0v6
        {
            get
            {
                return _Au0v6 ?? (_Au0v6 =
                    new CoordinateOperationParameter("EPSG::8680", "Au0v6", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu5v0.
        /// </summary>
        public static CoordinateOperationParameter Bu5v0
        {
            get
            {
                return _Bu5v0 ?? (_Bu5v0 =
                    new CoordinateOperationParameter("EPSG::8681", "Bu5v0", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu4v1.
        /// </summary>
        public static CoordinateOperationParameter Bu4v1
        {
            get
            {
                return _Bu4v1 ?? (_Bu4v1 =
                    new CoordinateOperationParameter("EPSG::8682", "Bu4v1", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu3v2.
        /// </summary>
        public static CoordinateOperationParameter Bu3v2
        {
            get
            {
                return _Bu3v2 ?? (_Bu3v2 =
                    new CoordinateOperationParameter("EPSG::8683", "Bu3v2", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu2v3.
        /// </summary>
        public static CoordinateOperationParameter Bu2v3
        {
            get
            {
                return _Bu2v3 ?? (_Bu2v3 =
                    new CoordinateOperationParameter("EPSG::8684", "Bu2v3", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu1v4.
        /// </summary>
        public static CoordinateOperationParameter Bu1v4
        {
            get
            {
                return _Bu1v4 ?? (_Bu1v4 =
                    new CoordinateOperationParameter("EPSG::8685", "Bu1v4", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu0v5.
        /// </summary>
        public static CoordinateOperationParameter Bu0v5
        {
            get
            {
                return _Bu0v5 ?? (_Bu0v5 =
                    new CoordinateOperationParameter("EPSG::8686", "Bu0v5", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu6v0.
        /// </summary>
        public static CoordinateOperationParameter Bu6v0
        {
            get
            {
                return _Bu6v0 ?? (_Bu6v0 =
                    new CoordinateOperationParameter("EPSG::8687", "Bu6v0", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu5v1.
        /// </summary>
        public static CoordinateOperationParameter Bu5v1
        {
            get
            {
                return _Bu5v1 ?? (_Bu5v1 =
                    new CoordinateOperationParameter("EPSG::8688", "Bu5v1", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu4v2.
        /// </summary>
        public static CoordinateOperationParameter Bu4v2
        {
            get
            {
                return _Bu4v2 ?? (_Bu4v2 =
                    new CoordinateOperationParameter("EPSG::8689", "Bu4v2", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu3v3.
        /// </summary>
        public static CoordinateOperationParameter Bu3v3
        {
            get
            {
                return _Bu3v3 ?? (_Bu3v3 =
                    new CoordinateOperationParameter("EPSG::8690", "Bu3v3", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu2v4.
        /// </summary>
        public static CoordinateOperationParameter Bu2v4
        {
            get
            {
                return _Bu2v4 ?? (_Bu2v4 =
                    new CoordinateOperationParameter("EPSG::8691", "Bu2v4", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu1v5.
        /// </summary>
        public static CoordinateOperationParameter Bu1v5
        {
            get
            {
                return _Bu1v5 ?? (_Bu1v5 =
                    new CoordinateOperationParameter("EPSG::8692", "8692", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu0v6.
        /// </summary>
        public static CoordinateOperationParameter Bu0v6
        {
            get
            {
                return _Bu0v6 ?? (_Bu0v6 =
                    new CoordinateOperationParameter("EPSG::8693", "Bu0v6", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Scaling factor for source CRS coord differences.
        /// </summary>
        public static CoordinateOperationParameter ScalingFactorForSourceCoordinateDifferences
        {
            get
            {
                return _scalingFactorForSourceCoordinateDifferences ?? (_scalingFactorForSourceCoordinateDifferences =
                    new CoordinateOperationParameter("EPSG::8694", "Scaling factor for source CRS coord differences", UnitQuantityType.Scale,
                                                     "Used in general polynomial transformations to normalize coordinate differences to an acceptable numerical range."));
            }
        }

        /// <summary>
        /// Scaling factor for target CRS coord differences.
        /// </summary>
        public static CoordinateOperationParameter ScalingFactorForTargetCoordinateDifferences
        {
            get
            {
                return _scalingFactorForTargetCoordinateDifferences ?? (_scalingFactorForTargetCoordinateDifferences =
                    new CoordinateOperationParameter("EPSG::8694", "Scaling factor for target CRS coord differences", UnitQuantityType.Scale,
                                                     "Used in general polynomial transformations to normalize coordinate differences to an acceptable numerical range."));
            }
        }

        /// <summary>
        /// Scaling factor for coord differences.
        /// </summary>
        public static CoordinateOperationParameter ScalingFactorForCoordinateDifferences
        {
            get
            {
                return _scalingFactorForCoordinateDifferences ?? (_scalingFactorForCoordinateDifferences =
                    new CoordinateOperationParameter("EPSG::8696", "Scaling factor for coord differences", UnitQuantityType.Scale,
                                                     "Used in reversible polynomial transformations to normalize coordinate differences to an acceptable numerical range. For the reverse transformation the forward target CRS becomes the reverse source CRS and forward source CRS becomes the reverse target CRS."));
            }
        }

        /// <summary>
        /// Au5v2.
        /// </summary>
        public static CoordinateOperationParameter Au5v2
        {
            get
            {
                return _Au5v2 ?? (_Au5v2 =
                    new CoordinateOperationParameter("EPSG::8697", "Au5v2", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au0v8.
        /// </summary>
        public static CoordinateOperationParameter Au0v8
        {
            get
            {
                return _Au0v8 ?? (_Au0v8 =
                    new CoordinateOperationParameter("EPSG::8698", "Au0v8", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au9v0.
        /// </summary>
        public static CoordinateOperationParameter Au9v0
        {
            get
            {
                return _Au9v0 ?? (_Au9v0 =
                    new CoordinateOperationParameter("EPSG::8699", "Au9v0", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au2v7.
        /// </summary>
        public static CoordinateOperationParameter Au2v7
        {
            get
            {
                return _Au2v7 ?? (_Au2v7 =
                    new CoordinateOperationParameter("EPSG::8700", "Au2v7", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au1v9.
        /// </summary>
        public static CoordinateOperationParameter Au1v9
        {
            get
            {
                return _Au1v9 ?? (_Au1v9 =
                    new CoordinateOperationParameter("EPSG::8701", "Au1v9", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au3v9.
        /// </summary>
        public static CoordinateOperationParameter Au3v9
        {
            get
            {
                return _Au3v9 ?? (_Au3v9 =
                    new CoordinateOperationParameter("EPSG::8702", "Au3v9", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu7v0.
        /// </summary>
        public static CoordinateOperationParameter Bu7v0
        {
            get
            {
                return _Bu7v0 ?? (_Bu7v0 =
                    new CoordinateOperationParameter("EPSG::8703", "Bu7v0", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu6v1.
        /// </summary>
        public static CoordinateOperationParameter Bu6v1
        {
            get
            {
                return _Bu6v1 ?? (_Bu6v1 =
                    new CoordinateOperationParameter("EPSG::8704", "Bu6v1", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu4v4.
        /// </summary>
        public static CoordinateOperationParameter Bu4v4
        {
            get
            {
                return _Bu4v4 ?? (_Bu4v4 =
                    new CoordinateOperationParameter("EPSG::8705", "Bu4v4", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu8v1.
        /// </summary>
        public static CoordinateOperationParameter Bu8v1
        {
            get
            {
                return _Bu8v1 ?? (_Bu8v1 =
                    new CoordinateOperationParameter("EPSG::8706", "Bu8v1", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu7v2.
        /// </summary>
        public static CoordinateOperationParameter Bu7v2
        {
            get
            {
                return _Bu7v2 ?? (_Bu7v2 =
                    new CoordinateOperationParameter("EPSG::8707", "Bu7v2", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu2v7
        /// </summary>
        public static CoordinateOperationParameter Bu2v7
        {
            get
            {
                return _Bu2v7 ?? (_Bu2v7 =
                    new CoordinateOperationParameter("EPSG::8708", "Bu2v7", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu0v9.
        /// </summary>
        public static CoordinateOperationParameter Bu0v9
        {
            get
            {
                return _Bu0v9 ?? (_Bu0v9 =
                    new CoordinateOperationParameter("EPSG::8709", "Bu0v9", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu4v6.
        /// </summary>
        public static CoordinateOperationParameter Bu4v6
        {
            get
            {
                return _Bu4v6 ?? (_Bu4v6 =
                    new CoordinateOperationParameter("EPSG::8710", "Bu4v6", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu9v2.
        /// </summary>
        public static CoordinateOperationParameter Bu9v2
        {
            get
            {
                return _Bu9v2 ?? (_Bu9v2 =
                    new CoordinateOperationParameter("EPSG::8711", "Bu9v2", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu8v3.
        /// </summary>
        public static CoordinateOperationParameter Bu8v3
        {
            get
            {
                return _Bu8v3 ?? (_Bu8v3 =
                    new CoordinateOperationParameter("EPSG::8712", "Bu8v3", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu5v7.
        /// </summary>
        public static CoordinateOperationParameter Bu5v7
        {
            get
            {
                return _Bu5v7 ?? (_Bu5v7 =
                    new CoordinateOperationParameter("EPSG::8713", "Bu5v7", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu9v4.
        /// </summary>
        public static CoordinateOperationParameter Bu9v4
        {
            get
            {
                return _Bu9v4 ?? (_Bu9v4 =
                    new CoordinateOperationParameter("EPSG::8714", "Bu9v4", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu4v9.
        /// </summary>
        public static CoordinateOperationParameter Bu4v9
        {
            get
            {
                return _Bu4v9 ?? (_Bu4v9 =
                    new CoordinateOperationParameter("EPSG::8715", "Bu4v9", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au1v0.
        /// </summary>
        public static CoordinateOperationParameter Au1v0
        {
            get
            {
                return _Au1v0 ?? (_Au1v0 =
                    new CoordinateOperationParameter("EPSG::8716", "Au1v0", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au0v1.
        /// </summary>
        public static CoordinateOperationParameter Au0v1
        {
            get
            {
                return _Au0v1 ?? (_Au0v1 =
                    new CoordinateOperationParameter("EPSG::8717", "Au0v1", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au2v0.
        /// </summary>
        public static CoordinateOperationParameter Au2v0
        {
            get
            {
                return _Au2v0 ?? (_Au2v0 =
                    new CoordinateOperationParameter("EPSG::8718", "Au2v0", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au1v1.
        /// </summary>
        public static CoordinateOperationParameter Au1v1
        {
            get
            {
                return _Au1v1 ?? (_Au1v1 =
                    new CoordinateOperationParameter("EPSG::8719", "Au1v1", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au0v2.
        /// </summary>
        public static CoordinateOperationParameter Au0v2
        {
            get
            {
                return _Au0v2 ?? (_Au0v2 =
                    new CoordinateOperationParameter("EPSG::8720", "Au0v2", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au3v0.
        /// </summary>
        public static CoordinateOperationParameter Au3v0
        {
            get
            {
                return _Au3v0 ?? (_Au3v0 =
                    new CoordinateOperationParameter("EPSG::8721", "Au3v0", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au2v1.
        /// </summary>
        public static CoordinateOperationParameter Au2v1
        {
            get
            {
                return _Au2v1 ?? (_Au2v1 =
                    new CoordinateOperationParameter("EPSG::8722", "Au2v1", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Au1v2.
        /// </summary>
        public static CoordinateOperationParameter Au1v2
        {
            get
            {
                return _Au1v2 ?? (_Au1v2 =
                    new CoordinateOperationParameter("EPSG::8723", "Au1v2", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu1v0.
        /// </summary>
        public static CoordinateOperationParameter Bu1v0
        {
            get
            {
                return _Bu1v0 ?? (_Bu1v0 =
                    new CoordinateOperationParameter("EPSG::8724", "Bu1v0", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu0v1.
        /// </summary>
        public static CoordinateOperationParameter Bu0v1
        {
            get
            {
                return _Bu0v1 ?? (_Bu0v1 =
                    new CoordinateOperationParameter("EPSG::8725", "Bu0v1", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Bu2v0.
        /// </summary>
        public static CoordinateOperationParameter Bu2v0
        {
            get
            {
                return _Bu2v0 ?? (_Bu2v0 =
                    new CoordinateOperationParameter("EPSG::8726", "Bu2v0", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Easting offset.
        /// </summary>
        public static CoordinateOperationParameter EastingOffset
        {
            get
            {
                return _eastingOffset ?? (_eastingOffset =
                    new CoordinateOperationParameter("EPSG::8728", "Easting offset", UnitQuantityType.Length,
                                                     "The difference between the easting values of a point in the target and source coordinate reference systems."));
            }
        }

        /// <summary>
        /// Latitude of natural origin.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOfNaturalOrigin
        {
            get
            {
                return _latitudeOfNaturalOrigin ?? (_latitudeOfNaturalOrigin =
                    new CoordinateOperationParameter("EPSG::8801", "Latitude of natural origin",
                                                     null, new String[] { "Latitude of origin" }, 
                                                     UnitQuantityType.Angle,
                                                     "The latitude of the point from which the values of both the geographical coordinates on the ellipsoid and the grid coordinates on the projection are deemed to increment or decrement for computational purposes. Alternatively it may be considered as the latitude of the point which in the absence of application of false coordinates has grid coordinates of (0,0)."));
            }
        }

        /// <summary>
        /// Longitude of natural origin.
        /// </summary>
        public static CoordinateOperationParameter LongitudeOfNaturalOrigin
        {
            get
            {
                return _longitudeOfNaturalOrigin ?? (_longitudeOfNaturalOrigin =
                    new CoordinateOperationParameter("EPSG::8802", "Longitude of natural origin", 
                                                     null, new String[] { "Central Meridian", "CM" }, 
                                                     UnitQuantityType.Angle,
                                                     "The longitude of the point from which the values of both the geographical coordinates on the ellipsoid and the grid coordinates on the projection are deemed to increment or decrement for computational purposes. Alternatively it may be considered as the longitude of the point which in the absence of application of false coordinates has grid coordinates of (0,0). Sometimes known as \"central meridian (CM)\"."));
            }
        }

        /// <summary>
        /// Scale factor at natural origin.
        /// </summary>
        public static CoordinateOperationParameter ScaleFactorAtNaturalOrigin
        {
            get
            {
                return _scaleFactorAtNaturalOrigin ?? (_scaleFactorAtNaturalOrigin =
                    new CoordinateOperationParameter("EPSG::8805", "Scale factor at natural origin", UnitQuantityType.Scale,
                                                     "The factor by which the map grid is reduced or enlarged during the projection process, defined by its value at the natural origin."));
            }
        }

        /// <summary>
        /// False easting.
        /// </summary>
        public static CoordinateOperationParameter FalseEasting
        {
            get
            {
                return _falseEasting ?? (_falseEasting =
                    new CoordinateOperationParameter("EPSG::8806", "False easting", UnitQuantityType.Length,
                    "Since the natural origin may be at or near the centre of the projection and under normal coordinate circumstances would thus give rise to negative coordinates over parts of the mapped area, this origin is usually given false coordinates which are large enough to avoid this inconvenience. The False Easting, FE, is the value assigned to the abscissa (east or west) axis of the projection grid at the natural origin."));
            }
        }

        /// <summary>
        /// False northing.
        /// </summary>
        public static CoordinateOperationParameter FalseNorthing
        {
            get
            {
                return _falseNorthing ?? (_falseNorthing =
                    new CoordinateOperationParameter("EPSG::8807", "False northing", UnitQuantityType.Length,
                                                     "Since the natural origin may be at or near the centre of the projection and under normal coordinate circumstances would thus give rise to negative coordinates over parts of the mapped area, this origin is usually given false coordinates which are large enough to avoid this inconvenience. The False Northing, FN, is the value assigned to the ordinate (north or south) axis of the projection grid at the natural origin."));
            }
        }

        /// <summary>
        /// Latitude of projection centre.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOfProjectionCentre
        {
            get
            {
                return _latitudeOfProjectionCentre ?? (_latitudeOfProjectionCentre =
                    new CoordinateOperationParameter("EPSG::8811", "Latitude of projection centre", UnitQuantityType.Angle,
                                                     "For an oblique projection, this is the latitude of the point at which the azimuth of the central line is defined."));
            }
        }

        /// <summary>
        /// Longitude of projection centre.
        /// </summary>
        public static CoordinateOperationParameter LongitudeOfProjectionCentre
        {
            get
            {
                return _longitudeOfProjectionCentre ?? (_longitudeOfProjectionCentre =
                    new CoordinateOperationParameter("EPSG::8812", "Longitude of projection centre", UnitQuantityType.Angle,
                                                     "For an oblique projection, this is the longitude of the point at which the azimuth of the central line is defined."));
            }
        }

        /// <summary>
        /// Azimuth of initial line.
        /// </summary>
        public static CoordinateOperationParameter AzimuthOfInitialLine
        {
            get
            {
                return _azimuthOfInitialLine ?? (_azimuthOfInitialLine =
                    new CoordinateOperationParameter("EPSG::8813", "Azimuth of initial line", UnitQuantityType.Angle,
                                                     "The azimuthal direction (north zero, east of north being positive) of the great circle which is the centre line of an oblique projection. The azimuth is given at the projection centre."));
            }
        }

        /// <summary>
        /// Angle from Rectified to Skew Grid.
        /// </summary>
        public static CoordinateOperationParameter AngleFromRectifiedToSkewGrid
        {
            get
            {
                return _angleFromRectifiedToSkewGrid ?? (_angleFromRectifiedToSkewGrid =
                    new CoordinateOperationParameter("EPSG::8814", "Angle from Rectified to Skew Grid", UnitQuantityType.Angle,
                                                     "The angle at the natural origin of an oblique projection through which the natural coordinate reference system is rotated to make the projection north axis parallel with true north."));
            }
        }

        /// <summary>
        /// Scale factor on initial line.
        /// </summary>
        public static CoordinateOperationParameter ScaleFactorOnInitialLine
        {
            get
            {
                return _scaleFactorOnInitialLine ?? (_scaleFactorOnInitialLine =
                    new CoordinateOperationParameter("EPSG::8815", "Scale factor on initial line", UnitQuantityType.Scale,
                                                     "The factor by which the map grid is reduced or enlarged during the projection process, defined by its value at the projection center."));
            }
        }

        /// <summary>
        /// Easting at projection centre.
        /// </summary>
        public static CoordinateOperationParameter EastingAtProjectionCentre
        {
            get
            {
                return _eastingAtProjectionCentre ?? (_eastingAtProjectionCentre =
                    new CoordinateOperationParameter("EPSG::8816", "Easting at projection centre", UnitQuantityType.Length,
                                                     "The easting value assigned to the projection centre."));
            }
        }

        /// <summary>
        /// Northing at projection centre.
        /// </summary>
        public static CoordinateOperationParameter NorthingAtProjectionCentre
        {
            get
            {
                return _northingAtProjectionCentre ?? (_northingAtProjectionCentre =
                    new CoordinateOperationParameter("EPSG::8817", "Northing at projection centre", UnitQuantityType.Length,
                                                     "The northing value assigned to the projection centre.  "));
            }
        }

        /// <summary>
        /// Latitude of pseudo standard parallel.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOfPseudoStandardParallel
        {
            get
            {
                return _latitudeOfPseudoStandardParallel ?? (_latitudeOfPseudoStandardParallel =
                    new CoordinateOperationParameter("EPSG::8818", "Latitude of pseudo standard parallel", UnitQuantityType.Angle,
                                                     "Latitude of the parallel on which the conic or cylindrical projection is based. This latitude is not geographic, but is defined on the conformal sphere AFTER its rotation to obtain the oblique aspect of the projection."));
            }
        }

        /// <summary>
        /// Scale factor on pseudo standard parallel.
        /// </summary>
        public static CoordinateOperationParameter ScaleFactorOnPseudoStandardParallel
        {
            get
            {
                return _scaleFactorOnPseudoStandardParallel ?? (_scaleFactorOnPseudoStandardParallel =
                    new CoordinateOperationParameter("EPSG::8819", "Scale factor on pseudo standard parallel", UnitQuantityType.Scale,
                                                     "The factor by which the map grid is reduced or enlarged during the projection process, defined by its value at the pseudo-standard parallel."));
            }
        }

        /// <summary>
        /// Latitude of false origin.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOfFalseOrigin
        {
            get
            {
                return _latitudeOfFalseOrigin ?? (_latitudeOfFalseOrigin =
                    new CoordinateOperationParameter("EPSG::8821", "Latitude of false origin", UnitQuantityType.Angle,
                                                     "The latitude of the point which is not the natural origin and at which grid coordinate values false easting and false northing are defined."));
            }
        }

        /// <summary>
        /// Longitude of false origin.
        /// </summary>
        public static CoordinateOperationParameter LongitudeOfFalseOrigin
        {
            get
            {
                return _longitudeOfFalseOrigin ?? (_longitudeOfFalseOrigin =
                    new CoordinateOperationParameter("EPSG::8822", "Longitude of false origin", UnitQuantityType.Angle,
                                                     "The longitude of the point which is not the natural origin and at which grid coordinate values false easting and false northing are defined."));
            }
        }

        /// <summary>
        /// Latitude of 1st standard parallel.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOf1stStandardParallel
        {
            get
            {
                return _latitudeOf1stStandardParallel ?? (_latitudeOf1stStandardParallel =
                    new CoordinateOperationParameter("EPSG::8823", "Latitude of 1st standard parallel", UnitQuantityType.Angle,
                                                     "For a conic projection with two standard parallels, this is the latitude of one of the parallels of intersection of the cone with the ellipsoid. It is normally but not necessarily that nearest to the pole. Scale is true along this parallel."));
            }
        }

        /// <summary>
        /// Latitude of 2nd standard parallel.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOf2ndStandardParallel
        {
            get
            {
                return _latitudeOf2ndStandardParallel ?? (_latitudeOf2ndStandardParallel =
                    new CoordinateOperationParameter("EPSG::8824", "Latitude of 2nd standard parallel", UnitQuantityType.Angle,
                                                     "For a conic projection with two standard parallels, this is the latitude of one of the parallels at which the cone intersects with the ellipsoid. It is normally but not necessarily that nearest to the equator. Scale is true along this parallel."));
            }
        }

        /// <summary>
        /// Easting at false origin.
        /// </summary>
        public static CoordinateOperationParameter EastingAtFalseOrigin
        {
            get
            {
                return _eastingAtFalseOrigin ?? (_eastingAtFalseOrigin =
                    new CoordinateOperationParameter("EPSG::8826", "Easting at false origin", UnitQuantityType.Length,
                                                     "The easting value assigned to the false origin."));
            }
        }

        /// <summary>
        /// Northing at false origin.
        /// </summary>
        public static CoordinateOperationParameter NorthingAtFalseOrigin
        {
            get
            {
                return _northingAtFalseOrigin ?? (_northingAtFalseOrigin =
                    new CoordinateOperationParameter("EPSG::8827", "Northing at false origin", UnitQuantityType.Length,
                                                     "The northing value assigned to the false origin."));
            }
        }

        /// <summary>
        /// Northing offset.
        /// </summary>
        public static CoordinateOperationParameter NorthingOffset
        {
            get
            {
                return _northingOffset ?? (_northingOffset =
                    new CoordinateOperationParameter("EPSG::8829", "Northing offset", UnitQuantityType.Length,
                                                     "The difference between the northing values of a point in the target and source coordinate reference systems."));
            }
        }

        /// <summary>
        /// Initial longitude.
        /// </summary>
        public static CoordinateOperationParameter InitialLongitude
        {
            get
            {
                return _initialLongitude ?? (_initialLongitude =
                    new CoordinateOperationParameter("EPSG::8830", "Initial longitude", UnitQuantityType.Angle,
                                                     "The longitude of the western limit of the first zone of a Transverse Mercator zoned grid system."));
            }
        }

        /// <summary>
        /// Zone width.
        /// </summary>
        public static CoordinateOperationParameter ZoneWidth
        {
            get
            {
                return _zoneWidth ?? (_zoneWidth =
                    new CoordinateOperationParameter("EPSG::8831", "Zone width", UnitQuantityType.Angle,
                                                     "The longitude width of a zone of a Transverse Mercator zoned grid system. "));
            }
        }

        /// <summary>
        /// Latitude of standard parallel.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOfStandardParallel
        {
            get
            {
                return _latitudeOfStandardParallel ?? (_latitudeOfStandardParallel =
                    new CoordinateOperationParameter("EPSG::8832", "Latitude of standard parallel", UnitQuantityType.Angle,
                                                     "For polar aspect azimuthal projections, the parallel on which the scale factor is defined to be unity."));
            }
        }

        /// <summary>
        /// Longitude of origin.
        /// </summary>
        public static CoordinateOperationParameter LongitudeOfOrigin
        {
            get
            {
                return _longitudeOfOrigin ?? (_longitudeOfOrigin =
                    new CoordinateOperationParameter("EPSG::8833", "Longitude of origin", UnitQuantityType.Angle,
                                                     "For polar aspect azimuthal projections, the meridian along which the northing axis increments and also across which parallels of latitude increment towards the north pole."));
            }
        }

        /// <summary>
        /// Latitude of topocentric origin.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOfTopocentricOrigin
        {
            get
            {
                return _latitudeOfTopocentricOrigin ?? (_latitudeOfTopocentricOrigin =
                    new CoordinateOperationParameter("EPSG::8834", "Latitude of topocentric origin", UnitQuantityType.Angle,
                                                     "For topocentric CSs, the latitude of the topocentric origin."));
            }
        }

        /// <summary>
        /// Longitude of topocentric origin.
        /// </summary>
        public static CoordinateOperationParameter LongitudeOfTopocentricOrigin
        {
            get
            {
                return _longitudeOfTopocentricOrigin ?? (_longitudeOfTopocentricOrigin =
                    new CoordinateOperationParameter("EPSG::8835", "Longitude of topocentric origin", UnitQuantityType.Angle,
                                                     "For topocentric CSs, the longitude of the topocentric origin."));
            }
        }

        /// <summary>
        /// Ellipsoidal height of topocentric origin.
        /// </summary>
        public static CoordinateOperationParameter EllipsoidalHeightOfTopocentricOrigin
        {
            get
            {
                return _ellipsoidalHeightOfTopocentricOrigin ?? (_ellipsoidalHeightOfTopocentricOrigin =
                    new CoordinateOperationParameter("EPSG::8836", "Ellipsoidal height of topocentric origin", UnitQuantityType.Length,
                                                     "For topocentric CSs, the ellipsoidal height of the topocentric origin."));
            }
        }

        /// <summary>
        /// Latitude of topographic origin.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOfTopographicOrigin
        {
            get
            {
                return _latitudeOfTopographicOrigin ?? (_latitudeOfTopographicOrigin =
                    new CoordinateOperationParameter("EPSG::8834", "Latitude of topographic origin", UnitQuantityType.Angle,
                                                     "For topographic CSs, the latitude of the topographic origin."));
            }
        }

        /// <summary>
        /// Longitude of topographic origin.
        /// </summary>
        public static CoordinateOperationParameter LongitudeOfTopographicOrigin
        {
            get
            {
                return _longitudeOfTopographicOrigin ?? (_longitudeOfTopographicOrigin =
                    new CoordinateOperationParameter("EPSG::8835", "Longitude of topographic origin", UnitQuantityType.Angle,
                                                     "For topographic CSs, the longitude of the topographic origin."));
            }
        }

        /// <summary>
        /// Ellipsoidal height of topographic origin.
        /// </summary>
        public static CoordinateOperationParameter EllipsoidalHeightOfTopographicOrigin
        {
            get
            {
                return _ellipsoidalHeightOfTopogrphicOrigin ?? (_ellipsoidalHeightOfTopogrphicOrigin =
                    new CoordinateOperationParameter("EPSG::8836", "Ellipsoidal height of topographic origin", UnitQuantityType.Length,
                                                     "For topographic CSs, the ellipsoidal height of the topographic origin."));
            }
        }

        /// <summary>
        /// Geocentric X of topocentric origin.
        /// </summary>
        public static CoordinateOperationParameter GeocenticXOfTopocentricOrigin
        {
            get
            {
                return _geocenticXOfTopocentricOrigin ?? (_geocenticXOfTopocentricOrigin =
                    new CoordinateOperationParameter("EPSG::8837", "Geocentric X of topocentric origin", UnitQuantityType.Length,
                                                     "For topocentric CSs, the geocentric Cartesian X coordinate of the topocentric origin."));
            }
        }

        /// <summary>
        /// Geocentric Y of topocentric origin.
        /// </summary>
        public static CoordinateOperationParameter GeocenticYOfTopocentricOrigin
        {
            get
            {
                return _geocenticYOfTopocentricOrigin ?? (_geocenticYOfTopocentricOrigin =
                    new CoordinateOperationParameter("EPSG::8838", "Geocentric Y of topocentric origin", UnitQuantityType.Length,
                                                     "For topocentric CSs, the geocentric Cartesian Y coordinate of the topocentric origin."));
            }
        }

        /// <summary>
        /// Geocentric Z of topocentric origin.
        /// </summary>
        public static CoordinateOperationParameter GeocenticZOfTopocentricOrigin
        {
            get
            {
                return _geocenticZOfTopocentricOrigin ?? (_geocenticZOfTopocentricOrigin =
                    new CoordinateOperationParameter("EPSG::8839", "Geocentric Z of topocentric origin", UnitQuantityType.Length,
                                                     "For topocentric CSs, the geocentric Cartesian Z coordinate of the topocentric origin."));
            }
        }

        /// <summary>
        /// Viewpoint height.
        /// </summary>
        public static CoordinateOperationParameter ViewPointHeight
        {
            get
            {
                return _viewPointHeight ?? (_viewPointHeight =
                    new CoordinateOperationParameter("EPSG::8840", "Viewpoint height", UnitQuantityType.Length,
                                                     "For vertical perspective projections, the height of viewpoint above the topocentric origin."));
            }
        }

        /// <summary>
        /// C1.
        /// </summary>
        public static CoordinateOperationParameter C1
        {
            get
            {
                return _C1 ?? (_C1 =
                    new CoordinateOperationParameter("EPSG::1026", "C1", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// C2.
        /// </summary>
        public static CoordinateOperationParameter C2
        {
            get
            {
                return _C2 ?? (_C2 =
                    new CoordinateOperationParameter("EPSG::1027", "C2", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// C3.
        /// </summary>
        public static CoordinateOperationParameter C3
        {
            get
            {
                return _C3 ?? (_C3 =
                    new CoordinateOperationParameter("EPSG::1028", "C3", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// C4.
        /// </summary>
        public static CoordinateOperationParameter C4
        {
            get
            {
                return _C4 ?? (_C4 =
                    new CoordinateOperationParameter("EPSG::1029", "C4", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// C5.
        /// </summary>
        public static CoordinateOperationParameter C5
        {
            get
            {
                return _C5 ?? (_C5 =
                    new CoordinateOperationParameter("EPSG::1030", "C5", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// C6.
        /// </summary>
        public static CoordinateOperationParameter C6
        {
            get
            {
                return _C6 ?? (_C6 =
                    new CoordinateOperationParameter("EPSG::1031", "C6", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// C7.
        /// </summary>
        public static CoordinateOperationParameter C7
        {
            get
            {
                return _C7 ?? (_C7 =
                    new CoordinateOperationParameter("EPSG::1032", "C7", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// C8.
        /// </summary>
        public static CoordinateOperationParameter C8
        {
            get
            {
                return _C8 ?? (_C8 =
                    new CoordinateOperationParameter("EPSG::1033", "C8", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// C9.
        /// </summary>
        public static CoordinateOperationParameter C9
        {
            get
            {
                return _C9 ?? (_C9 =
                    new CoordinateOperationParameter("EPSG::1034", "C9", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// C10.
        /// </summary>
        public static CoordinateOperationParameter C10
        {
            get
            {
                return _C10 ?? (_C10 =
                    new CoordinateOperationParameter("EPSG::1035", "C10", UnitQuantityType.Scale,
                                                     "Coefficient used in polynomial transformations."));
            }
        }

        /// <summary>
        /// Scale factor for source coordinate reference system first axis.
        /// </summary>
        public static CoordinateOperationParameter ScaleFactorForSourceCoordReferenceSystem1stAxis
        {
            get
            {
                return _scaleFactorForSourceCoordReferenceSystem1stAxis ?? (_scaleFactorForSourceCoordReferenceSystem1stAxis =
                    new CoordinateOperationParameter("EPSG::8612", "Scale factor for source coordinate reference system first axis", UnitQuantityType.Scale,
                                                     "The unit of measure of the source coordinate reference system first axis, expressed in the unit of measure of the target coordinate reference system."));
            }
        }

        /// <summary>
        /// Scale factor for source coordinate reference system second axis.
        /// </summary>
        public static CoordinateOperationParameter ScaleFactorForSourceCoordReferenceSystem2ndAxis
        {
            get
            {
                return _scaleFactorForSourceCoordReferenceSystem2ndAxis ?? (_scaleFactorForSourceCoordReferenceSystem2ndAxis =
                    new CoordinateOperationParameter("EPSG::8613", "Scale factor for source coordinate reference system second axis", UnitQuantityType.Scale,
                                                     "The unit of measure of the source coordinate reference system second axis, expressed in the unit of measure of the target coordinate reference system."));
            }
        }

        /// <summary>
        /// Rotation angle of source coordinate reference system axes.
        /// </summary>
        public static CoordinateOperationParameter RotationAngleOfSourceCoordReferenceSystemAxes
        {
            get
            {
                return _rotationAngleOfSourceCoordReferenceSystemAxes ?? (_rotationAngleOfSourceCoordReferenceSystemAxes =
                    new CoordinateOperationParameter("EPSG::8614", "Rotation angle of source coordinate reference system axes", UnitQuantityType.Angle,
                                                     "Angle (counter-clockwise positive) through which both of the source coordinate reference system axes need to rotated to coincide with the corresponding target coordinate reference system axes. Alternatively, the bearing (clockwise positive) of the source coordinate reference system Y-axis measured relative to target coordinate reference system north."));
            }
        }

        /// <summary>
        /// Bin node increment on I-axis.
        /// </summary>
        public static CoordinateOperationParameter BinNodeIncrementOnIAxis
        {
            get
            {
                return _binNodeIncrementOnIAxis ?? (_binNodeIncrementOnIAxis =
                    new CoordinateOperationParameter("EPSG::8741", "Bin node increment on I-axis", UnitQuantityType.Scale,
                                                     "The numerical increment between successive bin nodes on the I-axis."));
            }
        }

        /// <summary>
        /// Bin node increment on J-axis.
        /// </summary>
        public static CoordinateOperationParameter BinNodeIncrementOnJAxis
        {
            get
            {
                return _binNodeIncrementOnJAxis ?? (_binNodeIncrementOnJAxis =
                    new CoordinateOperationParameter("EPSG::8742", "Bin node increment on J-axis", UnitQuantityType.Scale,
                                                     "The numerical increment between successive bin nodes on the J-axis."));
            }
        }

        /// <summary>
        /// Bin grid origin I.
        /// </summary>
        public static CoordinateOperationParameter BinGridOriginI
        {
            get
            {
                return _binGridOriginI ?? (_binGridOriginI =
                    new CoordinateOperationParameter("EPSG::8733", "Bin grid origin I", UnitQuantityType.Scale,
                                                     "The value of the I-axis coordinate at the bin grid definition point. The I-axis is rotated 90 degrees clockwise from the J-axis."));
            }
        }

        /// <summary>
        /// Bin grid origin J.
        /// </summary>
        public static CoordinateOperationParameter BinGridOriginJ
        {
            get
            {
                return _binGridOriginJ ?? (_binGridOriginJ =
                    new CoordinateOperationParameter("EPSG::8734", "Bin grid origin J", UnitQuantityType.Scale,
                                                     "The value of the J-axis coordinate at the bin grid definition point."));
            }
        }

        /// <summary>
        /// Bin grid origin Easting.
        /// </summary>
        public static CoordinateOperationParameter BinGridOriginEasting
        {
            get
            {
                return _binGridOriginEasting ?? (_binGridOriginEasting =
                    new CoordinateOperationParameter("EPSG::8735", "Bin grid origin Easting", UnitQuantityType.Length,
                                                     "The value of the map grid Easting at the bin grid definition point."));
            }
        }

        /// <summary>
        /// Bin grid origin Northing.
        /// </summary>
        public static CoordinateOperationParameter BinGridOriginNorthing
        {
            get
            {
                return _binGridOriginNorthing ?? (_binGridOriginNorthing =
                    new CoordinateOperationParameter("EPSG::8736", "Bin grid origin Northing", UnitQuantityType.Length,
                                                     "The value of the map grid Northing at the bin grid definition point."));
            }
        }

        /// <summary>
        /// Scale factor of bin grid.
        /// </summary>
        public static CoordinateOperationParameter ScaleFactorOfBinGrid
        {
            get
            {
                return _scaleFactorOfBinGrid ?? (_scaleFactorOfBinGrid =
                    new CoordinateOperationParameter("EPSG::8737", "Scale factor of bin grid", UnitQuantityType.Scale,
                                                     "The point scale factor of the map grid coordinate reference system at a point within the bin grid. Generally either the bin grid origin or the centre of the bin grid will be the chosen point."));
            }
        }

        /// <summary>
        /// Bin width on I-axis.
        /// </summary>
        public static CoordinateOperationParameter BinWidthOnIAxis
        {
            get
            {
                return _binWidthOnIAxis ?? (_binWidthOnIAxis =
                    new CoordinateOperationParameter("EPSG::8738", "Bin width on I-axis", UnitQuantityType.Length,
                                                     "The nominal separation of bin nodes on the bin grid I-axis. (Note: the actual bin node separation is the product of the nominal separation and the scale factor of the bin grid)."));
            }
        }

        /// <summary>
        /// Bin width on J-axis.
        /// </summary>
        public static CoordinateOperationParameter BinWidthOnJAxis
        {
            get
            {
                return _binWidthOnJAxis ?? (_binWidthOnJAxis =
                    new CoordinateOperationParameter("EPSG::8739", "Bin width on J-axis", UnitQuantityType.Length,
                                                     "The nominal separation of bin nodes on the bin grid J-axis. (Note: the actual bin node separation is the product of the nominal separation and the scale factor of the bin grid)."));
            }
        }

        /// <summary>
        /// Map grid bearing of bin grid J-axis.
        /// </summary>
        public static CoordinateOperationParameter MapGridBearingOfBinGridJAxis
        {
            get
            {
                return _mapGridBearingOfBinGridJAxis ?? (_mapGridBearingOfBinGridJAxis =
                    new CoordinateOperationParameter("EPSG::8740", "Map grid bearing of bin grid J-axis", UnitQuantityType.Angle,
                                                     "The orientation of the bin grid J-axis measured clockwise from map grid north."));
            }
        }

        #endregion
    }
}
