/// <copyright file="IdentifiedObjectConverter.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Tamás Nagy</author>

using ELTE.AEGIS.Reference;
using ELTE.AEGIS.Reference.Operations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.IO.WellKnown
{
    /// <summary>
    /// Represents a converter for Well-known Text (WKT) representation of <see cref="IdentifiedObject" /> instances.
    /// </summary>
    public static class IdentifiedObjectConverter
    {
        #region Private types

        /// <summary>
        /// Represents a delimiter within the Well-Known Text.
        /// </summary>
        private class Delimiter
        {
            #region Private Fields

            private readonly Char _left;
            private readonly Char _right;

            #endregion

            #region Public Properties

            /// <summary>
            /// Gets the left delimiter.
            /// </summary>
            /// <value>The left delimiter character.</value>
            public Char Left { get { return _left; } }

            /// <summary>
            /// Gets the right delimiter.
            /// </summary>
            /// <value>The right delimiter character.</value>
            public Char Right { get { return _right; } }

            #endregion

            #region Constructor

            /// <summary>
            /// Initializes a new instance of the <see cref="Delimiter" /> class.
            /// </summary>
            /// <param name="leftDelimiter">The left delimiter character.</param>
            /// <exception cref="System.ArgumentException">The delimiter is invalid.</exception>
            public Delimiter(Char leftDelimiter)
            {
                if (leftDelimiter != '[' && leftDelimiter != '(')
                    throw new ArgumentException("The delimiter is invalid.", "leftDelimiter");

                if (leftDelimiter == '[')
                {
                    _left = '['; 
                    _right = ']'; 
                }
                else if (leftDelimiter == '(')
                { 
                    _left = '('; 
                    _right = ')'; 
                }
            }

            #endregion
        }

        #endregion

        #region Public conversion methods from identified object to WKT

        /// <summary>
        /// Converts an identified object to Well-known Text (WKT) format.
        /// </summary>
        /// <param name="identifiedObject">The identified object.</param>
        /// <returns>The WKT representation of the identified object.</returns>
        /// <exception cref="System.ArgumentNullException">The identified object is null.</exception>
        /// <exception cref="System.ArgumentException">Conversion is not suppported with the specified identified object type.</exception>
        public static String ToWellKnownText(IdentifiedObject identifiedObject)
        {
            if (identifiedObject == null)
                throw new ArgumentNullException("identifiedObject", "The identified object is null.");

            if (identifiedObject is ProjectedCoordinateReferenceSystem)
                return ComputeText(identifiedObject as ProjectedCoordinateReferenceSystem);
            if (identifiedObject is GeographicCoordinateReferenceSystem)
                return ComputeText(identifiedObject as GeographicCoordinateReferenceSystem);
            if (identifiedObject is UnitOfMeasurement)
                return ComputeText(identifiedObject as UnitOfMeasurement);
            if (identifiedObject is GeodeticDatum)
                return ComputeText(identifiedObject as GeodeticDatum);
            if (identifiedObject is Ellipsoid)
                return ComputeText(identifiedObject as Ellipsoid);
            if (identifiedObject is Meridian)
                return ComputeText(identifiedObject as Meridian);

            throw new ArgumentException("Conversion is not suppported with the specified identified object type.", "identifiedObject");
        }

        /// <summary>
        /// Converts a reference system to Well-known Text (WKT) format.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The WKT representation of the reference system.</returns>
        /// <exception cref="System.ArgumentNullException">The reference system is null.</exception>
        /// <exception cref="System.ArgumentException">Conversion is not suppported with the specified refeence system type.</exception>
        public static String ToWellKnownText(IReferenceSystem referenceSystem)
        {
            if (referenceSystem == null)
                throw new ArgumentNullException("referenceSystem", "The reference system is null.");

            if (referenceSystem is ProjectedCoordinateReferenceSystem)
                return ComputeText(referenceSystem as ProjectedCoordinateReferenceSystem);
            if (referenceSystem is GeographicCoordinateReferenceSystem)
                return ComputeText(referenceSystem as GeographicCoordinateReferenceSystem);

            throw new ArgumentException("Conversion is not suppported with the specified refeence system type.", "referenceSystem");
        }

        #endregion

        #region Public conversion methods from WKT to identified object

        /// <summary>
        /// Converts the specified Well-known Text to identified object.
        /// </summary>
        /// <param name="text">The text representation of the object.</param>
        /// <returns>The identified object instance.</returns>
        /// <exception cref="System.ArgumentNullException">The text is null.</exception>
        /// <exception cref="System.ArgumentException">Conversion is not suppported with the specified identified object type.</exception>
        /// <exception cref="System.NotSupportedException">Geocentric coordinate reference systems are not supported.</exception>
        /// <exception cref="System.IO.InvalidDataException">The text is invalid.</exception>
        /// <exception cref="System.ArgumentException">Conversion is not suppported with the specified identified object type.</exception>
        public static IdentifiedObject ToIdentifiedObject(String text)
        {
            Delimiter delim;

            if (text == null)
                throw new ArgumentNullException("text", "The text is null.");

            if (String.IsNullOrEmpty(text))
                throw new ArgumentException("The text is empty.", "text");

            text = Regex.Replace(text.Trim(), @"\s+", "", RegexOptions.Multiline);

            // determining the delimiter used by the WKT representation
            Int32 contextStartIndex = text.IndexOf("[");
            if (contextStartIndex == -1)
            {
                contextStartIndex = text.IndexOf("(");
                if (contextStartIndex == -1)
                    throw new ArgumentException("The text is delimited with an invalid character.", "text");
                else
                    delim = new Delimiter('(');
            }
            else
                delim = new Delimiter('[');

            String type = text.Substring(0, contextStartIndex);

            try
            {
                String content = text.Substring(contextStartIndex + 1, text.Length - contextStartIndex - 2);

                switch (type)
                {
                    case "PROJCS":
                        return ComputeProjectedReferenceSystem(content, delim);
                    case "GEOGCS":
                        return ComputeGeographicCoordinateReferenceSystem(content, delim);
                    case "DATUM":
                        return ComputeGeodeticDatum(content, delim);
                    case "SPHEROID":
                    case "ELLIPSOID":
                        return ComputeEllipsoid(content);
                    case "PRIMEM":
                        return ComputePrimeMeridian(content);
                    case "GEOCCS":
                        throw new NotSupportedException("Geocentric coordinate reference systems are not supported.");
                    default:
                        throw new InvalidDataException("The text is invalid.");
                }
            }
            catch (NotSupportedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("The WKT text representation is invalid.", "text", ex);
            }
        }
        /// <summary>
        /// Converts the specified Well-known Text to reference system.
        /// </summary>
        /// <param name="text">The text representation of the reference system.</param>
        /// <returns>The reference system.</returns>
        /// <exception cref="System.ArgumentNullException">The text is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The text is empty.
        /// or
        /// The text is delimited with an invalid character.
        /// or
        /// The WKT text is invalid.
        /// </exception>
        /// <exception cref="System.NotSupportedException">Geocentric coordinate reference systems are not supported.</exception>
        /// <exception cref="System.IO.InvalidDataException">The text is invalid.</exception>
        public static IReferenceSystem ToReferenceSystem(String text)
        {
            Delimiter delim;

            if (text == null)
                throw new ArgumentNullException("text", "The text is null.");

            if (String.IsNullOrEmpty(text))
                throw new ArgumentException("The text is empty.", "text");

            text = Regex.Replace(text.Trim(), @"\s+", "", RegexOptions.Multiline);

            // determining the delimiter used by the WKT representation
            Int32 contextStartIndex = text.IndexOf("[");
            if (contextStartIndex == -1)
            {
                contextStartIndex = text.IndexOf("(");
                if (contextStartIndex == -1)
                    throw new ArgumentException("The text is delimited with an invalid character.", "text");
                else
                    delim = new Delimiter('(');
            }
            else
                delim = new Delimiter('[');

            String type = text.Substring(0, contextStartIndex);

            try
            {
                String content = text.Substring(contextStartIndex + 1, text.Length - contextStartIndex - 2);

                switch (type)
                {
                    case "PROJCS":
                        return ComputeProjectedReferenceSystem(content, delim);
                    case "GEOGCS":
                        return ComputeGeographicCoordinateReferenceSystem(content, delim);
                    case "GEOCCS":
                        throw new NotSupportedException("Geocentric coordinate reference systems are not supported.");
                    default:
                        throw new InvalidDataException("The text is invalid.");
                }
            }
            catch (NotSupportedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("The WKT text representation is invalid.", "text", ex);
            }
        }

        #endregion

        #region Private conversion methods from identified object to WKT

        /// <summary>
        /// Computes the WKT representation of the specified <see cref="ProjectedCoordinateReferenceSystem" />
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The WKT representation of the reference system.</returns>
        /// <exception cref="System.IO.InvalidDataException">The Linear Unit was not specified in the Projected Coordinate Reference System.</exception>
        private static String ComputeText(ProjectedCoordinateReferenceSystem referenceSystem)
        {
            StringBuilder result = new StringBuilder();
            result.Append("PROJCS[\"" + Regex.Replace(referenceSystem.Name, @"\s+", "_") + "\",");

            result.Append(ComputeText(referenceSystem.Base) + ",");

            result.Append("PROJECTION[\"" + Regex.Replace(referenceSystem.Projection.Name, @"\s+", "_") + "\"]" + ",");

            foreach (KeyValuePair<CoordinateOperationParameter, Object> param in referenceSystem.Projection.Parameters)
            {
                Double value;
                switch (param.Key.UnitType)
                {
                    case UnitQuantityType.Angle:
                        value = ((Angle)param.Value).Value;
                        break;
                    case UnitQuantityType.Length:
                        value = ((Length)param.Value).Value;
                        break;
                    default:
                        value = ((Double)param.Value);
                        break;
                }

                result.Append("PARAMETER[\"" + Regex.Replace(param.Key.Name, @"\s+", "_") + "\"," + value.ToString(CultureInfo.InvariantCulture) + "]");
            }

            Boolean foundLinearUnit = false;
            for (Int32 i = 0; i < referenceSystem.CoordinateSystem.Dimension && !foundLinearUnit; i++)
            {
                if (referenceSystem.CoordinateSystem[i].Unit.Type == UnitQuantityType.Length)
                {
                    result.Append(ComputeText(referenceSystem.CoordinateSystem[i].Unit));
                    foundLinearUnit = true;
                }
            }

            if (!foundLinearUnit)
                throw new InvalidDataException("The Linear Unit was not specified in the Projected Coordinate Reference System.");

            result.Append("]");

            return result.ToString();
        }
        /// <summary>
        /// Computes the WKT representation of the specified <see cref="GeographicCoordinateReferenceSystem" />.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The WKT representation of the reference system.</returns>
        /// <exception cref="System.IO.InvalidDataException">The angular unit is not specified in the Geodetic Coordinate Reference System.</exception>
        private static String ComputeText(GeographicCoordinateReferenceSystem referenceSystem)
        {
            StringBuilder result = new StringBuilder("GEOGCS[");

            result.Append("\"" + Regex.Replace(referenceSystem.Name, @"\s+", "_") + "\",");

            result.Append(ComputeText(referenceSystem.Datum as GeodeticDatum) + ",");
            result.Append(ComputeText((referenceSystem.Datum as GeodeticDatum).PrimeMeridian) + ",");

            UnitOfMeasurement linearUnit = null;
            UnitOfMeasurement angularUnit = null;

            for (Int32 i = 0; i < referenceSystem.CoordinateSystem.Dimension; i++)
            {
                if (referenceSystem.CoordinateSystem[i].Unit.Type == UnitQuantityType.Angle)
                    angularUnit = referenceSystem.CoordinateSystem[i].Unit;
                else if (referenceSystem.CoordinateSystem[i].Unit.Type == UnitQuantityType.Length)
                    linearUnit = referenceSystem.CoordinateSystem[i].Unit;
            }

            if (angularUnit == null)
                throw new InvalidDataException("The angular unit is not specified in the Geodetic Coordinate Reference System.");

            result.Append(ComputeText(angularUnit));

            if (linearUnit != null)
                result.Append("," + ComputeText(linearUnit));

            result.Append("]");
            return result.ToString();
        }
        /// <summary>
        /// Computes the WKT representation of the specified <see cref="UnitOfMeasurement" />.
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The WKT representation of the unit of measurement.</returns>
        private static String ComputeText(UnitOfMeasurement unit)
        {
            return "UNIT[\"" + Regex.Replace(unit.Name, @"\s+", "_") + "\"," + unit.BaseMultiple.ToString(CultureInfo.InvariantCulture) + "]";
        }
        /// <summary>
        /// Computes the WKT representation of the specified <see cref="GeodeticDatum" />
        /// </summary>
        /// <param name="datum">The geodetic datum.</param>
        /// <returns>The WKT representation of the geodetic datum.</returns>
        private static String ComputeText(GeodeticDatum datum)
        {
            return "DATUM[\"" + Regex.Replace(datum.Name, @"\s+", "_") + "\"," + ComputeText(datum.Ellipsoid) + "]";
        }
        /// <summary>
        /// Computes the WKT representation of the specified <see cref="Ellipsoid" />.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The WKT representation of the ellipsoid.</returns>
        private static String ComputeText(Ellipsoid ellipsoid)
        {
            return "SPHEROID[\"" + Regex.Replace(ellipsoid.Name, @"\s+", "_") + "\"," + 
                   ellipsoid.SemiMajorAxis.Value.ToString(CultureInfo.InvariantCulture) + "," + 
                   ellipsoid.InverseFattening.ToString(CultureInfo.InvariantCulture) + "]";
        }
        /// <summary>
        /// Computes the WKT representation of the specified <see cref="Meridian" />.
        /// </summary>
        /// <param name="meridian">The meridian.</param>
        /// <returns>The WKT representation of the meridian.</returns>
        private static String ComputeText(Meridian meridian)
        {
            return "PRIMEM[\"" + Regex.Replace(meridian.Name, @"\s+", "_") + "\"," + meridian.Longitude.Value.ToString(CultureInfo.InvariantCulture) + "]";
        }

        #endregion

        #region Private conversion methods from WKT to identified object

        /// <summary>
        /// Gets the Geodetic Coordinate Reference System from the given WKT Text
        /// </summary>
        /// <param name="text">Well-Known Text representation of the Geodetic Coordinate Reference System.</param>
        /// <param name="delim">The used delimiter in the Well-Known Text.</param>
        /// <returns>The Geodetic Coordinate Reference System.</returns>
        /// <exception cref="System.IO.InvalidDataException">The geographic coordinate reference system text is invalid.</exception>
        private static GeographicCoordinateReferenceSystem ComputeGeographicCoordinateReferenceSystem(String text, Delimiter delim)
        {
            try
            {
                // getting the name of the reference system
                Int32 nameEndIndex = text.IndexOf("\"", 1);
                if (nameEndIndex == -1)
                    throw new InvalidDataException("The geographic coordinate reference system text is invalid.");

                Int32 nameStartIndex = (nameEndIndex > 4 && text.Substring(1, 4) == "GCS_") ? 5 : 1;

                String referenceSystemName = Regex.Replace(text.Substring(nameStartIndex, nameEndIndex - nameStartIndex), "_", " ");
               
                text = text.Substring(nameEndIndex + 1);

                // computing the datum
                if (text.Substring(0, 6) != ",DATUM" || text[6] != delim.Left)
                    throw new InvalidDataException("The geographic coordinate reference system text is invalid.");

                Int32 datumClosingBracketIndex = FindClosingBracket(text, 6, delim);
                String datumText = text.Substring(7, datumClosingBracketIndex - 7);

                GeodeticDatum datum = ComputeGeodeticDatum(datumText, delim);
                text = text.Substring(datumClosingBracketIndex + 1);

                // computing the prime meridian
                if (text.Substring(0, 7) != ",PRIMEM" || text[7] != delim.Left)
                    throw new InvalidDataException("The geographic coordinate reference system text is invalid.");

                Int32 meridianTextLength = FindClosingBracket(text, 7, delim) - 8;
                
                // if the datum is not a known geodetic datum, then it should be extended with the prime meridian
                if (GeodeticDatums.FromIdentifier(datum.Identifier).FirstOrDefault() == null)
                {
                    String primeMeridianText = text.Substring(8, meridianTextLength);

                    Meridian primeMeridian = ComputePrimeMeridian(primeMeridianText);

                    GeodeticDatum tmp = datum;
                    datum = new GeodeticDatum(datum.Identifier, datum.Name, 
                                              datum.AnchorPoint, datum.RealizationEpoch,
                                              datum.AreaOfUse, datum.Ellipsoid, primeMeridian);
                }

                // computing the angular unit
                text = text.Substring(9 + meridianTextLength);
                if (text.Substring(0, 5) != ",UNIT" || text[5] != delim.Left)
                    throw new InvalidDataException("The geographic coordinate reference system text is invalid.");

                Int32 angularUnitClosingBracketIndex = FindClosingBracket(text, 5, delim);
                String angularUnitText = text.Substring(6, angularUnitClosingBracketIndex - 6);

                UnitOfMeasurement angularUnit = ComputeAngularUnit(angularUnitText);

                text = text.Substring(angularUnitClosingBracketIndex + 1);
               
                // computing the coordinate system
                CoordinateSystem coordinateSystem;

                if (!String.IsNullOrEmpty(text))
                {
                    // three-dimensional coordinate system 

                    if (!String.IsNullOrWhiteSpace(referenceSystemName))
                    {
                        // if it matches one of the known reference systems, it can be returned
                        GeographicCoordinateReferenceSystem referenceSystem = Geographic3DCoordinateReferenceSystems.FromName(referenceSystemName).FirstOrDefault();
                        if (referenceSystem != null)
                            return referenceSystem;
                    }

                    if (text.Substring(0, 5) != ",UNIT" || text[5] != delim.Left || text[text.Length] != delim.Right)
                        throw new InvalidDataException("The geographic coordinate reference system text is invalid.");

                    // computing the optional linear unit
                    String linearUnitText = text.Substring(6, text.Length - 7);
                    UnitOfMeasurement linearUnit = ComputeLinearUnit(linearUnitText);

                    coordinateSystem = new CoordinateSystem(CoordinateSystem.UserDefinedIdentifier, CoordinateSystem.UserDefinedName, 
                                                            CoordinateSystemType.Ellipsoidal,
                                                            CoordinateSystemAxisFactory.GeodeticLatitude(angularUnit),
                                                            CoordinateSystemAxisFactory.GeodeticLongitude(angularUnit),
                                                            CoordinateSystemAxisFactory.EllipsoidalHeight(linearUnit));

                }
                else
                {
                    // two-dimensional coordinate system

                    if (!String.IsNullOrWhiteSpace(referenceSystemName))
                    {
                        // if it matches one of the known reference systems, it can be returned
                        GeographicCoordinateReferenceSystem referenceSystem = Geographic2DCoordinateReferenceSystems.FromName(referenceSystemName).FirstOrDefault();
                        if (referenceSystem != null)
                            return referenceSystem;
                    }

                    coordinateSystem = new CoordinateSystem(CoordinateSystem.UserDefinedIdentifier, CoordinateSystem.UserDefinedName, 
                                                            CoordinateSystemType.Ellipsoidal,
                                                            CoordinateSystemAxisFactory.GeodeticLatitude(angularUnit),
                                                            CoordinateSystemAxisFactory.GeodeticLongitude(angularUnit));
                }

                return new GeographicCoordinateReferenceSystem(GeographicCoordinateReferenceSystem.UserDefinedIdentifier, referenceSystemName, coordinateSystem, datum, AreasOfUse.World);
            }
            catch
            {
                throw new InvalidDataException("The geographic coordinate reference system text is invalid.");
            }
        }

        /// <summary>
        /// Gets the Projected Reference System from the given WKT Text
        /// </summary>
        /// <param name="referenceSystemText">Well-Known Text representation of a Projected Coordinate Reference System.</param>
        /// <param name="delim">The Used delimiter in the Well-Known Text.</param>
        /// <returns>The Projected Coordinate Reference System.</returns>
        /// <exception cref="System.IO.InvalidDataException">The projected coordinate reference system text is invalid.</exception>
        private static ProjectedCoordinateReferenceSystem ComputeProjectedReferenceSystem(String referenceSystemText, Delimiter delim)
        {
            try
            {
                // getting the name of the reference system
                Int32 nameEndIndex = referenceSystemText.IndexOf('"', 1);
                if (nameEndIndex == -1)
                    throw new InvalidDataException("The projected coordinate reference system text is invalid.");

                String referenceSystemName = Regex.Replace(referenceSystemText.Substring(1, nameEndIndex - 1), "_", " ");
                referenceSystemText = referenceSystemText.Substring(nameEndIndex + 1);

                if (!String.IsNullOrWhiteSpace(referenceSystemName))
                {
                    // if it matches one of the known reference systems, it can be returned
                    ProjectedCoordinateReferenceSystem referenceSystem = ProjectedCoordinateReferenceSystems.FromName(referenceSystemName).FirstOrDefault();
                    if (referenceSystem != null)
                        return referenceSystem;
                }

                // computing the geodetic coordinate reference system
                if (referenceSystemText.Substring(0, 7) != ",GEOGCS" || referenceSystemText[7] != delim.Left)
                    throw new InvalidDataException("The projected coordinate reference system text is invalid.");

                Int32 geographicClosingBracketIndex = FindClosingBracket(referenceSystemText, 7, delim);
                String geographicText = referenceSystemText.Substring(8, geographicClosingBracketIndex - 8);

                GeographicCoordinateReferenceSystem geographicReferenceSystem = ComputeGeographicCoordinateReferenceSystem(geographicText, delim);
                referenceSystemText = referenceSystemText.Substring(geographicClosingBracketIndex + 1);

                // computing the projection
                if (referenceSystemText.Substring(0, 11) != ",PROJECTION" || referenceSystemText[11] != delim.Left)
                    throw new InvalidDataException("The projected coordinate reference system text is invalid.");

                Int32 projectionNameEndIndex = referenceSystemText.IndexOf('"', 13);
                String projectionName = Regex.Replace(referenceSystemText.Substring(13, projectionNameEndIndex - 13), "_", " ");

                if (String.IsNullOrWhiteSpace(projectionName))
                    throw new InvalidDataException("The projected coordinate reference system text is invalid.");

                referenceSystemText = referenceSystemText.Substring(projectionNameEndIndex + 2);

                // computing the projection parameters
                Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
                while (referenceSystemText.Substring(0, 10) == ",PARAMETER" && referenceSystemText[10] == delim.Left)
                {
                    Int32 closingBracketIndex = FindClosingBracket(referenceSystemText, 10, delim);

                    Int32 parameterNameEndIndex = referenceSystemText.IndexOf('"', 12);
                    String parameterName = Regex.Replace(referenceSystemText.Substring(12, parameterNameEndIndex - 12), "_", " ");

                    List<CoordinateOperationParameter> parameterList = CoordinateOperationParameters.FromName(parameterName).ToList();
                    foreach (CoordinateOperationParameter parameter in parameterList)
                    {
                        Double parameterValue = Double.Parse(referenceSystemText.Substring(parameterNameEndIndex + 2,
                                                closingBracketIndex - parameterNameEndIndex - 2), CultureInfo.InvariantCulture);

                        switch (parameter.UnitType)
                        {
                            case UnitQuantityType.Angle:
                                parameters.Add(parameter, Angle.FromRadian(parameterValue));
                                break;
                            case UnitQuantityType.Length:
                                parameters.Add(parameter, Length.FromMetre(parameterValue));
                                break;
                            default:
                                parameters.Add(parameter, parameterValue);
                                break;
                        }
                    }

                    referenceSystemText = referenceSystemText.Substring(closingBracketIndex + 1);
                }

                CoordinateProjection projection = CoordinateProjectionFactory.FromMethodName(projectionName, parameters, (geographicReferenceSystem.Datum as GeodeticDatum).Ellipsoid, AreasOfUse.World).FirstOrDefault();

                if (projection == null)
                    throw new InvalidDataException("The projection is unknown.");

                // computing the coordinate system
                if (referenceSystemText.Substring(0, 5) != ",UNIT" || referenceSystemText[5] != delim.Left)
                    throw new InvalidDataException("The projected coordinate reference system text is invalid.");

                Int32 unitBracketCloseIndex = FindClosingBracket(referenceSystemText, 5, delim);
                UnitOfMeasurement linearUnit = ComputeLinearUnit(referenceSystemText.Substring(6, unitBracketCloseIndex - 6));

                CoordinateSystem coordinateSystem = new CoordinateSystem(CoordinateSystem.UserDefinedIdentifier, CoordinateSystem.UserDefinedName,
                                                                         CoordinateSystemType.Cartesian,
                                                                         CoordinateSystemAxisFactory.Easting(linearUnit),
                                                                         CoordinateSystemAxisFactory.Northing(linearUnit));

                return new ProjectedCoordinateReferenceSystem(ProjectedCoordinateReferenceSystem.UserDefinedIdentifier, referenceSystemName, geographicReferenceSystem,
                                                              coordinateSystem, AreasOfUse.World, projection);
            }
            catch
            {
                throw new InvalidDataException("The projected coordinate reference system text is invalid.");
            }
        }

        /// <summary>
        /// Computes the linear unit of measurement from the WKT.
        /// </summary>
        /// <param name="text">The WKT representation of the linear unit.</param>
        /// <returns>The linear unit of measurement.</returns>
        private static UnitOfMeasurement ComputeLinearUnit(String text)
        {
            return ComputeUnitOfMeasurement(text, UnitQuantityType.Length);
        }
        /// <summary>
        /// Computes the angular unit of measurement from the WKT.
        /// </summary>
        /// <param name="text">The WKT representation of the angular unit.</param>
        /// <returns>The angular unit of measurement.</returns>
        private static UnitOfMeasurement ComputeAngularUnit(String text)
        {
            return ComputeUnitOfMeasurement(text, UnitQuantityType.Angle);
        }
        /// <summary>
        /// Computes the unit of measurement from the WKT.
        /// </summary>
        /// <param name="text">The WKT representation of the unit.</param>
        /// <param name="unitQuantityType">Type of the unit, linear or angular.</param>
        /// <returns>The unit of measurement.</returns>
        /// <exception cref="System.IO.InvalidDataException">The unit of measurement text is invalid.</exception>
        private static UnitOfMeasurement ComputeUnitOfMeasurement(String text, UnitQuantityType unitQuantityType)
        {
            try
            {
                String[] splitText = text.Split(',');

                String unitName = Regex.Replace(splitText[0].Trim('"'), "_", " ");
                Double baseMultiple = Double.Parse(splitText[1], CultureInfo.InvariantCulture);
    
                return new UnitOfMeasurement(UnitOfMeasurement.UserDefinedIdentifier, unitName, null, baseMultiple, unitQuantityType);
            }
            catch
            {
                throw new InvalidDataException("The unit of measurement text is invalid.");
            }
        }

        /// <summary>
        /// Computes the prime meridian from the WKT.
        /// </summary>
        /// <param name="text">The WKT representation of the prime meridian.</param>
        /// <returns>The prime meridian.</returns>
        private static Meridian ComputePrimeMeridian(String text)
        {
            try
            {
                Int32 meridianNameEndIndex = text.IndexOf('"', 1);
                String meridianName = Regex.Replace(text.Substring(1, meridianNameEndIndex - 1), "_", " ");

                // search for known meridian
                if (!String.IsNullOrWhiteSpace(meridianName))
                {
                    Meridian meridian = Meridians.FromName(meridianName).FirstOrDefault();
                    if (meridian != null)
                        return meridian;
                }

                String longitudeString = text.Substring(meridianNameEndIndex + 2,
                    text.Length - meridianNameEndIndex - 2);

                Double longitude = Double.Parse(longitudeString, CultureInfo.InvariantCulture);

                return new Meridian(Meridian.UserDefinedIdentifier, meridianName, Angle.FromRadian(longitude));
            }
            catch
            {
                throw new InvalidDataException("The prime meridian text is invalid.");
            }
        }

        /// <summary>
        /// Computes the geodetic datum from the WKT.
        /// </summary>
        /// <param name="text">The WKT representation of the geodetic datum.</param>
        /// <param name="delim">Used Delimiter.</param>
        /// <returns>The geodetic datum.</returns>
        /// <exception cref="System.IO.InvalidDataException">
        /// Invalid geodetic datum text.
        /// or
        /// The geodetic datum text is invalid.
        /// </exception>
        private static GeodeticDatum ComputeGeodeticDatum(String text, Delimiter delim)
        {
            try
            {
                Int32 nameEndIndex = text.IndexOf("\"", 1);
                Int32 nameStartIndex = (nameEndIndex > 2 && text.Substring(1, 2) == "D_") ? 3 : 1;

                String datumName = Regex.Replace(text.Substring(nameStartIndex, nameEndIndex - nameStartIndex), "_", " ");

                // seach for known geodetic datum
                if (!String.IsNullOrWhiteSpace(datumName))
                {
                    GeodeticDatum datum = GeodeticDatums.FromName(datumName).FirstOrDefault();
                    if (datum != null)
                        return datum;
                }

                String ellipsoidText;
                if (text.Substring(nameEndIndex + 1, 9) == ",SPHEROID")
                    ellipsoidText = text.Substring(nameEndIndex + 11, text.Length - (nameEndIndex + 11) - 1);
                else if (text.Substring(nameEndIndex + 1, 10) == ",ELLIPSOID")
                    ellipsoidText = text.Substring(nameEndIndex + 12, text.Length - (nameEndIndex + 12) - 1);
                else
                    throw new InvalidDataException("Invalid geodetic datum text.");

                return new GeodeticDatum(GeodeticDatum.UserDefinedIdentifier, datumName, null, null, AreasOfUse.World, ComputeEllipsoid(ellipsoidText), null);
            }
            catch (InvalidDataException)
            {
                throw;
            }
            catch 
            {
                throw new InvalidDataException("The geodetic datum text is invalid.");
            }
        }

        /// <summary>
        /// Computes the ellipsoid from the WKT.
        /// </summary>
        /// <param name="text">The WKT representation of the ellipsoid.</param>
        /// <returns>The ellipsoid.</returns>
        /// <exception cref="System.IO.InvalidDataException">The ellipsoid text is invalid.</exception>
        private static Ellipsoid ComputeEllipsoid(String text)
        {
            try
            {
                Int32 nameEndIndex = text.IndexOf("\"", 1);

                String ellipsoidName = Regex.Replace(text.Substring(1, nameEndIndex - 1), "_", " ");

                if (!String.IsNullOrWhiteSpace(ellipsoidName))
                {
                    Ellipsoid ellipsoid = Ellipsoids.FromName(ellipsoidName).FirstOrDefault();
                    if (ellipsoid != null)
                        return ellipsoid;
                }

                // the first value is the semi-major axis, the second is the inverse flattening
                String[] axisAndFlattening = text.Substring(nameEndIndex + 2, text.Length - nameEndIndex - 2).Split(',').ToArray();
                Double semiMajorAxis, inverseFlattening;

                semiMajorAxis = Double.Parse(axisAndFlattening[0], CultureInfo.InvariantCulture);
                inverseFlattening = Double.Parse(axisAndFlattening[1], CultureInfo.InvariantCulture);

                return Ellipsoid.FromInverseFlattening(String.Empty, ellipsoidName, semiMajorAxis, inverseFlattening);
            }
            catch
            {
                throw new InvalidDataException("The ellipsoid text is invalid.");
            }
        }

        #endregion
       
        #region Private utility methods

        /// <summary>
        /// Finds the closing bracket for an opening bracket.
        /// </summary>
        /// <param name="text">The examined text.</param>
        /// <param name="openingBracketIndex">The zero-based index of the opening bracket.</param>
        /// <param name="delim">The used delimiter.</param>
        /// <returns>The index of the closing bracket.</returns>
        /// <exception cref="System.IO.InvalidDataException">
        /// There is no opening bracket at the given index position.
        /// or
        /// There is no closing bracket at the given index position.
        /// </exception>
        private static Int32 FindClosingBracket(String text, Int32 openingBracketIndex, Delimiter delim)
        {
            if (text[openingBracketIndex] != delim.Left)
                throw new InvalidDataException("There is no opening bracket at the given index position.");

            Int32 openedBrackets = 1;
            openingBracketIndex++;

            while (openedBrackets > 0 && openingBracketIndex < text.Length)
            {
                if (text[openingBracketIndex] == delim.Left)
                    openedBrackets++;
                else if (text[openingBracketIndex] == delim.Right)
                    openedBrackets--;

                if (openedBrackets == 0)
                    return openingBracketIndex;

                openingBracketIndex++;
            }

            throw new InvalidDataException("There is no closing bracket at the given index position.");
        }
        #endregion
    }
}
