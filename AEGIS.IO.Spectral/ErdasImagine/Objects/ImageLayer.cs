/// <copyright file="ErdasImgLayer.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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
/// <author>Tamas Nagy</author>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ELTE.AEGIS.IO.ErdasImagine.Types;
using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Reference;
using ELTE.AEGIS.Reference.Operations;

namespace ELTE.AEGIS.IO.ErdasImagine.Objects
{
    /// <summary>
    /// Represents an image layer.
    /// </summary>
    public class ImageLayer : EhfaStructure
    {
        #region Private types

        /// <summary>
        /// Represents information about a raster block in a layer.
        /// </summary>
        private class BlockInformation
        {
            #region Constructor

            /// <summary>
            /// Initializes a new instance of the <see cref="BlockInformation"/> class.
            /// </summary>
            /// <param name="offset">The offset.</param>
            /// <param name="size">The size.</param>
            /// <param name="compressionType">Type of the compression.</param>
            public BlockInformation(Int32 offset, UInt32 size, String compressionType)
            {
                Offset = offset;
                Size = size;
                CompressionType = compressionType;
            }

            #endregion

            #region Public properties

            /// <summary>
            /// Gets the offset.
            /// </summary>
            /// <value>
            /// The offset.
            /// </value>
            public Int32 Offset { get; }

            /// <summary>
            /// Gets the size.
            /// </summary>
            /// <value>
            /// The size.
            /// </value>
            public UInt32 Size { get; }

            /// <summary>
            /// Gets the type of the compression.
            /// </summary>
            /// <value>
            /// The type of the compression.
            /// </value>
            public String CompressionType { get; }

            #endregion
        }

        #endregion

        #region Private fields

        /// <summary>
        /// The block informations.
        /// </summary>
        /// <value>The block informations.</value>
        private IReadOnlyList<BlockInformation> _blockInfo;

        /// <summary>
        /// The width of the blocks.
        /// </summary>
        private Int32? _blockWidth;

        /// <summary>
        /// The height of the blocks.
        /// </summary>
        private Int32? _blockHeight;

        /// <summary>
        /// The pixel type.
        /// </summary>
        private PixelType? _pixelType;

        /// <summary>
        /// The layer width.
        /// </summary>
        private UInt32? _width;

        /// <summary>
        /// The layer height.
        /// </summary>
        private UInt32? _height;

