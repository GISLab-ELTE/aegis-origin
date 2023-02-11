// <copyright file="TransformationStrategyFactory.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections.Generic;
using ELTE.AEGIS.Reference;
using ELTE.AEGIS.Reference.Operations;

namespace ELTE.AEGIS.Operations.Spatial.Strategy
{
    /// <summary>
    /// Represents a factory class for producing <see cref="ITransformationStrategy" /> instances.
    /// </summary>
    public static class TransformationStrategyFactory
    {
        #region Public static factory methods

        /// <summary>
        /// Creates a transformation strategy between reference systems.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns>The produced transformation strategy.</returns>
        /// <exception cref="System.NotSupportedException">Conversion is not supported between the specified coordinate reference systems.</exception>
        public static ITransformationStrategy CreateStrategy(ReferenceSystem source, ReferenceSystem target)
        {
            ITransformationStrategy<Coordinate, GeoCoordinate> conversionToGeographic = null;
            ITransformationStrategy<GeoCoordinate, GeoCoordinate> transformation = null;
            ITransformationStrategy<GeoCoordinate, Coordinate> conversionFromGeographic = null;

            switch (source.Type)
            { 
                case ReferenceSystemType.Projected:
                    conversionToGeographic = new ReverseProjectionStrategy(source as ProjectedCoordinateReferenceSystem);
                    break;
                case ReferenceSystemType.Geographic2D:
                case ReferenceSystemType.Geographic3D:
                    conversionToGeographic = CreateReverseConversion(source as CoordinateReferenceSystem);
                    break;
            }

            switch (target.Type)
            {
                case ReferenceSystemType.Projected:
                    conversionFromGeographic = new ForwardProjectionStrategy(target as ProjectedCoordinateReferenceSystem);
                    break;
                case ReferenceSystemType.Geographic2D:
                case ReferenceSystemType.Geographic3D:
                    conversionFromGeographic = CreateForwardConversion(target as CoordinateReferenceSystem);
                    break;
            }

            // if no transformation is needed
            if (conversionFromGeographic.TargetReferenceSystem.Equals(conversionToGeographic.SourceReferenceSystem))
                return new ConversionStrategy(conversionToGeographic, conversionFromGeographic);

            // load matching forward transformation
            IList<GeographicTransformation> transformations = GeographicTransformations.FromReferenceSystems(conversionToGeographic.TargetReferenceSystem as GeographicCoordinateReferenceSystem, 
                                                                                                             conversionFromGeographic.SourceReferenceSystem as GeographicCoordinateReferenceSystem);
            if (transformations.Count > 0)
            {
                transformation = new ForwardGeographicStrategy(conversionToGeographic.TargetReferenceSystem as GeographicCoordinateReferenceSystem,
                                                               conversionFromGeographic.SourceReferenceSystem as GeographicCoordinateReferenceSystem,
                                                               transformations[0]);
                return new CompoundTransformationStrategy(conversionToGeographic, transformation, conversionFromGeographic);
            }

            // if none found, load matching reverse transformation
            transformations = GeographicTransformations.FromReferenceSystems(conversionFromGeographic.SourceReferenceSystem as GeographicCoordinateReferenceSystem,
                                                                             conversionToGeographic.TargetReferenceSystem as GeographicCoordinateReferenceSystem);
            if (transformations.Count > 0)
            {
                transformation = new ReverseGeographicStrategy(conversionToGeographic.TargetReferenceSystem as GeographicCoordinateReferenceSystem,
                                                               conversionFromGeographic.SourceReferenceSystem as GeographicCoordinateReferenceSystem,
                                                               transformations[0]);
                return new CompoundTransformationStrategy(conversionToGeographic, transformation, conversionFromGeographic);
            }

            throw new NotSupportedException("Conversion is not supported between the specified coordinate reference systems.");
        }

        #endregion

        #region Private static factory methods

        /// <summary>
        /// Creates a conversion strategy from cartesian to geographic coordinate.
        /// </summary>
        /// <param name="referenceSystem">The coordinate reference system.</param>
        /// <returns>The produced conversion strategy.</returns>
        private static ITransformationStrategy<Coordinate, GeoCoordinate> CreateReverseConversion(CoordinateReferenceSystem referenceSystem)
        {
            if (referenceSystem.CoordinateSystem.GetAxis(0).Name.Equals("Geodetic latitude") &&
                referenceSystem.CoordinateSystem.GetAxis(1).Name.Equals("Geodetic longitude"))
            {
                if (referenceSystem.Dimension == 3)
                {
                    return new ReverseLatLonHiCoordinateInterpretationStrategy(referenceSystem);
                }
                else
                {
                    return new ReverseLatLonCoordinateInterpretationStrategy(referenceSystem);
                }
            }
            else
            {
                if (referenceSystem.Dimension == 3)
                {
                    return new ReverseLonLatHiCoordinateInterpretationStrategy(referenceSystem);
                }
                else
                {
                    return new ReverseLonLatCoordinateIntepretationStrategy(referenceSystem);
                }
            }
        }

        /// <summary>
        /// Creates a conversion strategy from geographic to cartesian coordinate.
        /// </summary>
        /// <param name="referenceSystem">The coordinate reference system.</param>
        /// <returns>The produced conversion strategy.</returns>
        private static ITransformationStrategy<GeoCoordinate, Coordinate> CreateForwardConversion(CoordinateReferenceSystem referenceSystem)
        {
            if (referenceSystem.CoordinateSystem.GetAxis(0).Name.Equals("Geodetic latitude") &&
                referenceSystem.CoordinateSystem.GetAxis(1).Name.Equals("Geodetic longitude"))
            {
                if (referenceSystem.Dimension == 3)
                {
                    return new ForwardLatLonHiCoordinateIntepretationStrategy(referenceSystem);
                }
                else
                {
                    return new ForwardLatLonCoordinateInterpretationStrategy(referenceSystem);
                }
            }
            else
            {
                if (referenceSystem.Dimension == 3)
                {
                    return new ForwardLonLatHiCoordinateInterpretationStrategy(referenceSystem);
                }
                else
                {
                    return new ForwardLonLatCoordinateIntepretationStrategy(referenceSystem);
                }
            }
        }

        #endregion
    }
}
