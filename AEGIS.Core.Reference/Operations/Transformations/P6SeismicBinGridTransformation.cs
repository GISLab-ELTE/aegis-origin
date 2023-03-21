// <copyright file="P6SeismicBinGridTransformation.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a P6 seismic bin grid transformation.
    /// </summary>
    public abstract class P6SeismicBinGridTransformation : CoordinateTransformation<Coordinate>
    {
        #region Protected types

        /// <summary>
        /// Defines the possible orientations of the grid.
        /// </summary>
        protected enum Orientation { LeftHanded, RightHanded }

        #endregion

        #region Protected fields

        protected Double _binGridOriginI;
        protected Double _binGridOriginJ;
        protected Double _binGridOriginEasting;
        protected Double _binGridOriginNorthing;
        protected Double _scaleFactorOfBinGrid;
        protected Double _binWidthOnIAxis;
        protected Double _binWidthOnJAxis;
        protected Double _mapGridBearingOfBinGridJAxis;
        protected Double _binNodeIncOnIAxis;
        protected Double _binNodeIncOnJAxis;
        protected Orientation _orientation;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SeismicBinGridTransformation" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The method is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The parameter is not an angular value as required by the method.
        /// or
        /// The parameter is not a length value as required by the method.
        /// or
        /// The parameter is not a double precision floating-point number as required by the method.
        /// </exception>
        protected P6SeismicBinGridTransformation(String identifier, String name, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters)
            : base(identifier, name, method, parameters)
        {
            // EPSG Guidance Note number 7, part 2, page 112

            _binGridOriginI = Convert.ToDouble(_parameters[CoordinateOperationParameters.BinGridOriginI]);
            _binGridOriginJ = Convert.ToDouble(_parameters[CoordinateOperationParameters.BinGridOriginJ]);
            _binGridOriginEasting = ((Length)_parameters[CoordinateOperationParameters.BinGridOriginEasting]).BaseValue;
            _binGridOriginNorthing = ((Length)_parameters[CoordinateOperationParameters.BinGridOriginNorthing]).BaseValue;
            _scaleFactorOfBinGrid = Convert.ToDouble(_parameters[CoordinateOperationParameters.ScaleFactorOfBinGrid]);
            _binWidthOnIAxis = ((Length)_parameters[CoordinateOperationParameters.BinWidthOnIAxis]).BaseValue;
            _binWidthOnJAxis = ((Length)_parameters[CoordinateOperationParameters.BinWidthOnJAxis]).BaseValue;
            _mapGridBearingOfBinGridJAxis = ((Angle)_parameters[CoordinateOperationParameters.MapGridBearingOfBinGridJAxis]).BaseValue;
            _binNodeIncOnIAxis = Convert.ToDouble(_parameters[CoordinateOperationParameters.BinNodeIncrementOnIAxis]);
            _binNodeIncOnJAxis = Convert.ToDouble(_parameters[CoordinateOperationParameters.BinNodeIncrementOnJAxis]);
        }

        #endregion

        #region Protected operation methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(Coordinate coordinate)
        {
            Double x = 0, y = 0;

            switch (_orientation)
            {
                case Orientation.LeftHanded:
                    x = _binGridOriginEasting - ((coordinate.X - _binGridOriginI) * Math.Cos(_mapGridBearingOfBinGridJAxis) * _scaleFactorOfBinGrid * _binWidthOnIAxis / _binNodeIncOnIAxis) + ((coordinate.Y - _binGridOriginJ) * Math.Sin(_mapGridBearingOfBinGridJAxis) * _scaleFactorOfBinGrid * _binWidthOnJAxis / _binNodeIncOnJAxis);
                    y = _binGridOriginNorthing + ((coordinate.X - _binGridOriginI) * Math.Sin(_mapGridBearingOfBinGridJAxis) * _scaleFactorOfBinGrid * _binWidthOnIAxis / _binNodeIncOnIAxis) + ((coordinate.Y - _binGridOriginJ) * Math.Cos(_mapGridBearingOfBinGridJAxis) * _scaleFactorOfBinGrid * _binWidthOnJAxis / _binNodeIncOnJAxis);
                    break;
                case Orientation.RightHanded:
                    x = _binGridOriginEasting + ((coordinate.X - _binGridOriginI) * Math.Cos(_mapGridBearingOfBinGridJAxis) * _scaleFactorOfBinGrid * _binWidthOnIAxis / _binNodeIncOnIAxis) + ((coordinate.Y - _binGridOriginJ) * Math.Sin(_mapGridBearingOfBinGridJAxis) * _scaleFactorOfBinGrid * _binWidthOnJAxis / _binNodeIncOnJAxis);
                    y = _binGridOriginNorthing - ((coordinate.X - _binGridOriginI) * Math.Sin(_mapGridBearingOfBinGridJAxis) * _scaleFactorOfBinGrid * _binWidthOnIAxis / _binNodeIncOnIAxis) + ((coordinate.Y - _binGridOriginJ) * Math.Cos(_mapGridBearingOfBinGridJAxis) * _scaleFactorOfBinGrid * _binWidthOnJAxis / _binNodeIncOnJAxis);
                    break;

            }

            return new Coordinate(x, y);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeReverse(Coordinate coordinate)
        {
            Double x = 0, y = 0;

            switch (_orientation)
            {
                case Orientation.LeftHanded:
                    x = _binGridOriginI - (((coordinate.X - _binGridOriginEasting) * Math.Cos(_mapGridBearingOfBinGridJAxis) - (coordinate.Y - _binGridOriginNorthing) * Math.Sin(_mapGridBearingOfBinGridJAxis)) * (_binNodeIncOnIAxis / (_scaleFactorOfBinGrid * _binWidthOnIAxis)));
                    y = _binGridOriginJ + (((coordinate.X - _binGridOriginEasting) * Math.Sin(_mapGridBearingOfBinGridJAxis) + (coordinate.Y - _binGridOriginNorthing) * Math.Cos(_mapGridBearingOfBinGridJAxis)) * (_binNodeIncOnJAxis / (_scaleFactorOfBinGrid * _binWidthOnJAxis)));
                    break;
                case Orientation.RightHanded:
                    x = _binGridOriginI + (((coordinate.X - _binGridOriginEasting) * Math.Cos(_mapGridBearingOfBinGridJAxis) - (coordinate.Y - _binGridOriginNorthing) * Math.Sin(_mapGridBearingOfBinGridJAxis)) * (_binNodeIncOnIAxis / (_scaleFactorOfBinGrid * _binWidthOnIAxis)));
                    y = _binGridOriginJ + (((coordinate.X - _binGridOriginEasting) * Math.Sin(_mapGridBearingOfBinGridJAxis) + (coordinate.Y - _binGridOriginNorthing) * Math.Cos(_mapGridBearingOfBinGridJAxis)) * (_binNodeIncOnJAxis / (_scaleFactorOfBinGrid * _binWidthOnJAxis)));
                    break;

            }

            return new Coordinate(x, y);
        }

        #endregion
    }
}