        /// <summary>
        /// The type of the layer.
        /// </summary>
        private String _layerType;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageLayer" /> class.
        /// </summary>
        /// <param name="type">The structure type.</param>
        /// <param name="layerEntry">The layer entry.</param>
        /// <exception cref="ArgumentNullException">The type is null.</exception>
        public ImageLayer(EhfaStructureType type, EhfaEntry layerEntry) : base(type)
        {
            LayerEntry = layerEntry;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the width of the blocks.
        /// </summary>
        /// <value>The width of the blocks.</value>
        public Int32 BlockWidth { get { return (_blockWidth ?? (_blockWidth = (Int32)GetValue<UInt32>("blockWidth"))).Value; } }

        /// <summary>
        /// Gets the height of the blocks.
        /// </summary>
        /// <value>The height of the blocks.</value>
        public Int32 BlockHeight { get { return (_blockHeight ?? (_blockHeight = (Int32)GetValue<UInt32>("blockHeight"))).Value; } }

        /// <summary>
        /// Gets the pixel type.
        /// </summary>
        /// <value>The pixel type.</value>
        public PixelType PixelType { get { return (_pixelType ?? (_pixelType = PixelTypeInfo.Parse(GetValue<String>("pixelType")))).Value; } }

        /// <summary>
        /// Gets the layer width.
        /// </summary>
        /// <value>The layer width.</value>
        public UInt32 Width { get { return (_width ?? (_width = GetValue<UInt32>("width"))).Value; } }

        /// <summary>
        /// Gets the layer height.
        /// </summary>
        /// <value>The layer height.</value>
        public UInt32 Height { get { return (_height ?? (_height = GetValue<UInt32>("height"))).Value; } }

        /// <summary>
        /// Gets the type of the layer.
        /// </summary>
        /// <value>The type of the layer.</value>
        public String LayerType { get { return _layerType ?? (_layerType = GetValue<String>("layerType")); } }

        /// <summary>
        /// Gets the number of blocks across the image. 
        /// </summary>
        /// <value>The number of blocks across the image.</value>
        public Int32 BlockCountAcross { get { return (Int32)Math.Ceiling((Double)Width / BlockWidth); } }

        /// <summary>
        /// Gets the number of blocks down in the image.
        /// </summary>
        /// <value>The number of blocks down in the image.</value>
        public Int32 BlockCountDown { get { return (Int32)Math.Ceiling((Double)Height / BlockHeight); } }

        /// <summary>
        /// Gets the upper left center coordinate of the layer.
        /// </summary>
        /// <value>The upper left center coordinate of the layer.</value>
        public Coordinate UpperLeftCenter { get; private set; }

        /// <summary>
        /// Gets the lower right center coordinate of the layer.
        /// </summary>
        /// <value>The lower right center coordinate of the layer.</value>
        public Coordinate LowerRightCenter { get; private set; }

        /// <summary>
        /// Gets the width of the pixel.
        /// </summary>
        /// <value>The width of the pixel.</value>
        public Double PixelWidth { get; private set; }

        /// <summary>
        /// Gets the height of the pixel.
        /// </summary>
        /// <value>The height of the pixel.</value>
        public Double PixelHeight { get; private set; }

        /// <summary>
        /// Gets the reference system of the layer.
        /// </summary>
        /// <value>The reference system of the layer.</value>
        public IReferenceSystem ReferenceSystem { get; private set; }

        /// <summary>
        /// Gets the raster mapper of the layer.
        /// </summary>
        /// <value>The raster mapper of the layer.</value>
        public RasterMapper RasterMapper { get; private set; }

        /// <summary>
        /// Gets the layer entry.
        /// </summary>
        /// <value>The layer entry.</value>
        public EhfaEntry LayerEntry { get; private set; }

        /// <summary>
        /// Gets the radiometric resolution of the layer.
        /// </summary>
        /// <value>The radiometric resolution of the layer.</value>
        public Int32 RadiometricResolution
        {
            get
            {
                return PixelTypeInfo.GetSize(PixelType);
            }
        }

        #endregion

        #region IEhfaObject methods

        /// <summary>
        /// Reads the contents of the <see cref="ImageLayer" /> from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        /// <exception cref="System.ArgumentException">The stream is invalid.</exception>
        public override void Read(Stream stream)
        {
            base.Read(stream);

            try
            {
                ReadLayerInformation(stream);
                ReadProjectionInformation(stream);
                ReadBlockInformation(stream);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("The stream is invalid.", nameof(stream), ex);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Reads a raster band containing floating-point values from the layer.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="rasterBand">The raster band.</param>
        /// <exception cref="ArgumentNullException">
        /// The stream is null.
        /// or
        /// The raster band is null.
        /// </exception>
        public void ReadRasterBand(Stream stream, IRasterBand rasterBand)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream), "The stream is null.");
            if (rasterBand == null)
                throw new ArgumentNullException(nameof(rasterBand), "The raster band is null.");

            for (Int32 blockIndexDown = 0; blockIndexDown < BlockCountDown; blockIndexDown++)
                for (Int32 blockIndexAcross = 0; blockIndexAcross < BlockCountAcross; blockIndexAcross++)
                {
                    BlockInformation blockInfo = _blockInfo[blockIndexDown * BlockCountAcross + blockIndexAcross];
                    Int32 withinBlockIndex = 0;
                    Int32 blockRowIndex = blockIndexDown * BlockWidth;
                    Int32 blockColumnIndex = blockIndexAcross * BlockHeight;

                    switch (PixelType)
                    {
                        case PixelType.UInt1:
                        case PixelType.UInt2:
                        case PixelType.UInt4:
                        case PixelType.UInt8:
                        case PixelType.UInt16:
                        case PixelType.UInt32:
                            IEnumerator<UInt32> uIntEnum = ReadUnsignedIntegerRasterBlock(stream, blockInfo).GetEnumerator();
                            while (uIntEnum.MoveNext())
                            {
                                Int32 rowIndex = blockRowIndex + withinBlockIndex / BlockWidth;
                                Int32 columIndex = blockColumnIndex + withinBlockIndex % BlockWidth;

                                if (rowIndex >= rasterBand.NumberOfRows || columIndex >= rasterBand.NumberOfColumns)
                                    continue;

                                rasterBand.SetValue(rowIndex, columIndex, uIntEnum.Current);

                                withinBlockIndex++;
                            }
                            break;
                        case PixelType.Int8:
                        case PixelType.Int16:
                        case PixelType.Int32:
                            IEnumerator<Int32> intEnum = ReadSignedIntegerRasterBlock(stream, blockInfo).GetEnumerator();
                            while (intEnum.MoveNext())
                            {
                                Int32 rowIndex = blockRowIndex + withinBlockIndex / BlockWidth;
                                Int32 columIndex = blockColumnIndex + withinBlockIndex % BlockWidth;

                                if (rowIndex >= rasterBand.NumberOfRows || columIndex >= rasterBand.NumberOfColumns)
                                    continue;

                                rasterBand.SetFloatValue(rowIndex, columIndex, intEnum.Current);

                                withinBlockIndex++;
                            }
                            break;
                        case PixelType.Float32:
                        case PixelType.Float64:
                            IEnumerator<Double> floatEnum = ReadFloatingRasterBlock(stream, blockInfo).GetEnumerator();
                            while (floatEnum.MoveNext())
                            {
                                Int32 rowIndex = blockRowIndex + withinBlockIndex / BlockWidth;
                                Int32 columIndex = blockColumnIndex + withinBlockIndex % BlockWidth;

                                if (rowIndex >= rasterBand.NumberOfRows || columIndex >= rasterBand.NumberOfColumns)
                                    continue;

                                rasterBand.SetFloatValue(rowIndex, columIndex, floatEnum.Current);

                                withinBlockIndex++;
                            }
                            break;
                    }


                }
        }

        #endregion

        #region Private methods
        
