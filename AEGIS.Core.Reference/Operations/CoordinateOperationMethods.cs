/// <copyright file="CoordinateOperationMethods.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a collection of known <see cref="CoordinateOperationMethod" /> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(CoordinateOperationMethod))]
    public static class CoordinateOperationMethods
    {
        #region Query fields

        private static CoordinateOperationMethod[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="CoordinateOperationMethod" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="CoordinateOperationMethod" /> instances within the collection.</value>
        public static IList<CoordinateOperationMethod> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(CoordinateOperationMethods).GetProperties().
                                                              Where(property => property.Name != "All").
                                                              Select(property => property.GetValue(null, null) as CoordinateOperationMethod).
                                                              ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="CoordinateOperationMethod" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="CoordinateOperationMethod" /> instances that match the specified identifier.</returns>
        public static IList<CoordinateOperationMethod> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            // identifier correction
            identifier = Regex.Escape(identifier);

            return All.Where(obj => Regex.IsMatch(obj.Identifier, identifier, RegexOptions.IgnoreCase)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="CoordinateOperationMethod" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="CoordinateOperationMethod" /> instances that match the specified name.</returns>
        public static IList<CoordinateOperationMethod> FromName(String name)
        {
            if (name == null)
                return null;

            // name correction
            name = Regex.Escape(name);

            return All.Where(obj => Regex.IsMatch(obj.Name, name, RegexOptions.IgnoreCase) ||
                                    obj.Aliases != null && obj.Aliases.Any(alias => Regex.IsMatch(alias, name, RegexOptions.IgnoreCase))).ToList().AsReadOnly();
        }

        #endregion

        #region Private static field

        private static CoordinateOperationMethod _affineParametricTransformation;
        private static CoordinateOperationMethod _albersEqualAreaProjection;
        private static CoordinateOperationMethod _americanPolyconicProjection;
        private static CoordinateOperationMethod _bonne;
        private static CoordinateOperationMethod _bonneSouthOrientated;
        private static CoordinateOperationMethod _cassiniSoldnerProjection;
        private static CoordinateOperationMethod _coordinateFrameRotationGeoc;
        private static CompoundCoordinateOperationMethod _coordinateFrameRotationGeog2D;
        private static CompoundCoordinateOperationMethod _coordinateFrameRotationGeog3D;
        private static CoordinateOperationMethod _complexPolynomial3Transformation;
        private static CoordinateOperationMethod _complexPolynomial4Transformation;
        private static CoordinateOperationMethod _ellipsoidToSphereTransformation;
        private static CoordinateOperationMethod _equidistantCylindricalProjection;
        private static CoordinateOperationMethod _equidistantCylindricalSphericalProjection;
        private static CoordinateOperationMethod _generalPolynomial2Transformation;
        private static CoordinateOperationMethod _generalPolynomial3Transformation;
        private static CoordinateOperationMethod _generalPolynomial4Transformation;
        private static CoordinateOperationMethod _generalPolynomial6Transformation;
        private static CoordinateOperationMethod _geographic2DOffsets;
        private static CoordinateOperationMethod _geographic3DTo2DConversion;
        private static CoordinateOperationMethod _geographicGridProjection;
        private static CoordinateOperationMethod _geographicToGeocentricConversion;
        private static CoordinateOperationMethod _geocentricToTopocentricConversion;
        private static CoordinateOperationMethod _geographicToTopocentricConversion;
        private static CoordinateOperationMethod _geocentricTranslationGeoc;
        private static CompoundCoordinateOperationMethod _geocentricTranslationGeog2D;
        private static CompoundCoordinateOperationMethod _geocentricTranslationGeog3D;
        private static CoordinateOperationMethod _gnomonicProjection;
        private static CoordinateOperationMethod _guamProjection;
        private static CoordinateOperationMethod _hotineObliqueMercatorAProjection;
        private static CoordinateOperationMethod _hotineObliqueMercatorBProjection;
        private static CoordinateOperationMethod _labordeObliqueMercatorProjection;
        private static CoordinateOperationMethod _hyperbolicCassiniSoldnerProjection;
        private static CoordinateOperationMethod _krovakProjection;
        private static CoordinateOperationMethod _krovakNorthOrientedProjection;
        private static CoordinateOperationMethod _krovakModifiedProjection;
        private static CoordinateOperationMethod _krovakModifiedNorthOrientedProjection;
        private static CoordinateOperationMethod _lambertAzimuthalEqualAreaProjection;
        private static CoordinateOperationMethod _lambertAzimuthalEqualAreaSphericalProjection; 
        private static CoordinateOperationMethod _lambertConicConformal1SPProjection;
        private static CoordinateOperationMethod _lambertConicConformal1SPWestOrientatedProjection;
        private static CoordinateOperationMethod _lambertConicConformal2SPProjection;
        private static CoordinateOperationMethod _lambertConicConformal2SPBelgiumProjection;
        private static CoordinateOperationMethod _lambertConicNearConformalProjection;
        private static CoordinateOperationMethod _lambertCylindricalEqualAreaEllipsoidalProjection;
        private static CoordinateOperationMethod _lambertCylindricalEqualAreaSphericalProjection;
        private static CoordinateOperationMethod _mercatorAProjection;
        private static CoordinateOperationMethod _mercatorBProjection;
        private static CoordinateOperationMethod _mercatorCProjection;
        private static CoordinateOperationMethod _mercatorSphericalProjection;
        private static CoordinateOperationMethod _militaryGridProjection;
        private static CoordinateOperationMethod _modifiedAzimuthalEquidistantProjection;
        private static CoordinateOperationMethod _molodenskyBadekasTransformation;
        private static CoordinateOperationMethod _molodenskyTransformation;
        private static CoordinateOperationMethod _obliqueStereographicProjection;
        private static CoordinateOperationMethod _p6LeftHandedSeismicBinGridTransformation;
        private static CoordinateOperationMethod _p6RightHandedSeismicBinGridTransformation;
        private static CoordinateOperationMethod _polarStereographicAProjection;
        private static CoordinateOperationMethod _polarStereographicBProjection;
        private static CoordinateOperationMethod _polarStereographicCProjection;
        private static CoordinateOperationMethod _popularVisualisationPseudoMercatorProjection;
        private static CoordinateOperationMethod _positionVectorTransformation;
        private static CoordinateOperationMethod _pseudoPlateCareeProjection;
        private static CoordinateOperationMethod _reversiblePolynomial2Transformation;
        private static CoordinateOperationMethod _reversiblePolynomial3Transformation;
        private static CoordinateOperationMethod _reversiblePolynomial4Transformation;
        private static CoordinateOperationMethod _reversiblePolynomial13Transformation;
        private static CoordinateOperationMethod _similarityTransformation;
        private static CoordinateOperationMethod _sinusoidalProjection;
        private static CoordinateOperationMethod _transverseMercatorProjection;
        private static CoordinateOperationMethod _transverseMercatorSouthProjection;
        private static CoordinateOperationMethod _transverseMercatorZonedProjection;
        private static CoordinateOperationMethod _verticalPerspectiveOrthographicProjection;
        private static CoordinateOperationMethod _verticalPerspectiveProjection;
        private static CoordinateOperationMethod _worldMillerCylindricalProjection;

        #endregion

        #region Public static properties

        /// <summary>
        /// Affine parametric transformation.
        /// </summary>
        public static CoordinateOperationMethod AffineParametricTransformation
        {
            get
            {
                return _affineParametricTransformation ??
                      (_affineParametricTransformation =
                           new CoordinateOperationMethod("EPSG::9624", "Affine parametric transformation", true,
                                                         CoordinateOperationParameters.A0,
                                                         CoordinateOperationParameters.A1,
                                                         CoordinateOperationParameters.A2,
                                                         CoordinateOperationParameters.B0,
                                                         CoordinateOperationParameters.B1,
                                                         CoordinateOperationParameters.B2));
            }
        }

        /// <summary>
        /// Albers Equal Area.
        /// </summary>
        public static CoordinateOperationMethod AlbersEqualAreaProjection
        {
            get
            {
                return _albersEqualAreaProjection ??
                      (_albersEqualAreaProjection =
                           new CoordinateOperationMethod("EPSG::9822", "Albers Equal Area", true,
                                                         CoordinateOperationParameters.LatitudeOfFalseOrigin,
                                                         CoordinateOperationParameters.LongitudeOfFalseOrigin,
                                                         CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                                         CoordinateOperationParameters.LatitudeOf2ndStandardParallel,
                                                         CoordinateOperationParameters.EastingAtFalseOrigin,
                                                         CoordinateOperationParameters.NorthingAtFalseOrigin));
            }
        }

        /// <summary>
        /// American Polyconic.
        /// </summary>
        public static CoordinateOperationMethod AmericanPolyconicProjection
        {
            get
            {
                if (_americanPolyconicProjection == null)
                    _americanPolyconicProjection =
                        new CoordinateOperationMethod("EPSG::9818", "American Polyconic", true,
                                                      CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _americanPolyconicProjection;
            }
        }

        /// <summary>
        /// Bonne.
        /// </summary>
        public static CoordinateOperationMethod Bonne
        {
            get
            {
                if (_bonne == null)
                    _bonne =
                        new CoordinateOperationMethod("EPSG::9827", "Bonne", true,
                                                      CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);
                return _bonne;
            }
        }

        /// <summary>
        /// Bonne South Orientated.
        /// </summary>
        public static CoordinateOperationMethod BonneSouthOrientated
        {
            get
            {
                if (_bonneSouthOrientated == null)
                    _bonneSouthOrientated =
                        new CoordinateOperationMethod("EPSG::9828", "Bonne South Orientated", true,
                                                      CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);
                return _bonneSouthOrientated;
            }
        }

        /// <summary>
        /// Cassini-Soldner.
        /// </summary>
        public static CoordinateOperationMethod CassiniSoldnerProjection
        {
            get
            {
                if (_cassiniSoldnerProjection == null)
                    _cassiniSoldnerProjection =
                        new CoordinateOperationMethod("EPSG::9806", "Cassini-Soldner", true,
                                                      CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _cassiniSoldnerProjection;
            }
        }

        /// <summary>
        /// Complex polynomial of degree 3.
        /// </summary>
        public static CoordinateOperationMethod ComplexPolynomial3
        {
            get
            {
                if (_complexPolynomial3Transformation == null)
                    _complexPolynomial3Transformation =
                        new CoordinateOperationMethod("EPSG::9652", "Complex polynomial of degree 3 ", false,
                                                      CoordinateOperationParameters.Ordinate1OfEvaluationPointInSource,
                                                      CoordinateOperationParameters.Ordinate2OfEvaluationPointInSource,
                                                      CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget,
                                                      CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget,
                                                      CoordinateOperationParameters.ScalingFactorForSourceCoordinateDifferences,
                                                      CoordinateOperationParameters.ScalingFactorForTargetCoordinateDifferences,
                                                      CoordinateOperationParameters.A1,
                                                      CoordinateOperationParameters.A2,
                                                      CoordinateOperationParameters.A3,
                                                      CoordinateOperationParameters.A4,
                                                      CoordinateOperationParameters.A5,
                                                      CoordinateOperationParameters.A6);

                return _complexPolynomial3Transformation;
            }
        }

        /// <summary>
        /// Complex polynomial of degree 4 .
        /// </summary>
        public static CoordinateOperationMethod ComplexPolynomial4
        {
            get
            {
                if (_complexPolynomial4Transformation == null)
                    _complexPolynomial4Transformation =
                        new CoordinateOperationMethod("EPSG::9653", "Complex polynomial of degree 4 ", false,
                                                      CoordinateOperationParameters.Ordinate1OfEvaluationPointInSource,
                                                      CoordinateOperationParameters.Ordinate2OfEvaluationPointInSource,
                                                      CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget,
                                                      CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget,
                                                      CoordinateOperationParameters.ScalingFactorForSourceCoordinateDifferences,
                                                      CoordinateOperationParameters.ScalingFactorForTargetCoordinateDifferences,
                                                      CoordinateOperationParameters.A1,
                                                      CoordinateOperationParameters.A2,
                                                      CoordinateOperationParameters.A3,
                                                      CoordinateOperationParameters.A4,
                                                      CoordinateOperationParameters.A5,
                                                      CoordinateOperationParameters.A6,
                                                      CoordinateOperationParameters.A7,
                                                      CoordinateOperationParameters.A8);

                return _complexPolynomial4Transformation;
            }
        }

        /// <summary>
        /// Coordinate Frame Rotation (geocentric domain).
        /// </summary>
        public static CoordinateOperationMethod CoordinateFrameRotationGeocentricDomain
        {
            get
            {
                return _coordinateFrameRotationGeoc ?? (_coordinateFrameRotationGeoc =
                        new CoordinateOperationMethod("EPSG::1032", "Coordinate Frame Rotation (geocentric domain)", true,
                                                      CoordinateOperationParameters.XAxisTranslation,
                                                      CoordinateOperationParameters.YAxisTranslation,
                                                      CoordinateOperationParameters.ZAxisTranslation,
                                                      CoordinateOperationParameters.XAxisRotation,
                                                      CoordinateOperationParameters.YAxisRotation,
                                                      CoordinateOperationParameters.ZAxisRotation,
                                                      CoordinateOperationParameters.ScaleDifference));
            }
        }

        /// <summary>
        /// Coordinate Frame Rotation (geog2D domain).
        /// </summary>
        public static CompoundCoordinateOperationMethod CoordinateFrameRotationGeographic2DDomain
        {
            get
            {
                return _coordinateFrameRotationGeog2D ?? (_coordinateFrameRotationGeog2D =
                        new CompoundCoordinateOperationMethod("EPSG::9607", "Coordinate Frame Rotation (geog2D domain)", true, 
                                                                  Geographic3DTo2DConversion,
                                                                  GeographicToGeocentricConversion,
                                                                  CoordinateFrameRotationGeocentricDomain,
                                                                  GeographicToGeocentricConversion,
                                                                  Geographic3DTo2DConversion));
            }
        }

        /// <summary>
        /// Coordinate Frame Rotation (geog3D domain).
        /// </summary>
        public static CompoundCoordinateOperationMethod CoordinateFrameRotationGeographic3DDomain
        {
            get
            {
                return _coordinateFrameRotationGeog3D ?? (_coordinateFrameRotationGeog3D =
                        new CompoundCoordinateOperationMethod("EPSG::1038", "Coordinate Frame Rotation (geog3D domain)", true,
                                                                  GeographicToGeocentricConversion,
                                                                  CoordinateFrameRotationGeocentricDomain,
                                                                  GeographicToGeocentricConversion));
            }
        }

        /// <summary>
        /// Ellipsoid to sphere transformation.
        /// </summary>
        public static CoordinateOperationMethod EllipsoidToSphereTransformation
        {
            get
            {
                return _ellipsoidToSphereTransformation ?? (_ellipsoidToSphereTransformation =
                    new CoordinateOperationMethod("AEGIS::735001", "Ellipsoid to sphere transformation", true, 
                                                  CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                                  CoordinateOperationParameters.LongitudeOfNaturalOrigin));
            }
        }

        /// <summary>
        /// Equidistant Cylindrical.
        /// </summary>
        public static CoordinateOperationMethod EquidistantCylindricalProjection
        {
            get
            {
                if (_equidistantCylindricalProjection == null)
                    _equidistantCylindricalProjection =
                        new CoordinateOperationMethod("EPSG::1028", "Equidistant Cylindrical", true,
                                                      CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _equidistantCylindricalProjection;
            }
        }

        /// <summary>
        /// Equidistant Cylindrical (Spherical).
        /// </summary>
        public static CoordinateOperationMethod EquidistantCylindricalSphericalProjection
        {
            get
            {
                if (_equidistantCylindricalSphericalProjection == null)
                    _equidistantCylindricalSphericalProjection =
                        new CoordinateOperationMethod("EPSG::1029", "Equidistant Cylindrical (Spherical)", true,
                                                      CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _equidistantCylindricalSphericalProjection;
            }
        }

        /// <summary>
        /// General polynomial of degree 2.
        /// </summary>
        public static CoordinateOperationMethod GeneralPolynomial2
        {
            get
            {
                if (_generalPolynomial2Transformation == null)
                    _generalPolynomial2Transformation =
                        new CoordinateOperationMethod("EPSG::9645", "General polynomial of degree 2", false,
                                                      CoordinateOperationParameters.Ordinate1OfEvaluationPointInSource,
                                                      CoordinateOperationParameters.Ordinate2OfEvaluationPointInSource,
                                                      CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget,
                                                      CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget,
                                                      CoordinateOperationParameters.ScalingFactorForSourceCoordinateDifferences,
                                                      CoordinateOperationParameters.ScalingFactorForTargetCoordinateDifferences,
                                                      CoordinateOperationParameters.A0,
                                                      CoordinateOperationParameters.Au1v0,
                                                      CoordinateOperationParameters.Au0v1,
                                                      CoordinateOperationParameters.Au2v0,
                                                      CoordinateOperationParameters.Au1v1,
                                                      CoordinateOperationParameters.Au0v2,
                                                      CoordinateOperationParameters.B0,
                                                      CoordinateOperationParameters.Bu1v0,
                                                      CoordinateOperationParameters.Bu0v1,
                                                      CoordinateOperationParameters.Bu2v0,
                                                      CoordinateOperationParameters.Bu1v1,
                                                      CoordinateOperationParameters.Bu0v2);

                return _generalPolynomial2Transformation;
            }
        }

        /// <summary>
        /// General polynomial of degree 3.
        /// </summary>
        public static CoordinateOperationMethod GeneralPolynomial3
        {
            get
            {
                if (_generalPolynomial3Transformation == null)
                    _generalPolynomial3Transformation =
                        new CoordinateOperationMethod("EPSG::9646", "General polynomial of degree 3", false,
                                                      CoordinateOperationParameters.Ordinate1OfEvaluationPointInSource,
                                                      CoordinateOperationParameters.Ordinate2OfEvaluationPointInSource,
                                                      CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget,
                                                      CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget,
                                                      CoordinateOperationParameters.ScalingFactorForSourceCoordinateDifferences,
                                                      CoordinateOperationParameters.ScalingFactorForTargetCoordinateDifferences,
                                                      CoordinateOperationParameters.A0,
                                                      CoordinateOperationParameters.Au1v0,
                                                      CoordinateOperationParameters.Au0v1,
                                                      CoordinateOperationParameters.Au2v0,
                                                      CoordinateOperationParameters.Au1v1,
                                                      CoordinateOperationParameters.Au0v2,
                                                      CoordinateOperationParameters.Au3v0,
                                                      CoordinateOperationParameters.Au2v1,
                                                      CoordinateOperationParameters.Au1v2,
                                                      CoordinateOperationParameters.Au0v3,
                                                      CoordinateOperationParameters.B0,
                                                      CoordinateOperationParameters.Bu1v0,
                                                      CoordinateOperationParameters.Bu0v1,
                                                      CoordinateOperationParameters.Bu2v0,
                                                      CoordinateOperationParameters.Bu1v1,
                                                      CoordinateOperationParameters.Bu0v2,
                                                      CoordinateOperationParameters.Bu3v0,
                                                      CoordinateOperationParameters.Bu2v1,
                                                      CoordinateOperationParameters.Bu1v2,
                                                      CoordinateOperationParameters.Bu0v3);

                return _generalPolynomial3Transformation;
            }
        }

        /// <summary>
        /// General polynomial of degree 4.
        /// </summary>
        public static CoordinateOperationMethod GeneralPolynomial4
        {
            get
            {
                if (_generalPolynomial4Transformation == null)
                    _generalPolynomial4Transformation =
                        new CoordinateOperationMethod("EPSG::9647", "General polynomial of degree 4", false,
                                                      CoordinateOperationParameters.Ordinate1OfEvaluationPointInSource,
                                                      CoordinateOperationParameters.Ordinate2OfEvaluationPointInSource,
                                                      CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget,
                                                      CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget,
                                                      CoordinateOperationParameters.ScalingFactorForSourceCoordinateDifferences,
                                                      CoordinateOperationParameters.ScalingFactorForTargetCoordinateDifferences,
                                                      CoordinateOperationParameters.A0,
                                                      CoordinateOperationParameters.Au1v0,
                                                      CoordinateOperationParameters.Au0v1,
                                                      CoordinateOperationParameters.Au2v0,
                                                      CoordinateOperationParameters.Au1v1,
                                                      CoordinateOperationParameters.Au0v2,
                                                      CoordinateOperationParameters.Au3v0,
                                                      CoordinateOperationParameters.Au2v1,
                                                      CoordinateOperationParameters.Au1v2,
                                                      CoordinateOperationParameters.Au0v3,
                                                      CoordinateOperationParameters.Au4v0,
                                                      CoordinateOperationParameters.Au3v1,
                                                      CoordinateOperationParameters.Au2v2,
                                                      CoordinateOperationParameters.Au1v3,
                                                      CoordinateOperationParameters.Au0v4,
                                                      CoordinateOperationParameters.B0,
                                                      CoordinateOperationParameters.Bu1v0,
                                                      CoordinateOperationParameters.Bu0v1,
                                                      CoordinateOperationParameters.Bu2v0,
                                                      CoordinateOperationParameters.Bu1v1,
                                                      CoordinateOperationParameters.Bu0v2,
                                                      CoordinateOperationParameters.Bu3v0,
                                                      CoordinateOperationParameters.Bu2v1,
                                                      CoordinateOperationParameters.Bu1v2,
                                                      CoordinateOperationParameters.Bu0v3,
                                                      CoordinateOperationParameters.Bu4v0,
                                                      CoordinateOperationParameters.Bu3v1,
                                                      CoordinateOperationParameters.Bu2v2,
                                                      CoordinateOperationParameters.Bu1v3,
                                                      CoordinateOperationParameters.Bu0v4);

                return _generalPolynomial4Transformation;
            }
        }

        /// <summary>
        /// General polynomial of degree 6.
        /// </summary>
        public static CoordinateOperationMethod GeneralPolynomial6
        {
            get
            {
                if (_generalPolynomial6Transformation == null)
                    _generalPolynomial6Transformation =
                        new CoordinateOperationMethod("EPSG::9648", "General polynomial of degree 6", false,
                                                      CoordinateOperationParameters.Ordinate1OfEvaluationPointInSource,
                                                      CoordinateOperationParameters.Ordinate2OfEvaluationPointInSource,
                                                      CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget,
                                                      CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget,
                                                      CoordinateOperationParameters.ScalingFactorForSourceCoordinateDifferences,
                                                      CoordinateOperationParameters.ScalingFactorForTargetCoordinateDifferences,
                                                      CoordinateOperationParameters.A0,
                                                      CoordinateOperationParameters.Au1v0,
                                                      CoordinateOperationParameters.Au0v1,
                                                      CoordinateOperationParameters.Au2v0,
                                                      CoordinateOperationParameters.Au1v1,
                                                      CoordinateOperationParameters.Au0v2,
                                                      CoordinateOperationParameters.Au3v0,
                                                      CoordinateOperationParameters.Au2v1,
                                                      CoordinateOperationParameters.Au1v2,
                                                      CoordinateOperationParameters.Au0v3,
                                                      CoordinateOperationParameters.Au4v0,
                                                      CoordinateOperationParameters.Au3v1,
                                                      CoordinateOperationParameters.Au2v2,
                                                      CoordinateOperationParameters.Au1v3,
                                                      CoordinateOperationParameters.Au0v4,
                                                      CoordinateOperationParameters.Au5v0,
                                                      CoordinateOperationParameters.Au4v1,
                                                      CoordinateOperationParameters.Au3v2,
                                                      CoordinateOperationParameters.Au2v3,
                                                      CoordinateOperationParameters.Au1v4,
                                                      CoordinateOperationParameters.Au0v5,
                                                      CoordinateOperationParameters.Au6v0,
                                                      CoordinateOperationParameters.Au5v1,
                                                      CoordinateOperationParameters.Au4v2,
                                                      CoordinateOperationParameters.Au3v3,
                                                      CoordinateOperationParameters.Au2v4,
                                                      CoordinateOperationParameters.Au1v5,
                                                      CoordinateOperationParameters.Au0v6,
                                                      CoordinateOperationParameters.B0,
                                                      CoordinateOperationParameters.Bu1v0,
                                                      CoordinateOperationParameters.Bu0v1,
                                                      CoordinateOperationParameters.Bu2v0,
                                                      CoordinateOperationParameters.Bu1v1,
                                                      CoordinateOperationParameters.Bu0v2,
                                                      CoordinateOperationParameters.Bu3v0,
                                                      CoordinateOperationParameters.Bu2v1,
                                                      CoordinateOperationParameters.Bu1v2,
                                                      CoordinateOperationParameters.Bu0v3,
                                                      CoordinateOperationParameters.Bu4v0,
                                                      CoordinateOperationParameters.Bu3v1,
                                                      CoordinateOperationParameters.Bu2v2,
                                                      CoordinateOperationParameters.Bu1v3,
                                                      CoordinateOperationParameters.Bu0v4,
                                                      CoordinateOperationParameters.Bu5v0,
                                                      CoordinateOperationParameters.Bu4v1,
                                                      CoordinateOperationParameters.Bu3v2,
                                                      CoordinateOperationParameters.Bu2v3,
                                                      CoordinateOperationParameters.Bu1v4,
                                                      CoordinateOperationParameters.Bu0v5,
                                                      CoordinateOperationParameters.Bu6v0,
                                                      CoordinateOperationParameters.Bu5v1,
                                                      CoordinateOperationParameters.Bu4v2,
                                                      CoordinateOperationParameters.Bu3v3,
                                                      CoordinateOperationParameters.Bu2v4,
                                                      CoordinateOperationParameters.Bu1v5,
                                                      CoordinateOperationParameters.Bu0v6);

                return _generalPolynomial6Transformation;
            }
        }

        /// <summary>
        /// Geocentric translations (geocentric domain).
        /// </summary>
        public static CoordinateOperationMethod GeocentricTranslationGeocentricDomain
        {
            get
            {
                if (_geocentricTranslationGeoc == null)
                    _geocentricTranslationGeoc =
                        new CoordinateOperationMethod("EPSG::1031", "Geocentric translations (geocentric domain)", true,
                                                      CoordinateOperationParameters.XAxisTranslation,
                                                      CoordinateOperationParameters.YAxisTranslation,
                                                      CoordinateOperationParameters.ZAxisTranslation);

                return _geocentricTranslationGeoc;
            }
        }

        /// <summary>
        /// Geocentric translations (geog2D domain).
        /// </summary>
        public static CompoundCoordinateOperationMethod GeocentricTranslationGeographic2DDomain
        {
            get
            {
                return _geocentricTranslationGeog2D ?? (_geocentricTranslationGeog2D =
                        new CompoundCoordinateOperationMethod("EPSG::9603", "Geocentric translations (geog2D domain)", true,
                                                                  Geographic3DTo2DConversion,
                                                                  GeographicToGeocentricConversion,
                                                                  GeocentricTranslationGeocentricDomain,
                                                                  GeographicToGeocentricConversion,
                                                                  Geographic3DTo2DConversion));
            }
        }

        /// <summary>
        /// Geocentric translations (geog3D domain).
        /// </summary>
        public static CompoundCoordinateOperationMethod GeocentricTranslationGeographic3DDomain
        {
            get
            {
                return _geocentricTranslationGeog3D ?? (_geocentricTranslationGeog3D =
                        new CompoundCoordinateOperationMethod("EPSG::1035", "Geocentric translations (geog3D domain)", true,
                                                                  GeographicToGeocentricConversion,
                                                                  GeocentricTranslationGeocentricDomain,
                                                                  GeographicToGeocentricConversion));
            }
        }

        /// <summary>
        /// Geographic2D offsets.
        /// </summary>
        public static CoordinateOperationMethod Geographic2DOffsets
        {
            get
            {
                if (_geographic2DOffsets == null)
                    _geographic2DOffsets =
                         new CoordinateOperationMethod("EPSG::9619", "Geographic2D offsets", true,
                                                       CoordinateOperationParameters.LatitudeOffset,
                                                       CoordinateOperationParameters.LongitudeOffset);

                return _geographic2DOffsets;
            }
        }

        /// <summary>
        /// Geographic3D to 2D conversion.
        /// </summary>
        public static CoordinateOperationMethod Geographic3DTo2DConversion
        {
            get
            {
                if (_geographic3DTo2DConversion == null)
                    _geographic3DTo2DConversion =
                        new CoordinateOperationMethod("EPSG::9659", "Geographic3D to 2D conversion", true);

                return _geographic3DTo2DConversion;
            }
        }

        /// <summary>
        /// Geographic grid projection.
        /// </summary>
        public static CoordinateOperationMethod GeographicGridProjection
        {
            get
            {
                return _geographicGridProjection ?? (_geographicGridProjection =
                    new CoordinateOperationMethod("AEGIS::735201", "Geographic grid projection", true));
            }
        }

        /// <summary>
        /// Geographic/geocentric conversion.
        /// </summary>
        public static CoordinateOperationMethod GeographicToGeocentricConversion
        {
            get
            {
                if (_geographicToGeocentricConversion == null)
                    _geographicToGeocentricConversion =
                        new CoordinateOperationMethod("EPSG::9602", "Geographic/geocentric conversion", true);

                return _geographicToGeocentricConversion;
            }
        }

        /// <summary>
        /// Geocentric/topocentric conversion.
        /// </summary>
        public static CoordinateOperationMethod GeocentricToTopocentricConversion
        {
            get
            {
                if (_geocentricToTopocentricConversion == null)
                    _geocentricToTopocentricConversion =
                        new CoordinateOperationMethod("EPSG::9836", "Geocentric/topocentric conversion", true,
                                                      CoordinateOperationParameters.GeocenticXOfTopocentricOrigin,
                                                      CoordinateOperationParameters.GeocenticYOfTopocentricOrigin,
                                                      CoordinateOperationParameters.GeocenticZOfTopocentricOrigin);

                return _geocentricToTopocentricConversion;
            }
        }

        /// <summary>
        /// Geographic/topocentric conversion.
        /// </summary>
        public static CoordinateOperationMethod GeographicToTopocentricConversion
        {
            get
            {
                if (_geographicToTopocentricConversion == null)
                    _geographicToTopocentricConversion =
                        new CoordinateOperationMethod("EPSG::9837", "Geographic/topocentric conversion", true, 
                                                      CoordinateOperationParameters.LatitudeOfTopocentricOrigin,
                                                      CoordinateOperationParameters.LongitudeOfTopocentricOrigin,
                                                      CoordinateOperationParameters.EllipsoidalHeightOfTopocentricOrigin);

                return _geographicToTopocentricConversion;
            }
        }

        /// <summary>
        /// Gnomonic Projection.
        /// </summary>
        public static CoordinateOperationMethod GnomonicProjection
        {
            get
            {
                if (_gnomonicProjection == null)
                    _gnomonicProjection =
                        new CoordinateOperationMethod("AEGIS::735137", "Gnomonic Projection", true,
                                                      CoordinateOperationParameters.LatitudeOfProjectionCentre,
                                                      CoordinateOperationParameters.LongitudeOfProjectionCentre,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);
                return _gnomonicProjection;
            }
        }

        /// <summary>
        /// Guam Projection.
        /// </summary>
        public static CoordinateOperationMethod GuamProjection
        {
            get
            {
                if (_guamProjection == null)
                    _guamProjection =
                        new CoordinateOperationMethod("AEGIS::9831", "Guam Projection", true,
                                                      CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _guamProjection;
            }
        }

        /// <summary>
        /// Hotine Oblique Mercator (variant A).
        /// </summary>
        public static CoordinateOperationMethod HotineObliqueMercatorAProjection
        {
            get
            {
                if (_hotineObliqueMercatorAProjection == null)
                    _hotineObliqueMercatorAProjection =
                        new CoordinateOperationMethod("EPSG::9812", "Hotine Oblique Mercator (variant A)", true,
                                                      CoordinateOperationParameters.LatitudeOfProjectionCentre,
                                                      CoordinateOperationParameters.LongitudeOfProjectionCentre,
                                                      CoordinateOperationParameters.AzimuthOfInitialLine,
                                                      CoordinateOperationParameters.AngleFromRectifiedToSkewGrid,
                                                      CoordinateOperationParameters.ScaleFactorOnInitialLine,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _hotineObliqueMercatorAProjection;
            }
        }

        /// <summary>
        /// Hotine Oblique Mercator (variant B).
        /// </summary>
        public static CoordinateOperationMethod HotineObliqueMercatorBProjection
        {
            get
            {
                if (_hotineObliqueMercatorBProjection == null)
                    _hotineObliqueMercatorBProjection =
                        new CoordinateOperationMethod("EPSG::9815", "Hotine Oblique Mercator (variant B)", true,
                                                      CoordinateOperationParameters.LatitudeOfProjectionCentre,
                                                      CoordinateOperationParameters.LongitudeOfProjectionCentre,
                                                      CoordinateOperationParameters.AzimuthOfInitialLine,
                                                      CoordinateOperationParameters.AngleFromRectifiedToSkewGrid,
                                                      CoordinateOperationParameters.ScaleFactorOnInitialLine,
                                                      CoordinateOperationParameters.EastingAtProjectionCentre,
                                                      CoordinateOperationParameters.NorthingAtProjectionCentre);

                return _hotineObliqueMercatorBProjection;
            }
        }

        /// <summary>
        /// Laborde Oblique Mercator.
        /// </summary>
        public static CoordinateOperationMethod LabordeObliqueMercatorProjection
        {
            get
            {
                if (_labordeObliqueMercatorProjection == null)
                    _labordeObliqueMercatorProjection =
                        new CoordinateOperationMethod("EPSG::9813", "Laborde Oblique Mercator", true,
                                                      CoordinateOperationParameters.LatitudeOfProjectionCentre,
                                                      CoordinateOperationParameters.LongitudeOfProjectionCentre,
                                                      CoordinateOperationParameters.AzimuthOfInitialLine,
                                                      CoordinateOperationParameters.ScaleFactorOnInitialLine,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _labordeObliqueMercatorProjection;
            }
        }

        /// <summary>
        /// Hyperbolic Cassini-Soldner.
        /// </summary>
        public static CoordinateOperationMethod HyperbolicCassiniSoldnerProjection
        {
            get
            {
                if (_hyperbolicCassiniSoldnerProjection == null)
                    _hyperbolicCassiniSoldnerProjection =
                        new CoordinateOperationMethod("EPSG::9833", "Hyperbolic Cassini-Soldner", true,
                                                      CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _hyperbolicCassiniSoldnerProjection;
            }
        }

        /// <summary>
        /// Krovak Projection.
        /// </summary>
        public static CoordinateOperationMethod KrovakProjection
        {
            get
            {
                if (_krovakProjection == null)
                    _krovakProjection =
                        new CoordinateOperationMethod("AEGIS::9819", "Krovak Projection", true,
                                                      CoordinateOperationParameters.LatitudeOfProjectionCentre,
                                                      CoordinateOperationParameters.LongitudeOfOrigin,
                                                      CoordinateOperationParameters.CoLatitudeOfConeAxis,
                                                      CoordinateOperationParameters.LatitudeOfPseudoStandardParallel,
                                                      CoordinateOperationParameters.ScaleFactorOnPseudoStandardParallel,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _krovakProjection;
            }
        }

        /// <summary>
        /// Krovak North Oriented Projection.
        /// </summary>
        public static CoordinateOperationMethod KrovakNorthOrientedProjection
        {
            get
            {
                if (_krovakNorthOrientedProjection == null)
                    _krovakNorthOrientedProjection =
                        new CoordinateOperationMethod("AEGIS::1041", "Krovak North Oriented Projection", true,
                                                      CoordinateOperationParameters.LatitudeOfProjectionCentre,
                                                      CoordinateOperationParameters.LongitudeOfOrigin,
                                                      CoordinateOperationParameters.CoLatitudeOfConeAxis,
                                                      CoordinateOperationParameters.LatitudeOfPseudoStandardParallel,
                                                      CoordinateOperationParameters.ScaleFactorOnPseudoStandardParallel,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _krovakNorthOrientedProjection;
            }
        }

        /// <summary>
        /// Krovak Modified Projection.
        /// </summary>
        public static CoordinateOperationMethod KrovakModifiedProjection
        {
            get
            {
                if (_krovakModifiedProjection == null)
                    _krovakModifiedProjection =
                        new CoordinateOperationMethod("AEGIS::1042", "Krovak Modified Projection", true,
                                                      CoordinateOperationParameters.LatitudeOfProjectionCentre,
                                                      CoordinateOperationParameters.LongitudeOfOrigin,
                                                      CoordinateOperationParameters.CoLatitudeOfConeAxis,
                                                      CoordinateOperationParameters.LatitudeOfPseudoStandardParallel,
                                                      CoordinateOperationParameters.ScaleFactorOnPseudoStandardParallel,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing,
                                                      CoordinateOperationParameters.Ordinate1OfEvaluationPoint,
                                                      CoordinateOperationParameters.Ordinate2OfEvaluationPoint,
                                                      CoordinateOperationParameters.C1,
                                                      CoordinateOperationParameters.C2,
                                                      CoordinateOperationParameters.C3,
                                                      CoordinateOperationParameters.C4,
                                                      CoordinateOperationParameters.C5,
                                                      CoordinateOperationParameters.C6,
                                                      CoordinateOperationParameters.C7,
                                                      CoordinateOperationParameters.C8,
                                                      CoordinateOperationParameters.C9,
                                                      CoordinateOperationParameters.C10);

                return _krovakModifiedProjection;
            }
        }

        /// <summary>
        /// Krovak Modified North Oriented Projection.
        /// </summary>
        public static CoordinateOperationMethod KrovakModifiedNorthOrientedProjection
        {
            get
            {
                if (_krovakModifiedNorthOrientedProjection == null)
                    _krovakModifiedNorthOrientedProjection =
                        new CoordinateOperationMethod("AEGIS::1043", "Krovak Modified North Oriented Projection", true,
                                                      CoordinateOperationParameters.LatitudeOfProjectionCentre,
                                                      CoordinateOperationParameters.LongitudeOfOrigin,
                                                      CoordinateOperationParameters.CoLatitudeOfConeAxis,
                                                      CoordinateOperationParameters.LatitudeOfPseudoStandardParallel,
                                                      CoordinateOperationParameters.ScaleFactorOnPseudoStandardParallel,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing,
                                                      CoordinateOperationParameters.Ordinate1OfEvaluationPoint,
                                                      CoordinateOperationParameters.Ordinate2OfEvaluationPoint,
                                                      CoordinateOperationParameters.C1,
                                                      CoordinateOperationParameters.C2,
                                                      CoordinateOperationParameters.C3,
                                                      CoordinateOperationParameters.C4,
                                                      CoordinateOperationParameters.C5,
                                                      CoordinateOperationParameters.C6,
                                                      CoordinateOperationParameters.C7,
                                                      CoordinateOperationParameters.C8,
                                                      CoordinateOperationParameters.C9,
                                                      CoordinateOperationParameters.C10);

                return _krovakModifiedNorthOrientedProjection;
            }
        }

        /// <summary>
        /// Lambert Azimuthal Equal Area Projection.
        /// </summary>
        public static CoordinateOperationMethod LambertAzimuthalEqualAreaProjection
        {
            get
            {
                return _lambertAzimuthalEqualAreaProjection ??
                      (_lambertAzimuthalEqualAreaProjection =
                           new CoordinateOperationMethod("AEGIS::9820", "Lambert Azimuthal Equal Area Projection", true,
                                                         CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                                         CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                         CoordinateOperationParameters.FalseEasting,
                                                         CoordinateOperationParameters.FalseNorthing));
            }
        }

        /// <summary>
        /// Lambert Azimuthal Equal Area (Spherical) Projection.
        /// </summary>
        public static CoordinateOperationMethod LambertAzimuthalEqualAreaSphericalProjection
        {
            get
            {
                return _lambertAzimuthalEqualAreaSphericalProjection ??
                      (_lambertAzimuthalEqualAreaSphericalProjection =
                           new CoordinateOperationMethod("AEGIS::1027", "Lambert Azimuthal Equal Area (Spherical) Projection", true,
                                                         CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                                         CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                         CoordinateOperationParameters.FalseEasting,
                                                         CoordinateOperationParameters.FalseNorthing));
            }
        }

        /// <summary>
        /// Lambert Conic Conformal (West Orientated).
        /// </summary>
        public static CoordinateOperationMethod LambertConicConformal1SPWestOrientatedProjection
        {
            get
            {
                if (_lambertConicConformal1SPWestOrientatedProjection == null)
                    _lambertConicConformal1SPWestOrientatedProjection =
                        new CoordinateOperationMethod("AEGIS::9826", "Lambert Conic Conformal (West Orientated)", true,
                                                      CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _lambertConicConformal1SPWestOrientatedProjection;
            }
        }

        /// <summary>
        /// Lambert Conic Conformal (1SP).
        /// </summary>
        public static CoordinateOperationMethod LambertConicConformal1SPProjection
        {
            get
            {
                if (_lambertConicConformal1SPProjection == null)
                    _lambertConicConformal1SPProjection =
                        new CoordinateOperationMethod("EPSG::9801", "Lambert Conic Conformal (1SP)", true,
                                                      CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _lambertConicConformal1SPProjection;
            }
        }

        /// <summary>
        /// Lambert Conic Conformal (2SP).
        /// </summary>
        public static CoordinateOperationMethod LambertConicConformal2SPProjection
        {
            get
            {
                if (_lambertConicConformal2SPProjection == null)
                    _lambertConicConformal2SPProjection =
                        new CoordinateOperationMethod("EPSG::9802", "Lambert Conic Conformal (2SP)", true,
                                                      CoordinateOperationParameters.LatitudeOfFalseOrigin,
                                                      CoordinateOperationParameters.LongitudeOfFalseOrigin,
                                                      CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                                      CoordinateOperationParameters.LatitudeOf2ndStandardParallel,
                                                      CoordinateOperationParameters.EastingAtFalseOrigin,
                                                      CoordinateOperationParameters.NorthingAtFalseOrigin);

                return _lambertConicConformal2SPProjection;
            }
        }

        /// <summary>
        /// Lambert Conic Conformal (2SP Belgium)
        /// </summary>
        public static CoordinateOperationMethod LambertConicConformal2SPBelgiumProjection
        {
            get
            {
                if (_lambertConicConformal2SPBelgiumProjection == null)
                    _lambertConicConformal2SPBelgiumProjection =
                        new CoordinateOperationMethod("AEGIS::9803", "Lambert Conic Conformal (2SP Belgium)", true,
                                                      CoordinateOperationParameters.LatitudeOfFalseOrigin,
                                                      CoordinateOperationParameters.LongitudeOfFalseOrigin,
                                                      CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                                      CoordinateOperationParameters.LatitudeOf2ndStandardParallel,
                                                      CoordinateOperationParameters.EastingAtFalseOrigin,
                                                      CoordinateOperationParameters.NorthingAtFalseOrigin);

                return _lambertConicConformal2SPBelgiumProjection;
            }
        }

        /// <summary>
        /// Lambert Conic Near-Conformal.
        /// </summary>
        public static CoordinateOperationMethod LambertConicNearConformalProjection
        {
            get
            {
                if (_lambertConicNearConformalProjection == null)
                    _lambertConicNearConformalProjection =
                        new CoordinateOperationMethod("EPSG::9817", "Lambert Conic Near-Conformal", true,
                                                      CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _lambertConicNearConformalProjection;
            }
        }

        /// <summary>
        /// Lambert Cylindrical Equal Area (ellipsoidal case).
        /// </summary>
        public static CoordinateOperationMethod LambertCylindricalEqualAreaEllipsoidalProjection
        {
            get
            {
                if (_lambertCylindricalEqualAreaEllipsoidalProjection == null)
                {
                    _lambertCylindricalEqualAreaEllipsoidalProjection =
                        new CoordinateOperationMethod("EPSG::9835", "Lambert Cylindrical Equal Area (ellipsoidal case)", true,
                        CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                        CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                        CoordinateOperationParameters.FalseEasting,
                        CoordinateOperationParameters.FalseNorthing);
                }
                return _lambertCylindricalEqualAreaEllipsoidalProjection;
            }
        }

        /// <summary>
        /// Lambert Cylindrical Equal Area (spherical case).
        /// </summary>
        public static CoordinateOperationMethod LambertCylindricalEqualAreaSphericalProjection
        {
            get
            {
                if (_lambertCylindricalEqualAreaSphericalProjection == null)
                {
                    _lambertCylindricalEqualAreaSphericalProjection = 
                        new CoordinateOperationMethod("EPSG::9834", "Lambert Cylindrical Equal Area (spherical case)", true,
                        CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                        CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                        CoordinateOperationParameters.FalseEasting,
                        CoordinateOperationParameters.FalseNorthing);
                }
                return _lambertCylindricalEqualAreaSphericalProjection;
            }
        }

        /// <summary>
        /// Mercator (variant A).
        /// </summary>
        public static CoordinateOperationMethod MercatorAProjection
        {
            get
            {
                if (_mercatorAProjection == null)
                    _mercatorAProjection =
                        new CoordinateOperationMethod("EPSG::9804", "Mercator (variant A)", true,
                                                      CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _mercatorAProjection;
            }
        }

        /// <summary>
        /// Mercator (variant B).
        /// </summary>
        public static CoordinateOperationMethod MercatorBProjection
        {
            get
            {
                if (_mercatorBProjection == null)
                    _mercatorBProjection =
                        new CoordinateOperationMethod("EPSG::9805", "Mercator (variant B)", true,
                                                      CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _mercatorBProjection;
            }
        }

        /// <summary>
        /// Mercator (variant C).
        /// </summary>
        public static CoordinateOperationMethod MercatorCProjection
        {
            get
            {
                if (_mercatorCProjection == null)
                    _mercatorCProjection =
                        new CoordinateOperationMethod("EPSG::1044", "Mercator (variant C)", true,
                                                      CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.LatitudeOfFalseOrigin,
                                                      CoordinateOperationParameters.EastingAtFalseOrigin,
                                                      CoordinateOperationParameters.NorthingAtFalseOrigin);

                return _mercatorCProjection;
            }
        }

        /// <summary>
        /// Mercator (Spherical).
        /// </summary>
        public static CoordinateOperationMethod MercatorSphericalProjection
        {
            get
            {
                if (_mercatorSphericalProjection == null)
                    _mercatorSphericalProjection =
                        new CoordinateOperationMethod("EPSG::1026", "Mercator (Spherical)", true,
                                                      CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _mercatorSphericalProjection;
            }
        }

        /// <summary>
        /// Military grid projection.
        /// </summary>
        public static CoordinateOperationMethod MilitaryGridProjection
        {
            get
            {
                if (_militaryGridProjection == null)
                    _militaryGridProjection =
                        new CoordinateOperationMethod("AEGIS::735202", "Military grid projection", true);

                return _militaryGridProjection;
            }
        }

        /// <summary>
        /// Modified Azimuthal Equidistant Projection.
        /// </summary>
        public static CoordinateOperationMethod ModifiedAzimuthalEquidistantProjection
        {
            get
            {
                if (_modifiedAzimuthalEquidistantProjection == null)
                    _modifiedAzimuthalEquidistantProjection =
                        new CoordinateOperationMethod("AEGIS::9832", "Modified Azimuthal Equidistant Projection", true,
                                                      CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _modifiedAzimuthalEquidistantProjection;
            }
        }

        /// <summary>
        /// Molodensky-Badekas (geocentric domain).
        /// </summary>
        public static CoordinateOperationMethod MolodenskyBadekasTransformation
        {
            get
            {
                if (_molodenskyBadekasTransformation == null)
                    _molodenskyBadekasTransformation =
                        new CoordinateOperationMethod("EPSG::1034", "Molodensky-Badekas (geocentric domain)", false,
                                                      CoordinateOperationParameters.XAxisTranslation,
                                                      CoordinateOperationParameters.YAxisTranslation,
                                                      CoordinateOperationParameters.ZAxisTranslation,
                                                      CoordinateOperationParameters.XAxisRotation,
                                                      CoordinateOperationParameters.YAxisRotation,
                                                      CoordinateOperationParameters.ZAxisRotation,
                                                      CoordinateOperationParameters.ScaleDifference,
                                                      CoordinateOperationParameters.Ordinate1OfEvaluationPoint,
                                                      CoordinateOperationParameters.Ordinate2OfEvaluationPoint,
                                                      CoordinateOperationParameters.Ordinate3OfEvaluationPoint);

                return _molodenskyBadekasTransformation;
            }
        }

        /// <summary>
        /// Molodensky.
        /// </summary>
        public static CoordinateOperationMethod MolodenskyTransformation
        {
            get
            {
                if (_molodenskyTransformation == null)
                    _molodenskyTransformation =
                        new CoordinateOperationMethod("EPSG::9604", "Molodensky", true,
                                                      CoordinateOperationParameters.XAxisTranslation,
                                                      CoordinateOperationParameters.YAxisTranslation,
                                                      CoordinateOperationParameters.ZAxisTranslation,
                                                      CoordinateOperationParameters.SemiMajorAxisLengthDifference,
                                                      CoordinateOperationParameters.FlatteningDifference);

                return _molodenskyTransformation;
            }
        }

        /// <summary>
        /// Oblique Stereographic.
        /// </summary>
        public static CoordinateOperationMethod ObliqueStereographicProjection
        {
            get
            {
                if (_obliqueStereographicProjection == null)
                    _obliqueStereographicProjection =
                        new CoordinateOperationMethod("EPSG::9809", "Oblique Stereographic", true,
                                                      CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _obliqueStereographicProjection;
            }
        }

        /// <summary>
        /// Polar Stereographic (variant A).
        /// </summary>
        public static CoordinateOperationMethod PolarStereographicAProjection
        {
            get
            {
                if (_polarStereographicAProjection == null)
                    _polarStereographicAProjection =
                        new CoordinateOperationMethod("EPSG::9810", "Polar Stereographic (variant A)", true,
                                                      CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _polarStereographicAProjection;
            }
        }

        /// <summary>
        /// Polar Stereographic (variant B).
        /// </summary>
        public static CoordinateOperationMethod PolarStereographicBProjection
        {
            get
            {
                if (_polarStereographicBProjection == null)
                    _polarStereographicBProjection =
                        new CoordinateOperationMethod("EPSG::9829", "Polar Stereographic (variant B)", true,
                                                      CoordinateOperationParameters.LatitudeOfStandardParallel,
                                                      CoordinateOperationParameters.LongitudeOfOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _polarStereographicBProjection;
            }
        }

        /// <summary>
        /// Polar Stereographic (variant C).
        /// </summary>
        public static CoordinateOperationMethod PolarStereographicCProjection
        {
            get
            {
                if (_polarStereographicCProjection == null)
                    _polarStereographicCProjection =
                        new CoordinateOperationMethod("EPSG::9830", "Polar Stereographic (variant C)", true,
                                                      CoordinateOperationParameters.LatitudeOfStandardParallel,
                                                      CoordinateOperationParameters.LongitudeOfOrigin,
                                                      CoordinateOperationParameters.EastingAtFalseOrigin,
                                                      CoordinateOperationParameters.NorthingAtFalseOrigin);

                return _polarStereographicCProjection;
            }
        }

        /// <summary>
        /// Popular Visualisation Pseudo Mercator. 
        /// </summary>
        public static CoordinateOperationMethod PopularVisualisationPseudoMercatorProjection
        {
            get
            {
                if (_popularVisualisationPseudoMercatorProjection == null)
                    _popularVisualisationPseudoMercatorProjection =
                        new CoordinateOperationMethod("EPSG::1024", "Popular Visualisation Pseudo Mercator", true,
                                                      CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _popularVisualisationPseudoMercatorProjection;
            }
        }

        /// <summary>
        /// Position Vector transformation (geocentric domain).
        /// </summary>
        public static CoordinateOperationMethod PositionVectorTransformation
        {
            get
            {
                if (_positionVectorTransformation == null)
                    _positionVectorTransformation =
                        new CoordinateOperationMethod("EPSG::1033", "Position Vector transformation (geocentric domain)", true,
                                                      CoordinateOperationParameters.XAxisTranslation,
                                                      CoordinateOperationParameters.YAxisTranslation,
                                                      CoordinateOperationParameters.ZAxisTranslation,
                                                      CoordinateOperationParameters.XAxisRotation,
                                                      CoordinateOperationParameters.YAxisRotation,
                                                      CoordinateOperationParameters.ZAxisRotation,
                                                      CoordinateOperationParameters.ScaleDifference);

                return _positionVectorTransformation;
            }
        }

        /// <summary>
        /// Pseudo Plate Carrée.
        /// </summary>
        public static CoordinateOperationMethod PseudoPlateCareeProjection
        {
            get
            {
                if (_pseudoPlateCareeProjection == null)
                    _pseudoPlateCareeProjection =
                        new CoordinateOperationMethod("EPSG::9825", "Pseudo Plate Carrée", true);

                return _pseudoPlateCareeProjection;
            }
        }

        /// <summary>
        /// Reversible polynomial of degree 2.
        /// </summary>
        public static CoordinateOperationMethod ReversiblePolynomial2
        {
            get
            {
                if (_reversiblePolynomial2Transformation == null)
                    _reversiblePolynomial2Transformation =
                        new CoordinateOperationMethod("EPSG::9649", "Reversible polynomial of degree 2", true,
                                                      CoordinateOperationParameters.Ordinate1OfEvaluationPoint,
                                                      CoordinateOperationParameters.Ordinate2OfEvaluationPoint,
                                                      CoordinateOperationParameters.ScalingFactorForCoordinateDifferences,
                                                      CoordinateOperationParameters.A0,
                                                      CoordinateOperationParameters.Au1v0,
                                                      CoordinateOperationParameters.Au0v1,
                                                      CoordinateOperationParameters.Au2v0,
                                                      CoordinateOperationParameters.Au1v1,
                                                      CoordinateOperationParameters.Au0v2,
                                                      CoordinateOperationParameters.B0,
                                                      CoordinateOperationParameters.Bu1v0,
                                                      CoordinateOperationParameters.Bu0v1,
                                                      CoordinateOperationParameters.Bu2v0,
                                                      CoordinateOperationParameters.Bu1v1,
                                                      CoordinateOperationParameters.Bu0v2);

                return _reversiblePolynomial2Transformation;
            }
        }

        /// <summary>
        /// Reversible polynomial of degree 3.
        /// </summary>
        public static CoordinateOperationMethod ReversiblePolynomial3
        {
            get
            {
                if (_reversiblePolynomial3Transformation == null)
                    _reversiblePolynomial3Transformation =
                        new CoordinateOperationMethod("EPSG::9650", "Reversible polynomial of degree 3", true,
                                                      CoordinateOperationParameters.Ordinate1OfEvaluationPoint,
                                                      CoordinateOperationParameters.Ordinate2OfEvaluationPoint,
                                                      CoordinateOperationParameters.ScalingFactorForCoordinateDifferences,
                                                      CoordinateOperationParameters.A0,
                                                      CoordinateOperationParameters.Au1v0,
                                                      CoordinateOperationParameters.Au0v1,
                                                      CoordinateOperationParameters.Au2v0,
                                                      CoordinateOperationParameters.Au1v1,
                                                      CoordinateOperationParameters.Au0v2,
                                                      CoordinateOperationParameters.Au3v0,
                                                      CoordinateOperationParameters.Au2v1,
                                                      CoordinateOperationParameters.Au1v2,
                                                      CoordinateOperationParameters.Au0v3,
                                                      CoordinateOperationParameters.B0,
                                                      CoordinateOperationParameters.Bu1v0,
                                                      CoordinateOperationParameters.Bu0v1,
                                                      CoordinateOperationParameters.Bu2v0,
                                                      CoordinateOperationParameters.Bu1v1,
                                                      CoordinateOperationParameters.Bu0v2,
                                                      CoordinateOperationParameters.Bu3v0,
                                                      CoordinateOperationParameters.Bu2v1,
                                                      CoordinateOperationParameters.Bu1v2,
                                                      CoordinateOperationParameters.Bu0v3);

                return _reversiblePolynomial3Transformation;
            }
        }

        /// <summary>
        /// Reversible polynomial of degree 4.
        /// </summary>
        public static CoordinateOperationMethod ReversiblePolynomial4
        {
            get
            {
                if (_reversiblePolynomial4Transformation == null)
                    _reversiblePolynomial4Transformation =
                        new CoordinateOperationMethod("EPSG::9651", "Reversible polynomial of degree 4", true,
                                                      CoordinateOperationParameters.Ordinate1OfEvaluationPoint,
                                                      CoordinateOperationParameters.Ordinate2OfEvaluationPoint,
                                                      CoordinateOperationParameters.ScalingFactorForCoordinateDifferences,
                                                      CoordinateOperationParameters.A0,
                                                      CoordinateOperationParameters.Au1v0,
                                                      CoordinateOperationParameters.Au0v1,
                                                      CoordinateOperationParameters.Au2v0,
                                                      CoordinateOperationParameters.Au1v1,
                                                      CoordinateOperationParameters.Au0v2,
                                                      CoordinateOperationParameters.Au3v0,
                                                      CoordinateOperationParameters.Au2v1,
                                                      CoordinateOperationParameters.Au1v2,
                                                      CoordinateOperationParameters.Au0v3,
                                                      CoordinateOperationParameters.Au4v0,
                                                      CoordinateOperationParameters.Au3v1,
                                                      CoordinateOperationParameters.Au2v2,
                                                      CoordinateOperationParameters.Au1v3,
                                                      CoordinateOperationParameters.Au0v4,
                                                      CoordinateOperationParameters.B0,
                                                      CoordinateOperationParameters.Bu1v0,
                                                      CoordinateOperationParameters.Bu0v1,
                                                      CoordinateOperationParameters.Bu2v0,
                                                      CoordinateOperationParameters.Bu1v1,
                                                      CoordinateOperationParameters.Bu0v2,
                                                      CoordinateOperationParameters.Bu3v0,
                                                      CoordinateOperationParameters.Bu2v1,
                                                      CoordinateOperationParameters.Bu1v2,
                                                      CoordinateOperationParameters.Bu0v3,
                                                      CoordinateOperationParameters.Bu4v0,
                                                      CoordinateOperationParameters.Bu3v1,
                                                      CoordinateOperationParameters.Bu2v2,
                                                      CoordinateOperationParameters.Bu1v3,
                                                      CoordinateOperationParameters.Bu0v4);

                return _reversiblePolynomial4Transformation;
            }
        }

        /// <summary>
        /// Reversible polynomial of degree 13.
        /// </summary>
        public static CoordinateOperationMethod ReversiblePolynomial13
        {
            get
            {
                if (_reversiblePolynomial13Transformation == null)
                    _reversiblePolynomial13Transformation =
                        new CoordinateOperationMethod("EPSG::9654", "Reversible polynomial of degree 13", true,
                                                      CoordinateOperationParameters.Ordinate1OfEvaluationPoint,
                                                      CoordinateOperationParameters.Ordinate2OfEvaluationPoint,
                                                      CoordinateOperationParameters.ScalingFactorForCoordinateDifferences,
                                                      CoordinateOperationParameters.A0,
                                                      CoordinateOperationParameters.Au1v0,
                                                      CoordinateOperationParameters.Au0v1,
                                                      CoordinateOperationParameters.Au2v0,
                                                      CoordinateOperationParameters.Au1v1,
                                                      CoordinateOperationParameters.Au1v9,
                                                      CoordinateOperationParameters.Au3v0,
                                                      CoordinateOperationParameters.Au3v9,
                                                      CoordinateOperationParameters.Au2v1,
                                                      CoordinateOperationParameters.Au2v7,
                                                      CoordinateOperationParameters.Au4v0,
                                                      CoordinateOperationParameters.Au4v1,
                                                      CoordinateOperationParameters.Au5v2,
                                                      CoordinateOperationParameters.Au0v8,
                                                      CoordinateOperationParameters.Au9v0,
                                                      CoordinateOperationParameters.B0,
                                                      CoordinateOperationParameters.Bu1v0,
                                                      CoordinateOperationParameters.Bu0v1,
                                                      CoordinateOperationParameters.Bu2v0,
                                                      CoordinateOperationParameters.Bu1v1,
                                                      CoordinateOperationParameters.Bu0v2,
                                                      CoordinateOperationParameters.Bu3v0,
                                                      CoordinateOperationParameters.Bu4v0,
                                                      CoordinateOperationParameters.Bu1v3,
                                                      CoordinateOperationParameters.Bu5v0,
                                                      CoordinateOperationParameters.Bu2v3,
                                                      CoordinateOperationParameters.Bu1v4,
                                                      CoordinateOperationParameters.Bu0v5,
                                                      CoordinateOperationParameters.Bu6v0,
                                                      CoordinateOperationParameters.Bu3v3,
                                                      CoordinateOperationParameters.Bu2v4,
                                                      CoordinateOperationParameters.Bu1v5,
                                                      CoordinateOperationParameters.Bu7v0,
                                                      CoordinateOperationParameters.Bu6v1,
                                                      CoordinateOperationParameters.Bu4v4,
                                                      CoordinateOperationParameters.Bu8v1,
                                                      CoordinateOperationParameters.Bu7v2,
                                                      CoordinateOperationParameters.Bu2v7,
                                                      CoordinateOperationParameters.Bu0v9,
                                                      CoordinateOperationParameters.Bu4v6,
                                                      CoordinateOperationParameters.Bu9v2,
                                                      CoordinateOperationParameters.Bu8v3,
                                                      CoordinateOperationParameters.Bu5v7,
                                                      CoordinateOperationParameters.Bu9v4,
                                                      CoordinateOperationParameters.Bu4v9);

                return _reversiblePolynomial13Transformation;
            }
        }

        /// <summary>
        /// P6 (I = J-90°) seismic bin grid transformation. 
        /// </summary>
        public static CoordinateOperationMethod P6LeftHandedSeismicBinGridTransformation
        {
            get
            {
                return _p6LeftHandedSeismicBinGridTransformation ??
                      (_p6LeftHandedSeismicBinGridTransformation =
                           new CoordinateOperationMethod("AEGIS::1049", "P6 (I = J-90°) seismic bin grid transformation", true,
                                                         CoordinateOperationParameters.BinGridOriginI,
                                                         CoordinateOperationParameters.BinGridOriginJ,
                                                         CoordinateOperationParameters.BinGridOriginEasting,
                                                         CoordinateOperationParameters.BinGridOriginNorthing,
                                                         CoordinateOperationParameters.ScaleFactorOfBinGrid,
                                                         CoordinateOperationParameters.BinWidthOnIAxis,
                                                         CoordinateOperationParameters.BinWidthOnJAxis,
                                                         CoordinateOperationParameters.MapGridBearingOfBinGridJAxis,
                                                         CoordinateOperationParameters.BinNodeIncrementOnIAxis,
                                                         CoordinateOperationParameters.BinNodeIncrementOnJAxis));
            }
        }

        /// <summary>
        /// P6 (I = J+90°) seismic bin grid transformation.
        /// </summary>
        public static CoordinateOperationMethod P6RightHandedSeismicBinGridTransformation
        {
            get
            {
                return _p6RightHandedSeismicBinGridTransformation ??
                      (_p6RightHandedSeismicBinGridTransformation =
                           new CoordinateOperationMethod("AEGIS::9666", "P6 (I = J+90°) seismic bin grid transformation", true,
                                                         CoordinateOperationParameters.BinGridOriginI,
                                                         CoordinateOperationParameters.BinGridOriginJ,
                                                         CoordinateOperationParameters.BinGridOriginEasting,
                                                         CoordinateOperationParameters.BinGridOriginNorthing,
                                                         CoordinateOperationParameters.ScaleFactorOfBinGrid,
                                                         CoordinateOperationParameters.BinWidthOnIAxis,
                                                         CoordinateOperationParameters.BinWidthOnJAxis,
                                                         CoordinateOperationParameters.MapGridBearingOfBinGridJAxis,
                                                         CoordinateOperationParameters.BinNodeIncrementOnIAxis,
                                                         CoordinateOperationParameters.BinNodeIncrementOnJAxis));
            }
        }

        /// <summary>
        /// Similarity Transformation.
        /// </summary>
        public static CoordinateOperationMethod SimilarityTransformation
        {
            get
            {
                if (_similarityTransformation == null)
                    _similarityTransformation =
                        new CoordinateOperationMethod("AEGIS::9621", "Similarity Transformation", true,
                                                      CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget,
                                                      CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget,
                                                      CoordinateOperationParameters.ScaleDifference,
                                                      CoordinateOperationParameters.XAxisRotation);


                return _similarityTransformation;
            }
        }

        /// <summary>
        /// Sinusoidal Projection.
        /// </summary>
        public static CoordinateOperationMethod SinusoidalProjection
        {
            get
            {
                if (_sinusoidalProjection == null)
                    _sinusoidalProjection =
                        new CoordinateOperationMethod("ESRI::53008", "Sinusoidal Projection", true,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);
                return _sinusoidalProjection;
            }
        }

        /// <summary>
        /// Transverse Mercator.
        /// </summary>
        public static CoordinateOperationMethod TransverseMercatorProjection
        {
            get
            {
                if (_transverseMercatorProjection == null)
                    _transverseMercatorProjection =
                        new CoordinateOperationMethod("EPSG::9807", "Transverse Mercator", true,
                                                      CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _transverseMercatorProjection;
            }
        }

        /// <summary>
        /// Transverse Mercator (South Orientated).
        /// </summary>
        public static CoordinateOperationMethod TransverseMercatorSouthProjection
        {
            get
            {
                if (_transverseMercatorSouthProjection == null)
                    _transverseMercatorSouthProjection =
                        new CoordinateOperationMethod("EPSG::9808", "Transverse Mercator (South Orientated)", true,
                                                      CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _transverseMercatorSouthProjection;
            }
        }

        /// <summary>
        /// Transverse Mercator Zoned Grid System.
        /// </summary>
        public static CoordinateOperationMethod TransverseMercatorZonedProjection
        {
            get
            {
                if (_transverseMercatorZonedProjection == null)
                    _transverseMercatorZonedProjection =
                        new CoordinateOperationMethod("EPSG::9824", "Transverse Mercator Zoned Grid System", true,
                                                      CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.InitialLongitude,
                                                      CoordinateOperationParameters.ZoneWidth,
                                                      CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);

                return _transverseMercatorZonedProjection;
            }
        }

        /// <summary>
        /// Vertical Perspective (Orthographic case).
        /// </summary>
        public static CoordinateOperationMethod VerticalPerspectiveOrthographicProjection
        {
            get
            {
                if (_verticalPerspectiveOrthographicProjection == null)
                    _verticalPerspectiveOrthographicProjection =
                        new CoordinateOperationMethod("EPSG::9839", "Vertical Perspective (Orthographic case)", false,
                                                      CoordinateOperationParameters.LatitudeOfTopocentricOrigin,
                                                      CoordinateOperationParameters.LongitudeOfTopocentricOrigin);

                return _verticalPerspectiveOrthographicProjection;
            }
        }

        /// <summary>
        /// Vertical Perspective.
        /// </summary>
        public static CoordinateOperationMethod VerticalPerspectiveProjection
        {
            get
            {
                if (_verticalPerspectiveProjection == null)
                    _verticalPerspectiveProjection =
                        new CoordinateOperationMethod("EPSG::9838", "Vertical Perspective", false,
                                                      CoordinateOperationParameters.LatitudeOfTopocentricOrigin,
                                                      CoordinateOperationParameters.LongitudeOfTopocentricOrigin,
                                                      CoordinateOperationParameters.EllipsoidalHeightOfTopocentricOrigin,
                                                      CoordinateOperationParameters.ViewPointHeight);

                return _verticalPerspectiveProjection;
            }
        }

        /// <summary>
        /// Miller Cylindrical Projection.
        /// </summary>
        public static CoordinateOperationMethod WorldMillerCylindricalProjection
        {
            get
            {
                if (_worldMillerCylindricalProjection == null)
                    _worldMillerCylindricalProjection =
                        new CoordinateOperationMethod("ESRI::54002", "Miller Cylindrical Projection", true,
                                                      CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                                      CoordinateOperationParameters.FalseEasting,
                                                      CoordinateOperationParameters.FalseNorthing);
                return _worldMillerCylindricalProjection;
            }
        }

        #endregion
    }
}