        /// <summary>
        /// Reads the layer information.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <exception cref="InvalidDataException">No map information entries in the layer.</exception>
        /// <exception cref="NotSupportedException">Converting from polynomial orders higher than 1 is not supported.</exception>
        private void ReadLayerInformation(Stream stream)
        {
            EhfaEntry mapInfoEntry = LayerEntry.Children.FirstOrDefault(x => x.DataType.Equals("Eprj_MapInfo"));
            EhfaEntry mapInformationEntry = LayerEntry.Children.FirstOrDefault(x => x.Name.Equals("MapInformation"));

            if (mapInformationEntry == null && mapInfoEntry == null)
                throw new InvalidDataException("No map information entries in the layer.");

            if (mapInfoEntry != null)
            {
                EhfaStructure mapInfo = mapInfoEntry.ReadData(stream);

                EhfaStructure upperLeftStructure = mapInfo.GetValue<EhfaStructure>("upperLeftCenter");
                UpperLeftCenter = new Coordinate(upperLeftStructure.GetValue<Double>("x"), upperLeftStructure.GetValue<Double>("y"));

                EhfaStructure lowerRightStructure = mapInfo.GetValue<EhfaStructure>("lowerRightCenter");
                LowerRightCenter = new Coordinate(lowerRightStructure.GetValue<Double>("x"), lowerRightStructure.GetValue<Double>("y"));

                EhfaStructure pixelSize = mapInfo.GetValue<EhfaStructure>("pixelSize");
                PixelWidth = pixelSize.GetValue<Double>("width");
                PixelHeight = pixelSize.GetValue<Double>("height");

                CoordinateVector scale = new CoordinateVector(PixelWidth, PixelHeight);

                RasterMapper = RasterMapper.FromTransformation(RasterMapMode.ValueIsCoordinate, UpperLeftCenter, scale);
            }
            else
            {
                EhfaEntry xformEntry = LayerEntry.Children.First(x => x.Name.Equals("MapToPixelXForm"));
                EhfaStructure xform0 = xformEntry.Child.ReadData(stream);
                EhfaStructure forward = xform0.GetValue<EhfaStructure>("forward");

                UInt32 order = forward.GetValue<UInt32>("order");
                if (order != 1)
                    throw new NotSupportedException("Converting from polynomial orders higher than 1 is not supported.");

                BaseData coefficientMatrix = forward.GetValue<BaseData>("polycoefmtx");
                BaseData coefficientVector = forward.GetValue<BaseData>("polycoefvector");

                Matrix transformation = new Matrix(4, 4)
                {
                    [0, 0] = (Double)coefficientMatrix.Values[0, 0],
                    [0, 1] = (Double)coefficientMatrix.Values[0, 1],
                    [1, 0] = (Double)coefficientMatrix.Values[1, 0],
                    [1, 1] = (Double)coefficientMatrix.Values[1, 1],
                    [0, 3] = (Double)coefficientVector.Values[0, 0],
                    [1, 3] = (Double)coefficientVector.Values[1, 0],
                    [2, 2] = 1,
                    [3, 3] = 1
                };

                RasterMapper = RasterMapper.FromTransformation(RasterMapMode.ValueIsCoordinate, transformation);
            }
        }

        /// <summary>
        /// Reads the projection information.
        /// </summary>
        /// <param name="stream">The stream.</param>
        private void ReadProjectionInformation(Stream stream)
        {
            EhfaEntry mapInformationEntry = LayerEntry.Children.FirstOrDefault(x => x.Name.Equals("MapInformation"));
            if (mapInformationEntry != null)
            {
                EhfaStructure mapInformation = mapInformationEntry.ReadData(stream);
                String projection = mapInformation.GetValue<EhfaStructure>("projection").GetValue<String>("string");

                if (projection.StartsWith("GCS", StringComparison.OrdinalIgnoreCase) ||
                    projection.StartsWith("PCS", StringComparison.OrdinalIgnoreCase))
                {
                    projection = projection.Substring(3);
                }

                projection = projection.Replace('_', ' ').Trim();
                CoordinateReferenceSystem referenceSystem = ProjectedCoordinateReferenceSystems.FromName(projection).FirstOrDefault()
                                                ?? (CoordinateReferenceSystem)Geographic2DCoordinateReferenceSystems.FromName(projection).FirstOrDefault();

                if (referenceSystem != null)
                {
                    ReferenceSystem = referenceSystem;
                    return;
                }
            }

            EhfaEntry projectionParametersEntry = LayerEntry.Children.FirstOrDefault(x => x.DataType.Equals("Eprj_ProParameters"));
            if (projectionParametersEntry == null)
            {
                ReferenceSystem = null;
                return;
            }

            EhfaStructure imgProjection = projectionParametersEntry.ReadData(stream);

            EhfaStructure proSpheroid = imgProjection.GetValue<EhfaStructure>("proSpheroid");
            Ellipsoid ellipsoid = ComputeEllipsoid(proSpheroid);


            EhfaStructure imgDatum = LayerEntry.Children.First(x => x.DataType.Equals("Eprj_ProParameters")).Child.ReadData(stream);

            GeodeticDatum datum = GeodeticDatums.FromName(imgDatum.GetValue<String>("datumname")).FirstOrDefault();
            if (datum == null)
            {
                ReferenceSystem = null;
                return;
            }

            GeographicCoordinateReferenceSystem coordinateReferenceSystem = new GeographicCoordinateReferenceSystem(IdentifiedObject.UserDefinedIdentifier, IdentifiedObject.UserDefinedName,
                                                                                                                    CoordinateSystems.EllipsoidalLatLonD, datum, AreasOfUse.World);

            ReferenceSystem = ComputeProjectedReferenceSystem(imgProjection, coordinateReferenceSystem, ellipsoid, datum);
        }

        /// <summary>
        /// Reads the block information.
        /// </summary>
        /// <param name="stream">The stream.</param>
        private void ReadBlockInformation(Stream stream)
        {
            EhfaEntry rasterDMSEntry = LayerEntry.Children.First(x => x.Name.Equals("RasterDMS"));
            EhfaStructure rasterDMSState = rasterDMSEntry.ReadData(stream);

            IEnumerable<EhfaStructure> blockInfos = rasterDMSState.GetValue<IEnumerable<EhfaStructure>>("blockinfo");
            _blockInfo = blockInfos.Select(x => new BlockInformation(x.GetValue<Int32>("offset"), x.GetValue<UInt32>("size"), x.GetValue<String>("compressionType"))).ToList();
        }

        /// <summary>
        /// Computes the projected reference system.
        /// </summary>
        /// <param name="imgProjection">The img projection.</param>
        /// <param name="baseReferenceSystem">The base reference system.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="datum">The datum.</param>
        /// <returns>The coordinate reference system.</returns>
        /// <exception cref="NotSupportedException">The projection is not supported.</exception>
        private CoordinateReferenceSystem ComputeProjectedReferenceSystem(EhfaStructure imgProjection, GeographicCoordinateReferenceSystem baseReferenceSystem, Ellipsoid ellipsoid, GeodeticDatum datum)
        {
            CoordinateProjection projection;

            String projectionName = imgProjection.GetValue<String>("proName");
            CoordinateProjection coordinateProjection = CoordinateProjectionFactory.FromName(projectionName, ellipsoid).FirstOrDefault();
            if (coordinateProjection != null)
            {
                projection = coordinateProjection;

                return new ProjectedCoordinateReferenceSystem(IdentifiedObject.UserDefinedIdentifier, IdentifiedObject.UserDefinedName,
                                              baseReferenceSystem, CoordinateSystems.CartesianENM, AreasOfUse.World, projection);

            }

            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            List<Double> projectionParameters = imgProjection.GetValue<IEnumerable<Object>>("proParams").Select(x => (Double)x).ToList();
            Int32 projectionZone = (Int32)imgProjection.GetValue<UInt32>("proZone");
            Int32 projectionNumber = (Int32)imgProjection.GetValue<UInt32>("proNumber");

            switch (projectionNumber)
            {
                case 0: // 0="Geographic(Latitude/Longitude)"
                    return new GeographicCoordinateReferenceSystem(IdentifiedObject.UserDefinedIdentifier, IdentifiedObject.UserDefinedName,
                                                                   CoordinateSystems.EllipsoidalLatLonD, datum, AreasOfUse.World);
                case 1:  // 1="UTM"
                    EllipsoidHemisphere hemisphere = projectionParameters[3] > 0 ? EllipsoidHemisphere.North : EllipsoidHemisphere.South;
                    projection = CoordinateProjectionFactory.UniversalTransverseMercatorZone(ellipsoid, projectionZone, hemisphere);
                    break;
                case 2:
                    return Math.Abs(projectionParameters[0]) < 0.001 ? Geographic2DCoordinateReferenceSystems.NAD27 : Geographic2DCoordinateReferenceSystems.NAD83;
                case 3: // 3="Albers Conical Equal Area"
                    parameters.Add(CoordinateOperationParameters.LatitudeOfFalseOrigin, Angle.FromDegree(projectionParameters[5]));
                    parameters.Add(CoordinateOperationParameters.LongitudeOfFalseOrigin, Angle.FromDegree(projectionParameters[4]));
                    parameters.Add(CoordinateOperationParameters.LatitudeOf1stStandardParallel, Angle.FromDegree(projectionParameters[2]));
                    parameters.Add(CoordinateOperationParameters.LatitudeOf2ndStandardParallel, Angle.FromDegree(projectionParameters[3]));
                    parameters.Add(CoordinateOperationParameters.EastingAtFalseOrigin, Length.FromMetre(projectionParameters[6]));
                    parameters.Add(CoordinateOperationParameters.NorthingAtFalseOrigin, Length.FromMetre(projectionParameters[7]));

                    projection = CoordinateProjectionFactory.FromMethod(CoordinateOperationMethods.AlbersEqualAreaProjection, parameters, ellipsoid, AreasOfUse.World);
                    break;
                case 4: // "Lambert Conformal Conic"
                    parameters.Add(CoordinateOperationParameters.LatitudeOfFalseOrigin, Angle.FromDegree(projectionParameters[5]));
                    parameters.Add(CoordinateOperationParameters.LongitudeOfFalseOrigin, Angle.FromDegree(projectionParameters[4]));
                    parameters.Add(CoordinateOperationParameters.LatitudeOf1stStandardParallel, Angle.FromDegree(projectionParameters[2]));
                    parameters.Add(CoordinateOperationParameters.LatitudeOf2ndStandardParallel, Angle.FromDegree(projectionParameters[3]));
                    parameters.Add(CoordinateOperationParameters.EastingAtFalseOrigin, Length.FromMetre(projectionParameters[6]));
                    parameters.Add(CoordinateOperationParameters.NorthingAtFalseOrigin, Length.FromMetre(projectionParameters[7]));

                    projection = CoordinateProjectionFactory.FromMethod(CoordinateOperationMethods.LambertConicConformal2SPProjection, parameters, ellipsoid, AreasOfUse.World);
                    break;
                case 5: // "Mercator"
                    parameters.Add(CoordinateOperationParameters.LatitudeOfFalseOrigin, Angle.FromDegree(projectionParameters[5]));
                    parameters.Add(CoordinateOperationParameters.LongitudeOfFalseOrigin, Angle.FromDegree(projectionParameters[4]));
                    parameters.Add(CoordinateOperationParameters.EastingAtFalseOrigin, Length.FromMetre(projectionParameters[6]));
                    parameters.Add(CoordinateOperationParameters.NorthingAtFalseOrigin, Length.FromMetre(projectionParameters[7]));

                    projection = CoordinateProjectionFactory.FromMethod(CoordinateOperationMethods.MercatorSphericalProjection, parameters, ellipsoid, AreasOfUse.World);
                    break;
                case 6: // "Polar Stereographic"
                    parameters.Add(CoordinateOperationParameters.LatitudeOfStandardParallel, projectionParameters[5]);
                    parameters.Add(CoordinateOperationParameters.LongitudeOfOrigin, projectionParameters[4]);
                    parameters.Add(CoordinateOperationParameters.FalseEasting, projectionParameters[6]);
                    parameters.Add(CoordinateOperationParameters.FalseNorthing, projectionParameters[7]);

                    projection = CoordinateProjectionFactory.FromMethod(CoordinateOperationMethods.PolarStereographicBProjection, parameters, ellipsoid, AreasOfUse.World);
                    break;
                case 7: // 7="Polyconic"
                    parameters.Add(CoordinateOperationParameters.LatitudeOfFalseOrigin, Angle.FromDegree(projectionParameters[5]));
                    parameters.Add(CoordinateOperationParameters.LongitudeOfFalseOrigin, Angle.FromDegree(projectionParameters[4]));
                    parameters.Add(CoordinateOperationParameters.EastingAtFalseOrigin, Length.FromMetre(projectionParameters[6]));
                    parameters.Add(CoordinateOperationParameters.NorthingAtFalseOrigin, Length.FromMetre(projectionParameters[7]));

                    projection = CoordinateProjectionFactory.FromMethod(CoordinateOperationMethods.AmericanPolyconicProjection, parameters, ellipsoid, AreasOfUse.World);
                    break;
                case 8:  // 8="Equidistant Conic"
                    throw new NotSupportedException("Equidistant conic projection is not supported");
                case 9: // 9="Transverse Mercator"
                    parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, Angle.FromDegree(projectionParameters[2]));
                    parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(projectionParameters[4]));
                    parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(projectionParameters[5]));
                    parameters.Add(CoordinateOperationParameters.EastingAtFalseOrigin, Length.FromMetre(projectionParameters[6]));
                    parameters.Add(CoordinateOperationParameters.NorthingAtFalseOrigin, Length.FromMetre(projectionParameters[7]));

                    projection = CoordinateProjectionFactory.FromMethod(CoordinateOperationMethods.TransverseMercatorProjection, parameters, ellipsoid, AreasOfUse.World);
                    break;
                case 10: // 10="Stereographic"
                    parameters.Add(CoordinateOperationParameters.LongitudeOfOrigin, Angle.FromDegree(projectionParameters[4]));
                    parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(projectionParameters[5]));
                    parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(projectionParameters[6]));
                    parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(projectionParameters[7]));

                    projection = CoordinateProjectionFactory.FromMethod(CoordinateOperationMethods.PolarStereographicBProjection, parameters, ellipsoid, AreasOfUse.World);
                    break;
                case 11: // 11="Lambert Azimuthal Equal-area"
                    parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(projectionParameters[4]));
                    parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(projectionParameters[5]));
                    parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(projectionParameters[6]));
                    parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(projectionParameters[7]));

                    projection = CoordinateProjectionFactory.FromMethod(CoordinateOperationMethods.LambertAzimuthalEqualAreaProjection, parameters, ellipsoid, AreasOfUse.World);
                    break;
                case 12: // 12="Azimuthal Equidistant"
                    parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(projectionParameters[4]));
                    parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(projectionParameters[5]));
                    parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(projectionParameters[6]));
                    parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(projectionParameters[7]));

                    projection = CoordinateProjectionFactory.FromMethod(CoordinateOperationMethods.ModifiedAzimuthalEquidistantProjection, parameters, ellipsoid, AreasOfUse.World);
                    break;
                case 13: // 13="Gnomonic"
                    throw new NotSupportedException("Gnomonic projection is not supported");
                case 14:// 14="Orthographic"
                    throw new NotSupportedException("Orthographic projection is not supported");
                case 15: // 15="General Vertical Near-Side Perspective"
                    parameters.Add(CoordinateOperationParameters.ViewPointHeight, Length.FromMetre(projectionParameters[2]));
                    parameters.Add(CoordinateOperationParameters.LongitudeOfTopocentricOrigin, Angle.FromDegree(projectionParameters[4]));
                    parameters.Add(CoordinateOperationParameters.LatitudeOfTopocentricOrigin, Angle.FromDegree(projectionParameters[5]));
                    parameters.Add(CoordinateOperationParameters.EllipsoidalHeightOfTopocentricOrigin, 0);

                    projection = CoordinateProjectionFactory.FromMethod(CoordinateOperationMethods.VerticalPerspectiveProjection, parameters, ellipsoid, AreasOfUse.World);
                    break;
                case 16:  // 16="Sinusoidal"
                    throw new NotSupportedException("Sinusoidal projection is not supported");
                case 17: // 17="Equirectangular"
                    parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, projectionParameters[4]);
                    parameters.Add(CoordinateOperationParameters.LatitudeOf1stStandardParallel, projectionParameters[5]);

                    projection = CoordinateProjectionFactory.FromMethod(CoordinateOperationMethods.EquidistantCylindricalProjection, parameters, ellipsoid, AreasOfUse.World);
                    break;
                case 18: // 18="Miller Cylindrical"
                    throw new NotSupportedException("Miller Cylindrical projection is not supported");
                case 19: // 19="Van der Grinten I"
                    throw new NotSupportedException("Van der Grinten projection is not supported");
                case 20: // 20="Oblique Mercator (Hotine)"
                    if (Math.Abs(projectionParameters[12]) < 0.001) // case 0 Two Point Case
                    {
                        throw new NotSupportedException("Two point case of HotineObliqueMercator is not supported");
                    }
                    else // case 1 Azimuth case
                    {
                        parameters.Add(CoordinateOperationParameters.LatitudeOfProjectionCentre, Angle.FromDegree(projectionParameters[5]));
                        parameters.Add(CoordinateOperationParameters.LongitudeOfProjectionCentre, Angle.FromDegree(projectionParameters[4]));
                        parameters.Add(CoordinateOperationParameters.AzimuthOfInitialLine, Angle.FromDegree(projectionParameters[3]));
                        parameters.Add(CoordinateOperationParameters.ScaleFactorOnInitialLine, projectionParameters[2]);
                        parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(projectionParameters[6]));
                        parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(projectionParameters[7]));

                        projection = CoordinateProjectionFactory.FromMethod(CoordinateOperationMethods.LabordeObliqueMercatorProjection, parameters, ellipsoid, AreasOfUse.World);
                    }
                    break;
                case 21: // 21="Space Oblique Mercator" 
                    throw new NotSupportedException("Space Oblique Mercator projection is not supported");
                case 22: // 22 = "Modified Transverse Mercator"
                    throw new NotSupportedException("Modified Transverse Mercator projection is not supported");
                default:
                    throw new NotSupportedException("Computing the projection with the specified ID is not supported.");
            }

            return new ProjectedCoordinateReferenceSystem(IdentifiedObject.UserDefinedIdentifier, IdentifiedObject.UserDefinedName,
                              baseReferenceSystem, CoordinateSystems.CartesianENM, AreasOfUse.World, projection);
        }

        /// <summary>
        /// Computes the ellipsoid.
        /// </summary>
        /// <param name="proSpheroid">The projection spheroid.</param>
        /// <returns>The ellipsoid.</returns>
        private Ellipsoid ComputeEllipsoid(EhfaStructure proSpheroid)
        {
            Ellipsoid ellipsoid = Ellipsoids.FromIdentifier(proSpheroid.GetValue<String>("sphereName")).FirstOrDefault();
            if (ellipsoid == null)
            {
                Double semiMinorAxis = proSpheroid.GetValue<Double>("a");
                Double semiMajorAxis = proSpheroid.GetValue<Double>("b");
                ellipsoid = Ellipsoid.FromSemiMinorAxis(IdentifiedObject.UserDefinedIdentifier, IdentifiedObject.UserDefinedName, semiMinorAxis, semiMajorAxis);
            }

            return ellipsoid;
        }

        /// <summary>
        /// Reads a raster block containing floating-point values from the specified stream.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="blockInfo">The block information.</param>
        /// <returns>The collection of values within the block.</returns>
        private IEnumerable<Double> ReadFloatingRasterBlock(Stream stream, BlockInformation blockInfo)
        {
            stream.Seek(blockInfo.Offset, SeekOrigin.Begin);
            
            if (blockInfo.CompressionType != null && blockInfo.CompressionType != "no compression")
            {
                return DecompressFloatingBlock(stream, blockInfo);
            }
            else
            {
                Byte[] buffer = new Byte[blockInfo.Size];
                stream.Read(buffer, 0, buffer.Length);

                return ReadFloatingValuesFromBlock(buffer, RadiometricResolution, 0);
            }
        }

        /// <summary>
        /// Reads a raster block containing signed integer values from the specified stream.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="blockInfo">The block information.</param>
        /// <returns>The collection of values within the block.</returns>
        private IEnumerable<Int32> ReadSignedIntegerRasterBlock(Stream stream, BlockInformation blockInfo)
        {
            stream.Seek(blockInfo.Offset, SeekOrigin.Begin);

            if (blockInfo.CompressionType != null && blockInfo.CompressionType != "no compression")
            {
                return DecompressSignedIntegerBlock(stream, blockInfo);
            }
            else
            {
                Byte[] buffer = new Byte[blockInfo.Size];
                stream.Read(buffer, 0, buffer.Length);

                return ReadSignedIntegerValuesFromBlock(buffer, RadiometricResolution, 0);
            }
        }

        /// <summary>
        /// Reads a raster block containing unsigned integer values from the specified stream.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="blockInfo">The block information.</param>
        /// <returns>The collection of values within the block.</returns>
        private IEnumerable<UInt32> ReadUnsignedIntegerRasterBlock(Stream stream, BlockInformation blockInfo)
        {
            stream.Seek(blockInfo.Offset, SeekOrigin.Begin);

            if (blockInfo.CompressionType != null && blockInfo.CompressionType != "no compression")
            {
                return DecompressUnsignedIntegerBlock(stream, blockInfo);
            }
            else
            {
                Byte[] buffer = new Byte[blockInfo.Size];
                stream.Read(buffer, 0, buffer.Length);

                return ReadUnsignedIntegerValuesFromBlock(buffer, RadiometricResolution, 0);
            }
        }

        /// <summary>
        /// Decompresses a block containing floating-point values.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="blockInfo">The block information.</param>
        /// <returns>The array of decompressed values within the block.</returns>
        private IEnumerable<Double> DecompressFloatingBlock(Stream stream, BlockInformation blockInfo)
        {
            Int64 blockStartPointer = stream.Position;
            EhfaStructure encodingParameters = new EhfaStructure(EhfaStructureType.EdmsRLCParamsType);
            encodingParameters.Read(stream);

            Int32 blockMinimumValue = encodingParameters.GetValue<Int32>("min");

            Byte bitsPerValue = (Byte)encodingParameters.GetValue<String>("numbitspervalue").FirstOrDefault();
            if (bitsPerValue == 0)
            {
                for (Int32 valueIndex = 0; valueIndex < BlockWidth * BlockHeight; valueIndex++)
                    yield return blockMinimumValue;
            }

            Int32 blockSegmentCount = encodingParameters.GetValue<Int32>("numsegments");

            if (blockSegmentCount == -1)
            {
                // if the algorithm decides that it is not worthwhile to compress runs, 
                // numsegments will be set to -1 and you will just find the range compressed data at the data offset

                Byte[] buffer = new Byte[(Int32)(BlockWidth * BlockHeight * (8.0 / bitsPerValue))];
                stream.Read(buffer, 0, buffer.Length);

                foreach (Double value in ReadFloatingValuesFromBlock(buffer, bitsPerValue, blockMinimumValue))
                    yield return value;
            }
            else
            {
                Int32[] repeatCount = new Int32[blockSegmentCount];

                for (Int32 segment = 0; segment < blockSegmentCount; segment++)
                {
                    Byte firstByte = (Byte)stream.ReadByte();

                    Int32 numByteCounts = firstByte >> 6;
                    Byte firstByteCount = (Byte)(firstByte << 2);
                    firstByteCount = (Byte)(firstByteCount >> 2);

                    List<Byte> countByteList = new List<Byte> { firstByteCount };

                    while (numByteCounts-- > 0)
                        countByteList.Add((Byte)stream.ReadByte());

                    while (countByteList.Count < 4)
                        countByteList.Insert(0, 0);

                    Int32 repeats = EndianBitConverter.ToInt32(countByteList.ToArray(), ByteOrder.BigEndian);
                    repeatCount[segment] = repeats;
                }

                Byte[] buffer = new Byte[blockInfo.Size - (stream.Position - blockStartPointer)];
                stream.Read(buffer, 0, buffer.Length);

                IEnumerator<Double> enumerator = ReadFloatingValuesFromBlock(buffer, bitsPerValue, blockMinimumValue).GetEnumerator();

                for (Int32 segment = 0; segment < blockSegmentCount; segment++)
                {
                    enumerator.MoveNext();

                    while (repeatCount[segment] > 0)
                    {
                        repeatCount[segment]--;
                        yield return enumerator.Current;
                    }
                }
            }
        }

        /// <summary>
        /// Decompresses a block containing signed integer values.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="blockInfo">The block information.</param>
        /// <returns>The array of decompressed values within the block.</returns>
        private IEnumerable<Int32> DecompressSignedIntegerBlock(Stream stream, BlockInformation blockInfo)
        {
            Int64 blockStartPointer = stream.Position;
            EhfaStructure encodingParameters = new EhfaStructure(EhfaStructureType.EdmsRLCParamsType);
            encodingParameters.Read(stream);

            Int32 blockMinimumValue = encodingParameters.GetValue<Int32>("min");

            Byte bitsPerValue = (Byte)encodingParameters.GetValue<String>("numbitspervalue").FirstOrDefault();
            if (bitsPerValue == 0)
            {
                for (Int32 valueIndex = 0; valueIndex < BlockWidth * BlockHeight; valueIndex++)
                    yield return blockMinimumValue;
            }

            Int32 blockSegmentCount = encodingParameters.GetValue<Int32>("numsegments");

            if (blockSegmentCount == -1)
            {
                // if the algorithm decides that it is not worthwhile to compress runs, 
                // numsegments will be set to -1 and you will just find the range compressed data at the data offset

                Byte[] buffer = new Byte[(Int32)(BlockWidth * BlockHeight * (8.0 / bitsPerValue))];
                stream.Read(buffer, 0, buffer.Length);

                foreach (Int32 value in ReadSignedIntegerValuesFromBlock(buffer, bitsPerValue, blockMinimumValue))
                    yield return value;
            }
            else
            {
                Int32[] repeatCount = new Int32[blockSegmentCount];

                for (Int32 segment = 0; segment < blockSegmentCount; segment++)
                {
                    Byte firstByte = (Byte)stream.ReadByte();

                    Int32 numByteCounts = firstByte >> 6;
                    Byte firstByteCount = (Byte)(firstByte << 2);
                    firstByteCount = (Byte)(firstByteCount >> 2);

                    List<Byte> countByteList = new List<Byte> { firstByteCount };

                    while (numByteCounts-- > 0)
                        countByteList.Add((Byte)stream.ReadByte());

                    while (countByteList.Count < 4)
                        countByteList.Insert(0, 0);

                    Int32 repeats = EndianBitConverter.ToInt32(countByteList.ToArray(), ByteOrder.BigEndian);
                    repeatCount[segment] = repeats;
                }

                Byte[] buffer = new Byte[blockInfo.Size - (stream.Position - blockStartPointer)];
                stream.Read(buffer, 0, buffer.Length);

                IEnumerator<Int32> enumerator = ReadSignedIntegerValuesFromBlock(buffer, bitsPerValue, blockMinimumValue).GetEnumerator();

                for (Int32 segment = 0; segment < blockSegmentCount; segment++)
                {
                    enumerator.MoveNext();

                    while (repeatCount[segment] > 0)
                    {
                        repeatCount[segment]--;
                        yield return enumerator.Current;
                    }
                }
            }
        }

        /// <summary>
        /// Decompresses a block containing unsigned integer values.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="blockInfo">The block information.</param>
        /// <returns>The array of decompressed values within the block.</returns>
        private IEnumerable<UInt32> DecompressUnsignedIntegerBlock(Stream stream, BlockInformation blockInfo)
        {
            Int64 blockStartPointer = stream.Position;
            EhfaStructure encodingParameters = new EhfaStructure(EhfaStructureType.EdmsRLCParamsType);
            encodingParameters.Read(stream);

            UInt32 blockMinimumValue = (UInt32)encodingParameters.GetValue<Int32>("min");

            Byte bitsPerValue = (Byte)encodingParameters.GetValue<String>("numbitspervalue").FirstOrDefault();
            if (bitsPerValue == 0)
            {
                for (Int32 valueIndex = 0; valueIndex < BlockWidth * BlockHeight; valueIndex++)
                    yield return blockMinimumValue;
            }

            Int32 blockSegmentCount = encodingParameters.GetValue<Int32>("numsegments");

            if (blockSegmentCount == -1)
            {
                // if the algorithm decides that it is not worthwhile to compress runs, 
                // numsegments will be set to -1 and you will just find the range compressed data at the data offset

                Byte[] buffer = new Byte[(Int32)(BlockWidth * BlockHeight * (8.0 / bitsPerValue))];
                stream.Read(buffer, 0, buffer.Length);

                foreach (UInt32 value in ReadUnsignedIntegerValuesFromBlock(buffer, bitsPerValue, blockMinimumValue))
                    yield return value;
            }
            else
            {
                Int32[] repeatCount = new Int32[blockSegmentCount];

                for (Int32 segment = 0; segment < blockSegmentCount; segment++)
                {
                    Byte firstByte = (Byte)stream.ReadByte();

                    Int32 numByteCounts = firstByte >> 6;
                    Byte firstByteCount = (Byte)(firstByte << 2);
                    firstByteCount = (Byte)(firstByteCount >> 2);

                    List<Byte> countByteList = new List<Byte> { firstByteCount };

                    while (numByteCounts-- > 0)
                        countByteList.Add((Byte)stream.ReadByte());

                    while (countByteList.Count < 4)
                        countByteList.Insert(0, 0);

                    Int32 repeats = EndianBitConverter.ToInt32(countByteList.ToArray(), ByteOrder.BigEndian);
                    repeatCount[segment] = repeats;
                }
                
                Byte[] buffer = new Byte[blockInfo.Size - (stream.Position - blockStartPointer)];
                stream.Read(buffer, 0, buffer.Length);

                IEnumerator<UInt32> enumerator = ReadUnsignedIntegerValuesFromBlock(buffer, bitsPerValue, blockMinimumValue).GetEnumerator();

                for (Int32 segment = 0; segment < blockSegmentCount; segment++)
                {
                    enumerator.MoveNext();

                    while (repeatCount[segment] > 0)
                    {
                        repeatCount[segment]--;
                        yield return enumerator.Current;
                    }
                }
            }
        }

        /// <summary>
        /// Reads the floating-point values from the block.
        /// </summary>
        /// <param name="block">The block bytes.</param>
        /// <param name="bitsPerValue">The number of bits per value.</param>
        /// <param name="valueShift">A value by which all values are shifted.</param>
        /// <returns>The collection of values within the block.</returns>
        private IEnumerable<Double> ReadFloatingValuesFromBlock(Byte[] block, Int32 bitsPerValue, Int32 valueShift)
        {
            Double value = 0;
            Int32 byteIndex = 0;

            while (byteIndex < block.Length)
            {
                switch (bitsPerValue)
                {
                    case 32:
                        value = EndianBitConverter.ToSingleUnchecked(block, byteIndex);
                        byteIndex += 4;
                        break;
                    case 64:
                        value = EndianBitConverter.ToDoubleUnchecked(block, byteIndex);
                        byteIndex += 8;
                        break;
                }

                yield return value + valueShift;
            }
        }

        /// <summary>
        /// Reads the signed integer values from the byte array.
        /// </summary>
        /// <param name="block">The block bytes.</param>
        /// <param name="bitsPerValue">The number of bits per value.</param>
        /// <param name="valueShift">A value by which all values are shifted.</param>
        /// <returns>The collection of values within the block.</returns>
        private IEnumerable<Int32> ReadSignedIntegerValuesFromBlock(Byte[] block, Int32 bitsPerValue, Int32 valueShift)
        {
            Int32 value = 0;
            Int32 byteIndex = 0, bitOffset = 8;

            while (byteIndex < block.Length)
            {
                switch (bitsPerValue)
                {
                    case 1:
                    case 2:
                    case 4:
                        value = (SByte)((block[byteIndex] >> bitOffset) & bitsPerValue);
                        bitOffset += bitsPerValue;
                        if (bitOffset == 0)
                        {
                            bitOffset = 8;
                            byteIndex++;
                        }
                        break;
                    case 8:
                        value = block[byteIndex];
                        byteIndex++;
                        break;
                    case 16:
                        value = EndianBitConverter.ToInt16Unchecked(block, byteIndex);
                        byteIndex += 2;
                        break;
                    case 32:
                        value = EndianBitConverter.ToInt32Unchecked(block, byteIndex);
                        byteIndex += 4;
                        break;
                }

                yield return value + valueShift;
            }
        }

        /// <summary>
        /// Reads the unsigned integer values from the byte array.
        /// </summary>
        /// <param name="block">The block bytes.</param>
        /// <param name="bitsPerValue">The number of bits per value.</param>
        /// <param name="valueShift">A value by which all values are shifted.</param>
        /// <returns>The collection of values within the block.</returns>
        private IEnumerable<UInt32> ReadUnsignedIntegerValuesFromBlock(Byte[] block, Int32 bitsPerValue, UInt32 valueShift)
        {
            UInt32 value = 0;
            Int32 byteIndex = 0, bitOffset = 8;

            while (byteIndex < block.Length)
            {
                switch (bitsPerValue)
                {
                    case 1:
                    case 2:
                    case 4:
                        value = (Byte)((block[byteIndex] >> bitOffset) & bitsPerValue);
                        bitOffset -= bitsPerValue;
                        if (bitOffset == 0)
                        {
                            bitOffset = 8;
                            byteIndex++;
                        }
                        break;
                    case 8:
                        value = block[byteIndex];
                        byteIndex++;
                        break;
                    case 16:
                        value = EndianBitConverter.ToUInt16Unchecked(block, byteIndex);
                        byteIndex += 2;
                        break;
                    case 32:
                        value = EndianBitConverter.ToUInt32Unchecked(block, byteIndex);
                        byteIndex += 4;
                        break;
                }

                yield return value + valueShift;
            }
        }

        #endregion
    }
}
